using Sufra.DTOs.ReservationDTOs;

namespace Sufra.Infrastructure.Services
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string content);
        byte[] GenerateReservationQRCode(ReservationDTO reservation);
    }

}
