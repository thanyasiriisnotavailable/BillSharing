using System;
using System.Collections.Generic;

namespace BillSharing.Expenses;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string Title { get; set; }
    public Guid PaidByUserId { get; set; }
    public DateTime ExpenseDate { get; set; }

    public List<ExpenseItemDto> Items { get; set; }
}
