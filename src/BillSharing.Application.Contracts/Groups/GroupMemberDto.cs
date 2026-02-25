using System;

namespace BillSharing.Groups;
public class GroupMemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public GroupRole Role { get; set; }
    public DateTime JoinedAt { get; set; }

    public bool IsOwner => Role == GroupRole.Owner;
}

