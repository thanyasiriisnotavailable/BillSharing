namespace BillSharing.Permissions;

public static class BillSharingPermissions
{
    public const string GroupName = "BillSharing";

    public static class Groups
    {
        public const string Default = GroupName + ".Groups";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Bills
    {
        public const string Default = GroupName + ".Bills";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
