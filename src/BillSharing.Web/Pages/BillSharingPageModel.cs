using BillSharing.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BillSharing.Web.Pages;

public abstract class BillSharingPageModel : AbpPageModel
{
    protected BillSharingPageModel()
    {
        LocalizationResourceType = typeof(BillSharingResource);
    }
}
