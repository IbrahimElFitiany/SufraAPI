using SufraMVC.DTOs.ReservationDTOs;

namespace SufraMVC.Infrastructure.Services
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string content);
        byte[] GenerateReservationQRCode(ReservationDTO reservation);
    }

}
