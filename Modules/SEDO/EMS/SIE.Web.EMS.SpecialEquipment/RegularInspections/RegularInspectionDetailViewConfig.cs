using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands;
using System;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 检验明细视图配置
    /// </summary>
    public class RegularInspectionDetailViewConfig : WebViewConfig<RegularInspectionDetail>
    {

        /// <summary>
        /// 自定义添加视图
        /// </summary>
        public readonly static string InputRegularInspection = "InputRegularInspection";

        /// <summary>
        /// 只读视图
        /// </summary>
        public readonly static string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private readonly static int charWidth = 20;

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(InputRegularInspection, ReadonlyView);
            if (ViewGroup == InputRegularInspection)
            {
                InputRegularInspectionView();
            }
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadOnlyView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(RegularInspection));
            View.InlineEdit();
            View.UseCommands("SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.SelModelRegularInspectionCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MinValue).HasLabel("最小值").ShowInList(width: (charWidth * 4));
                View.Property(p => p.MaxValue).HasLabel("最大值").ShowInList(width: (charWidth * 4));
                View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();

                View.Property(p => p.Inspector).HasLabel("检验人").ShowInList(width: (charWidth * 4))
                    .DefaultValue(RT.Service.Resolve<EmployeeController>().GetLoginUserEmployee());

                View.Property(p => p.InspectionDateTime).ShowInList(width: (charWidth * 10))
                    .DefaultValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    .UseDateTimeEditor();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.RegularInspectionValueList).Show(ChildShowInWhere.Hide);
            }
        }


        /// <summary>
        /// 录入检验报告
        /// </summary>
        protected void InputRegularInspectionView()
        {
            View.AssignAuthorize(typeof(RegularInspection));
            View.InlineEdit();
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionDetailBehavior");
            View.UseCommands(RegularInspectionCommands.AllPassCommand, RegularInspectionCommands.ResetResultCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.InspectionResult).HasLabel("检验结果".L10N() + "*").ShowInList(width: (charWidth * 4));
                View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MinValue).HasLabel("最小值").ShowInList(width: (charWidth * 4));
                View.Property(p => p.MaxValue).HasLabel("最大值").ShowInList(width: (charWidth * 4));
                View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();

                View.Property(p => p.Inspector).HasLabel("检验人").ShowInList(width: (charWidth * 4))
                    .DefaultValue(RT.Service.Resolve<EmployeeController>().GetLoginUserEmployee());

                View.Property(p => p.InspectionDateTime).HasLabel("检验日期".L10N() + "*").ShowInList(width: (charWidth * 10))
                    .DefaultValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    .UseDateTimeEditor();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.RegularInspectionValueList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 检验明细只读视图
        /// </summary>
        protected void ConfigReadOnlyView()
        {
            View.AssignAuthorize(typeof(RegularInspection));
            View.ClearCommands();
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionDetailBehavior");
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.InspectionResult).HasLabel("检验结果").ShowInList(width: (charWidth * 4));
                View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MinValue).HasLabel("最小值").ShowInList(width: (charWidth * 4)).Readonly();
                View.Property(p => p.MaxValue).HasLabel("最大值").ShowInList(width: (charWidth * 4)).Readonly();
                View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Inspector).HasLabel("检验人").ShowInList(width: (charWidth * 4)).Readonly();
                View.Property(p => p.InspectionDateTime).ShowInList(width: (charWidth * 10)).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
            View.ChildrenProperty(p => p.RegularInspectionValueList).Show(ChildShowInWhere.Hide);
        }
    }
}