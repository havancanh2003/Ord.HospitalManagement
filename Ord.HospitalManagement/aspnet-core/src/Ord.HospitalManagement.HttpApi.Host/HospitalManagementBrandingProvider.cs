using Microsoft.Extensions.Localization;
using Ord.HospitalManagement.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Ord.HospitalManagement;

[Dependency(ReplaceServices = true)]
public class HospitalManagementBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<HospitalManagementResource> _localizer;

    public HospitalManagementBrandingProvider(IStringLocalizer<HospitalManagementResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
