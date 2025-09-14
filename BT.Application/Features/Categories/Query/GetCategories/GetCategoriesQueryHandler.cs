using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Categories;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Categories.Query.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetCategoriesQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    
    public async ValueTask<ApiResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var role = _claimService.GetRole;
        
        var categories = await _unitOfWork.GetRepository<Category>().GetPagingListAsync(
            selector: x => new GetCategoriesResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => role == nameof(ERole.Admin) || x.IsActive,
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? "CreatedDate",
            isAsc: request.IsAsc
        );
        
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Get categories successfully",
            Data = categories
        };
    }
}