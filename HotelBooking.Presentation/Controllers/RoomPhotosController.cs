﻿using System.Text.Json;
using HotelBooking.Presentation.Filters.ActionFilters;
using HotelBooking.Presentation.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.InputDtos;
using Shared.DataTransferObjects.UpdateDtos;
using Shared.RequestFeatures.UserParameters;
namespace HotelBooking.Presentation.Controllers;

[Route("api/rooms/{roomId:guid}/photos")]
[ApiController]
[Authorize]
public class RoomPhotosController : ControllerBase
{
    private readonly IServiceManager _service;
    public RoomPhotosController(IServiceManager service) => _service = service;


    [HttpGet]
    [HttpHead]
    public async Task<IActionResult> GetRoomPhotos(Guid roomId, [FromQuery] RoomPhotoParameters roomPhotoParameters)
    {
        var (roomPhotos, metaData) = await _service.RoomPhotoService.GetRoomPhotosAsync(roomId, roomPhotoParameters);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));
        return Ok(roomPhotos);
    }

    [HttpGet("{id:guid}", Name = "GetRoomPhotoForRoom")]
    public async Task<IActionResult> GetRoomPhoto(Guid roomId, Guid id)
    {
        var roomPhoto = await _service.RoomPhotoService.GetRoomPhotoAsync(roomId, id);
        return Ok(roomPhoto);
    }

    [HttpGet("collection/({ids})", Name = "RoomPhotoCollection")]
    public async Task<IActionResult> GetRoomPhotoCollection(Guid roomId,
        [ModelBinder(typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var roomPhotos = await _service.RoomPhotoService.GetByIdsAsync(roomId, ids);
        return Ok(roomPhotos);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateRoomPhoto(Guid roomId, [FromBody] RoomPhotoForCreationDto roomPhoto)
    {
        var createdRoomPhoto = await _service.RoomPhotoService.CreateRoomPhotoAsync(roomId, roomPhoto);
        return CreatedAtRoute("GetRoomPhotoForRoom", new { roomId, id = createdRoomPhoto.Id }, createdRoomPhoto);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateRoomPhotoCollection(Guid roomId, 
        [FromBody] IEnumerable<RoomPhotoForCreationDto> roomPhotoCollection)
    {
        var (roomPhotos, ids) = await _service.RoomPhotoService.CreateRoomPhotoCollectionAsync(roomId, roomPhotoCollection);
        return CreatedAtRoute("RoomPhotoCollection", new { roomId, ids }, roomPhotos);
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateRoomPhoto(Guid roomId, Guid id, [FromBody] RoomPhotoForUpdateDto photo)
    {
        var updatedRoomPhoto = await _service.RoomPhotoService.UpdateRoomPhotoAsync(roomId, id, photo);
        return Ok(updatedRoomPhoto);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateRoomPhoto(Guid roomId, Guid id,
        [FromBody] JsonPatchDocument<RoomPhotoForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object is null");

        var (roomPhotoToPatch, roomPhotoEntity) = await _service.RoomPhotoService.GetRoomPhotoForPatchAsync(roomId, id);
        patchDoc.ApplyTo(roomPhotoToPatch);

        TryValidateModel(roomPhotoToPatch);
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var updatedRoomPhoto = await _service.RoomPhotoService.SaveChangesForPatchAsync(roomPhotoToPatch, roomPhotoEntity);
        return Ok(updatedRoomPhoto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoomPhoto(Guid roomId, Guid id)
    {
        await _service.RoomPhotoService.DeleteRoomPhotoAsync(roomId, id);
        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetRoomPhotosOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST");
        return Ok();
    }
}