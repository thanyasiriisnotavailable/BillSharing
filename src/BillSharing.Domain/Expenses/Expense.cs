using BillSharing.Expenses;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

public class Expense : FullAuditedAggregateRoot<Guid>
{
    private readonly List<ExpenseItem> _items = new();
    public IReadOnlyCollection<ExpenseItem> Items => _items;

    public Guid GroupId { get; private set; }
    public string Title { get; private set; }
    public Guid PaidByUserId { get; private set; }
    public DateTime ExpenseDate { get; private set; }

    private Expense() { }

    public Expense(Guid id, Guid groupId, string title, Guid paidByUserId, DateTime date)
        : base(id)
    {
        GroupId = groupId;
        Title = title;
        PaidByUserId = paidByUserId;
        ExpenseDate = date;
    }

    public ExpenseItem AddItem(string name, decimal totalAmount)
    {
        var item = new ExpenseItem(Guid.NewGuid(), Id, name, totalAmount);
        _items.Add(item);
        return item;
    }

    public void MarkPayerSplitsAsPaid(DateTime paidAt)
    {
        foreach (var item in _items)
        {
            item.MarkUserSplitAsPaid(PaidByUserId, paidAt);
        }
    }
}