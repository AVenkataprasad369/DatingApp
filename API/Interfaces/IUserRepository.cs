using API.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        // Task<bool> SaveAllAsync(); // Before UnitOfWork
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int Id);
        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        Task<MemberDto> GetMemberAsync(string username);

        // This is added in terms of query optimization for GetUsers method
        Task<string> GetUserGender(string username);
    }
}