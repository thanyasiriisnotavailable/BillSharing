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