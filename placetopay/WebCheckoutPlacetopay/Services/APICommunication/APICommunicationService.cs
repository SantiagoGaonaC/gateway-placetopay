using System.Text;
using Newtonsoft.Json;
using WebCheckoutPlacetopay.Models;
using WebCheckoutPlacetopay.Models.WebCheckoutPlacetopay.Models;

namespace WebCheckoutPlacetopay.Services
{
    public class APICommunicationService
    {
        // Declaración del cliente HTTP que se utilizará para las solicitudes a la API
        private readonly HttpClient _httpClient;

        public APICommunicationService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<APIResponse> SendSessionRequest(APISessionRequest request)
        {
            // Serialización de la solicitud a JSON y envío de la solicitud POST a la API
            var jsonString = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://checkout-test.placetopay.com/api/session", content);
            // Verificación del estado de la respuesta y lanzamiento de una excepción si algo salió mal
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("API call failed with status: " + response.StatusCode);
            }
            // Deserialización de la respuesta de la API y retorno del objeto de respuesta
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseContent);

            if (apiResponse == null)
            {
                throw new Exception("API response is null.");
            }

            return apiResponse;
        }


        // Esta función asincrónica se encarga de obtener el estado de una sesión de pago de la API.
        // Para realizar esta consulta, se requiere el ID de la solicitud (requestId) y la información de autenticación (auth).
        public async Task<APIResponse> GetPaymentSessionStatus(int requestId, APIAuth auth)
        {
            var url = $"https://checkout-test.placetopay.com/api/session/{requestId}";

            // Crea un objeto que representa la autenticación requerida por la API.
            // Este objeto incluye los campos de login, tranKey, nonce y seed.
            var authObject = new
            {
                auth = new
                {
                    login = auth.login,
                    tranKey = auth.tranKey,
                    nonce = auth.nonce,
                    seed = auth.seed
                }
            };
            // Convierte el objeto de autenticación a una cadena JSON y crea un contenido HTTP a partir de él.
            var jsonString = JsonConvert.SerializeObject(authObject);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            // Realiza la solicitud POST a la API utilizando el cliente HTTP.
            // Si la respuesta tiene un código de estado exitoso, se continúa con el procesamiento de la respuesta.
            // Si no, se lanza una excepción indicando el código de estado de la respuesta.
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("API call failed with status: " + response.StatusCode);
            }
            // Lee el contenido de la respuesta y lo deserializa a un objeto de tipo APIResponse.
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseContent);
            // Comprueba si la respuesta de la API es nula y lanza una excepción en caso afirmativo.
            if (apiResponse == null)
            {
                throw new Exception("API response is null.");
            }
            // Retorna la respuesta de la API.
            return apiResponse;
        }

    }
}