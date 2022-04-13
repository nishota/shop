using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Services.Interface
{
    public interface IRoleInfoStore
        : IRoleStore<RoleInfo>
    {
    }
}
