using Common.Dto;
using Common.Enums;
using Common.ErrorModels;
using Common.KafkaProducer;
using FluentAssertions;
using Moq;
using UserService.Models;
using UserService.Repository;
using UserService.Services;

namespace UserService.Test.UnitTests
{
    public class UserServiceUnitTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IGenericKafkaProducer> _userProducerMock;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UsersService(_userRepositoryMock.Object);
        }

        /// <summary>
        /// Positive unit tests for creating a new user with test doubles such as dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateUser_Should_Return_When_Password_Identical_Positive()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "Phillip_andersen@hotmail.com",
                City = "TestCity",
                FirstName = "Phillip",
                Password = "MyPassword",
                PasswordRepeated = "MyPassword",
                StreetName = "Tornehegnet",
                ZipCode = "2630"
            };

            var dummyUser = new User { };

            _userRepositoryMock.Setup(_ => _.CreateUser(dummyUser)).ReturnsAsync(true);

            // Act
            var actualMocked = await _userService.CreateUser(createUserDto);
            var expected = true;


            // Assert
            actualMocked.Should().BeTrue();
            // Verify that a mock was called
            _userRepositoryMock.Verify(mock => mock.CreateUser(It.IsAny<User>()), Times.Exactly(1));
        }

        /// <summary>
        /// Negative unit tests for creating a new user when password not identical with test doubles such as dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateUser_Should_Return_When_Password_Not_Identical_Negative()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "Phillip_andersen@hotmail.com",
                City = "TestCity",
                FirstName = "Phillip",
                Password = "MyPassword",
                PasswordRepeated = "MyPassword2",
                StreetName = "Tornehegnet",
                ZipCode = "2630"
            };

            var dummyUser = new User { };

            _userRepositoryMock.Setup(_ => _.CreateUser(dummyUser))
                .ReturnsAsync(false);

            // Act
            bool actualMocked = false;

            Func<Task> act = async () =>
            {
                actualMocked = await _userService.CreateUser(createUserDto);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeFalse();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage("Passwords does not match");
        }


        /// <summary>
        /// Negative unit tests for creating a new user when email already exists with test doubles such as dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateUser_Should_Return_When_Email_Not_Exists_Identical_Negative()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Email = "Phillip_andersen@hotmail.com",
                City = "TestCity",
                FirstName = "Phillip",
                Password = "MyPassword",
                PasswordRepeated = "MyPassword2",
                StreetName = "Tornehegnet",
                ZipCode = "2630"
            };

            var dummyUser = new User { };

            _userRepositoryMock
                .Setup(_ => _.GetUserByEmail("Phillip_andersen@hotmail.com")) // setup assuming email already exists
                .ReturnsAsync(dummyUser);

            // Act
            bool actualMocked = false;

            Func<Task> act = async () =>
            {
                actualMocked = await _userService.CreateUser(createUserDto);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeFalse();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage("User email already exists");
        }

        /// <summary>
        /// Positive unit test for getting user by email when exists with dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserByEmail_Should_Return_User_Based_On_Email_Positive()
        {
            // Arrange
            var email = "Phillip.andersen@hotmail.com";

            var user = new User()
            {
                Email = "Phillip.andersen@hotmail.com",
                FirstName = "Phillip",
                Password = "MyPassword",
            };

            _userRepositoryMock.Setup(_ => _.GetUserByEmail(email)).ReturnsAsync(user);

            // Act
            var actualMocked = await _userService.GetUserByEmail(email);
            var expected = user;

            // Assert
            actualMocked.Should().BeOfType<User>();
            actualMocked.Should().BeEquivalentTo(user, options =>
                options.ExcludingMissingMembers()
            );

            // Verify using mock
            _userRepositoryMock.Verify(mock => mock.GetUserByEmail(It.IsAny<string>()), Times.Exactly(1));
        }

        /// <summary>
        /// Negative unit test for getting user by email when not exists with dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserByEmail_Should_Return_User_Based_On_Email_Already_Exists_Negative()
        {
            // Arrange
            var email = "Phillip.andersen@hotmail.com";


            _userRepositoryMock.Setup(_ => _.GetUserByEmail(email)); // setup assuming email already exists

            // Act
            User actualMocked = null;

            Func<Task> act = async () =>
            {
                actualMocked = await _userService.GetUserByEmail(email);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeNull();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage("User does not exist");
        }

        /// <summary>
        /// Positive unit test for getting user by id with dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserById_Should_Return_User_Based_On_UserId_Positive()
        {
            // Arrange
            var userId = 1;

            var user = new User()
            {
                Email = "Phillip.andersen@hotmail.com",
                FirstName = "Phillip",
                Password = "MyPassword",
            };

            _userRepositoryMock.Setup(_ => _.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var actualMocked = await _userService.GetUserById(userId);
            var expected = user;

            // Assert
            // Assert
            actualMocked.Should().BeOfType<User>();
            actualMocked.Should().BeEquivalentTo(user, options =>
                options.ExcludingMissingMembers()
            );

            // Verify using mock
            _userRepositoryMock.Verify(mock => mock.GetUserById(It.IsAny<int>()), Times.Exactly(1));
        }

        /// <summary>
        /// Negative unit test for getting user by id when not exists with dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserById_Should_Return_User_Based_On_UserId_Negative()
        {
            // Arrange
            var userId = 1;

            _userRepositoryMock.Setup(_ => _.GetUserById(userId)); // setup assuming email already exists

            // Act
            User actualMocked = null;

            Func<Task> act = async () =>
            {
                actualMocked = await _userService.GetUserById(userId);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeNull();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage("User does not exist");
        }

        /// <summary>
        /// Positive unit test for getting role by id with dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserRoleById_Should_Return_User_Based_On_UserId_Positive()
        {
            // Arrange
            var userId = 1;

            var role = new Role
            {
                Id = 1,
                RoleType = RoleTypes.Customer
            };

            _userRepositoryMock.Setup(_ => _.GetUserRoleById(userId)).ReturnsAsync(role);

            // Act
            var actualMocked = await _userService.GetUserRoleById(userId);
            var expected = role;

            // Assert
            // Assert
            actualMocked.Should().BeOfType<Role>();
            actualMocked.Should().BeEquivalentTo(role, options =>
                options.ExcludingMissingMembers()
            );

            // Verify using mock
            _userRepositoryMock.Verify(mock => mock.GetUserRoleById(It.IsAny<int>()), Times.Exactly(1));
        }

        /// <summary>
        /// Positive unit test for getting role by id when user does not exist with dummy objects, stubs and mocks
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetUserRoleById_Should_Return_User_Based_On_UserId_Negative()
        {
            // Arrange
            var userId = 1;

            _userRepositoryMock.Setup(_ => _.GetUserRoleById(userId)); // setup assuming email already exists

            // Act
            Role actualMocked = null;

            Func<Task> act = async () =>
            {
                actualMocked = await _userService.GetUserRoleById(userId);

                await Task.CompletedTask;
            };

            // Assert
            actualMocked.Should().BeNull();
            await act.Should().ThrowAsync<HttpStatusException>()
                .WithMessage("User does not exist");
        }
    }
}