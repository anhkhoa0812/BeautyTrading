    using BT.Domain.Enums;
    using BT.Domain.Models.Common;
    using Mediator;

    namespace BT.Application.Features.Orders.Query.GetAllOrder;

    public class GetAllOrderQuery : IRequest<ApiResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }