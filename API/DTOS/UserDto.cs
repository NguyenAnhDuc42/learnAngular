using System;

namespace API.DTOS;

public class UserDto
{
    public required string Username { get; set; }
    public required string Token { get; set; }
}
