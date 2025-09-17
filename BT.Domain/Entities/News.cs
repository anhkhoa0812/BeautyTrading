using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class News : EntityAuditBase<Guid>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
}