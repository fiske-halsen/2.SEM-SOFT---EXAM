using Common.Dto;
using Common.ErrorModels;
using Common.HttpUtils;
using Newtonsoft.Json;
using System.Text;
using IdentityModel.Client;
using System.Net.Http;

namespace GraphqlDemo.Services
{
    public interface IApiService
    {
        public Task<IEnumerable<T>> Get<T>(string url, ApplicationCredentials applicationCredentials);
        public Task<T> GetSingle<T>(string url, ApplicationCredentials applicationCredentials);
        public Task<bool> Post(string url, string contentJson, ApplicationCredentials applicationCredentials);
        public Task<bool> Patch(string url, string contentJson, ApplicationCredentials applicationCredentials);
        public Task<bool> Delete(string url, ApplicationCredentials applicationCredentials);
    }

    public class ApiService : IApiService
    {
        private readonly ITokenService _tokenService;

        public ApiService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<bool> Delete(string url, ApplicationCredentials? applicationCredentials)
        {
            var httpClient = HttpClientInitializer.GetClient();

            if (applicationCredentials != null)
            {
                var token = await _tokenService.RequestTokenClientFromInternalMicroService(applicationCredentials);
                httpClient.SetBearerToken(token.TokenResponse.AccessToken);
            }

            using (var response = await httpClient.DeleteAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var exception =
                        JsonConvert.DeserializeObject<ExceptionDto>(await response.Content.ReadAsStringAsync());
                    throw new HttpStatusException(exception.StatusCode, exception.Message);
                }
            }
        }

        public async Task<IEnumerable<T>> Get<T>(string url, ApplicationCredentials? applicationCredentials)
        {
            var httpClient = HttpClientInitializer.GetClient();

            if (applicationCredentials != null)
            {
                var token = await _tokenService.RequestTokenClientFromInternalMicroService(applicationCredentials);
                httpClient.SetBearerToken(token.TokenResponse.AccessToken);
            }

            using (var response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    IEnumerable<T> result = JsonConvert.DeserializeObject<IEnumerable<T>>(content);
                    return result;
                }
                else
                {
                    var exception =
                        JsonConvert.DeserializeObject<ExceptionDto>(await response.Content.ReadAsStringAsync());
                    throw new HttpStatusException(exception.StatusCode, exception.Message);
                }
            }
        }

        public async Task<T> GetSingle<T>(string url, ApplicationCredentials? applicationCredentials)
        {
            var httpClient = HttpClientInitializer.GetClient();

            if (applicationCredentials != null)
            {
                var token = await _tokenService.RequestTokenClientFromInternalMicroService(applicationCredentials);
                httpClient.SetBearerToken(token.TokenResponse.AccessToken);
            }

            using (var response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(content);
                    return result;
                }
                else
                {
                    var exception =
                        JsonConvert.DeserializeObject<ExceptionDto>(await response.Content.ReadAsStringAsync());
                    throw new HttpStatusException(exception.StatusCode, exception.Message);
                }
            }
        }

        public async Task<bool> Patch(string url, string contentJson, ApplicationCredentials? applicationCredentials)
        {
            var httpClient = HttpClientInitializer.GetClient();

            if (applicationCredentials != null)
            {
                var token = await _tokenService.RequestTokenClientFromInternalMicroService(applicationCredentials);
                httpClient.SetBearerToken(token.TokenResponse.AccessToken);
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            using (var response = await httpClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var exception =
                        JsonConvert.DeserializeObject<ExceptionDto>(await response.Content.ReadAsStringAsync());
                    throw new HttpStatusException(exception.StatusCode, exception.Message);
                }
            }
        }

        public async Task<bool> Post(string url, string contentJson, ApplicationCredentials? applicationCredentials)
        {
            var httpClient = HttpClientInitializer.GetClient();

            if (applicationCredentials != null)
            {
                var token = await _tokenService.RequestTokenClientFromInternalMicroService(applicationCredentials);
                httpClient.SetBearerToken(token.TokenResponse.AccessToken);
            }

            HttpContent httpContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(url, httpContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var exception =
                        JsonConvert.DeserializeObject<ExceptionDto>(await response.Content.ReadAsStringAsync());
                    throw new HttpStatusException(exception.StatusCode, exception.Message);
                }
            }
        }
    }
}