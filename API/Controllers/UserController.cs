
using API.DTOS;
using API.Entities;
using API.Extensions;
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
public class UserController(IUserRepository repo,IMapper mapper,IPhotoService photoservice) : BaseApiController
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
        // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // if(username == null) return BadRequest("no username found in token");
        // up is old way 
        // new way is to use (User.GetUsername) ad extension ClaimPricible to get username
        var user = await repo.GetUserByNameAsync(User.GetUsername());

        if(user == null) return BadRequest("could not find user");

        mapper.Map(memberdto, user);

        if(await repo.SaveAllAsync()) return NoContent();

        return BadRequest("failed to update user");
    }
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){
        var user = await repo.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("cant update user");

        var result = await photoservice.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);

         var photo = new Photo{
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
         };

         if(user.Photos.Count == 0) photo.IsMain = true;
        user.Photos.Add(photo);
        if(await repo.SaveAllAsync()) return CreatedAtAction(nameof(Getuser), new {username=user.UserName},mapper.Map<PhotoDto>(photo));
        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoid){
        var user = await repo.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("could not find user") ;

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoid);
        if (photo == null || photo.IsMain) return BadRequest("cant use this as main photo");
        var currentMain=user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain= false;
        photo.IsMain=true;

        if(await repo.SaveAllAsync())return NoContent();
        return BadRequest("problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoid}")]
    public async Task<ActionResult> DeletePhoto(int photoid){
        var user = await repo.GetUserByNameAsync(User.GetUsername());
        if(user == null) return BadRequest("user not found") ;
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoid);
        if(photo == null || photo.IsMain) return BadRequest("this photo cant be delete");
        if(photo.PublicId != null){
            var result = await photoservice.DeletePhotoAsync(photo.PublicId) ;
            if (result.Error !=null) return BadRequest(result.Error.Message) ;
        }
        user.Photos.Remove(photo);
        if(await repo.SaveAllAsync()) return NoContent();
        return BadRequest("probliem deleting photo");
    }
}
