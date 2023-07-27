﻿using Contracts.Repositories.UserRepositories;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using Shared.RequestFeatures.UserParameters;
namespace Repository.UserRepositoriesImpl;

public class RoomPhotoRepository : RepositoryBase<RoomPhoto>, IRoomPhotoRepository
{
    public RoomPhotoRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    { }


    public async Task<PagedList<RoomPhoto>> GetRoomPhotosAsync(Guid roomId, RoomPhotoParameters roomPhotoParameters, bool trackChanges)
    {
        var roomPhotos = await FindByCondition(p => p.RoomId.Equals(roomId), trackChanges)
           .OrderBy(p => p.Path)
           .ToListAsync();

        return PagedList<RoomPhoto>.ToPagedList(roomPhotos, roomPhotoParameters.PageNumber, roomPhotoParameters.PageSize);
    }

    public async Task<IEnumerable<RoomPhoto>> GetByIdsAsync(Guid roomId, IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(p =>
            p.RoomId.Equals(roomId) &&
            ids.Contains(p.Id), trackChanges)
        .OrderBy(p => p.Path)
        .ToListAsync();

    public async Task<RoomPhoto?> GetRoomPhotoAsync(Guid roomId, Guid id, bool trackChanges) =>
        await FindByCondition(p =>
            p.RoomId.Equals(roomId) &&
            p.Id.Equals(id), trackChanges)
        .SingleOrDefaultAsync();

    public void CreateRoomPhoto(Guid roomId, RoomPhoto roomPhoto)
    {
        roomPhoto.RoomId = roomId;
        Create(roomPhoto);
    }

    public void DeleteRoomPhoto(RoomPhoto roomPhoto) =>
        Delete(roomPhoto);
}