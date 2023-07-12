﻿namespace Shared.DataTransferObjects;

public record RoomTypeDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}