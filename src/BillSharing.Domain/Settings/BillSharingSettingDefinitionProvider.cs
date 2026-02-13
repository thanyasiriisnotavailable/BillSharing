using Volo.Abp.Settings;

namespace BillSharing.Settings;

public class BillSharingSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(BillSharingSettings.MySetting1));
    }
}
