using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.News.Command.UpdateNews;

public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IUploadService _uploadService;
    
    public UpdateNewsCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _uploadService = uploadService;
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        var news = await _unitOfWork.GetRepository<Domain.Entities.News>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.NewsId
        );
        if (news == null)
        {
            throw new NotFoundException("News not found");
        }
        
        news.Title = request.Title ?? news.Title;
        news.Content = request.Content ?? news.Content;
        news.IsActive = request.IsActive ?? news.IsActive;
        if (request.Image != null)
        {
            var imageUrl = await _uploadService.UploadImageAsync(request.Image);
            news.ImageUrl = imageUrl;
        }
        _unitOfWork.GetRepository<Domain.Entities.News>().UpdateAsync(news);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Failed to update news with id {NewsId}", request.NewsId);
            throw new Exception("Failed to update news");
        }
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "News updated successfully",
            Data = news.Id
        };
    }
}