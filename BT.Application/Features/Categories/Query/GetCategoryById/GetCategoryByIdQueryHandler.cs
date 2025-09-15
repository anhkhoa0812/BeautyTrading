using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Models.Categories;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Categories.Query.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetCategoryByIdQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
    }
    public async ValueTask<ApiResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var role = _claimService.GetRole;
        
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.CateogoryId 
                            && (role == nameof(Domain.Enums.ERole.Admin) || x.IsActive)
        );

        if (category == null)
        {
            throw new NotFoundException("Category not found");
        }
        var response = new GetCategoryByIdResponse()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedDate = category.CreatedDate,
            LastModifiedDate = category.LastModifiedDate
        };
        
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Get category successfully",
            Data = response
        };
    }
}