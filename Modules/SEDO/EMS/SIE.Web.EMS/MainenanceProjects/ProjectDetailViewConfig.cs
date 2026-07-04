using SIE.EMS.Common.Entity;
using SIE.EMS.MainenanceProjects;
using SIE.MetaModel.View;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.MainenanceProjects.Commands;
using System;

namespace SIE.Web.EMS.Projects
{
    /// <summary>
    /// 点检保养项目视图配置
    /// </summary>
    internal class ProjectDetailViewConfig : WebViewConfig<ProjectDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Projects.Behaviors.ProjectDetailBehavior");
            View.UseClientOrder();
            View.UseDefaultCommands();
            View.UseCommands(typeof(ProjectDetailImportCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Name);
                View.Property(p => p.ProjectType)
                    .Cascade(p => p.CycleType, null)
                    .Cascade(p => p.CycleTypeInfo, null);
                View.Property(p => p.CycleTypeInfo).UseCycleTypeEditor(p =>
                {
                    p.Editable = false;
                    p.ReloadDataOnPopping = true;
                    p.JsClassName = "SIE.Web.EMS.MainenanceProjects.Scripts.CycleTypeComboList";
                    p.ValueField = CycleTypeInfo.IdProperty.Name;
                }).Readonly(p => p.ProjectType == ProjectType.PeriodicalInsp || p.ProjectType == ProjectType.Verify || p.ProjectType == ProjectType.PlanRepair || p.ProjectType == ProjectType.Lubrication);

                View.Property(p => p.Part);
                View.Property(p => p.Consumable);
                View.Property(p => p.Method);
                View.Property(p => p.Standard);
                View.Property(p => p.MinValue).UseSpinEditor(p =>
                {
                    p.AllowNegative = false;
                    p.AllowDecimals = true;
                    p.AllowBlank = true;
                    p.DecimalPrecision = 2;
                    p.MinValue = 0;
                    p.Step = 0.1;
                });
                View.Property(p => p.MaxValue).UseSpinEditor(p =>
                {
                    p.AllowNegative = false;
                    p.AllowDecimals = true;
                    p.AllowBlank = true;
                    p.DecimalPrecision = 2;
                    p.MinValue = 0;
                    p.Step = 0.1;
                });
                View.Property(p => p.Unit);
                View.Property(p => p.UseTime).UseSpinEditor(p =>
                {
                    p.AllowNegative = false;
                    p.AllowDecimals = true;
                    p.AllowBlank = true;
                    p.DecimalPrecision = 2;
                    p.MinValue = 0;
                    p.Step = 0.1;
                });
            }
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.ProjectType);
            View.Property(p => p.CycleType);
            View.Property(p => p.Part);
            View.Property(p => p.Method);
            View.Property(p => p.Standard);
            View.Property(p => p.MinValue);
            View.Property(p => p.MaxValue);
            View.Property(p => p.Unit);
            View.Property(p => p.UseTime);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Name).ImportIndexer();
            View.Property(p => p.ProjectType).ImportIndexer();
            View.Property(p => p.CycleType);
            View.Property(p => p.Part);
            View.Property(p => p.Consumable);
            View.Property(p => p.Method);
            View.Property(p => p.Standard);
            View.Property(p => p.MinValue).UseSpinEditor(p =>
            {
                p.AllowNegative = true;
                p.AllowDecimals = true;
                p.AllowBlank = true;
                p.DecimalPrecision = 1;
                p.Step = 0.1;
            });
            View.Property(p => p.MaxValue).UseSpinEditor(p =>
            {
                p.AllowNegative = true;
                p.AllowDecimals = true;
                p.AllowBlank = true;
                p.DecimalPrecision = 1;
                p.Step = 0.1;
            });
            View.Property(p => p.Unit);
            View.Property(p => p.UseTime);
        }
    }
}
