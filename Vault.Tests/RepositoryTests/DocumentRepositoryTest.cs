using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Moq;
using Vault.Data.Context;
using Vault.Interfaces;
using Vault.Models;
using Vault.Repositories;
namespace Vault.Tests.RepositoryTests;

public class DocumentRepositoryTest
{
    [Fact]
    public async Task AddAsync_ShouldPersisitDocument_ToInMemoryDb()
    {
        
        //Arrange
        var options = new DbContextOptionsBuilder<VaultContext>().UseInMemoryDatabase(databaseName: "VaultTestDb" + Guid.NewGuid()).Options;
        //seed the data
        using(var context = new VaultContext(options))
        {
            var repository = new VaultRepository<Document>(context);

            var doc = new Document
            {
                Id = "doc-1",
                Path = "/tmp/test.pdf",
                Content = "Test Content",
                ProjectId = "p-1",
                Metadata = "{}",
                ParentId = "root",
                Language = "en"
            };

            await repository.AddAsync(doc);
            await repository.SaveChangesAsync();
        }

        //Assert
        using(var context = new VaultContext(options))
        {
            var repository = new VaultRepository<Document>(context);
            var savedDoc = await repository.GetByIdAsync("doc-1");

            Assert.NotNull(savedDoc);
            Assert.Equal("/tmp/test.pdf", savedDoc.Path);
            Assert.Equal("Test Content", savedDoc.Content);
        }

        
        

    }
}
