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
    }
}