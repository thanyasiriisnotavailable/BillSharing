using BillSharing.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace BillSharing.Permissions;

public class BillSharingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BillSharingPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(BillSharingPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BillSharingResource>(name);
    }
}
