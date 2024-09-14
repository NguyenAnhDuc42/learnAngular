using System;
using API.DTOS;
using API.Entities;

namespace API.Interfaces;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(int SourceUserId,int TagetUserId);
    Task<IEnumerable<MemberDto>> GetUserLikes (string predicate,int userid);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
    Task<bool> SaveChanges() ;

}
