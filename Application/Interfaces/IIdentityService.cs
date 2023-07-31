using Application.Identity;
using Domain.DTOs;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResponse> AuthorizeAsync(string username, string password);

    }
}
