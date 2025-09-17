using BT.Application.Services.Interface;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.News.Command.CreateNews;

public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IUploadService _uploadService;
    public CreateNewsCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _uploadService = uploadService;
    }
    
    public async ValueTask<ApiResponse> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        var news = new Domain.Entities.News()
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            Content = request.Content,
            IsActive = true
        };
        var imageUrl = await _uploadService.UploadImageAsync(request.Image);
        news.ImageUrl = imageUrl;
        
        await _unitOfWork.GetRepository<Domain.Entities.News>().InsertAsync(news);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Failed to create news");
            throw new Exception("Failed to create news");
        }
        return new ApiResponse
        {
            Status = StatusCodes.Status201Created,
            Message = "News created successfully",
            Data = news.Id
        };
    }
}