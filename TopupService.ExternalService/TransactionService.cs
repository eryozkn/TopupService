using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TopupService.ExternalService.Models.ServiceRequests;
using TopupService.ExternalService.Models.ServiceResponses;

namespace TopupService.ExternalService
{
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _httpClient;
        private const string _userBalanceAPI = "balance"; // Move to config
        private const string _transactionAPI = "transaction"; // Move to config
        private ILogger<TransactionService> _logger;
        public TransactionService(ILogger<TransactionService> logger) 
        {
            _httpClient = new HttpClient();
            //ToDo: need to move this to config
            _httpClient.BaseAddress = new Uri("https://localhost:81/api/v1/"); 
            _logger = logger;
        }

        public async Task<UserBalanceServiceResponse> GetUserBalance(long userId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_userBalanceAPI}/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var serviceResponse = await response.Content.ReadFromJsonAsync<UserBalanceServiceResponse>();

                    if (serviceResponse != null)
                    {
                        return serviceResponse;
                    }
                    else
                    {
                        _logger.LogError($"Error in calling transaction service - get balance - service response is null");

                        return new UserBalanceServiceResponse()
                        {
                            UserId = userId,
                            Balance = 0,
                            Currency = "AED"
                        };
                    }
                }

                return new UserBalanceServiceResponse()
                {
                    UserId = userId,
                    Balance = 0,
                    Currency = "AED"
                };
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, $"Error in calling transaction service - get balance");

                //swallow error and return zero balance
                return new UserBalanceServiceResponse()
                {
                    UserId = userId,
                    Balance = 0,
                    Currency = "AED"
                };
            }
        }

        public async Task<TransactionResponse?> CreateTransaction(CreateTransactionRequest request)
        {
            try
            {
                HttpContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_transactionAPI}", content);

                if (response.IsSuccessStatusCode)
                {
                    var serviceResponse = await response.Content.ReadFromJsonAsync<TransactionResponse>();

                    if (serviceResponse != null)
                    {
                        return serviceResponse;
                    }
                }

                _logger.LogError($"Create transaction response is null with response status - {response.StatusCode}");
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Error in calling transaction service - get balance");
                throw;
            }
        }
    }
}