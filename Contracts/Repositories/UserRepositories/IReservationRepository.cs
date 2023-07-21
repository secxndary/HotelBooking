﻿using Entities.Models;
namespace Contracts.Repositories.UserRepositories;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetReservationsAsync(Guid roomId, bool trackChanges);
    Task<Reservation?> GetReservationAsync(Guid roomId, Guid id, bool trackChanges);
    Task<Reservation?> GetReservationAsync(Guid id, bool trackChanges);
    void CreateReservationForRoom(Guid roomId,  Reservation reservation);
    void DeleteReservation(Reservation reservation);
}