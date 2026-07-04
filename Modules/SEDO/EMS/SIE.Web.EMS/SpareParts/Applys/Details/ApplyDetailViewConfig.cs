using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.Applys.Details
{
    /// <summary>
    /// 申请单明细 视图配置
    /// </summary>
    public class ApplyDetailViewConfig : WebViewConfig<ApplyDetail>
    {
        /// <summary>
        /// 查看视图
        /// </summary>
        public const string ReadonlyView = "Readonly";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadonlyView);
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadonlyView();
            }


        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.AddBehavior("SIE.Web.EMS.SpareParts.Applys.Behaviors.DetailBehavior");//行为
            View.UseCommands(
                "SIE.Web.EMS.SpareParts.Applys.Commands.AddDetailCommand",//添加
                "SIE.Web.EMS.SpareParts.Applys.Commands.EditDetailCommand",//修改
                "SIE.Web.EMS.SpareParts.Applys.Commands.DeleteDetailCommand"//删除
                );
            View.Property(p => p.SparePart)
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartId), nameof(SparePart.Id));
                    keyValues.Add(nameof(e.SparePartName), nameof(SparePart.SparePartName));
                    keyValues.Add(nameof(e.Specification), nameof(SparePart.Specification));
                    keyValues.Add(nameof(e.SparePartTypeCode), nameof(SparePart.SparePartTypeCode));
                    keyValues.Add(nameof(e.EquipModelCode), nameof(SparePart.EquipModelCode));
                    keyValues.Add(nameof(e.UnitName), nameof(SparePart.UnitName));
                    keyValues.Add(nameof(e.SpartType), nameof(SparePart.SpartType));
                    m.DicLinkField = keyValues;

                })
                 .UseDataSource((e, p, o) =>
                 {
                     var entity = e as ApplyDetail;
                    var result = RT.Service.Resolve<SparePartController>().GetSparePartByEquipModelId(p, o, entity.EquipModelId);
                     return result;
                 })
                .Readonly(p => !(p.AuditState == AuditState.Create || p.AuditState == AuditState.Returned)).HasLabel("备件编码");
            View.Property(p => p.SparePartName).Readonly();
            View.Property(p => p.Specification).Readonly();
            View.Property(p => p.SparePartPart)
                .Readonly(p => !(p.AuditState == AuditState.Create || p.AuditState == AuditState.Returned));
            View.Property(p => p.SpartType).Readonly();
            View.Property(p => p.SparePartTypeCode).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.EquipModelCode).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.UnitName).Readonly();
            View.Property(p => p.Warehouse).UsePagingLookUpEditor((m, e) =>
            {
                m.BindDisplayField = ApplyDetail.WarehouseNameProperty.Name;
            }).Readonly(p => !(p.AuditState == AuditState.Create || p.AuditState == AuditState.Returned));            
            
            View.Property(p => p.ApplyAmount).UseSpinEditor(m => m.MinValue = 0)
                .Readonly(p => !(p.AuditState == AuditState.Create || p.AuditState == AuditState.Returned));
            View.Property(p => p.OutDepotAmount).Readonly();
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        protected void ConfigReadonlyView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartId).ShowInList().Readonly();
                View.Property(p => p.SparePartName).ShowInList().Readonly();
                View.Property(p => p.Specification).ShowInList().Readonly();
                View.Property(p => p.SparePartPart).ShowInList().Readonly();
                View.Property(p => p.SparePartTypeCode).ShowInList().Readonly();
                View.Property(p => p.EquipModelCode).ShowInList().Readonly();
                View.Property(p => p.UnitName).ShowInList().Readonly();                
                
                View.Property(p => p.ApplyAmount).ShowInList().Readonly();
                View.Property(p => p.DepotAmount).ShowInList().Readonly();
                View.Property(p => p.OutDepotAmount).ShowInList().Readonly();
            }
        }



        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.SparePart);
            View.Property(p => p.SparePartName).Readonly();
            View.Property(p => p.Specification).Readonly();
            View.Property(p => p.SparePartTypeCode).Readonly();
            View.Property(p => p.EquipModelCode).Readonly();
            View.Property(p => p.UnitName).Readonly();            
            View.Property(p => p.ApplyAmount);
            View.Property(p => p.DepotAmount).Readonly();
            View.Property(p => p.OutDepotAmount).Readonly();
            View.Property(p => p.UseAmount).Readonly();
        }
    }
}
