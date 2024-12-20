using Volo.Abp.Settings;

namespace Ord.HospitalManagement.Settings;

public class HospitalManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(HospitalManagementSettings.MySetting1));
    }
}
