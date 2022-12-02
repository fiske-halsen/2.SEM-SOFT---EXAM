using Common.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Common.ErrorModels;
using IdentityModel.Client;
using UserService.Test.IntegrationTestConfig;
using UserService.Models;
using System.Security.Policy;
using IdentityServer4.Models;
using System;

namespace UserService.Test.IntegrationTests
{
    [TestFixture]
    public class IntegrationTestUserService
    {
        private IntegrationTestingWebAppFactory<Program> _factory;
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new IntegrationTestingWebAppFactory<Program>();


            _httpClient = _factory.CreateClient();
        }

        /// <summary>
        /// Positive integration test when user registers
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Should_Return_True_If_User_Registers_With_Valid_Input()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "Test@email.com",
                FirstName = "TestFirstname",
                City = "TestCity",
                Password = "TestPassword",
                PasswordRepeated = "TestPassword",
                StreetName = "TestStreet",
                ZipCode = "TestZip",
            };

            var serializedCreateUserDto = JsonConvert.SerializeObject(createUserDto);
            var httpContent = new StringContent(serializedCreateUserDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync("api/user", httpContent);
            var result = bool.Parse(await response.Content.ReadAsStringAsync());

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            result.Should().BeTrue();
        }


        /// <summary>
        /// Negative integration test when user enters invalid input
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Should_Return_False_If_User_Registers_With_Valid_Input()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "Test@email.com",
                FirstName = "TestFirstname",
                City = "TestCity",
                Password = "TestPassword",
                PasswordRepeated = "TestPassword5555",
                StreetName = "TestStreet",
                ZipCode = "TestZip",
            };

            var serializedCreateUserDto = JsonConvert.SerializeObject(createUserDto);
            var httpContent = new StringContent(serializedCreateUserDto, Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync("api/user", httpContent);
            var result = JsonConvert.DeserializeObject<ExceptionDto>(await response.Content.ReadAsStringAsync());

            var expected = new ExceptionDto
            {
                StatusCode = 400,
                Message = "Passwords does not match"
            };

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected, options =>
                options.ExcludingMissingMembers()
            );
        }

        /// <summary>
        /// Positive integration test when user enters valid credentials
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Should_Return_Token_If_User_Registers_With_Valid_Input()
        {
            //Arrange
           var dictParams = new Dictionary<string, string>();
            dictParams.Add("client_id", "Gateway");
            dictParams.Add("client_secret", "ec8ea455-541f-410d-ba96-b46587ab7fcc");
            dictParams.Add("grant_type", "password");
            dictParams.Add("scope", "Gateway");
            dictParams.Add("username", "phillip.andersen1999@gmail.com");
            dictParams.Add("password", "P@ris2027!");


            HttpResponseMessage response = _httpClient.PostAsync("connect/token", new FormUrlEncodedContent(dictParams)).Result;
            var token = response.Content.ReadAsStringAsync().Result;

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            token.Should().NotBeNullOrWhiteSpace();
        }


        //public async string GetTokenIdentityServer()
        //{
        //    return "";
        //}
    }
}