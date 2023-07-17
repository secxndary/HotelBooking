﻿using Entities.Models;
namespace Contracts.Repositories.UserRepositories;

public interface IRoomRepository
{
    IEnumerable<Room> GetRooms(Guid hotelId, bool trackChanges);
    IEnumerable<Room> GetByIdsForHotel(Guid hotelId, IEnumerable<Guid> ids, bool trackChanges);
    Room? GetRoom(Guid hotelId, Guid id, bool trackChanges);
    Room? GetRoom(Guid id, bool trackChanges);
    void CreateRoomForHotel(Guid hotelId, Room room);
    void DeleteRoom(Room room);
}