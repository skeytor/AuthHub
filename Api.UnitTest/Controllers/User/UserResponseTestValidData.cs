using AuthHub.Api.Dtos;

namespace Api.UnitTest.Controllers.User;

public class UserResponseTestValidData : TheoryData<List<UserResponse>>
{
    public UserResponseTestValidData()
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