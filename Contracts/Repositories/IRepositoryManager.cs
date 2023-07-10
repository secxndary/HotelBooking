﻿using Contracts.Repositories;
namespace Contracts.Repository;

public interface IRepositoryManager
{
    IRoleRepository Role { get; }
    IUserRepository User { get; }
    IRoomTypeRepository RoomType { get; }
    IRoomRepository Room { get; }
    IHotelRepository Hotel { get; }
    IReservationRepository Reservation { get; }
    IFeedbackRepository Feedback { get; }
    IRoomPhotoRepository RoomPhoto { get; }
    IHotelPhotoRepository HotelPhoto { get; }
    void Save();
}