using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.News;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.News.Query.GetNewsById;

public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetNewsByIdQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    
    public async ValueTask<ApiResponse> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var role = _claimService.GetRole;
        var news = await _unitOfWork.GetRepository<Domain.Entities.News>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.NewsId && (role == nameof(ERole.Admin) || x.IsActive)
        );
        if (news == null)
        {
            throw new NotFoundException("News not found");
        }

        var response = new GetNewsByIdResponse()
        {
            Id = news.Id,
            Title = news.Title,
            Content = news.Content,
            ImageUrl = news.ImageUrl,
            IsActive = news.IsActive,
            CreatedDate = news.CreatedDate,
            LastModifiedDate = news.LastModifiedDate
        };
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "News retrieved successfully",
            Data = response
        };
    }
}