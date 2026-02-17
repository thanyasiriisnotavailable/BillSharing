using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace BillSharing.Expenses;
public class ExpenseItem : Entity<Guid>
{
    private readonly List<ItemSplit> _splits = new();
    public IReadOnlyCollection<ItemSplit> Splits => _splits;

    public Guid ExpenseId { get; private set; }
    public string ItemName { get; private set; }
    public int Unit { get; private set; }
    public decimal TotalAmount { get; private set; }

    private ExpenseItem() { }

    internal ExpenseItem(Guid id, Guid expenseId, string itemName, decimal totalAmount)
    {
        Id = id;
        ExpenseId = expenseId;
        ItemName = itemName;
        TotalAmount = totalAmount;
    }

    public void AddSplit(Guid userId, decimal shareAmount)
    {
        if (shareAmount <= 0)
            throw new BusinessException("Share amount must be positive");

        _splits.Add(new ItemSplit(Guid.NewGuid(), Id, userId, shareAmount));
    }

    public void ValidateSplitTotal()
    {
        if (_splits.Sum(x => x.ShareAmount) != TotalAmount)
            throw new BusinessException("Split total must equal item total");
    }

    public void AddSplitsEqually(IEnumerable<Guid> userIds)
    {
        var users = userIds.Distinct().ToList();

        if (!users.Any())
            throw new BusinessException("At least one user required.");

        Unit = users.Count;

        var share = Math.Round(TotalAmount / Unit, 2);

        decimal totalAssigned = 0;

        for (int i = 0; i < users.Count; i++)
        {
            decimal userShare = share;

            // handle rounding remainder on last user
            if (i == users.Count - 1)
                userShare = TotalAmount - totalAssigned;

            var split = new ItemSplit(
                Guid.NewGuid(),
                Id,
                users[i],
                userShare
            );

            _splits.Add(split);
            totalAssigned += userShare;
        }
    }
}

