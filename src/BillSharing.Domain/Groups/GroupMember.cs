using System;
using Volo.Abp.Domain.Entities;

namespace BillSharing.Groups;
public class GroupMember : Entity<Guid>
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public Group Group { get; set; }
}
