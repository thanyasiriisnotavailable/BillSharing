using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using BillSharing.Localization;

namespace BillSharing.Web;

[Dependency(ReplaceServices = true)]
public class BillSharingBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<BillSharingResource> _localizer;

    public BillSharingBrandingProvider(IStringLocalizer<BillSharingResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
