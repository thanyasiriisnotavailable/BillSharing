using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillSharing.Expenses;
public class UpdateExpenseDto
{
    [Required]
    [MaxLength(256)]
    public string Title { get; set; }

    [Required]
    public Guid PaidByUserId { get; set; }

    public DateTime ExpenseDate { get; set; }

    [Required]
    public List<UpdateExpenseItemDto> Items { get; set; }
}
