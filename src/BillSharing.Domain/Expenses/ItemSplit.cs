using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace BillSharing.Expenses;
public class ItemSplit : Entity<Guid>
{
    public Guid ExpenseItemId { get; private set; }
    public Guid UserId { get; private set; }
    public decimal ShareAmount { get; private set; }

    public bool IsPaid { get; private set; }
    public DateTime? PaidAt { get; private set; }

    private ItemSplit() { }

    internal ItemSplit(Guid id, Guid expenseItemId, Guid userId, decimal shareAmount)
        : base(id)
    {
        if (shareAmount <= 0)
            throw new BusinessException("Share amount must be positive.");

        ExpenseItemId = expenseItemId;
        UserId = userId;
        ShareAmount = shareAmount;

        IsPaid = false;
    }

    public void MarkAsPaid(DateTime paidAt)
    {
        if (IsPaid)
            throw new BusinessException("Split already paid.");

        IsPaid = true;
        PaidAt = paidAt;
    }

    public void MarkAsUnpaid()
    {
        if (!IsPaid)
            return;

        IsPaid = false;
        PaidAt = null;
    }
}
