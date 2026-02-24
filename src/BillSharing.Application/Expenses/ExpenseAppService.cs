using BillSharing.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace BillSharing.Expenses;

public class ExpenseAppService : ApplicationService, IExpenseAppService
{
    private readonly IRepository<Expense, Guid> _expenseRepository;
    private readonly IRepository<IdentityUser, Guid> _userRepository;
    private readonly IGuidGenerator _guidGenerator;

    public ExpenseAppService(
        IRepository<Expense, Guid> expenseRepository,
        IRepository<IdentityUser, Guid> userRepository,
        IGuidGenerator guidGenerator)
    {
        _expenseRepository = expenseRepository;
        _userRepository = userRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(BillSharingPermissions.Bills.Default)]
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

        var dto = ObjectMapper.Map<Expense, ExpenseDto>(expense);
        await FillUserNamesAsync(dto);
        return dto;
    }

    [Authorize(BillSharingPermissions.Bills.Create)]
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

        expense.MarkPayerSplitsAsPaid(DateTime.UtcNow);

        await _expenseRepository.InsertAsync(expense, autoSave: true);

        var dto = ObjectMapper.Map<Expense, ExpenseDto>(expense);
        await FillUserNamesAsync(dto);
        return dto;
    }

    [Authorize(BillSharingPermissions.Bills.Edit)]
    public async Task<ExpenseDto> UpdateAsync(Guid id, UpdateExpenseDto input)
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

        // Update simple properties
        expense.SetTitle(input.Title);
        expense.SetPaidBy(input.PaidByUserId);
        expense.SetExpenseDate(input.ExpenseDate);

        // Remove existing items
        expense.ClearItems();

        // Re-create items
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

        expense.MarkPayerSplitsAsPaid(DateTime.UtcNow);

        await _expenseRepository.UpdateAsync(expense, autoSave: true);

        var dto = ObjectMapper.Map<Expense, ExpenseDto>(expense);
        await FillUserNamesAsync(dto);

        return dto;
    }

    [Authorize(BillSharingPermissions.Bills.Default)]
    public async Task<List<ExpenseDto>> GetListByGroupIdAsync(Guid groupId)
    {
        var queryable = await _expenseRepository.GetQueryableAsync();

        var expenses = await queryable
            .Where(e => e.GroupId == groupId)
            .Include(e => e.Items)
                .ThenInclude(i => i.Splits)
            .ToListAsync();

        var userIds = expenses
            .SelectMany(e => e.Items)
            .SelectMany(i => i.Splits)
            .Select(s => s.UserId)
            .Distinct()
            .ToList();

        var users = await _userRepository.GetListAsync(u => userIds.Contains(u.Id));

        var userDict = users.ToDictionary(u => u.Id, u => u.UserName);

        var dtos = ObjectMapper.Map<List<Expense>, List<ExpenseDto>>(expenses);
        await FillUserNamesAsync(dtos.ToArray());

        return dtos;
    }


    [Authorize(BillSharingPermissions.Bills.Default)]
    public async Task SetMyPaymentStatusAsync(Guid expenseId, bool isPaid)
    {
        var userId = CurrentUser.GetId();

        var queryable = await _expenseRepository.GetQueryableAsync();

        var expense = await queryable
            .Include(x => x.Items)
                .ThenInclude(i => i.Splits)
            .FirstOrDefaultAsync(x => x.Id == expenseId);

        if (expense == null)
        {
            throw new BusinessException(
                BillSharingDomainErrorCodes.ExpenseNotFound
            );
        }

        foreach (var item in expense.Items)
        {
            item.SetUserSplitPaymentStatus(userId, isPaid, DateTime.Now);
        }

        await _expenseRepository.UpdateAsync(expense, autoSave: true);
    }

    private async Task FillUserNamesAsync(params ExpenseDto[] dtos)
    {
        if (dtos == null || !dtos.Any()) return;

        // Extract all unique user IDs from all splits in all expenses
        var userIds = dtos
            .SelectMany(e => e.Items ?? new())
            .SelectMany(i => i.Splits ?? new())
            .Select(s => s.UserId)
            .Distinct()
            .ToList();

        if (!userIds.Any()) return;

        // Fetch usernames from the identity repository
        var users = await _userRepository.GetListAsync(u => userIds.Contains(u.Id));
        var userDict = users.ToDictionary(u => u.Id, u => u.UserName);

        // Assign names back to the DTOs
        foreach (var dto in dtos)
        {
            foreach (var item in dto.Items ?? new())
            {
                foreach (var split in item.Splits ?? new())
                {
                    split.UserName = userDict.GetValueOrDefault(split.UserId);
                }
            }
        }
    }
}