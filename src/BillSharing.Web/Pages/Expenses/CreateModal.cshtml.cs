using BillSharing.Expenses;
using BillSharing.Groups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity;

namespace BillSharing.Web.Pages.Expenses;

public class CreateModalModel : AbpPageModel
{
    [BindProperty]
    public CreateExpenseViewModel Expense { get; set; } = new()
    {
        Items = new List<CreateExpenseItemViewModel>()
    };

    public List<SelectListItem> UserSelectList { get; set; }

    private readonly IExpenseAppService _expenseAppService;
    private readonly IGroupAppService _groupAppService;

    public CreateModalModel(IExpenseAppService expenseAppService, IGroupAppService groupAppService)
    {
        _expenseAppService = expenseAppService;
        _groupAppService = groupAppService;
    }

    public async Task OnGetAsync(Guid groupId)
    {
        Expense = new CreateExpenseViewModel
        {
            GroupId = groupId,
            ExpenseDate = DateTime.Now,
            Items = new List<CreateExpenseItemViewModel>
        {
            new()
        }
        };

        var users = await _groupAppService.GetGroupMembersAsync(groupId);

        UserSelectList = users
            .Select(u => new SelectListItem(u.UserName, u.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateExpenseViewModel, CreateExpenseDto>(Expense);

        await _expenseAppService.CreateAsync(dto);

        return NoContent();
    }

    public class CreateExpenseViewModel
    {
        public Guid GroupId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Guid PaidByUserId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public List<CreateExpenseItemViewModel> Items { get; set; }
    }

    public class CreateExpenseItemViewModel
    {
        [Required]
        public string ItemName { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public List<Guid> UserIds { get; set; }
    }
}
