using ConferenceRoomApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<BookingServiceLink> BookingServiceLinks => Set<BookingServiceLink>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasOne<Room>()
            .WithMany()
            .HasForeignKey(booking => booking.RoomId);

        modelBuilder.Entity<BookingServiceLink>()
            .HasOne<Booking>()
            .WithMany()
            .HasForeignKey(bookingService => bookingService.BookingId);

        modelBuilder.Entity<BookingServiceLink>()
            .HasOne<Booking>()
            .WithMany(booking => booking.ServiceLinks)
            .HasForeignKey(bookingService => bookingService.BookingId);

        modelBuilder.Entity<BookingServiceLink>()
            .HasKey(bookingService => new
            {
                bookingService.BookingId,
                bookingService.ServiceId
            });

        modelBuilder.Entity<Room>().HasData(
        new Room
        {
            Id = 1,
            Name = "Room A",
            Capacity = 50,
            BaseHourlyRate = 2000
        },
        new Room
        {
            Id = 2,
            Name = "Room B",
            Capacity = 100,
            BaseHourlyRate = 3500
        },
        new Room
        {
            Id = 3,
            Name = "Room C",
            Capacity = 30,
            BaseHourlyRate = 1500
        });

    modelBuilder.Entity<Service>().HasData(
        new Service
        {
            Id = 1,
            Name = "Projector",
            Price = 500
        },
        new Service
        {
            Id = 2,
            Name = "Wi-Fi",
            Price = 300
        },
        new Service
        {
            Id = 3,
            Name = "Sound",
            Price = 700
        });
    }
}