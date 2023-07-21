﻿using HotelBooking.Presentation.ModelBinders;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.InputDtos;
using Shared.DataTransferObjects.UpdateDtos;
namespace HotelBooking.Presentation.Controllers;

[Route("api/rooms/{roomId:guid}/photos")]
[ApiController]
public class RoomPhotosController : ControllerBase
{
    private readonly IServiceManager _service;
    public RoomPhotosController(IServiceManager service) => _service = service;


    [HttpGet]
    public IActionResult GetRoomPhotos(Guid roomId)
    {
        var roomPhotos = _service.RoomPhotoService.GetRoomPhotos(roomId, trackChanges: false);
        return Ok(roomPhotos);
    }

    [HttpGet("{id:guid}", Name = "GetRoomPhotoForRoom")]
    public IActionResult GetRoomPhoto(Guid roomId, Guid id)
    {
        var roomPhoto = _service.RoomPhotoService.GetRoomPhoto(roomId, id, trackChanges: false);
        return Ok(roomPhoto);
    }

    [HttpGet("collection/({ids})", Name = "RoomPhotoCollection")]
    public IActionResult GetRoomPhotoCollection(
        Guid roomId,
        [ModelBinder(typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var roomPhotos = _service.RoomPhotoService.GetByIds(roomId, ids, trackChanges: false);
        return Ok(roomPhotos);
    }

    [HttpPost]
    public IActionResult CreateRoomPhoto(Guid roomId, [FromBody] RoomPhotoForCreationDto roomPhoto)
    {
        if (roomPhoto is null)
            return BadRequest("RoomPhotoForCreationDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdRoomPhoto = _service.RoomPhotoService.CreateRoomPhoto(roomId, roomPhoto, trackChanges: false);
        return CreatedAtRoute("GetRoomPhotoForRoom", new { roomId, id = createdRoomPhoto.Id }, createdRoomPhoto);
    }

    [HttpPost("collection")]
    public IActionResult CreateRoomPhotoCollection(
        Guid roomId,
        [FromBody] IEnumerable<RoomPhotoForCreationDto> roomPhotoCollection)
    {
        var result = _service.RoomPhotoService.CreateRoomPhotoCollection(roomId, roomPhotoCollection);
        return CreatedAtRoute("RoomPhotoCollection", new { roomId, result.ids }, result.roomPhotos);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateRoomPhoto(Guid roomId, Guid id, [FromBody] RoomPhotoForUpdateDto photo)
    {
        if (photo is null)
            return BadRequest("RoomPhotoForUpdateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _service.RoomPhotoService.UpdateRoomPhoto(roomId, id, photo,
            roomTrackChanges: false, photoTrackChanges: true);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateRoomPhoto(Guid roomId, Guid id,
        [FromBody] JsonPatchDocument<RoomPhotoForUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object is null");

        var (roomPhotoToPatch, roomPhotoEntity) = _service.RoomPhotoService.GetRoomPhotoForPatch
            (roomId, id, roomTrackChanges: false, photoTrackChanges: true);
        patchDoc.ApplyTo(roomPhotoToPatch);

        TryValidateModel(roomPhotoToPatch);
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _service.RoomPhotoService.SaveChangesForPatch(roomPhotoToPatch, roomPhotoEntity);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteRoomPhoto(Guid roomId, Guid id)
    {
        _service.RoomPhotoService.DeleteRoomPhoto(roomId, id, trackChanges: false);
        return NoContent();
    }
}