using SIE.CSM.Common;
using SIE.Dock.YardMaintains.Service;
using SIE.Dock.YardMaintains;
using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.CSM._Extentions_;
using SIE.Web.Dock.YardZones.Commands;
using System.Collections.Generic;

namespace SIE.Web.Dock.YardZones
{
	/// <summary>
	/// 园片区维护视图配置
	/// </summary>
	internal class YardZoneViewConfig : WebViewConfig<YardZone>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddAddressCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, typeof(EditAddressCommand).FullName);
            View.UseCommands(typeof(DeleteYardZoneCommand).FullName, typeof(SaveYardZoneCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
			{
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.YardMaintainId).UseDataSource((o, c, r) =>
                {
                    var yardzone = o as YardZone;
                    if (yardzone == null)
                    {
                        return new EntityList<YardMaintain>();
                    }

                    return RT.Service.Resolve<YardMaintainService>().GetEnableYardMaintains(r, c);
                }).UsePagingLookUpEditor((c, p) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(p.Country), nameof(p.YardMaintain.Country));
                    dic.Add(nameof(p.Province), nameof(p.YardMaintain.Province));
                    dic.Add(nameof(p.City), nameof(p.YardMaintain.City));
                    dic.Add(nameof(p.Area), nameof(p.YardMaintain.Area));
                    dic.Add(nameof(p.Address), nameof(p.YardMaintain.Address));
                    c.DicLinkField = dic;
                });
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
                View.Property(p => p.Longitude).UseSpinEditor(p => { p.AllowDecimals = true; p.MinValue = -180; p.MaxValue = 180; p.DecimalPrecision = 6; });
                View.Property(p => p.Latitude).UseSpinEditor(p => { p.AllowDecimals = true; p.MinValue = -90; p.MaxValue = 90; p.DecimalPrecision = 6; });
                View.Property(p => p.Distance).UseSpinEditor(p => { p.AllowDecimals = true; p.MinValue = 0; })
                    .UseListSetting(e => { e.HelpInfo = "0代表没有距离要求"; });
                View.Property(p => p.State).Readonly().DefaultValue(Domain.State.Enable);
                View.Property(p => p.Remark);
                View.ChildrenProperty(p => p.DockHandlingList);
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