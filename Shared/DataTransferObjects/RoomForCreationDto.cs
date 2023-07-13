﻿namespace Shared.DataTransferObjects;

public record RoomForCreationDto
(
    int Price,
    int Quantity,
    int SleepingPlaces,
    Guid RoomTypeId
);