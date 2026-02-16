using System;

namespace BillSharing.Groups;
public class GroupMemberDto
{
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; }
}

