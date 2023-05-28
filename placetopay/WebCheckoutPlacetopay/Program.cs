using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebCheckoutPlacetopay.Models;
using WebCheckoutPlacetopay.Models.WebCheckoutPlacetopay.Models;
using WebCheckoutPlacetopay.Services;
using Microsoft.Extensions.Configuration.Json;

namespace WebCheckoutPlacetopay
{
    class Program
    {
        private static APICommunicationService _apiService;
        private static APIAuthService _authService;
        private static IConfiguration _config;

        static async Task Main(string[] args)
        {
            _apiService = new APICommunicationService();
            _authService = new APIAuthService();

            APIAuth auth = GenerateAuth();
            var sessionRequest = CreateSessionRequest(auth);
            var response = await SendSessionRequest(sessionRequest);
            await HandleResponse(response, auth);
        }

        private static APIAuth GenerateAuth()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            var keyLogin = config.GetSection("credential:Login").Value;
            var keySecretKey = config.GetSection("credential:SecretKey").Value;

            if (string.IsNullOrEmpty(keyLogin) || string.IsNullOrEmpty(keySecretKey))
            {
                Console.WriteLine("Login or SecretKey is not set in the configuration.");
            }
            return _authService.GenerateAPIAuth(keyLogin, keySecretKey);
        }

        private static APISessionRequest CreateSessionRequest(APIAuth auth)
        {
            return new APISessionRequest
            {
                locale = "es_CO",
                auth = auth,
                payment = new Payment
                {
                    reference = "20230527132",
                    description = "Prueba de Pago",
                    amount = new Amount
                    {
                        currency = "COP",
                        total = 10000
                    }
                },
                expiration = DateTime.UtcNow.AddMinutes(5),
                returnUrl = "https://checkout-test.placetopay.com/",
                ipAddress = "127.0.0.1",
                userAgent = "PlacetoPay Sandbox"
            };
        }

        private static async Task<APIResponse> SendSessionRequest(APISessionRequest request)
        {
            var response = await _apiService.SendSessionRequest(request);
            Console.WriteLine($"API call: status: {response.status.status} message: {response.status.message} requestId: {response.requestId}");
            return response;
        }

        private static async Task HandleResponse(APIResponse response, APIAuth auth)
        {
            if (response.status.status != "OK")
            {
                Console.WriteLine($"API call failed with status: {response.status.status}");
                return;
            }
            Console.WriteLine($"Opening URL: {response.processUrl}");
            Process.Start(new ProcessStartInfo
            {
                FileName = response.processUrl,
                UseShellExecute = true
            });

            await WaitForPaymentToComplete(response, auth);
        }

        private static async Task WaitForPaymentToComplete(APIResponse response, APIAuth auth)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(30)); // Esperar 30 segundos
                var statusResponse = await _apiService.GetPaymentSessionStatus(response.requestId, auth);
                Console.WriteLine($"Payment session status: {statusResponse.status.status} requestId: {statusResponse.status.requestId} message: {statusResponse.status.message}");
                if (statusResponse.status.status != "PENDING") // Si el estado no es "PENDING", salir del bucle
                    break;
            }
        }
    }
}