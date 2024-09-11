using System;
using API.DTOS;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUserAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByNameAsync(string username);
    Task<MemberDto?> GetMemberAsync(string username);
    Task<PagedList<MemberDto>> GetAllMemberAsync(UserParams userParams);

   
    
}
