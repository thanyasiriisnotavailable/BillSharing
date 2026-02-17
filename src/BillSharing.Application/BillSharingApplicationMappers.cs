using BillSharing.Expenses;
using BillSharing.Groups;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace BillSharing;

[Mapper]
public partial class GroupToGroupDtoMapper
    : MapperBase<Group, GroupDto>
{
    public override partial GroupDto Map(Group source);

    public override partial void Map(Group source, GroupDto destination);
}

[Mapper]
public partial class CreateUpdateGroupDtoToGroupMapper
    : MapperBase<CreateUpdateGroupDto, Group>
{
    public override partial Group Map(CreateUpdateGroupDto source);

    public override partial void Map(CreateUpdateGroupDto source, Group destination);
}

[Mapper]
public partial class ExpenseToExpenseDtoMapper
    : MapperBase<Expense, ExpenseDto>
{
    public override partial ExpenseDto Map(Expense source);

    public override partial void Map(Expense source, ExpenseDto destination);
}

[Mapper]
public partial class ExpenseItemToDtoMapper
    : MapperBase<ExpenseItem, ExpenseItemDto>
{
    public override partial ExpenseItemDto Map(ExpenseItem source
    );

    public override partial void Map(ExpenseItem source, ExpenseItemDto destination);
}

[Mapper]
public partial class ItemSplitToDtoMapper
    : MapperBase<ItemSplit, ItemSplitDto>
{
    public override partial ItemSplitDto Map(ItemSplit source);

    public override partial void Map(ItemSplit source, ItemSplitDto destination);
}
