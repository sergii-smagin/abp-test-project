using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomApi.DTOs;

public record CreateBookingRequest(
    [Range(1, int.MaxValue)]
    int RoomId,

    DateTime StartTime,

    DateTime EndTime,

    List<int>? ServiceIds
);