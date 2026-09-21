namespace ConferenceRoomApi.Results;

public enum BookingCreateResult
{
    Created,
    RoomNotFound,
    ServiceNotFound,
    DuplicateService,
    InvalidTime,
    OutsideWorkingHours,
    Conflict
}