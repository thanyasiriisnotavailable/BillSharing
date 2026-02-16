using BillSharing.Expenses;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace BillSharing.Groups;
public class Group : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<GroupMember> Members { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}
