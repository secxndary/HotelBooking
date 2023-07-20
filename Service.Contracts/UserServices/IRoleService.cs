﻿using Entities.Models;
using Shared.DataTransferObjects.InputDtos;
using Shared.DataTransferObjects.OutputDtos;
using Shared.DataTransferObjects.UpdateDtos;
namespace Service.Contracts.UserServices;

public interface IRoleService
{
    IEnumerable<RoleDto> GetAllRoles(bool trackChanges);
    RoleDto GetRole(Guid id, bool trackChanges);
    RoleDto CreateRole(RoleForCreationDto role);
    void UpdateRole(Guid id, RoleForUpdateDto role, bool trackChanges);
    (RoleForUpdateDto roleToPatch, Role roleEntity) GetRoleForPatch(Guid id, bool trackChanges);
    void SaveChangesForPatch(RoleForUpdateDto roleToPatch, Role roleEntity);
    void DeleteRole(Guid id, bool trackChanges);
}