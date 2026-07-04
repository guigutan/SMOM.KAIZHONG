using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.Web.Common;
using SIE.Web.Resources;

namespace SIE.Web.LES.RetreatItemManage.MaterialReturns
{
    /// <summary>
    /// 
    /// </summary>
    public class MaterialReturnForSelectViewConfig : WebViewConfig<MaterialReturnForSelect>
    {

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaterialReturn));
            View.RemoveCommands();
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands();
            View.DisableEditing();
            View.WithoutPaging();
            View.FormEdit();
            View.Property(p => p.NO).HasLabel("退料单号").Show(ShowInWhere.Hide);
            View.Property(p => p.ReturnType).DisableSort();
            View.Property(p => p.ReturnState).DisableSort();
            View.Property(p => p.ItemCode).DisableSort();
            View.Property(p => p.ItemName).DisableSort();
            View.Property(p => p.ItemExtPropName).DisableSort();
            View.Property(p => p.Label).DisableSort();
            View.Property(p => p.IsSerial).DisableSort();
            View.Property(p => p.BatchNO).DisableSort();
            View.Property(p => p.Qty).DisableSort();
            View.Property(p => p.WorkOrderId).HasLabel("关联工单").DisableSort();
            View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("工厂").DisableSort();
            View.Property(p => p.WipResourceId).HasLabel("生产资源").DisableSort();
            View.Property(p => p.ReturnWarehouse).HasLabel("退料仓库").DisableSort();
            View.Property(p => p.ReturnWarehouseLocation).HasLabel("退料库位").DisableSort();
            View.Property(p => p.ReturnReason).Show(ShowInWhere.All).UseCatalogEditor(p => { p.CatalogType = MaterialReturn.ReasonMaterialReturn; p.CatalogReloadData = true; }).DisableSort();
            View.Property(p => p.ReturnReasonDesc).ShowInDetail(columnSpan: 3, width: "65%").DisableSort();
            View.Property(p => p.EmployeeId).Show(ShowInWhere.Hide).HasLabel("提交人").DisableSort();
            View.Property(p => p.SubmitDate).Show(ShowInWhere.Hide).HasLabel("提交时间").DisableSort();
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

    }
}
