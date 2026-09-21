using ConferenceRoomApi.Domain;
using ConferenceRoomApi.Domain.Pricing;
using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Repositories;
using ConferenceRoomApi.Results;

namespace ConferenceRoomApi.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly PricingCalculator _pricingCalculator;

    public BookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IServiceRepository serviceRepository,
        PricingCalculator pricingCalculator)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _serviceRepository = serviceRepository;
        _pricingCalculator = pricingCalculator;
    }

    public async Task<BookingCreateResult> CreateAsync(CreateBookingRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId);

        if (room is null)
        {
            return BookingCreateResult.RoomNotFound;
        }

        if (request.ServiceIds is not null &&
            request.ServiceIds.Count != request.ServiceIds.Distinct().Count())
        {
            return BookingCreateResult.DuplicateService;
        }    

        var serviceIds = request.ServiceIds ?? [];

        var services = new List<Service>();

        foreach (var serviceId in serviceIds)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);

            if (service is null)
            {
                return BookingCreateResult.ServiceNotFound;
            }

            services.Add(service);
        }

        if (request.EndTime <= request.StartTime)
        {
            return BookingCreateResult.InvalidTime;
        }

        var openingTime = TimeSpan.FromHours(6);
        var closingTime = TimeSpan.FromHours(23);

        if (request.StartTime.TimeOfDay < openingTime ||
            request.EndTime.TimeOfDay > closingTime)
        {
            return BookingCreateResult.OutsideWorkingHours;
        }

        var overlappingBookings = await _bookingRepository.GetOverlappingAsync(
            request.RoomId,
            request.StartTime,
            request.EndTime);

        if (overlappingBookings.Count > 0)
        {
            return BookingCreateResult.Conflict;
        }

        var bookingInterval = new TimeInterval(
            request.StartTime,
            request.EndTime);

        var totalPrice = _pricingCalculator.Calculate(
            bookingInterval,
            room.BaseHourlyRate);

        var servicesTotal = services.Sum(service => service.Price);

        totalPrice += servicesTotal;

        var booking = new Booking
        {
            RoomId = request.RoomId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TotalPrice = totalPrice
        };

        var serviceLinks = services
            .Select(service => new BookingServiceLink
            {
                BookingId = booking.Id,
                ServiceId = service.Id,
                Price = service.Price
            })
            .ToList();

        await _bookingRepository.AddAsync(
            booking,
            serviceLinks);

        return BookingCreateResult.Created;
    }
    
    public async Task<List<BookingResponse>> GetAllAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();

        return bookings
            .Select(booking => new BookingResponse(
                booking.Id,
                booking.RoomId,
                booking.StartTime,
                booking.EndTime,
                booking.TotalPrice,
                booking.ServiceLinks
                    .Select(link => new BookingServiceResponse(
                        link.ServiceId,
                        link.Service.Name,
                        link.Price))
                    .ToList()))
            .ToList();
    }
}