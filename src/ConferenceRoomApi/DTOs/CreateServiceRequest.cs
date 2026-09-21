using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomApi.DTOs;

public record CreateServiceRequest(
    [Required]
    [MinLength(1)]
    string Name,

    [Range(0.01, double.MaxValue)]
    decimal Price
);