using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BillSharing.Web.Pages;

[Authorize]
public class IndexModel : BillSharingPageModel
{

}
