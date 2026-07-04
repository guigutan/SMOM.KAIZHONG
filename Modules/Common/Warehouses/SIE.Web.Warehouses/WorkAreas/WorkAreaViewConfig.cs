using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 工作区视图配置
    /// </summary>
    internal class WorkAreaViewConfig : WebViewConfig<WorkArea>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(WorkAreaAddCommand).FullName, WebCommandNames.Edit, typeof(WorkAreaDeleteCommand).FullName, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}编码", "单号生成规则", "工作区") + ",新增状态可编辑"; });
            View.Property(p => p.Name);
            View.Property(p => p.Desc);
            View.Property(p => p.WarehouseId).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.State).Readonly();
            View.ChildrenProperty(p => p.WorkAreaLocationList).HasLabel("工作区与库位关系").OrderNo = 1;
            View.ChildrenProperty(p => p.WorkAreaEmployeeList).HasLabel("工作区与员工关系").OrderNo = 2;
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Desc);
            View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
        }
    }
}