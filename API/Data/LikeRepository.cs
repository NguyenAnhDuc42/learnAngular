using System;
using API.DTOS;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(DataContext context,IMapper mapper) : ILikesRepository
{
    public void AddLike(UserLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
        return await context.Likes.Where(u => u.SourceUserId == currentUserId)
        .Select(x => x.TagetUserId)
        .ToListAsync();
    }

    public async Task<UserLike?> GetUserLike(int SourceUserId, int TagetUserId)
    {
        return await context.Likes.FindAsync(SourceUserId,TagetUserId);
    }

    public async Task<IEnumerable<MemberDto>> GetUserLikes(string predicate, int userid)
    {
        var like = context.Likes.AsQueryable();
        switch (predicate)
        {
            case "liked":
            return await like.Where(x => x.SourceUserId == userid)
                             .Select(x => x.TagetUser)
                             .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                             .ToListAsync();
            case "likedBy":
            return await like.Where(x => x.TagetUserId == userid)
                             .Select(x => x.SourceUser)
                             .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                             .ToListAsync();
            default:
             var likeIds = await GetCurrentUserLikeIds(userid);
             return await like
                                .Where(x => x.TagetUserId == userid && likeIds.Contains(x.SourceUserId))
                                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                                .ToListAsync();
        }
    }

    public async Task<bool> SaveChanges()
    {
       
       return await context.SaveChangesAsync() > 0;
    }
}
