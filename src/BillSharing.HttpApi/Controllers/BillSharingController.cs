using BillSharing.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace BillSharing.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BillSharingController : AbpControllerBase
{
    protected BillSharingController()
    {
        LocalizationResource = typeof(BillSharingResource);
    }
}
