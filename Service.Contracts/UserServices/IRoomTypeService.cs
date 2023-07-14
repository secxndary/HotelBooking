﻿using Shared.DataTransferObjects.OutputDtos;

namespace Service.Contracts.UserServices;

public interface IRoomTypeService
{
    IEnumerable<RoomTypeDto> GetAllRoomTypes(bool trackChanges);
    RoomTypeDto GetRoomType(Guid id, bool trackChanges);
    void DeleteRoomType(Guid id, bool trackChanges);
}