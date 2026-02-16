using BillSharing.Expenses;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BillSharing.Groups;

public class GroupAppService : ApplicationService, IGroupAppService
{
    private readonly IRepository<Group, Guid> _groupRepository;

    public GroupAppService(IRepository<Group, Guid> groupRepository)
    {
        _groupRepository = groupRepository;
    }
    public async Task<GroupDto> GetAsync(Guid id)
    {
        var group = await _groupRepository.GetAsync(id);

        var dto = ObjectMapper.Map<Group, GroupDto>(group);
        dto.MemberCount = group.Members?.Count ?? 0;

        return dto;
    }

    // ----------------------------
    // Get Paged List
    // ----------------------------
    public async Task<PagedResultDto<GroupDto>> GetListAsync(GetGroupListDto input)
    {
        var queryable = await _groupRepository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            queryable = queryable.Where(x => x.Name.Contains(input.Filter));
        }

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        var groups = await AsyncExecuter.ToListAsync(
            queryable
                .OrderBy(x => x.Name)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
        );

        var groupDtos = groups.Select(group =>
        {
            var dto = ObjectMapper.Map<Group, GroupDto>(group);
            dto.MemberCount = group.Members?.Count ?? 0;
            return dto;
        }).ToList();

        return new PagedResultDto<GroupDto>(totalCount, groupDtos);
    }

    // ----------------------------
    // Create
    // ----------------------------
    public async Task<GroupDto> CreateAsync(CreateUpdateGroupDto input)
    {
        var group = new Group(input.Name, input.Description);

        await _groupRepository.InsertAsync(group, autoSave: true);

        return ObjectMapper.Map<Group, GroupDto>(group);
    }

    // ----------------------------
    // Update
    // ----------------------------
    public async Task<GroupDto> UpdateAsync(Guid id, CreateUpdateGroupDto input)
    {
        var group = await _groupRepository.GetAsync(id);

        group.Name = input.Name;
        group.Description = input.Description;

        await _groupRepository.UpdateAsync(group, autoSave: true);

        return ObjectMapper.Map<Group, GroupDto>(group);
    }

    // ----------------------------
    // Delete
    // ----------------------------
    public async Task DeleteAsync(Guid id)
    {
        await _groupRepository.DeleteAsync(id);
    }

    // ----------------------------
    // Regenerate Invite Code
    // ----------------------------
    public async Task<string> RegenerateInviteCodeAsync(Guid id)
    {
        var group = await _groupRepository.GetAsync(id);

        group.GenerateInviteCode();

        await _groupRepository.UpdateAsync(group, autoSave: true);

        return group.InviteCode;
    }
}
