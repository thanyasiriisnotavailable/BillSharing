using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace BillSharing.Expenses;
public class Expense : FullAuditedAggregateRoot<Guid>
{
    public Guid GroupId { get; set; }
    public string Title { get; set; }
    public Guid PaidByUserId { get; set; }
    public DateTime ExpenseDate { get; set; }

    public ICollection<ExpenseItem> Items { get; set; }
}
