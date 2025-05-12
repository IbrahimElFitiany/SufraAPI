using Sufra_MVC.Infrastructure.Services;
using QRCoder;
using Newtonsoft.Json;
using DTOs.ReservationDTOs;
using Models.Reservation;

public class QRCodeService : IQRCodeService
{
    public byte[] GenerateQRCode(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(10);
    }

    public byte[] GenerateReservationQRCode(ReservationDTO reservation)
    {
        var content = JsonConvert.SerializeObject(
            reservation,
            Formatting.Indented
        );

        return GenerateQRCode(content);
    }
}
