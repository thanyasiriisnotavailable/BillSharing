using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillSharing.Expenses;

public class CreateExpenseDto
{
    [Required]
    public Guid GroupId { get; set; }

    [Required]
    [MaxLength(256)]
    public string Title { get; set; }

    [Required]
    public Guid PaidByUserId { get; set; }

    public DateTime ExpenseDate { get; set; } = DateTime.Now;

    [Required]
    public List<CreateExpenseItemDto> Items { get; set; }
}