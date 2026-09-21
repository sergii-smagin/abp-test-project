using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomApi.DTOs;

public record AvailableRoomsRequest(
    DateTime StartTime,
    DateTime EndTime,

    [Range(1, int.MaxValue)]
    int Capacity
);