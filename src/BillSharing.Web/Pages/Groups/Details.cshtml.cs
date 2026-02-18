using BillSharing.Expenses;
using BillSharing.Groups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillSharing.Web.Pages.Groups;

public class DetailModel : PageModel
{
    private readonly IExpenseAppService _expenseAppService;
    private readonly IGroupAppService _groupAppService;

    public Guid GroupId { get; set; }
    public GroupDto Group { get; set; }
    public List<ExpenseDto> Expenses { get; set; }

    public DetailModel(
        IExpenseAppService expenseAppService,
        IGroupAppService groupAppService)
    {
        _expenseAppService = expenseAppService;
        _groupAppService = groupAppService;
    }

    public async Task OnGetAsync(Guid id)
    {
        GroupId = id;

        Group = await _groupAppService.GetAsync(id);

        Expenses = await _expenseAppService
            .GetListByGroupIdAsync(id);
    }
}
