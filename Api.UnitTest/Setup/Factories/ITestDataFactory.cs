namespace Api.UnitTest.Setup.Factories;

internal interface ITestDataFactory<T> where T : class
{
    ITestData<T> Create();
}
