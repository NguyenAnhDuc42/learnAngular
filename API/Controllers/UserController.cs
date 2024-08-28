using System;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// [ApiController]
// [Route("api/[controller]")] //  api/user
// public class UserController : ControllerBase
// now that created a BaseApiController to avoid reapeat code this inherit from it
//                                  ||
[Authorize]
public class UserController(IUserRepository repo) : BaseApiController
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
}
