﻿using System.ComponentModel.DataAnnotations;
using Shared.DataTransferObjects.InputDtos;
namespace Shared.DataTransferObjects.Contracts;

public record HotelForManipulationDto
{
    [Required(ErrorMessage = "Name is a required field.")]
    [MaxLength(50, ErrorMessage = "Maximum length for the Name is 50 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Description is a required field.")]
    [MaxLength(3000, ErrorMessage = "Maximum length for the Description is 3000 characters.")]
    public string? Description { get; init; }

    [Range(1, 5, ErrorMessage = "The stars should be in the range between 1 and 5.")]
    [Required(ErrorMessage = "Stars is a required field.")]
    public int Stars { get; init; }

    public IEnumerable<RoomForCreationDto>? Rooms { get; init; }
}