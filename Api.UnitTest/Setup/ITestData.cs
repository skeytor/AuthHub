using Bogus;

namespace Api.UnitTest.Setup;

internal interface ITestData<T> where T : class
{
    T Single();
    List<T> Multiple(int n);
}
