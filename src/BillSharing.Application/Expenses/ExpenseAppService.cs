using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace BillSharing.Expenses;

public class ExpenseAppService : ApplicationService, IExpenseAppService
{
    private readonly IRepository<Expense, Guid> _expenseRepository;
    private readonly IGuidGenerator _guidGenerator;

    public ExpenseAppService(
        IRepository<Expense, Guid> expenseRepository,
        IGuidGenerator guidGenerator)
    {
        _expenseRepository = expenseRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task<ExpenseDto> GetAsync(Guid id)
    {
        var queryable = await _expenseRepository.GetQueryableAsync();

        var expense = await queryable
            .Include(x => x.Items)
                .ThenInclude(i => i.Splits)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (expense == null)
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.ExpenseNotFound
            ).WithData("Id", id);
        }

        return ObjectMapper.Map<Expense, ExpenseDto>(expense);
    }


    public async Task<ExpenseDto> CreateAsync(CreateExpenseDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Title))
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.ExpenseTitleRequired
            );
        }

        if (input.Items == null || !input.Items.Any())
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.ExpenseItemRequired
            );
        }

        var expense = new Expense(
            _guidGenerator.Create(),
            input.GroupId,
            input.Title,
            input.PaidByUserId,
            input.ExpenseDate
        );

        foreach (var itemDto in input.Items)
        {
            if (itemDto.TotalAmount <= 0)
            {
                throw new BusinessException(
                    BillSharingDomainErrorCodes.ExpenseAmountInvalid
                ).WithData("Amount", itemDto.TotalAmount);
            }

            if (itemDto.UserIds == null || !itemDto.UserIds.Any())
            {
                throw new BusinessException(
                    BillSharingDomainErrorCodes.ExpenseSplitUserRequired
                );
            }

            var item = expense.AddItem(
                itemDto.ItemName,
                itemDto.TotalAmount
            );

            item.AddSplitsEqually(itemDto.UserIds);
        }

        await _expenseRepository.InsertAsync(expense, autoSave: true);

        return ObjectMapper.Map<Expense, ExpenseDto>(expense);
    }
}