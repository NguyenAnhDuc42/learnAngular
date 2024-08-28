using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOS;

public class RegisterDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    [StringLength(8,MinimumLength =3)]
    public required string Password { get; set; }

}
