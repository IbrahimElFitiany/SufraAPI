﻿
using QRCoder.Extensions;
using Sufra.DTOs.ReservationDTOs;
using Sufra.Infrastructure.Services;
using Sufra.Models.Customers;
using Sufra.Models.Reservations;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;
using Sufra.Exceptions;
using System.Data;

namespace Sufra.Services.Services
{
    public class ReservationServices : IReservationServices
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;          //need to remove it in DDD
        private readonly ICustomerRepository _customerRepository;          //need to remove it in DDD
        private readonly IQRCodeService _qrCodeService;
        private readonly IEmailServices _emailServices;

        public ReservationServices(IReservationRepository reservationRepository,IRestaurantRepository restaurantRepository,IQRCodeService qrCodeService,IEmailServices emailServices,ICustomerRepository customerRepository)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository; //will be removed and replaced with messaging (out side of bound context)
            _qrCodeService = qrCodeService;
            _restaurantRepository = restaurantRepository;  //will be removed and replaced with messaging (out side of bound context)
            _emailServices = emailServices;
        }

        //generic function
        public async Task<Restaurant> ValidateRestaurant(int restaurantId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException();
            }
            if (restaurant.IsApproved == false)
            {
                throw new RestaurantNotApprovedException();
            }
            return restaurant;
        }

        //--------------------------------------
        public async Task<CreateReservationResDTO> CreateAsync(ReservationDTO reservationDTO)
        {

            Restaurant restaurant = await ValidateRestaurant(reservationDTO.RestaurantId);

            if (!restaurant.IsInOpeningHour(reservationDTO.ReservationDateTime)) throw new OutOfOpeningHoursException("Not in opening hours.");

            //fixed time duration w maxeffort rn 
            int maxEffort = 2;
            TimeSpan reservationDuration = TimeSpan.FromMinutes(45);
            DateTime desiredStart = reservationDTO.ReservationDateTime;
            DateTime desiredEnd = desiredStart.Add(reservationDuration);

            //get tables with capacity > party size and <= (party size + maxEffort).
            IEnumerable<Table> suitableTables = restaurant.GetTables()
                .Where(t => t.Capacity >= reservationDTO.PartySize && t.Capacity <= reservationDTO.PartySize + maxEffort)  
                .OrderBy(t => t.Capacity);

            if (!suitableTables.Any()) throw new NoAvailableTablesException("No available tables for this party size.");


            foreach (Table table in suitableTables)
            {
                var approvedReservations = await _reservationRepository.GetApprovedReservationsByTableAsync(table);

                bool hasApprovedOverlap = approvedReservations.Any(res =>
                {
                    DateTime existingStart = res.ReservationDateTime;
                    DateTime existingEnd = existingStart.Add(reservationDuration);
                    return desiredStart < existingEnd && desiredEnd > existingStart;
                });
                Console.WriteLine($"el table {table.Id} 3ndha approved reservation fl m3ad dah?: " + hasApprovedOverlap);

                if (!hasApprovedOverlap)
                {
                    var pendingReservations = await _reservationRepository.GetPendingReservationsByTableAsync(table);

                    bool hasPendingOverlap = pendingReservations.Any(res =>
                    {
                        DateTime existingStart = res.ReservationDateTime;
                        DateTime existingEnd = existingStart.Add(reservationDuration);
                        return desiredStart < existingEnd && desiredEnd > existingStart;
                    });

                    Console.WriteLine($"el table {table.Id} 3ndha pending reservation fl m3ad dah?: " + hasPendingOverlap);

                    if (!hasPendingOverlap)
                    {
                        // No approved or pending overlap — best case
                        var reservation = new Reservation
                        {
                            CustomerId = reservationDTO.CustomerId,
                            RestaurantId = restaurant.Id,
                            TableId = table.Id,
                            ReservationDateTime = DateTime.SpecifyKind(reservationDTO.ReservationDateTime, DateTimeKind.Utc),
                            PartySize = reservationDTO.PartySize,
                            Status = ReservationStatus.Pending
                        };

                        await _reservationRepository.CreateAsync(reservation);

                        return new CreateReservationResDTO
                        {
                            message = "Reservation created and pending approval. An email will be sent to you."
                        };
                    }
                }
            }

            // If we reach here, all tables have pending or approved overlaps → fallback reservation
            var fallbackReservation = new Reservation
            {
                CustomerId = reservationDTO.CustomerId,
                RestaurantId = restaurant.Id,
                TableId = suitableTables.First().Id,
                ReservationDateTime = DateTime.SpecifyKind(reservationDTO.ReservationDateTime, DateTimeKind.Utc),
                PartySize = reservationDTO.PartySize,
                Status = ReservationStatus.Pending
            };

            await _reservationRepository.CreateAsync(fallbackReservation);

            return new CreateReservationResDTO
            {
                message = "All tables at this restaurant that can take your party size are busy at the selected time. we will send an email if one table became avalible"
            };
        }

        public async Task<IEnumerable<ReservationDTO>> GetAllAsync(ReservationQueryDTO queryDTO)
        {
            var reservations = await _reservationRepository.GetAllAsync(queryDTO);

            return reservations.Select(reservation => new ReservationDTO
            {
                ReservationId = reservation.Id,

                CustomerId = reservation.CustomerId,
                CustomerName = reservation.Customer.Fname,
                CustomerEmail = reservation.Customer.Email,
                CustomerPhone = reservation.Customer.Phone,

                RestaurantId = reservation.RestaurantId,
                RestaurantName = reservation.Restaurant.Name,
                
                TableId = reservation.TableId,
                TabelLabel = reservation.Table.TableLabel,

                ReservationDateTime = reservation.ReservationDateTime,
                PartySize = reservation.PartySize,
                reservationStatus = reservation.Status.ToString()
            }).ToList();
        }


        public async Task ApproveAsync(int reservationId , int restaurantId)
        {
            //validate
            Restaurant restaurant = await ValidateRestaurant(restaurantId);
            
            //get reservation
            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.RestaurantId != restaurantId)
            {
                throw new Exception("no reservation with this ID");
            }

            await _reservationRepository.ApproveAsync(reservation);

            Console.WriteLine(reservation.Status.GetStringValue());
            byte[] QRCodeImageBase64 = _qrCodeService.GenerateReservationQRCode(new ReservationDTO
            {
                ReservationId = reservation.Id,
                CustomerId = reservation.CustomerId,
                ReservationDateTime = reservation.ReservationDateTime,
                TableId = reservation.TableId,
                TabelLabel = reservation.Table.TableLabel,
                PartySize = reservation.PartySize,
                RestaurantId = reservation.RestaurantId,
                reservationStatus = reservation.Status.ToString()
            });

            string img = Convert.ToBase64String(QRCodeImageBase64);
            await _emailServices.SendApprovalEmailAsync(reservation.Customer.Email, "Reservation Approved, Thank You for using Sufra", "",img);
                           
        }
        public async Task RejectAsync(int reservationId, int restaurantId)
        {
            Restaurant restaurant = await ValidateRestaurant(restaurantId);

            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.RestaurantId != restaurantId)
            {
                throw new Exception("no reservation with this ID");
            }

            await _reservationRepository.RejectAsync(reservation);
            await _emailServices.SendRejectionEmailAsync(reservation.Customer.Email, "Reservation Rejected");
        }
        public async Task CancelAsync(int reservationId, int customerId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null) throw new UserNotFoundException("Customer Not Found");


            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null) throw new ReservationNotFoundException("Reservation Not Found");

            if (reservation.CustomerId != customer.Id) throw new UnauthorizedAccessException("Unauthorized");

            await _reservationRepository.CancelAsync(reservation);
        }


        public Task RescheduleAsync(Reservation reservation)
        {
            throw new NotImplementedException();
        }

    }
}
