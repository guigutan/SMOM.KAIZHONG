using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.EMS.Equipments.Accounts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备型号润滑项目视图配置
    /// </summary>
    internal class EquipAccountLubricationProjectViewConfig : WebViewConfig<EquipAccountLubricationProject>
    {
        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private const int charWidth = 20;
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipAccount));
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(0.71, 0.29);
            View.ClearCommands();
            View.AddBehavior("SIE.Web.EMS.Equipments.Scripts.AddEquipAccountLubricationProjectBehavior");
            View.UseCommands("SIE.Web.EMS.Equipments.Accounts.Commands.EquipAccountLubricationProjectAddCommand", 
                WebCommandNames.Edit, 
                WebCommandNames.Delete,
                typeof(EquipAccountLubricationProjectSaveCommand).FullName);
            


            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectDetailId).UseDataSource((e, page, code) =>
                {
                    return RT.Service.Resolve<ProjectDetailController>().GetProjectDetailByProjectType(ProjectType.Lubrication, page, code);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.CycleType), nameof(e.ProjectDetail.CycleType));
                    keyValues.Add(nameof(e.MinValue), nameof(e.ProjectDetail.MinValue));
                    keyValues.Add(nameof(e.MaxValue), nameof(e.ProjectDetail.MaxValue));
                    keyValues.Add(nameof(e.Consumable), nameof(e.ProjectDetail.Consumable));
                    keyValues.Add(nameof(e.Method), nameof(e.ProjectDetail.Method));
                    keyValues.Add(nameof(e.Part), nameof(e.ProjectDetail.Part));
                    keyValues.Add(nameof(e.Standard), nameof(e.ProjectDetail.Standard));
                    keyValues.Add(nameof(e.Unit), nameof(e.ProjectDetail.Unit));
                    keyValues.Add(nameof(e.UseTime), nameof(e.ProjectDetail.UseTime));
                    m.DicLinkField = keyValues;
                }).HasLabel("项目名称").ShowInList(width: (charWidth * 12));
                View.Property(p => p.CycleType).DefaultValue(CycleType.Day).Readonly().ShowInList(width: (charWidth * 4));
                View.Property(p => p.ProjectCycle).HasLabel("周期".L10N()+"*").UseSpinEditor(p =>
                {
                    p.AllowDecimals = false;
                    p.MinValue = 0;
                }).ShowInList(width: (charWidth * 4));
                View.Property(p => p.WarningPeriod).HasLabel("预警期".L10N() + "*").UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                }).ShowInList(width: (charWidth * 4));
                View.Property(p => p.LubricatingType).HasLabel("润滑方式".L10N() + "*").ShowInList(width: (charWidth * 4));
                //View.Property(p => p.Part).ShowInList(width: (charWidth * 10));
                View.Property(p => p.DepartmentId).HasLabel("责任部门".L10N() +"*").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.DepartmentNameView), nameof(e.Department.Name));

                    m.DicLinkField = dic;
                    m.DisplayField = EquipAccountCheckProject.DepartmentNameViewProperty.Name;
                    m.BindDisplayField = EquipAccountCheckProject.DepartmentNameViewProperty.Name;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword);
                    if (departments == null || departments.Count <= 0)
                        return new EntityList<Enterprise>();
                    departments.ForEach(p => p.TreePId = null);
                    return departments;
                }).UsePagingLookUpEditor().ShowInList(width: (charWidth * 4));

                View.Property(p => p.MinValue).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                }).HasLabel("加油量下限".L10N() + "*").ShowInList(width: (charWidth * 5));
                View.Property(p => p.MaxValue).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                }).HasLabel("加油量上限".L10N() +"* ").ShowInList(width: (charWidth * 5));
                View.Property(p => p.Unit).Readonly().ShowInList(width: (charWidth * 3));

                View.Property(p => p.LastDate).HasLabel("上次润滑日期".L10N() + "*").UseDateEditor().ShowInList(width: (charWidth * 6));
                View.Property(p => p.NextDate).HasLabel("下次润滑日期".L10N() +"* ").UseDateEditor().Readonly().ShowInList(width: (charWidth * 6));

                View.Property(p => p.Method).ShowInList(width: (charWidth * 15));
                View.Property(p => p.Standard).ShowInList(width: (charWidth * 12));
                View.Property(p => p.Consumable).ShowInList(width: (charWidth * 8));
                View.Property(p => p.UseTime).UseSpinEditor(p => { p.MinValue = 0;p.AllowNegative = false; }).ShowInList(width: (charWidth * 5));
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
