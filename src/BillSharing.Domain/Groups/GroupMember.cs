using System;
using Volo.Abp.Domain.Entities;

namespace BillSharing.Groups;
public class GroupMember : Entity<Guid>
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public GroupRole Role { get; set; } = GroupRole.Member;

    public Group Group { get; set; }

    protected GroupMember() { }

    public GroupMember(Guid userId, GroupRole role = GroupRole.Member)
    {
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }
}
