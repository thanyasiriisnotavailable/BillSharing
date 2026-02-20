using BillSharing.Expenses;
using BillSharing.Groups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace BillSharing.Web.Pages.Groups;

public class DetailModel : PageModel
{
    private readonly IExpenseAppService _expenseAppService;
    private readonly IGroupAppService _groupAppService;
    private readonly ICurrentUser _currentUser;

    public Guid GroupId { get; set; }
    public GroupDto Group { get; set; }
    public List<ExpenseDto> Expenses { get; set; }
    public Guid? CurrentUserId { get; set; }

    public DetailModel(
        IExpenseAppService expenseAppService,
        IGroupAppService groupAppService,
        ICurrentUser currentUser)
    {
        _expenseAppService = expenseAppService;
        _groupAppService = groupAppService;
        _currentUser = currentUser;
    }

    public async Task OnGetAsync(Guid id)
    {
        GroupId = id;
        CurrentUserId = _currentUser.GetId();

        Group = await _groupAppService.GetAsync(id);

        Expenses = await _expenseAppService
            .GetListByGroupIdAsync(id);
    }
}
