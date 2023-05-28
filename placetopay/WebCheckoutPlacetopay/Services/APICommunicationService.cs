using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using WebCheckoutPlacetopay.Models;
using WebCheckoutPlacetopay.Models.WebCheckoutPlacetopay.Models;

namespace WebCheckoutPlacetopay.Services
{
    public class APICommunicationService
    {
        private readonly HttpClient _httpClient;

        public APICommunicationService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<APIResponse> SendSessionRequest(APISessionRequest request)
        {
            var jsonString = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://checkout-test.placetopay.com/api/session", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("API call failed with status: " + response.StatusCode);
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseContent);
            
            return apiResponse;
        }
        public async Task<APIResponse> GetPaymentSessionStatus(int requestId, APIAuth auth)
        {
            var url = $"https://checkout-test.placetopay.com/api/session/{requestId}";

            // Crear objeto con la autenticación
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

            var jsonString = JsonConvert.SerializeObject(authObject);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("API call failed with status: " + response.StatusCode);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseContent);

            return apiResponse;
        }
    }
}