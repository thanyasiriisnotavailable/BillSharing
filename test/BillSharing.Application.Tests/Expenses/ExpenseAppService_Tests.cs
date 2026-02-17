using BillSharing.Groups;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace BillSharing.Expenses;

public abstract class ExpenseAppService_Tests<TStartupModule>
    : BillSharingApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IExpenseAppService _expenseAppService;
    private readonly IGroupAppService _groupAppService;

    protected ExpenseAppService_Tests()
    {
        _expenseAppService = GetRequiredService<IExpenseAppService>();
        _groupAppService = GetRequiredService<IGroupAppService>();
    }

    /* =====================================================
     * Helper — Get Seeded Group Id
     * =====================================================*/
    private async Task<Guid> GetSeededGroupIdAsync()
    {
        var groups = await _groupAppService.GetListAsync(
            new GetGroupListDto { MaxResultCount = 10 }
        );

        return groups.Items
            .First(g => g.Name == "Chiang Mai Trip")
            .Id;
    }

    /* =====================================================
     * Get Expense By Id
     * =====================================================*/

    [Fact]
    public async Task Should_Get_Expense_By_Id()
    {
        // Arrange
        var groupId = await GetSeededGroupIdAsync();

        var created = await _expenseAppService.CreateAsync(
            new CreateExpenseDto
            {
                GroupId = groupId,
                Title = "Dinner",
                PaidByUserId = Guid.NewGuid(),
                ExpenseDate = DateTime.Now,

                Items = new()
                {
                    new CreateExpenseItemDto
                    {
                        ItemName = "Fried Rice",
                        TotalAmount = 200m,
                        UserIds = new()
                        {
                            Guid.NewGuid(),
                            Guid.NewGuid()
                        }
                    }
                }
            }
        );

        // Act
        var result = await _expenseAppService.GetAsync(created.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(created.Id);
        result.Title.ShouldBe("Dinner");
        result.Items.Count.ShouldBe(1);
        result.Items.First().ItemName.ShouldBe("Fried Rice");
    }

    /* =====================================================
     * Create
     * =====================================================*/

    [Fact]
    public async Task Should_Create_Expense()
    {
        // Arrange
        var groupId = await GetSeededGroupIdAsync();

        var paidByUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var input = new CreateExpenseDto
        {
            GroupId = groupId,
            Title = "Lunch",
            PaidByUserId = paidByUserId,
            ExpenseDate = DateTime.Now,

            Items = new()
            {
                new CreateExpenseItemDto
                {
                    ItemName = "Pad Thai",
                    TotalAmount = 120m,
                    UserIds = new()
                    {
                        paidByUserId,
                        otherUserId
                    }
                }
            }
        };

        // Act
        var result = await _expenseAppService.CreateAsync(input);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Title.ShouldBe("Lunch");
        result.GroupId.ShouldBe(groupId);

        result.Items.Count.ShouldBe(1);

        var item = result.Items.First();
        item.ItemName.ShouldBe("Pad Thai");
        item.TotalAmount.ShouldBe(120m);
    }

    /* =====================================================
     * Validation Tests
     * =====================================================*/

    [Fact]
    public async Task Should_Not_Create_Expense_Without_Items()
    {
        var groupId = await GetSeededGroupIdAsync();

        var input = new CreateExpenseDto
        {
            GroupId = groupId,
            Title = "Invalid Expense",
            PaidByUserId = Guid.NewGuid(),
            Items = null // Required
        };

        await Should.ThrowAsync<Exception>(async () =>
        {
            await _expenseAppService.CreateAsync(input);
        });
    }

    [Fact]
    public async Task Should_Not_Create_Item_With_Zero_Amount()
    {
        var groupId = await GetSeededGroupIdAsync();

        var input = new CreateExpenseDto
        {
            GroupId = groupId,
            Title = "Invalid Item Amount",
            PaidByUserId = Guid.NewGuid(),

            Items = new()
            {
                new CreateExpenseItemDto
                {
                    ItemName = "Free Food",
                    TotalAmount = 0m, // Invalid
                    UserIds = new() { Guid.NewGuid() }
                }
            }
        };

        await Should.ThrowAsync<Exception>(async () =>
        {
            await _expenseAppService.CreateAsync(input);
        });
    }

    [Fact]
    public async Task Should_Not_Create_Item_Without_Users()
    {
        var groupId = await GetSeededGroupIdAsync();

        var input = new CreateExpenseDto
        {
            GroupId = groupId,
            Title = "No Split Users",
            PaidByUserId = Guid.NewGuid(),

            Items = new()
            {
                new CreateExpenseItemDto
                {
                    ItemName = "Pizza",
                    TotalAmount = 300m,
                    UserIds = new() // Required but empty
                }
            }
        };

        await Should.ThrowAsync<Exception>(async () =>
        {
            await _expenseAppService.CreateAsync(input);
        });
    }
}
