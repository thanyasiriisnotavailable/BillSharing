using BillSharing.Expenses;
using BillSharing.Groups;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DependencyInjection;

namespace BillSharing;
public class BillSharingDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Group, Guid> _groupRepository;
    private readonly IRepository<GroupMember, Guid> _groupMemberRepository;
    private readonly IRepository<Expense, Guid> _expenseRepository;
    private readonly IRepository<ExpenseItem, Guid> _expenseItemRepository;
    private readonly IRepository<ItemSplit, Guid> _itemSplitRepository;

    public BillSharingDataSeederContributor(
        IRepository<Group, Guid> groupRepository,
        IRepository<GroupMember, Guid> groupMemberRepository,
        IRepository<Expense, Guid> expenseRepository,
        IRepository<ExpenseItem, Guid> expenseItemRepository,
        IRepository<ItemSplit, Guid> itemSplitRepository)
    {
        _groupRepository = groupRepository;
        _groupMemberRepository = groupMemberRepository;
        _expenseRepository = expenseRepository;
        _expenseItemRepository = expenseItemRepository;
        _itemSplitRepository = itemSplitRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _groupRepository.GetCountAsync() > 0)
        {
            return;
        }

        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        var userC = Guid.NewGuid();
        var userD = Guid.NewGuid();

        /* -----------------------------
         * Group
         * -----------------------------*/
        var group = await _groupRepository.InsertAsync(
            new Group("Chiang Mai Trip", "Food & travel expense sharing"),
            autoSave: true
        );

        /* -----------------------------
         * Members
         * -----------------------------*/
        var members = new List<GroupMember>
    {
        new() { GroupId = group.Id, UserId = userA, JoinedAt = DateTime.Now },
        new() { GroupId = group.Id, UserId = userB, JoinedAt = DateTime.Now },
        new() { GroupId = group.Id, UserId = userC, JoinedAt = DateTime.Now },
        new() { GroupId = group.Id, UserId = userD, JoinedAt = DateTime.Now }
    };

        foreach (var member in members)
        {
            await _groupMemberRepository.InsertAsync(member, autoSave: true);
        }

        /* -----------------------------
         * Expense — Dinner
         * -----------------------------*/
        var dinner = await _expenseRepository.InsertAsync(
            new Expense
            {
                GroupId = group.Id,
                Title = "Dinner",
                PaidByUserId = userA,
                ExpenseDate = DateTime.Now
            },
            autoSave: true
        );

        var friedRice = await _expenseItemRepository.InsertAsync(
            new ExpenseItem
            {
                ExpenseId = dinner.Id,
                ItemName = "Fried Rice",
                Unit = 4,
                TotalAmount = 200m
            },
            autoSave: true
        );

        var omelette = await _expenseItemRepository.InsertAsync(
            new ExpenseItem
            {
                ExpenseId = dinner.Id,
                ItemName = "Egg Omelette",
                Unit = 2,
                TotalAmount = 80m
            },
            autoSave: true
        );

        var splits = new List<ItemSplit>
    {
        new() { ExpenseItemId = friedRice.Id, UserId = userA, ShareAmount = 50, IsPaid = true,  PaidAt = DateTime.Now },
        new() { ExpenseItemId = friedRice.Id, UserId = userB, ShareAmount = 50, IsPaid = false },
        new() { ExpenseItemId = friedRice.Id, UserId = userC, ShareAmount = 50, IsPaid = false },
        new() { ExpenseItemId = friedRice.Id, UserId = userD, ShareAmount = 50, IsPaid = true,  PaidAt = DateTime.Now },

        new() { ExpenseItemId = omelette.Id, UserId = userA, ShareAmount = 40, IsPaid = true,  PaidAt = DateTime.Now },
        new() { ExpenseItemId = omelette.Id, UserId = userB, ShareAmount = 40, IsPaid = false }
    };

        foreach (var split in splits)
        {
            await _itemSplitRepository.InsertAsync(split, autoSave: true);
        }
    }
}
