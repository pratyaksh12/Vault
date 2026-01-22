using System;
using Vault.Index.IServices;
using Vault.Index.Services;

namespace Vault.Tests.Services;

public class ElasticSerachServiceTest
{
    [Fact]
    public void Constructor_ShouldInitializeClient()
    {
        //Arrange
        var uri = new Uri("http://localhost:9200");

        //Act
        var service = new ElasticSearchService(uri);

        //Arrange
        Assert.NotNull(service);
    }
}
