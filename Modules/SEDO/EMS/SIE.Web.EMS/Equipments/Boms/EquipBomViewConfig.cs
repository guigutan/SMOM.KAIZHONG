using SIE.Domain;
using SIE.EMS.Equipments.Boms;
using SIE.MetaModel.View;
using SIE.Web.EMS.Equipments.Boms.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Boms
{
	/// <summary>
	/// 设备BOM视图配置
	/// </summary>
	internal class EquipBomViewConfig : WebViewConfig<EquipBom>
	{
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
		{ 	  
			View.UseDefaultCommands();
			View.RemoveCommands(WebCommandNames.Copy);
			View.RemoveCommands(WebCommandNames.ExportXls);
			View.RemoveCommands(WebCommandNames.ExportXlsSelection);
			View.RemoveCommands(WebCommandNames.ExportXlsAll);
			View.UseCommands(typeof(EquipBomCopyCommand).FullName,typeof(ImportBomsCommand).FullName);
			View.Property(p => p.EquipModel).UseDataSource((e, c, r) =>
			{				
				return RT.Service.Resolve<EquipBomController>().GetEquipModels(r, c);
			}).UsePagingLookUpEditor((m, e) =>
			{
				Dictionary<string, string> keyValues = new Dictionary<string, string>();
				keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
				keyValues.Add(nameof(e.EquipTypeName), "Specifications");
				m.DicLinkField = keyValues;
			});
			View.Property(p => p.EquipModelName).Readonly();
			View.Property(p => p.EquipTypeName).Readonly();

			View.AssociateChildrenProperty(EquipBomExtension.EquipBomDetailListProperty, e =>
			{
				var parent = e.Parent as EquipBom;
				if (parent == null) return new EntityList<EquipBomDetail>();
				return RT.Service.Resolve<EquipBomController>().GetEquipBomDetails(parent.Id, null, null);
			}).HasLabel("备件").Show(ChildShowInWhere.All);
		}
    }
}