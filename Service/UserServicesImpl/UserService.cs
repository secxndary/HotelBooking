﻿using AutoMapper;
using Contracts;
using Contracts.Repository;
using Entities.Exceptions.NotFound;
using Entities.Models;
using Service.Contracts.UserServices;
using Shared.DataTransferObjects.InputDtos;
using Shared.DataTransferObjects.OutputDtos;
using Shared.DataTransferObjects.UpdateDtos;
namespace Service.UserServicesImpl;

public sealed class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UserService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(bool trackChanges)
    {
        var users = await _repository.User.GetAllUsersAsync(trackChanges);
        var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
        return usersDto;
    }

    public async Task<UserDto> GetUserAsync(Guid id, bool trackChanges)
    {
        var user = await _repository.User.GetUserAsync(id, trackChanges);
        if (user is null)
            throw new UserNotFoundException(id);

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<UserDto> CreateUserAsync(UserForCreationDto user)
    {
        var role = await _repository.Role.GetRoleAsync(user.RoleId, trackChanges: false);
        if (role is null)
            throw new RoleNotFoundException(user.RoleId);

        var userEntity = _mapper.Map<User>(user);

        _repository.User.CreateUser(userEntity);
        await _repository.SaveAsync();

        var userToReturn = _mapper.Map<UserDto>(userEntity);
        return userToReturn;
    }

    public async Task UpdateUserAsync(Guid id, UserForUpdateDto userForUpdate, bool trackChanges)
    {
        var role = await _repository.Role.GetRoleAsync(userForUpdate.RoleId, trackChanges: false);
        if (role is null)
            throw new RoleNotFoundException(userForUpdate.RoleId);

        var userEntity = await _repository.User.GetUserAsync(id, trackChanges);
        if (userEntity is null)
            throw new UserNotFoundException(id);

        _mapper.Map(userForUpdate, userEntity);
        await _repository.SaveAsync();
    }

    public async Task<(UserForUpdateDto userToPatch, User userEntity)> GetUserForPatchAsync(Guid id, bool trackChanges)
    {
        var userEntity = await _repository.User.GetUserAsync(id, trackChanges);
        if (userEntity is null)
            throw new UserNotFoundException(id);

        var userToPatch = _mapper.Map<UserForUpdateDto>(userEntity);
        return (userToPatch, userEntity);
    }

    public async Task SaveChangesForPatchAsync(UserForUpdateDto userToPatch, User userEntity)
    {
        var role = await _repository.Role.GetRoleAsync(userToPatch.RoleId, trackChanges: false);
        if (role is null)
            throw new RoleNotFoundException(userToPatch.RoleId);

        _mapper.Map(userToPatch, userEntity);
        await _repository.SaveAsync();
    }

    public async Task DeleteUserAsync(Guid id, bool trackChanges)
    {
        var user = await _repository.User.GetUserAsync(id, trackChanges);
        if (user is null)
            throw new UserNotFoundException(id);

        _repository.User.DeleteUser(user);
        await _repository.SaveAsync();
    }
}