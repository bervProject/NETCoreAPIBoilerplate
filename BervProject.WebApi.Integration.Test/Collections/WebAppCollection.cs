using BervProject.WebApi.Integration.Test.Fixtures;

namespace BervProject.WebApi.Integration.Test.Collections
{
    [CollectionDefinition("Webapp")]
    public class WebAppCollection : ICollectionFixture<WebAppFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
