﻿using HotelBooking.Presentation.ModelBinders;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.InputDtos;
using Shared.DataTransferObjects.UpdateDtos;
namespace HotelBooking.Presentation.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IServiceManager _service;
    public HotelsController(IServiceManager service) => _service = service;


    [HttpGet]
    public async Task<IActionResult> GetHotels()
    {
        var hotels = await _service.HotelService.GetAllHotelsAsync(trackChanges: false);
        return Ok(hotels);
    }

    [HttpGet("collection/({ids})", Name = "HotelCollection")]
    public async Task<IActionResult> GetHotelCollection(
        [ModelBinder(typeof(ArrayModelBinder))]
        IEnumerable<Guid> ids)
    {
        var hotels = await _service.HotelService.GetByIdsAsync(ids, trackChanges: false);
        return Ok(hotels);
    }

    [HttpGet("{id:guid}", Name = "HotelById")]
    public async Task<IActionResult> GetHotel(Guid id)
    {
        var hotel = await _service.HotelService.GetHotelAsync(id, trackChanges: false);
        return Ok(hotel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] HotelForCreationDto hotel)
    {
        if (hotel is null)
            return BadRequest("HotelForCreationDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdHotel = await _service.HotelService.CreateHotelAsync(hotel);
        return CreatedAtRoute("HotelById", new { id = createdHotel.Id }, createdHotel);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateHotelCollection(
        [FromBody] 
        IEnumerable<HotelForCreationDto> hotelCollection)
    {
        var result = await _service.HotelService.CreateHotelCollectionAsync(hotelCollection);
        return CreatedAtRoute("HotelCollection", new { result.ids }, result.hotels);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] HotelForUpdateDto hotel)
    {
        if (hotel is null)
            return BadRequest("HotelForUpdateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.HotelService.UpdateHotelAsync(id, hotel, trackChanges: true);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateHotel(
        Guid id,
        [FromBody] JsonPatchDocument<HotelForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object is null");

        var (hotelToPatch, hotelEntity) = await _service.HotelService.GetHotelForPatchAsync
            (id, trackChanges: true);
        patchDoc.ApplyTo(hotelToPatch);

        TryValidateModel(hotelToPatch);
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.HotelService.SaveChangesForPatchAsync(hotelToPatch, hotelEntity);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await _service.HotelService.DeleteHotelAsync(id, trackChanges: false);
        return NoContent();
    }
}