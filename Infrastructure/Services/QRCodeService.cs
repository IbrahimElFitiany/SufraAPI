using QRCoder;
using Newtonsoft.Json;
using SufraMVC.Infrastructure.Services;
using SufraMVC.DTOs.ReservationDTOs;

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
