using BillSharing.Expenses;
using BillSharing.Groups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ObjectMapping;

namespace BillSharing.Web.Pages.Expenses
{
    public class EditModalModel : AbpPageModel
    {
        [BindProperty]
        public UpdateExpenseViewModel Expense { get; set; }

        public List<SelectListItem> UserSelectList { get; set; }

        private readonly IExpenseAppService _expenseAppService;
        private readonly IGroupAppService _groupAppService;

        public EditModalModel(
            IExpenseAppService expenseAppService,
            IGroupAppService groupAppService)
        {
            _expenseAppService = expenseAppService;
            _groupAppService = groupAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            var expenseDto = await _expenseAppService.GetAsync(id);

            var users = await _groupAppService
                .GetGroupMembersAsync(expenseDto.GroupId);

            UserSelectList = users
                .Select(u => new SelectListItem(u.UserName, u.UserId.ToString()))
                .ToList();

            Expense = new UpdateExpenseViewModel
            {
                Id = expenseDto.Id,
                GroupId = expenseDto.GroupId,
                Title = expenseDto.Title,
                PaidByUserId = expenseDto.PaidByUserId,
                ExpenseDate = expenseDto.ExpenseDate,
                Items = expenseDto.Items.Select(i => new UpdateExpenseItemViewModel
                {
                    ItemName = i.ItemName,
                    TotalAmount = i.TotalAmount,
                    UserIds = i.Splits.Select(s => s.UserId).ToList()
                }).ToList()
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = ObjectMapper.Map<
                UpdateExpenseViewModel,
                UpdateExpenseDto>(Expense);

            await _expenseAppService.UpdateAsync(Expense.Id, dto);

            return NoContent();
        }

        public class UpdateExpenseViewModel
        {
            public Guid Id { get; set; }
            public Guid GroupId { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public Guid PaidByUserId { get; set; }

            public DateTime ExpenseDate { get; set; }

            public List<UpdateExpenseItemViewModel> Items { get; set; }
        }

        public class UpdateExpenseItemViewModel
        {
            [Required]
            public string ItemName { get; set; }

            [Range(0.01, double.MaxValue)]
            public decimal TotalAmount { get; set; }

            [Required]
            public List<Guid> UserIds { get; set; }
        }
    }
}
