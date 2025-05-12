using DTOs.ReservationDTOs;
using MimeKit.Cryptography;
using Models.Reservation;
using QRCoder.Extensions;
using Sufra_MVC.Exceptions;
using Sufra_MVC.Infrastructure.Services;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.RestaurantModels;
using Sufra_MVC.Repositories;
using Sufra_MVC.Services.IServices;
using System.Data;

namespace Sufra_MVC.Services.Services
{
    public class ReservationServices : IReservationServices
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;          //need to remove it ind DDD
        private readonly ICustomerRepository _customerRepository;          //need to remove it ind DDD
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
            //check on reservation Date
            if (reservationDTO.ReservationDateTime < DateTime.UtcNow)
            {
                throw new Exception("invalid can't reserve in the past");
            }

            Restaurant restaurant = await ValidateRestaurant(reservationDTO.RestaurantId);

            if (!restaurant.IsInOpeningHour(reservationDTO.ReservationDateTime))
                throw new Exception("Not in opening hours.");

            //fixed time duration w maxeffort rn 
            int maxEffort = 2;
            TimeSpan reservationDuration = TimeSpan.FromMinutes(45);
            DateTime desiredStart = reservationDTO.ReservationDateTime;
            DateTime desiredEnd = desiredStart.Add(reservationDuration);

            //get el tables el el capacity akbr mn el ps , capcity as8r or equal ps + maxeffort
            IEnumerable<Table> tables = restaurant.GetTables()
                .Where(t => t.Capacity >= reservationDTO.PartySize && t.Capacity <= reservationDTO.PartySize + maxEffort)  
                .OrderBy(t => t.Capacity);

            if (!tables.Any())
                throw new Exception("No available tables for this party size.");


            foreach (Table table in tables)
            {
                var approvedReservations = await _reservationRepository.GetApprovedReservationByTableAsync(table);

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
                TableId = tables.First().Id,
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



        public async Task ApproveAsync(int reservationId , int restaurantId)
        {
            Restaurant restaurant = await ValidateRestaurant(restaurantId);
            
            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.RestaurantId != restaurantId)
            {
                throw new Exception("no reservation with this ID");
            }

            await _reservationRepository.ApproveAsync(reservation);

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
            Console.WriteLine(img);

            await _emailServices.SendEmailAsync(reservation.Customer.Email, "Reservation Approved, Thank You for using Sufra", "ayn kan",img);
                           
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
            if (customer == null)
            {
                throw new Exception("Customer Not Found");
            }

            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.CustomerId != customer.Id)
            {
                throw new Exception("no reservation with this ID");
            }

            await _reservationRepository.CancelAsync(reservation);
        }


        public Task RescheduleAsync(Reservation reservation)
        {
            throw new NotImplementedException();
        }

    }
}
