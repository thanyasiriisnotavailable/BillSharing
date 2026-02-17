using System;

namespace BillSharing.Expenses;

public class ItemSplitDto
{
    public Guid UserId { get; set; }
    public decimal ShareAmount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
}
