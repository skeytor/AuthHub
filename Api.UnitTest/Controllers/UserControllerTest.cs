using AuthHub.Api.Controllers;
using AuthHub.Api.Dtos;
using AuthHub.Api.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.UnitTest.Controllers;

public class UserControllerTest
{
    [Fact]
    public async Task GetAll_Should_Return200StatusCode_WhenInvokeUserServiceExactlyOnce()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync([]);
        UserController sut = new(mockUserService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetAll();

        // Assert
        mockUserService
            .Verify(service => service.GetAllUsers(), Times.Once());
        Assert.Equal(200, result.StatusCode);
    }

    [Theory, ClassData(typeof(UserResponseTestData))]
    public async Task GetAll_Should_ReturnUserResponseList_WhenInvokeUserServiceExactrlyOnce(List<UserResponse> fakeUsers)
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(fakeUsers);
        UserController sut = new(mockUserService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetAll();

        // Assert
        var data = (IReadOnlyCollection<UserResponse>)result.Value!;
        mockUserService
            .Verify(service => service.GetAllUsers(), Times.Once());
        Assert.IsAssignableFrom<IReadOnlyList<UserResponse>>(result.Value);
        Assert.NotEmpty(data);
    }

    [Fact]
    public async Task GetAll_Should_ReturnEmptyUserResponseList_WhenInvokeUserService()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync([]);
        UserController sut = new(mockUserService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetAll();

        // Assert
        var data = (IReadOnlyCollection<UserResponse>)result.Value!;
        mockUserService
            .Verify(service => service.GetAllUsers(), Times.Once());
        Assert.Empty(data);
    }
}
public class UserResponseTestData : TheoryData<List<UserResponse>>
{
    public UserResponseTestData()
    {
        Add(GenerateFakeData(10));
    }
    private static List<UserResponse> GenerateFakeData(int nroRecords)
    {
        List<UserResponse> data = [];
        for (int i = 0; i < nroRecords; i++)
        {
            data.Add(new(Guid.NewGuid(), Faker.Name.First(), Faker.Name.Last(), Faker.Internet.Email()));
        }
        return data;
    }
}