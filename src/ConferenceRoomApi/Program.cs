using ConferenceRoomApi.Data;
using ConferenceRoomApi.Domain.Pricing;
using ConferenceRoomApi.Repositories;
using ConferenceRoomApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IReportsService, ReportsService>();

builder.Services.AddScoped<IPricingRule, MorningPricingRule>();
builder.Services.AddScoped<IPricingRule, StandardPricingRule>();
builder.Services.AddScoped<IPricingRule, PeakPricingRule>();
builder.Services.AddScoped<IPricingRule, EveningPricingRule>();

builder.Services.AddScoped<PricingCalculator>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();