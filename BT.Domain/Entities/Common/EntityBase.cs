using BT.Domain.Entities.Common.Interface;

namespace BT.Domain.Entities.Common;

public class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}