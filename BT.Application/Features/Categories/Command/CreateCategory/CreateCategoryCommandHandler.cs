using BT.Domain.Entities;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Categories.Command.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public CreateCategoryCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async ValueTask<ApiResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };
        await _unitOfWork.GetRepository<Category>().InsertAsync(category);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Create category failed");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Create category successfully",
            Data = category.Id
        };
    }
}