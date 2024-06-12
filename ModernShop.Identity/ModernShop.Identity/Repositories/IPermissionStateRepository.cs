using Identity.API.Models.Dto;

namespace Identity.API.Repositories
{
    public interface IPermissionStateRepository
    {
        Task SaveUserPermissionsState(Guid userId, List<UserPermissionDto> permissions);
        Task<List<UserPermissionDto>> GetUserPermissionsState(Guid userId);
    }
}
