using BT.Domain.Entities.Common;
using BT.Domain.Enums;

namespace BT.Domain.Entities;

public class Account : EntityAuditBase<Guid>
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public ERole Role { get; set; }

    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
}