namespace Ord.HospitalManagement.Permissions;

public static class HospitalManagementPermissions
{
    public const string GroupName = "HospitalManagement";

    public static class Patient
    {
        public const string Default = GroupName + ".Patient";
    }
}
