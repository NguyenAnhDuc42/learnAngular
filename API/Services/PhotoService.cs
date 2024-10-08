using System;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var  acc = new Account(config.Value.CloudName,config.Value.ApiKey,config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadresult = new ImageUploadResult();
        if(file.Length > 0){
            using var stream = file.OpenReadStream();
            var uploadparams = new ImageUploadParams{
                File = new FileDescription(file.FileName,stream),
                Transformation = new Transformation().Height(500)
                                                     .Width(500)
                                                     .Crop("fill")
                                                     .Gravity("face"),
                Folder ="dotnet-8"
                
            };
            uploadresult =  await _cloudinary.UploadAsync(uploadparams);
        }
        return uploadresult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string photoId)
    {
        var deleteparam = new DeletionParams(photoId);
        return await _cloudinary.DestroyAsync(deleteparam);
    }
}
