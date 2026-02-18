using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace BillSharing.Expenses;

public interface IExpenseAppService : IApplicationService
{
    Task<ExpenseDto> CreateAsync(CreateExpenseDto input);

    Task<ExpenseDto> GetAsync(Guid id);

    Task<List<ExpenseDto>> GetListByGroupIdAsync(Guid groupId);
}
