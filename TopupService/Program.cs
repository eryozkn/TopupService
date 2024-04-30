using Microsoft.EntityFrameworkCore;
using TopupService.DAL;
using TopupService.DAL.Implementations;
using TopupService.DAL.Interfaces;
using TopupService.ExternalService;
using TopupService.Facade.Implementations;
using TopupService.Facade.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TopupDb"))
);

builder.Services
    .AddScoped<ITopupFacade, TopupFacade>()
    .AddScoped<IBeneficiaryFacade, BeneficiaryFacade>()
    .AddScoped<ITopupRepository, TopupRepository>()
    .AddScoped<IBeneficiaryRepository, BeneficiaryRepository>()
    .AddScoped<ITransactionService, TransactionService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
