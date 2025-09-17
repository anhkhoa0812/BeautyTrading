namespace BT.Application.Services.Interface;

public interface IUploadService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task<string> UploadVideoAsync(IFormFile file);
}