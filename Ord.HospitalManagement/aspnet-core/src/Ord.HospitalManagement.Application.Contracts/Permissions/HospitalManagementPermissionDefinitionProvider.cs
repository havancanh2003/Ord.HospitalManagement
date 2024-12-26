using Ord.HospitalManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Ord.HospitalManagement.Permissions;

public class HospitalManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HospitalManagementPermissions.GroupName, L("Permission:HospitalManagement"));

        var booksPermission = myGroup.AddPermission(HospitalManagementPermissions.Patient.Default, L("Permission:HospitalManagement.Patient"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(HospitalManagementPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HospitalManagementResource>(name);
    }
}
