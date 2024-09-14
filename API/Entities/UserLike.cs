using System;

namespace API.Entities;

public class UserLike
{
    public AppUser SourceUser { get; set;} = null;
    public int SourceUserId { get; set; }
    public AppUser TagetUser { get; set; } = null;
    public int TagetUserId { get; set; }
}
