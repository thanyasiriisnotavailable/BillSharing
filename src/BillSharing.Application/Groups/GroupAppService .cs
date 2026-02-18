using BillSharing.Expenses;
using BillSharing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace BillSharing.Groups;

public class GroupAppService : ApplicationService, IGroupAppService
{
    private readonly IRepository<Group, Guid> _groupRepository;

    public GroupAppService(IRepository<Group, Guid> groupRepository)
    {
        _groupRepository = groupRepository;
    }

    [Authorize(BillSharingPermissions.Groups.Default)]
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
    [Authorize(BillSharingPermissions.Groups.Default)]
    public async Task<PagedResultDto<GroupDto>> GetListAsync(GetGroupListDto input)
    {
        var queryable = await _groupRepository.WithDetailsAsync(x => x.Members);

        if (!CurrentUser.IsInRole("admin"))
        {
            var currentUserId = CurrentUser.Id;

            // Membership filter
            queryable = queryable.Where(group =>
                group.Members.Any(m => m.UserId == currentUserId));
        }

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            queryable = queryable.Where(x => x.Name.Contains(input.Filter));
        }

        var totalCount = await AsyncExecuter.CountAsync(queryable);

        var groups = await AsyncExecuter.ToListAsync(
            queryable
                .OrderBy(x => x.CreationTime)
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
    [Authorize(BillSharingPermissions.Groups.Create)]
    public async Task<GroupDto> CreateAsync(CreateUpdateGroupDto input)
    {
        var existing = await _groupRepository.FirstOrDefaultAsync(x => x.Name == input.Name);

        if (existing != null)
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.GroupNameAlreadyExists
            ).WithData("Name", input.Name);
        }

        var group = new Group(input.Name, input.Description);

        group.AddMember((Guid)CurrentUser.Id);

        await _groupRepository.InsertAsync(group, autoSave: true);

        return ObjectMapper.Map<Group, GroupDto>(group);
    }

    // ----------------------------
    // Update
    // ----------------------------
    [Authorize(BillSharingPermissions.Groups.Edit)]
    public async Task<GroupDto> UpdateAsync(Guid id, CreateUpdateGroupDto input)
    {
        var group = await _groupRepository.GetAsync(id);

        var duplicate = await _groupRepository
        .FirstOrDefaultAsync(x =>
            x.Name == input.Name && x.Id != id
        );

        if (duplicate != null)
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.GroupNameAlreadyExists
            ).WithData("Name", input.Name);
        }

        group.Name = input.Name;
        group.Description = input.Description;

        await _groupRepository.UpdateAsync(group, autoSave: true);

        return ObjectMapper.Map<Group, GroupDto>(group);
    }

    // ----------------------------
    // Delete
    // ----------------------------
    [Authorize(BillSharingPermissions.Groups.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var group = await _groupRepository.FindAsync(id);

        if (group == null)
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.GroupNotFound
            ).WithData("Id", id);
        }

        await _groupRepository.DeleteAsync(id);
    }

    [Authorize] // Any logged-in user can join
    public async Task JoinByInviteCodeAsync(string inviteCode)
    {
        var group = await _groupRepository.WithDetailsAsync(x => x.Members)
            .ContinueWith(t => t.Result.FirstOrDefault(x => x.InviteCode == inviteCode));

        if (group == null)
        {
            throw new UserFriendlyException("Invalid invite code.");
        }

        if (group.InviteCodeExpiry < DateTime.UtcNow)
        {
            throw new UserFriendlyException("This invite code has expired.");
        }

        var currentUserId = CurrentUser.GetId();
        if (group.Members.Any(m => m.UserId == currentUserId))
        {
            throw new UserFriendlyException("You are already a member of this group.");
        }

        group.AddMember(currentUserId);

        await _groupRepository.UpdateAsync(group, autoSave: true);
    }

    // ----------------------------
    // Regenerate Invite Code
    // ----------------------------
    public async Task<string> RegenerateInviteCodeAsync(Guid id)
    {
        var group = await _groupRepository.FindAsync(id);

        if (group == null)
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.GroupNotFound
            ).WithData("Id", id);
        }

        group.GenerateInviteCode();

        await _groupRepository.UpdateAsync(group, autoSave: true);

        return group.InviteCode;
    }
}
