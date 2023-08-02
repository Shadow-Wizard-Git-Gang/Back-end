using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository;
using VC.Data;
using VC.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoDbConfig = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
builder.Services.AddSingleton(mongoDbConfig);

var mongoDbContext = new ApplicationMongoDbContext(mongoDbConfig);
builder.Services.AddSingleton<IMongoDbContext>(mongoDbContext);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<IMongoDbContext>(mongoDbContext)
    .AddDefaultTokenProviders();

// Configure the HTTP request pipeline.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
