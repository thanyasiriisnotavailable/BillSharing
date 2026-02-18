using BillSharing.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace BillSharing.Permissions;

public class BillSharingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(BillSharingPermissions.GroupName, L("Permission:BillSharing"));

        // Groups permissions
        var groupsPermission = group.AddPermission(BillSharingPermissions.Groups.Default, L("Permission:Groups"));
        groupsPermission.AddChild(BillSharingPermissions.Groups.Create, L("Permission:Groups.Create"));
        groupsPermission.AddChild(BillSharingPermissions.Groups.Edit, L("Permission:Groups.Edit"));
        groupsPermission.AddChild(BillSharingPermissions.Groups.Delete, L("Permission:Groups.Delete"));

        // Bills permissions
        var billsPermission = group.AddPermission(BillSharingPermissions.Bills.Default, L("Permission:Bills"));
        billsPermission.AddChild(BillSharingPermissions.Bills.Create, L("Permission:Bills.Create"));
        billsPermission.AddChild(BillSharingPermissions.Bills.Edit, L("Permission:Bills.Edit"));
        billsPermission.AddChild(BillSharingPermissions.Bills.Delete, L("Permission:Bills.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BillSharingResource>(name);
    }
}
