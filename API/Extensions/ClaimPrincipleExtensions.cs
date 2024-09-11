using System;
using System.Security.Claims;

namespace API.Extensions;

public  static class ClaimPrincipleExtensions
{
    public static string GetUsername(this ClaimsPrincipal user){
        var username = user.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("Cant get username from token");
        return username;
    }
     public static int GetUserId(this ClaimsPrincipal user){
        var userid =int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cant get username from token"));
        return userid;
    }
}

