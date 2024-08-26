using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOS;

public class RegisterDto
{
    [Required]
    public  string Username { get; set; }
    [Required]
    [StringLength(8,MinimumLength =3)]
    public  string Password { get; set; }

}
