using BillSharing.Expenses;
using BillSharing.Groups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace BillSharing;
public class BillSharingDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Group, Guid> _groupRepository;
    private readonly IRepository<GroupMember, Guid> _groupMemberRepository;
    private readonly IRepository<Expense, Guid> _expenseRepository;
    private readonly IGuidGenerator _guidGenerator;

    public BillSharingDataSeederContributor(
        IRepository<Group, Guid> groupRepository,
        IRepository<GroupMember, Guid> groupMemberRepository,
        IRepository<Expense, Guid> expenseRepository,
        IGuidGenerator guidGenerator)
    {
        _groupRepository = groupRepository;
        _groupMemberRepository = groupMemberRepository;
        _expenseRepository = expenseRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _groupRepository.GetCountAsync() > 0)
            return;

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
        var dinner = new Expense(
            _guidGenerator.Create(),
            group.Id,
            "Dinner",
            userA,
            DateTime.Now
        );

        // Add Items via domain method
        var friedRice = dinner.AddItem("Fried Rice", 200m);
        var omelette = dinner.AddItem("Egg Omelette", 80m);

        // Equal split using UserIds list
        friedRice.AddSplitsEqually(new List<Guid>
        {
            userA, userB, userC, userD
        });

        omelette.AddSplitsEqually(new List<Guid>
        {
            userA, userB
        });

        // Mark some as paid
        foreach (var split in friedRice.Splits)
        {
            if (split.UserId == userA || split.UserId == userD)
                split.MarkAsPaid(DateTime.Now);
        }

        foreach (var split in omelette.Splits)
        {
            if (split.UserId == userA)
                split.MarkAsPaid(DateTime.Now);
        }

        // Persist aggregate root ONLY
        await _expenseRepository.InsertAsync(dinner, autoSave: true);
    }
}
