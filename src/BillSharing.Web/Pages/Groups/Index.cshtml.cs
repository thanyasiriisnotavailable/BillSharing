using BillSharing.Groups;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillSharing.Web.Pages.Groups;

public class IndexModel : PageModel
{
    private readonly IGroupAppService _groupAppService;

    public List<GroupDto> Groups { get; set; }

    public IndexModel(IGroupAppService groupAppService)
    {
        _groupAppService = groupAppService;
    }

    public async Task OnGetAsync()
    {
        var result = await _groupAppService.GetListAsync(
            new GetGroupListDto { MaxResultCount = 100 }
        );

        Groups = result.Items.ToList();
    }
}
