using Microsoft.Extensions.Configuration;
using WebCheckoutPlacetopay.Models;
using WebCheckoutPlacetopay.Services;

namespace WebCheckoutPlacetopay
{
    class Program
    {
        // Declaración de los servicios que se utilizarán en la aplicación
        private static APICommunicationService? _apiService;
        private static APIAuthService? _authService;
        private static IConfiguration? _config;
        private static PaymentService? _paymentService;

        static async Task Main(string[] args)
        {
            // Inicialización de los servicios
            _apiService = new APICommunicationService();
            _authService = new APIAuthService();
            _paymentService = new PaymentService();

            // Carga de la configuración desde el archivo appsettings.json
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            // Obtención de los valores de login y secretKey de la configuración
            string login = _config.GetSection("credential:Login").Value ?? string.Empty;
            string secretKey = _config.GetSection("credential:SecretKey").Value ?? string.Empty;
            // Verificación de que los valores no sean nulos antes de generar la autenticación
            if (login != null && secretKey != null)
            {
                // Generación de la autenticación usando el AuthService
                APIAuth auth = _authService.GenerateAPIAuth(login, secretKey);
                // Creación de la solicitud de sesión usando el PaymentService
                var sessionRequest = _paymentService.CreateSessionRequest(auth, _config);
                // Envío de la solicitud de sesión y manejo de la respuesta
                var response = await _paymentService.SendSessionRequest(sessionRequest);
                await _paymentService.HandleResponse(response, auth);
            }
        }
    }
}