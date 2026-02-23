using System;
using System.Collections.Generic;

namespace BillSharing.Expenses;

public class ExpenseItemDto
{
    public Guid Id { get; set; }
    public string ItemName { get; set; }
    public int Unit { get; set; }
    public decimal TotalAmount { get; set; }

    public List<ItemSplitDto> Splits { get; set; }
}
