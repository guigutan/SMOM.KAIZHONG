using Microsoft.CodeAnalysis.Operations;
using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.Items._Extentions_;
using SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.LES.RetreatItemManage.MaterialReturns
{
    /// <summary>
    /// 
    /// </summary>
    public class MaterialReturnViewConfig : WebViewConfig<MaterialReturn>
    {
        /// <summary>
        /// 添加页面
        /// </summary>

        public const string AddDetailPageView = "AddDetailPageView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaterialReturn));
            View.DeclareExtendViewGroup(AddDetailPageView);
            if (ViewGroup == AddDetailPageView)
            {
                ConfigAddDetailPageView();
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.FormEdit();
            View.UseCommands("SIE.Web.LES.RetreatItemManage.MaterialReturns.AddRetrunCommand", "SIE.Web.LES.RetreatItemManage.MaterialReturns.EditRetrunCommand",
                typeof(RedrawReturnCommand).FullName, typeof(SubmitReturnCommand).FullName
                );
            View.Property(p => p.NO).HasLabel("退料单号");
            View.Property(p => p.ReturnType);
            View.Property(p => p.ReturnState);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.ItemExtPropName);
            View.Property(p => p.Label);
            View.Property(p => p.IsSerial).Readonly();
            View.Property(p => p.BatchNO);
            View.Property(p => p.Qty);
            View.Property(p => p.WorkOrderId).HasLabel("关联工单");
            View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("工厂");
            View.Property(p => p.WipResourceId).HasLabel("资源编码");
            View.Property(p => p.WipResourceName).HasLabel("资源名称");
            View.Property(p => p.ReturnWarehouse).HasLabel("退料仓库");
            View.Property(p => p.ReturnWarehouseLocationId).HasLabel("退料库位");
            View.Property(p => p.ReturnReason).Show(ShowInWhere.All).UseCatalogEditor(p => { p.CatalogType = MaterialReturn.ReasonMaterialReturn; p.CatalogReloadData = true; });
            View.Property(p => p.ReturnReasonDesc).ShowInDetail(columnSpan: 3, width: "65%");
            View.Property(p => p.EmployeeId).HasLabel("提交人").Readonly();
            View.Property(p => p.SubmitDate).HasLabel("提交时间").Readonly();
        }


        /// <summary>
        ///明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.UseCommands(typeof(EditSaveReturnCommand).FullName, typeof(DetailRedrawReturnCommand).FullName, typeof(DetailSumitRrturnCommand).FullName);
            View.HasDetailColumnsCount(3);
            //View.Property(p => p.NO).HasLabel("退料单号").Readonly();
            View.Property(p => p.ReturnType).Readonly();
            View.Property(p => p.ReturnState).Readonly();
            View.Property(p => p.ItemCode).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.Label).Readonly();
            View.Property(p => p.BatchNO).Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.AlreadyQty).Readonly();
            View.Property(p => p.WorkOrderId).HasLabel("关联工单").Readonly();
            View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("工厂").Readonly();
            View.Property(p => p.WipResourceId).HasLabel("资源编码").Readonly();
            View.Property(p => p.ReturnWarehouse).HasLabel("退料仓库").Readonly();
            View.Property(p => p.ReturnWarehouseLocation).HasLabel("退料库位").Readonly();
            View.Property(p => p.ReturnReason).Show(ShowInWhere.All).UseCatalogEditor(p => { p.CatalogType = MaterialReturn.ReasonMaterialReturn; p.CatalogReloadData = true; });
            View.Property(p => p.ReturnReasonDesc).Show(ShowInWhere.All);
        }

        /// <summary>
        /// 配置新增页面
        /// </summary>
        protected void ConfigAddDetailPageView()
        {
            View.ClearCommands();
            View.AddBehavior("SIE.Web.LES.RetreatItemManage.MaterialReturns.Behaviors.AddPageBehavior");
            View.UseCommands(typeof(SaveReturnCommand).FullName, typeof(DetailSumitRrturnCommand).FullName);
            View.HasDetailColumnsCount(6);
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).UseDisplayEditor(p => p.XType = "RetrunMaterialEditor").ShowInDetail(columnSpan: 6);
                View.Property(p => p.NO).ShowInDetail(columnSpan: 2).HasLabel("退料单号").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ReturnType).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ReturnState).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ItemCode).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ItemName).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ItemExtPropName).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Label).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.BatchNO).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Qty).ShowInDetail(columnSpan: 2).UseItemUnitEditor().Show(ShowInWhere.All).Readonly(p => p.NO == "" || p.IsSerial == true);
                View.Property(p => p.AlreadyQty).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ReturnReason).ShowInDetail(columnSpan: 2).Show(ShowInWhere.All).UseCatalogEditor(p => { p.CatalogType = MaterialReturn.ReasonMaterialReturn;p.CatalogReloadData = true; }).Readonly(p => p.NO == "");
                View.Property(p => p.ReturnReasonDesc).ShowInDetail(columnSpan: 2).Readonly(p => p.NO == "");
                View.Property(p => p.WorkOrderId).ShowInDetail(columnSpan: 2).HasLabel("关联工单").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.FactoryId).ShowInDetail(columnSpan: 2).UseFactoryEditor().HasLabel("工厂").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WipResourceId).ShowInDetail(columnSpan: 2).HasLabel("资源编码").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ReturnWarehouse).ShowInDetail(columnSpan: 2).HasLabel("退料仓库").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ReturnWarehouseLocationId).ShowInDetail(columnSpan: 2).HasLabel("退料库位").Show(ShowInWhere.All).Readonly();
            }
        }
    }
}
