using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using WebCheckoutPlacetopay.Models;
using WebCheckoutPlacetopay.Models.WebCheckoutPlacetopay.Models;

namespace WebCheckoutPlacetopay.Services
{
    public class PaymentService
    {
        // Declaración del servicio de comunicación API que se utilizará para enviar solicitudes
        private readonly APICommunicationService _apiService;

        public PaymentService()
        {
            _apiService = new APICommunicationService();
        }

        public APISessionRequest CreateSessionRequest(APIAuth auth, IConfiguration config)
        {
            decimal total;
            decimal.TryParse(config.GetSection("request:total").Value, out total);
            return new APISessionRequest
            {
                locale = "es_CO",
                auth = auth,
                payment = new Payment
                {
                    reference = GenerateReference(),
                    description = config.GetSection("request:description").Value,
                    amount = new Amount
                    {
                        currency = config.GetSection("request:currency").Value,
                        total = total
                    }
                },
                expiration = DateTime.UtcNow.AddMinutes(5),
                returnUrl = config.GetSection("request:returnUrl").Value,
                ipAddress = config.GetSection("request:ipAddress").Value,
                userAgent = config.GetSection("request:userAgent").Value,
            };
        }

        public async Task<APIResponse> SendSessionRequest(APISessionRequest request)
        {
            var response = await _apiService.SendSessionRequest(request);
            if (response == null)
            {
                throw new Exception("API call failed. Response is null.");
            }
            Console.WriteLine($"API call: status: {response.status?.status} message: {response.status?.message} requestId: {response.requestId}");
            return response;
        }

        public async Task HandleResponse(APIResponse response, APIAuth auth)
        {
            if (response != null)
            {
                if (response.status != null && response.status.status != "OK")
                {
                    Console.WriteLine($"API call failed with status: {response.status.status}");
                    return;
                }

                if (!string.IsNullOrEmpty(response.processUrl))
                {
                    Console.WriteLine($"Opening URL: {response.processUrl}");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = response.processUrl,
                        UseShellExecute = true
                    });
                }

                await WaitForPaymentToComplete(response, auth);
            }
        }
        private async Task WaitForPaymentToComplete(APIResponse response, APIAuth auth)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(30)); // Esperar 30 segundos
                var statusResponse = await _apiService.GetPaymentSessionStatus(response.requestId, auth);
                if (statusResponse != null)
                {
                    Console.WriteLine($"Payment session status: {statusResponse.status?.status} requestId: {statusResponse.status?.requestId} message: {statusResponse.status?.message}");
                    if (statusResponse.status?.status != "PENDING") // Si el estado no es "PENDING", salir del bucle
                        break;
                }
            }
        }
        private string GenerateReference()
        {
            var now = DateTime.Now;
            var reference = string.Concat(now.Year, now.Month.ToString("D2"), now.Day.ToString("D2"), now.Hour.ToString("D2"), now.Minute.ToString("D2"));
            return reference;
        }
    }
}