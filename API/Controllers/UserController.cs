using System;
using API.Data;
using API.Entities;
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
public class UserController : BaseApiController
{
    private readonly DataContext _context;
    public UserController(DataContext context){
        _context = context;
    }
    [AllowAnonymous]
    [HttpGet] //need to be different every new method because the program
              //will not know which request to get (same Route)
    public async Task<ActionResult<IEnumerable<AppUser>>>  AllUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users;
        //can return more(example:400 error)
    }
    [Authorize]
    [HttpGet("{id:int}")] // ex:/api/user/2 
    public async Task<ActionResult<AppUser>>  Getuser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if(user == null) return NotFound();
        return user;
    }
}
