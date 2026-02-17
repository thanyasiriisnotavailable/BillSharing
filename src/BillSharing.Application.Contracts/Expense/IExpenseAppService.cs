using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace BillSharing.Expenses;

public interface IExpenseAppService : IApplicationService
{
    Task<ExpenseDto> CreateAsync(CreateExpenseDto input);

    Task<ExpenseDto> GetAsync(Guid id);
}
