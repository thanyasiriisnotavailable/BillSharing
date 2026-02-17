using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillSharing.Expenses;

public class CreateExpenseItemDto
{
    [Required]
    [MaxLength(256)]
    public string ItemName { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    [Required]
    public List<Guid> UserIds { get; set; }
}

