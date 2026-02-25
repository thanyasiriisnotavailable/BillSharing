using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Linq;
using Volo.Abp.Users;

namespace BillSharing.Groups;
public class GroupPermissionChecker : DomainService
{
    private readonly IRepository<Group, Guid> _groupRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IAsyncQueryableExecuter _asyncExecuter;

    public GroupPermissionChecker(
        IRepository<Group, Guid> groupRepository,
        ICurrentUser currentUser,
        IAsyncQueryableExecuter asyncExecuter)
    {
        _groupRepository = groupRepository;
        _currentUser = currentUser;
        _asyncExecuter = asyncExecuter;
    }

    public async Task CheckMemberAsync(Guid groupId)
    {
        if (_currentUser.IsInRole("admin"))
        {
            return;
        }

        var query = await _groupRepository.WithDetailsAsync(x => x.Members);

        var group = await _asyncExecuter.FirstOrDefaultAsync(
            query.Where(x => x.Id == groupId)
        );

        if (group == null || !group.IsMember(_currentUser.GetId()))
        {
            throw new BusinessException("Group:NotMember");
        }
    }

    public async Task CheckOwnerAsync(Guid groupId)
    {
        if (_currentUser.IsInRole("admin"))
        {
            return;
        }

        var query = await _groupRepository.WithDetailsAsync(x => x.Members);

        var group = await _asyncExecuter.FirstOrDefaultAsync(
            query.Where(x => x.Id == groupId)
        );

        if (group == null || !group.IsOwner(_currentUser.GetId()))
        {
            throw new BusinessException("Group:NotOwner");
        }
    }
}
