using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using BillSharing.Groups;

namespace BillSharing.Expenses;
public interface IGroupAppService : IApplicationService
{
    Task<GroupDto> GetAsync(Guid id);

    Task<PagedResultDto<GroupDto>> GetListAsync(GetGroupListDto input);

    Task<GroupDto> CreateAsync(CreateUpdateGroupDto input);

    Task<GroupDto> UpdateAsync(Guid id, CreateUpdateGroupDto input);

    Task DeleteAsync(Guid id);

    Task<string> RegenerateInviteCodeAsync(Guid id);
}
