using BT.Application.Services.Interface;
using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.News.Command.CreateNewsImage;

public class CreateNewImageCommandHandler : IRequestHandler<CreateNewImageCommand, ApiResponse>
{
    private readonly IUploadService _uploadService;

    public CreateNewImageCommandHandler(IUploadService uploadService)
    {
        _uploadService = uploadService;
    }
    public async ValueTask<ApiResponse> Handle(CreateNewImageCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = await _uploadService.UploadImageAsync(request.Image);

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Image uploaded successfully",
            Data = imageUrl
        };
    }
}