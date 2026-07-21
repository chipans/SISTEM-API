using System.Security.Claims;
using SistemApi.Api.Endpoints.Common;
using SistemApi.Application.Dto.User;
using SistemApi.Application.Services.User;

namespace SistemApi.Api.Endpoints.User;

public static class UserGroupEndpoint
{
    public static void MapUserGroupEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/users").WithTags("Users").RequireAuthorization();

        group.MapGet("/", async (IUserManagementService service) =>
            (await service.GetAllAsync()).ToApiResult())
            .RequireAuthorization("RequireAdmin");
        
        group.MapPut("/{id:int}", async (int id, UpdateUserDto request, IUserManagementService service) =>
            (await service.UpdateAsync(id, request)).ToApiResult())
            .RequireAuthorization("RequireAdmin");

        group.MapPatch("/{id:int}/role", async (int id, ChangeRoleDto request, IUserManagementService service, HttpContext httpContext) =>
        {
            var currentUserId = httpContext.GetCurrentUserId();
            return (await service.ChangeRoleAsync(id, currentUserId, request)).ToApiResult();
        })
        .RequireAuthorization("RequireAdmin");

        group.MapPatch("/{id:int}/activate", async (int id, IUserManagementService service) =>
            (await service.ActivateAsync(id)).ToApiResult())
            .RequireAuthorization("RequireAdmin");

        group.MapPatch("/{id:int}/deactivate", async (int id, IUserManagementService service) =>
            (await service.DeactivateAsync(id)).ToApiResult())
            .RequireAuthorization("RequireAdmin");

        group.MapPatch("/me/password", async (ChangePasswordDto request, IUserManagementService service, HttpContext httpContext) =>
        {
            var currentUserId = httpContext.GetCurrentUserId();
            return (await service.ChangePasswordAsync(currentUserId, request)).ToApiResult();
        });
    }
}