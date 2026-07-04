using SIE.Fixtures.Models;
using SIE.Web.Core.Common.Commands;
using SIE.Web.Fixtures.Models.Commands;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
	/// 设备清单视图配置
	/// </summary>
	internal class FixtureModelEquipDetailViewConfig : WebViewConfig<FixtureModelEquipDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(SelEquipModelCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
            View.UseCommands(typeof(FixtureModelEquipImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.EquipModelCode).Readonly();
            View.Property(p => p.EquipModelName).Readonly();
        }

        /// <summary>
        /// 导入
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.FixtureModel.Code).HasLabel("工治具型号编码");
            View.Property(p => p.EquipModelCode).HasLabel("设备型号编码");
        }
    }
}
