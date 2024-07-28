namespace Api.UnitTest.Setup.Factories;

internal class TestDataFactory<TImplementation, TType> : ITestDataFactory<TType>
    where TType : class
    where TImplementation : class, ITestData<TType>, new()
{
    public ITestData<TType> Create()
    {
        return new TImplementation();
    }
}
