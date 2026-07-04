using SIE.CSM.Common;
using SIE.Dock.YardMaintains;
using SIE.MetaModel.View;
using SIE.Web.CSM._Extentions_;
using SIE.Web.Dock.YardMaintains.Commands;

namespace SIE.Web.Dock.YardMaintains
{
    /// <summary>
    /// 园区维护视图配置
    /// </summary>
    internal class YardMaintainViewConfig : WebViewConfig<YardMaintain>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddAddressCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, typeof(EditAddressCommand).FullName);
            View.UseCommands(typeof(DeleteYardMaintainCommand).FullName, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
			{
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Country).UseDropDownEditor(() => RT.Service.Resolve<RegionalInfoController>().GetRegionDic(null, null)).ShowInList(width: 60);
                View.Property(p => p.Province).UseSupplierProvinceEditor(2, p =>
                {
                    p.Editable = false;
                    p.ValueField = RegionalInfo.RegionProperty.Name;
                }).UseListSetting(e => { e.HelpInfo = "显示选择国家的省份"; }).ShowInList(width: 80);
                View.Property(p => p.City).UseSupplierProvinceEditor(3, p =>
                {
                    p.Editable = false;
                    p.ValueField = RegionalInfo.RegionProperty.Name;
                }).UseListSetting(e => { e.HelpInfo = "显示选择省份的地级市"; }).ShowInList(width: 80);
                View.Property(p => p.Area).UseSupplierProvinceEditor(4, p =>
                {
                    p.Editable = false;
                    p.ValueField = RegionalInfo.RegionProperty.Name;
                }).UseListSetting(e => { e.HelpInfo = "显示选择地级市的城区"; }).ShowInList(width: 80);
                View.Property(p => p.Address);
                View.Property(p => p.State).Readonly().DefaultValue(Domain.State.Enable);
                View.Property(p => p.Remark);
            }
		}

        ///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
    }
}