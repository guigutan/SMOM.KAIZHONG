using SIE.Equipments.EquipFaults;
using SIE.MetaModel.View;
using SIE.Web.Equipments.EquipFaults.Commands;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipFaults
{
    /// <summary>
    /// 设备故障与系统缺陷对应关系视图配置
    /// </summary>
    internal class EquipFaultAndDefectViewConfig : WebViewConfig<EquipFaultAndDefect>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.InlineEdit().UseCommand(typeof(EquipFaultAndDefectImportCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Equipments.EquipFaults.Commands.EquipFaultAndDefectCopyCommand");
            View.Property(p => p.EquipModelId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.EquipModelName).Readonly();
            View.Property(p => p.EquipBadCode);
            View.Property(p => p.EquipDefectName);
            View.Property(p => p.DefectId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.DefectDesc), nameof(e.Defect.Description));
                keyValues.Add(nameof(e.DefectCategoryCode), nameof(e.Defect.DefectCategory.Code));
                keyValues.Add(nameof(e.DefectCategoryDesc), nameof(e.Defect.DefectCategory.Description));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.DefectDesc).Readonly();
            View.Property(p => p.DefectCategoryCode).Readonly();
            View.Property(p => p.DefectCategoryDesc).Readonly();
        }

        protected override void ConfigSelectionView()
        {
            View.Property(p => p.EquipModelId);
            View.Property(p => p.EquipModelName);
            View.Property(p => p.EquipBadCode);
            View.Property(p => p.EquipDefectName);
            View.Property(p => p.DefectId);
            View.Property(p => p.DefectDesc);
            View.Property(p => p.DefectCategoryCode);
            View.Property(p => p.DefectCategoryDesc);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }

		protected override void ConfigImportView()
		{
            View.Property(p => p.EquipModelId).HasLabel("设备型号");
            View.Property(p => p.EquipModelName).HasLabel("型号名称");
            View.Property(p => p.EquipBadCode).HasLabel("设备不良代码*");
            View.Property(p => p.EquipDefectName).HasLabel("设备缺陷名称");
            View.Property(p => p.DefectId).HasLabel("缺陷代码*");
            View.Property(p => p.DefectDesc).HasLabel("缺陷描述");
            View.Property(p => p.DefectCategoryCode).HasLabel("缺陷分类代码");
            View.Property(p => p.DefectCategoryDesc).HasLabel("缺陷分类描述");
        }
	}
}