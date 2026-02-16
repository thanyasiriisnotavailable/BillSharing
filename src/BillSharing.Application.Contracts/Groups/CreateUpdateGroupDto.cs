using System.ComponentModel.DataAnnotations;

namespace BillSharing.Groups;

public class CreateUpdateGroupDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
}
