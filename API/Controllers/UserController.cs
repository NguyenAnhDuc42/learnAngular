
using System.Security.Claims;
using API.DTOS;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// [ApiController]
// [Route("api/[controller]")] //  api/user
// public class UserController : ControllerBase
// now that created a BaseApiController to avoid reapeat code this inherit from it
//                                  ||
[Authorize]
public class UserController(IUserRepository repo,IMapper mapper) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet] //need to be different every new method because the program
              //will not know which request to get (same Route)
    public async Task<ActionResult<IEnumerable<MemberDto>>>  AllUsers()
    {
        var users = await repo.GetAllMemberAsync();

        return Ok(users);
        //can return more(example:400 error)
    }
    [Authorize]
    [HttpGet("{username}")] // ex:/api/user/2 

    // this is mapping
    //  public async Task<ActionResult<MemberDto>>  Getuser(string username)
    // {
    //     var user = await repo.GetUserByNameAsync(username);
    //     if(user == null) return NotFound();
    //      MAPPING
    //     return new MemberDto {   
    //         Id = user.Id,
    //         UserName = user.UserName,    
    //     };
    // }
    public async Task<ActionResult<MemberDto>>  Getuser(string username)
    {
        var user = await repo.GetMemberAsync(username);
        if(user == null) return NotFound();
        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberdto){
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(username == null) return BadRequest("no username found in token");

        var user = await repo.GetUserByNameAsync(username);

        if(user == null) return BadRequest("could not find user");

        mapper.Map(memberdto, user);

        if(await repo.SaveAllAsync()) return NoContent();

        return BadRequest("failed to update user");
    }
}
