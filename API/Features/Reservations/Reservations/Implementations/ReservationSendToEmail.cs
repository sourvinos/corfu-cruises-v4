using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using API.Features.Reservations.Parameters;
using API.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using ZXing.QrCode;

namespace API.Features.Reservations.Reservations {

    public class ReservationSendToEmail : IReservationSendToEmail {

        private readonly EmailReservationSettings emailReservationSettings;
        private readonly IReservationParametersRepository parametersRepo;

        public ReservationSendToEmail(IOptions<EmailReservationSettings> emailSettings, IReservationParametersRepository parametersRepo) {
            this.emailReservationSettings = emailSettings.Value;
            this.parametersRepo = parametersRepo;
        }

        public async Task SendReservationToEmail(BoardingPassReservationVM reservation) {
            using var smtp = new SmtpClient();
            smtp.Connect(emailReservationSettings.SmtpClient, emailReservationSettings.Port);
            smtp.Authenticate(emailReservationSettings.Username, emailReservationSettings.Password);
            await smtp.SendAsync(await BuildMessage(reservation));
            smtp.Disconnect(true);
        }

        private async Task<MimeMessage> BuildMessage(BoardingPassReservationVM reservation) {
            var message = new MimeMessage { Sender = MailboxAddress.Parse(emailReservationSettings.Username) };
            message.From.Add(new MailboxAddress(emailReservationSettings.From, emailReservationSettings.Username));
            message.To.Add(MailboxAddress.Parse(reservation.Email));
            message.Subject = "Your reservation is ready!";
            message.Body = new BodyBuilder { HtmlBody = await BuildTemplate(reservation) }.ToMessageBody();
            return message;
        }

        private async Task<string> BuildTemplate(BoardingPassReservationVM reservation) {
            RazorLightEngine engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
                .Build();
            return await engine.CompileRenderStringAsync(
                "key",
                LoadTemplateFromFile(),
                new BoardingPassReservationVM {
                    LogoTextBase64 = SetLogoTextAsBackground(),
                    Date = DateHelpers.FormatDateStringToLocaleString(reservation.Date),
                    Destination = reservation.Destination,
                    Customer = reservation.Customer,
                    RefNo = reservation.RefNo,
                    TicketNo = reservation.TicketNo,
                    TotalPax = reservation.TotalPax,
                    Phones = reservation.Phones,
                    CompanyPhones = parametersRepo.GetAsync().Result.Phones,
                    PickupPoint = reservation.PickupPoint,
                    Remarks = reservation.Remarks,
                    Passengers = reservation.Passengers,
                    Barcode = SetBarcodeAsBackground(reservation.RefNo)
                });
        }

        private static string LoadTemplateFromFile() {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\BoardingPass.cshtml";
            StreamReader str = new(FilePath);
            string template = str.ReadToEnd();
            str.Close();
            return template;
        }

        private static string SetLogoTextAsBackground() {
            return "width: 116px; height: 23px; background: url(data:image/png;base64," + LogoService.GetBase64LogoText() + ")";
        }

        private static string SetBarcodeAsBackground(string refNo) {
            return "background: url(data:image/png;base64," + Convert.ToBase64String(CreateBarcode(refNo, 200, 200, 2)) + ")";
        }

        private static byte[] CreateBarcode(string text, int width, int height, int margin) {
            byte[] byteArray;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var pixelData = qrCodeWriter.Write(text);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb)) {
                using var ms = new MemoryStream();
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally {
                    bitmap.UnlockBits(bitmapData);
                }
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byteArray = ms.ToArray();
            }
            return byteArray;
        }

    }

}
