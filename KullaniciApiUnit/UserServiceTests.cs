using KayitSistemiApi.Models.ViewModel;
using KayitSistemiApi.Models;
using KayitSistemiApi.Repository.Abstraction;
using KayitSistemiApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KullaniciApiUnit
{
    public class UserServiceTests
    {
        private readonly Mock<IGenericRepository<User>> _repositoryMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IGenericRepository<User>>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _httpContextAccessorMock.Setup(_ => _.HttpContext.Request.Scheme).Returns("https");
            _httpContextAccessorMock.Setup(_ => _.HttpContext.Request.Host).Returns(new HostString("localhost"));
            _httpContextAccessorMock.Setup(_ => _.HttpContext.Request.PathBase).Returns(new PathString("/"));

            _userService = new UserService(_repositoryMock.Object, _loggerMock.Object, _httpContextAccessorMock.Object);
        }

       
      

        [Fact]
        public void Delete_ShouldReturnSuccessResult_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User", Email = "test@example.com", Password = "TestPassword" };
            _repositoryMock.Setup(r => r.FindById(1)).Returns(user);

            // Act
            var result = _userService.Delete(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("User successfully deleted.", result.Message);
            Assert.True(result.Data);
        }

       

        [Fact]
        public void GetById_ShouldReturnSuccessResult_WhenUserIsFound()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User", Email = "test@example.com", Password = "TestPassword" };
            _repositoryMock.Setup(r => r.FindById(1)).Returns(user);

            // Act
            var result = _userService.GetById(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("User successfully retrieved.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(user.Id, result.Data.Id);
            Assert.Equal(user.Name, result.Data.Name);
            Assert.Equal(user.Email, result.Data.Email);
        }

      
    }
}
