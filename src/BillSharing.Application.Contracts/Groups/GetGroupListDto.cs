using Volo.Abp.Application.Dtos;

namespace BillSharing.Groups;
public class GetGroupListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}