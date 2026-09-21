using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomApi.DTOs;

public record CreateRoomRequest(
    [Required]
    [MinLength(1)]
    string Name,

    [Range(1, int.MaxValue)]
    int Capacity,

    [Range(0.01, double.MaxValue)]
    decimal BaseHourlyRate
);