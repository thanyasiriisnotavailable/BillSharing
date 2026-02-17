using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace BillSharing.Groups;

public abstract class GroupAppService_Tests<TStartupModule>
    : BillSharingApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IGroupAppService _groupAppService;

    protected GroupAppService_Tests()
    {
        _groupAppService = GetRequiredService<IGroupAppService>();
    }

    /* =====================================================
     * Get
     * =====================================================*/

    [Fact]
    public async Task Should_Get_Seeded_Group_List()
    {
        // Act
        var result = await _groupAppService.GetListAsync(
            new GetGroupListDto
            {
                SkipCount = 0,
                MaxResultCount = 10
            }
        );

        // Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBeGreaterThan(0);

        result.Items.Any(g => g.Name == "Chiang Mai Trip")
            .ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Get_Seeded_Group_By_Id()
    {
        // Arrange — get seeded group first
        var list = await _groupAppService.GetListAsync(
            new GetGroupListDto { MaxResultCount = 10 }
        );

        var seededGroup = list.Items
            .First(g => g.Name == "Chiang Mai Trip");

        // Act
        var result = await _groupAppService.GetAsync(seededGroup.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(seededGroup.Id);
        result.Name.ShouldBe("Chiang Mai Trip");
    }

    /* =====================================================
     * Create
     * =====================================================*/

    [Fact]
    public async Task Should_Create_Group()
    {
        var input = new CreateUpdateGroupDto
        {
            Name = "Test Group",
            Description = "Test Description"
        };

        var result = await _groupAppService.CreateAsync(input);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("Test Group");
    }

    /* =====================================================
     * Update
     * =====================================================*/

    [Fact]
    public async Task Should_Update_Group()
    {
        // Arrange
        var created = await _groupAppService.CreateAsync(
            new CreateUpdateGroupDto
            {
                Name = "Old Name",
                Description = "Old Description"
            }
        );

        // Act
        var updated = await _groupAppService.UpdateAsync(
            created.Id,
            new CreateUpdateGroupDto
            {
                Name = "New Name",
                Description = "New Description"
            }
        );

        // Assert
        updated.Name.ShouldBe("New Name");
    }

    /* =====================================================
     * Delete
     * =====================================================*/

    [Fact]
    public async Task Should_Delete_Group()
    {
        // Arrange
        var created = await _groupAppService.CreateAsync(
            new CreateUpdateGroupDto
            {
                Name = "Delete Me",
                Description = "Delete Description"
            }
        );

        // Act
        await _groupAppService.DeleteAsync(created.Id);

        // Assert
        await Should.ThrowAsync<Exception>(async () =>
        {
            await _groupAppService.GetAsync(created.Id);
        });
    }

    /* =====================================================
     * Invite Code
     * =====================================================*/

    [Fact]
    public async Task Should_Regenerate_Invite_Code()
    {
        // Arrange
        var created = await _groupAppService.CreateAsync(
            new CreateUpdateGroupDto
            {
                Name = "Invite Test",
                Description = "Test Description"
            }
        );

        // Act
        var code1 = await _groupAppService
            .RegenerateInviteCodeAsync(created.Id);

        var code2 = await _groupAppService
            .RegenerateInviteCodeAsync(created.Id);

        // Assert
        code1.ShouldNotBeNullOrWhiteSpace();
        code2.ShouldNotBeNullOrWhiteSpace();
        code2.ShouldNotBe(code1);
    }
}
