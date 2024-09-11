using System;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class UserRepository(DataContext context,IMapper mapper) : IUserRepository
{
    public async Task<PagedList<MemberDto>> GetAllMemberAsync(UserParams userParams)
    {
        var query = context.Users.AsQueryable();

        query = query.Where(x => x.UserName != userParams.CurrentUserName);
        if(userParams.Gender != null){
            query = query.Where(x => x.Gender == userParams.Gender);
        }
        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge -1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
        query = userParams.OrderBy switch
        {
            "create" => query.OrderByDescending(x => x.Created),
            _ =>query.OrderByDescending(x => x.LastActive)
        };

        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider),
         userParams.pageNumber,userParams.PageSize);
    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users.Where(u =>u.UserName == username).ProjectTo<MemberDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<AppUser>> GetUserAsync()
    {
        return await context.Users.Include(p =>p.Photos).ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByNameAsync(string username)
    {
        return await context.Users.Include(p =>p.Photos).FirstOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() >0;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
