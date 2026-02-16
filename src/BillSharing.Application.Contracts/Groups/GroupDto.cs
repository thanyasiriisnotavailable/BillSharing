using System;
using Volo.Abp.Application.Dtos;

namespace BillSharing.Groups;

public class GroupDto : EntityDto<Guid>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string InviteCode { get; set; }

    public DateTime? InviteCodeExpiry { get; set; }

    public int MemberCount { get; set; }
}