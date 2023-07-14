﻿namespace Shared.DataTransferObjects.OutputDtos;

public record RoleDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}