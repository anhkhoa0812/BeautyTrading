using BT.Application.Common.Exceptions;
using BT.Domain.Entities;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Categories.Command.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public UpdateCategoryCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.CategoryId
        );
        if (category == null)
        {
            throw new NotFoundException("Category not found");
        }
        
        category.Name = request.Name ?? category.Name;
        category.Description = request.Description ?? category.Description;
        category.IsActive = request.IsActive ?? category.IsActive;
        
        _unitOfWork.GetRepository<Category>().UpdateAsync(category);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Update category failed");
            throw new Exception("Update category failed");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Update category successfully",
            Data = category.Id
        };
    }
}