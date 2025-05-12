using DTOs.ReservationDTOs;
using Models.Reservation;

namespace Sufra_MVC.Infrastructure.Services
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string content);
        byte[] GenerateReservationQRCode(ReservationDTO reservation);
    }

}
