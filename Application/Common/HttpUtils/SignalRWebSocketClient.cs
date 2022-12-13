using Common.Dto;
using IdentityModel.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Common.HttpUtils
{
    public interface ISignalRWebSocketClient : IDisposable
    {
        // Properties
        HubConnection HubConnection { get; set; }
        bool IsConnected { get; }
        bool IsDisconnected { get; }

        // Methods
        Task<bool> Connect();
        Task<bool> Disconnect();
        Task<bool> ConnectWithRetryAsync(HubConnection hubConnection, CancellationToken token);

        Task AddToGroup(string groupName);
        Task RemoveFromGroup(string groupName);

        Task<bool> SendErrorResponseToClient(GenericResponse genericResponse);
        Task<bool> SendNewOrderToRestaurantOwner(CreateOrderDto createdOrderDto);
    }

    public class SignalRWebSocketClient : ISignalRWebSocketClient
    {
        private HubConnection _connection;

        // Token
        private static DateTime _expiryTime;

        private static TokenResult _token;

        // Groups
        private readonly string _restaurantOwnerGroupName = "RestaurantOwner";
        private readonly string _CustomerOwnerGroup = "Customer";

        private readonly string _DeliveryGroup = "Delivery";

        // Api key
        private readonly string _apiKey = "a1071862-67a0-4b49-b0b0-4c00ec34f3c2";

        private string _hubUrl = "https://localhost:5011";

        public SignalRWebSocketClient()
        {
            // Build the connection
            _connection = new HubConnectionBuilder().WithUrl($"{_hubUrl}", options =>
            {
                //options.AccessTokenProvider = async () => await GetToken(hardwareApiUrl, apiKey); // security wiped out for now
            }).WithAutomaticReconnect().Build();

            _connection.Closed += SignalROnClosed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task SignalROnClosed(Exception? arg)
        {
            Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpiryTime
        {
            get { return _expiryTime; }

            set { _expiryTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public HubConnection HubConnection
        {
            get { return _connection; }

            set { _connection = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected
        {
            get { return _connection.State == HubConnectionState.Connected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisconnected
        {
            get { return _connection.State == HubConnectionState.Disconnected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public async void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Disconnect()
        {
            bool isDisconnected;
            try
            {
                await _connection.StopAsync();
                isDisconnected = true;
            }
            catch (Exception ex)
            {
                isDisconnected = false;
            }

            return isDisconnected;
        }

        /// <summary>
        /// Generic method used to send error responses via websockets to the client
        /// </summary>
        /// <param name="genericResponse"></param>
        /// <returns></returns>
        public async Task<bool> SendErrorResponseToClient(GenericResponse genericResponse)
        {
            try
            {
                await Invoke("SendErrorMessage", JsonConvert.SerializeObject(genericResponse));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Sends new orders to restaurant owners
        /// </summary>
        /// <param name="createdOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> SendNewOrderToRestaurantOwner(CreateOrderDto createdOrderDto)
        {
            try
            {
                await Invoke("SendNewOrder", JsonConvert.SerializeObject(createdOrderDto));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Connect()
        {
            bool isConnected;
            try
            {
                await _connection.StartAsync();
                isConnected = true;
            }
            catch (Exception ex)
            {
                isConnected = false;
            }

            return isConnected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ConnectWithRetryAsync(HubConnection hubConnection, CancellationToken token)
        {
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    await hubConnection.StartAsync(token);
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    await Task.Delay(5000);
                }
            }
        }


        /// <summary>
        /// Generic method invoking methods on the server side
        /// </summary>
        /// <param name="hubMethod"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task Invoke(string hubMethod, object data)
        {
            if (_connection == null || _connection.State == HubConnectionState.Disconnected)
            {
                await Connect();
            }

            if (_connection.State == HubConnectionState.Connected)
            {
                if (data != null)
                {
                    await _connection.InvokeAsync(hubMethod, data);
                }
                else
                {
                    await _connection.InvokeAsync(hubMethod);
                }
            }
        }


        /// <summary>
        /// Method used to retrieve an token from the identity server....
        /// </summary>
        /// <param name="url"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<string> GetToken(string IdentityServerUrl, string apiKey)
        {
            if (_token == null || _expiryTime < DateTime.UtcNow)
            {
                var client = HttpClientInitializer.GetClient();
                var disco = await client.GetDiscoveryDocumentAsync(IdentityServerUrl);

                if (disco.IsError)
                {
                    return string.Empty;
                }
                else
                {
                    var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                        new ClientCredentialsTokenRequest
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = "chub-client",
                            ClientSecret = apiKey,
                        });

                    _expiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 10);
                    _token = new TokenResult {Succeeded = !tokenResponse.IsError, Token = tokenResponse};
                }
            }

            return _token.Token.AccessToken;
        }

        public Task AddToGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        /// </summary>
        public class TokenRequest
        {
            public string Url { get; set; }
            public string ApiKey { get; set; }
            public string ClientId { get; set; }
        }

        /// <summary>
        /// For the token response
        /// </summary>
        public class TokenResult
        {
            public bool Succeeded { get; set; }
            public TokenResponse Token { get; set; }
            public string Error { get; set; }
        }
    }
}