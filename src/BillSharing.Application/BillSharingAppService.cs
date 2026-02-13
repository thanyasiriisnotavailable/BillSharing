using BillSharing.Localization;
using Volo.Abp.Application.Services;

namespace BillSharing;

/* Inherit your application services from this class.
 */
public abstract class BillSharingAppService : ApplicationService
{
    protected BillSharingAppService()
    {
        LocalizationResource = typeof(BillSharingResource);
    }
}
