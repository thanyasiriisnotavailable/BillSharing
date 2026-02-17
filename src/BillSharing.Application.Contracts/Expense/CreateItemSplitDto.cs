using System;
using System.ComponentModel.DataAnnotations;

namespace BillSharing.Expenses;

public class CreateItemSplitDto
{
    [Required]
    public Guid UserId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal ShareAmount { get; set; }
}
