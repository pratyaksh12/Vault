using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vault.Data.Context;
using Vault.Interfaces;
using Vault.Repositories;
using Vault.Index.IServices;
using Vault.Index.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// [NEW] Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin() // [FIX] simpler for dev
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddOpenApi();
builder.Services.AddDbContext<VaultContext>(Options =>
{
    Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped(typeof(IVaultRepository<>), typeof(VaultRepository<>));
var elasticUri = new Uri(builder.Configuration["Elasticsearch:Uri"] ?? "http://localhost:9200");
builder.Services.AddSingleton<IElasticSearchService>(sp => new ElasticSearchService(elasticUri));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseCors("AllowFrontend"); 

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


