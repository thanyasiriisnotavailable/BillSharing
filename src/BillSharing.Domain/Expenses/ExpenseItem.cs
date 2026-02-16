using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace BillSharing.Expenses;
public class ExpenseItem : Entity<Guid>
{
    public Guid ExpenseId { get; set; }

    public string ItemName { get; set; }
    public int Unit { get; set; }
    public decimal TotalAmount { get; set; }

    public Expense Expense { get; set; }
    public ICollection<ItemSplit> Splits { get; set; }
}
