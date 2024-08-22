using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    // base on the file name i think
    // the http will get the Account in the file AccountController and turn it ToLower(account)
    //it will make a http like this /api/account
    //the /api is the [Route("api/[controller]")] in BaseApiControlle

    public async Task<ActionResult<UserDto>> Regiser(RegisterDto regi)
    {
        if (await UserExist(regi.Username)) return BadRequest("user name already exist");
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = regi.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(regi.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto login)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == login.Username.ToLower());
        if (user == null) return Unauthorized("invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computehash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

        for (int i = 0; i < computehash.Length; i++)
        {
            if (computehash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExist(string username)
    {
        return await context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
    }
}
