﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
namespace HotelBooking.Presentation.Controllers;

[Route("api/roomTypes")]
[ApiController]
public class RoomTypesController : ControllerBase
{
    private readonly IServiceManager _service;
    public RoomTypesController(IServiceManager service) => _service = service;


    [HttpGet]
    public IActionResult GetRoomTypes()
    {
        var roomTypes = _service.RoomTypeService.GetAllRoomTypes(trackChanges: false);
        return Ok(roomTypes);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetRoomType(Guid id)
    {
        var roomType = _service.RoomTypeService.GetRoomType(id, trackChanges: false);
        return Ok(roomType);
    }
}