using SIE.Domain;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.EMS.Purchases.EquipmentSetups.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试备件使用视图配置
    /// </summary>
    internal class SetupSparePartViewConfig : WebViewConfig<SetupSparePart>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentSetups.SetupSparePartBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddSetupSparePartCommand", WebCommandNames.Edit, typeof(DeleteSetupSparePartCommand).FullName,
                WebCommandNames.Copy);
            View.Property(p => p.EquipAccountId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SetupSparePart;
                if (entity == null)
                {
                    return new EntityList<EquipAccount>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetEquipmentsBySetupId(entity.EquipmentSetupId, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("设备编码");
            View.Property(p => p.EquipAccountName).Readonly();
            View.Property(p => p.PartOutDepotDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SetupSparePart;
                if (entity == null)
                {
                    return new EntityList<PartOutDepotDetail>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetPartOutDepotDetails(pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.OutDepotCount), nameof(e.PartOutDepotDetail.OutDepotCount));
                keyValues.Add(nameof(e.OutDepotNo), nameof(e.PartOutDepotDetail.OutDepotNoView));
                m.DicLinkField = keyValues;
            }).Readonly(p => p.IsOutDepotInfo).ShowInList(100).HasLabel("出库单行号");
            View.Property(p => p.OutDepotNo).ShowInList(150).Readonly();
            View.Property(p => p.SparePartId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SetupSparePart;
                if (entity == null)
                {
                    return new EntityList<SparePart>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetOutDepotSpareParts(entity.PartOutDepotDetailId, keyword, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                keyValues.Add("UnitId_Display", nameof(e.SparePart.UnitName));
                keyValues.Add(nameof(e.UnitId), nameof(e.SparePart.UnitId));
                keyValues.Add(nameof(e.Specification), nameof(e.SparePart.Specification));
                keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                m.DicLinkField = keyValues;
            }).HasLabel("备件编码").Readonly(p => p.IsOutDepotInfo).Cascade(p => p.LotNo, null).Cascade(p => p.Sn, null);
            View.Property(p => p.SparePartName).Readonly(p => p.SparePartId > 0);
            View.Property(p => p.Specification).Readonly();
            View.Property(p => p.ControlMethod).ShowInList(80).Readonly();

            //批次号、序列号
            ConfigLotSnView();

            View.Property(p => p.OutDepotCount).ShowInList(80).Readonly();
            View.Property(p => p.UseQty).ShowInList(80).UseSpinEditor(p =>
            {
                p.MinValue = 1;
            }).Readonly(p => p.ControlMethod == ControlMethod.Sn && !p.IsOutDepotInfo);
            View.Property(p => p.SurplusQty).ShowInList(80).Readonly();
            View.Property(p => p.UnitId).ShowInList(80).Readonly(p => p.SparePartId > 0);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置批次号、序列号视图
        /// </summary>
        protected void ConfigLotSnView()
        {
            //批次号
            View.Property(p => p.LotInfoId).ShowInList(150).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SetupSparePart;
                if (entity == null || entity.SparePartId == null)
                {
                    return new EntityList<SetupLotSnInfo>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetLotInfos(entity.PartOutDepotDetailId, entity.SparePartId.Value, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.LotNo), nameof(e.LotInfo.Value));
                m.DicLinkField = keyValues;
            }).Readonly(p => p.IsOutDepotInfo || p.SparePartId == null || p.ControlMethod != ControlMethod.Batch);

            //序列号
            View.Property(p => p.SnInfoId).ShowInList(150).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as SetupSparePart;
                if (entity == null || entity.SparePartId == null)
                {
                    return new EntityList<SetupLotSnInfo>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetSnInfos(entity.PartOutDepotDetailId, entity.SparePartId.Value, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.Sn), nameof(e.SnInfo.Value));
                keyValues.Add(nameof(e.UseQty), nameof(e.SnInfo.Qty));
                m.DicLinkField = keyValues;
            }).Readonly(p => p.IsOutDepotInfo || p.SparePartId == null || p.ControlMethod != ControlMethod.Sn);
        }
    }
}