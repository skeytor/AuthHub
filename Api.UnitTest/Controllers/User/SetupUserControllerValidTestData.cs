using AuthHub.Api.Dtos;

namespace Api.UnitTest.Controllers.User;

public class SetupUserControllerValidTestData : TheoryData<List<UserResponse>>
{
    public SetupUserControllerValidTestData()
    {
        Add(GenerateFakeData(10));
    }
    private static List<UserResponse> GenerateFakeData(int count)
    {
        List<UserResponse> data = [];
        for (int i = 0; i < count; i++)
        {
            data.Add(new(
                Guid.NewGuid(), 
                Faker.Name.First(), 
                Faker.Name.Last(), 
                Faker.Internet.Email()));
        }
        return data;
    }
}