
using API.Data;
using API.DTOS;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikesRepository repo) : BaseApiController
{
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId) {
        var sourceUserId = User.GetUserId();
        if (sourceUserId == targetUserId) return BadRequest("You cant like yourself");
        var exitstinglike = await repo.GetUserLike(sourceUserId,targetUserId);
        if(exitstinglike == null) {
            var like = new UserLike{
                SourceUserId = sourceUserId,
                TagetUserId = targetUserId,
            };
            repo.AddLike(like);
        }
        else{
            repo.DeleteLike(exitstinglike);
        }
        if(await repo.SaveChanges()) return Ok();
        return BadRequest("fail to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds(){
        return Ok( await repo.GetCurrentUserLikeIds(User.GetUserId()));
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes(string predicate){
        var users = await repo.GetUserLikes(predicate,User.GetUserId());    
        return Ok(users);

    }
}
