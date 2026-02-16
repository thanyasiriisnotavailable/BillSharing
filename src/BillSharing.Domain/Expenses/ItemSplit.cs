using System;
using Volo.Abp.Domain.Entities;

namespace BillSharing.Expenses;
public class ItemSplit : Entity<Guid>
{
    public Guid ExpenseItemId { get; set; }
    public Guid UserId { get; set; }

    public decimal ShareAmount { get; set; }

    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }

    public ExpenseItem ExpenseItem { get; set; }
}