using BT.Application.Services.Interface;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.News;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.News.Query.GetNews;

public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetNewsQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    
    public async ValueTask<ApiResponse> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var role = _claimService.GetRole;

        var newsList = await _unitOfWork.GetRepository<Domain.Entities.News>().GetPagingListAsync(
            selector: x => new GetNewsResponse()
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                ImageUrl = x.ImageUrl,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => role == nameof(ERole.Admin) || x.IsActive,
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? "CreatedDate",
            isAsc: request.IsAsc
        );

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "News retrieved successfully",
            Data = newsList
        };
    }
}