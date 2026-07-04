using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Web.Common.Configs.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.OutDepots
{
    /// <summary>
    /// 备件出库单视图配置
    /// </summary>
    public class OutDepotViewConfig : WebViewConfig<OutDepot>
    {
        /// <summary>
        /// 供应商信息视图
        /// </summary>
        public const string SupplierInfoViewGroup = "SupplierInfoViewGroup";

        /// <summary>
        /// 供应商信息添加视图
        /// </summary>
        public const string AddSupplierInfoViewGroup = "AddSupplierInfoViewGroup";

        /// <summary>
        /// 出库单拣货视图
        /// </summary>
        public const string PickOutDepotDetailsViewGroup = "PickOutDepotDetailsViewGroup";

        /// <summary>
        /// 出库单发货视图
        /// </summary>
        public const string SendOutDepotDetailsViewGroup = "SendOutDepotDetailsViewGroup";

        /// <summary>
        /// 出库单关单视图
        /// </summary>
        public const string CloseOutDepotViewGroup = "CloseOutDepotViewGroup";

        /// <summary>
        /// 配置视图属性
        /// </summary>
        protected override void ConfigView()
        {
             View.DeclareExtendViewGroup(new string[]{ SupplierInfoViewGroup,AddSupplierInfoViewGroup,PickOutDepotDetailsViewGroup,SendOutDepotDetailsViewGroup, CloseOutDepotViewGroup });

            if (ViewGroup == SupplierInfoViewGroup)
            {
                ConfigSupplierInfoView();
            }

            if (ViewGroup == AddSupplierInfoViewGroup)
            {
                ConfigAddSupplierInfoView();
            }

            if (ViewGroup == PickOutDepotDetailsViewGroup)
            {
                ConfigPickOutDepotDetailsView();
            }

            if (ViewGroup == SendOutDepotDetailsViewGroup)
            {
                ConfigSendOutDepotDetailsView();
            }

            if (ViewGroup == CloseOutDepotViewGroup)
            {
                ConfigCloseOutDepotView();
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            bool isCreateHandoverBill = RT.Service.Resolve<OutDepotController>().IsCreateHandoverBill();

            //View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepots.Behaviors.OutDepotBillBehavior");
            View.UseCommand(WebCommandNames.Add);
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.PickOutDepotDetailCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.SendOutDepotDetailCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.CloseOutDepotCommand");
            View.UseCommand(WebCommandNames.Save);
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 180);
                View.Property(p => p.OutDepotType);
                View.Property(p => p.SourceNo).ShowInList(width: 180);
                View.Property(p => p.OutDepotState);
                View.Property(p => p.QualityStatus);
                View.Property(p => p.ReleDoc).ShowInList(width: 180);
                View.Property(p => p.GetDepartment);
                View.Property(p => p.OutDepotDate).UseDateEditor(m => { m.Format = "Y/m/d"; }).Show(ShowInWhere.Hide);
                View.Property(p => p.WarehouseName);
                View.Property(p => p.CloseReason);
                View.ChildrenProperty((OutDepot c) => c.OutDepotDetailList).Show(ChildShowInWhere.All);
                View.ChildrenProperty((OutDepot c) => c.PartOutDepotDetailList).Show(ChildShowInWhere.Hide);
                View.AssociateChildrenProperty(OutDepot.PartOutDepotDetailListProperty,
                e =>
                {
                    var arg = e as ChildPagingDataWithParentEntityArgs;
                    var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<OutDepot>();

                    if (parent == null)
                    {
                        return new EntityList<PartOutDepotDetail>();
                    }
                    else
                    {
                        var keyword = parent.OutDepotDetailKeyWord;
                        return RT.Service.Resolve<OutDepotController>().GetOutDepotDetails(parent.Id, keyword, arg.SortInfo, arg.PagingInfo);
                    }
                }).HasLabel("出库明细");

                if (isCreateHandoverBill) 
                {
                    View.AttachChildrenProperty(typeof(OutDepotHandoverDetail), (e) =>
                    {
                        EntityList<OutDepotHandoverDetail> handoverDetailList = new EntityList<OutDepotHandoverDetail>();
                        var args = e as ChildPagingDataWithParentEntityArgs;
                        OutDepot entity = null;

                        if (args.ParentEntity != null)
                        {
                            entity = args.ParentEntity.ToJsonObject<OutDepot>();
                        }
                        else
                        {
                            entity = RF.GetById<OutDepot>(args.Parent.GetId());
                        }

                        if (entity != null)
                        {
                            handoverDetailList.AddRange(RT.Service.Resolve<OutDepotController>().GetOutDepotHandoverDetailList(args.SortInfo, args.PagingInfo, entity));
                        }
                        return handoverDetailList;
                    }, "OutDepotHandoverDetailViewGroup").HasLabel("接收明细");
                }
                
                View.ChildrenProperty((OutDepot c) => c.SupplierInfoList).Show(ChildShowInWhere.Hide);
                View.AttachDetailChildrenProperty(typeof(OutDepot), c=>
                {
                    OutDepot outDepot = c.Parent as OutDepot;
                    return RF.GetById<OutDepot>(outDepot.Id, new EagerLoadOptions().LoadWithViewProperty());
                }, SupplierInfoViewGroup, null).HasLabel("供应商信息");
            }
        }

        /// <summary>
        /// 添加视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            bool trueValue = true;
            bool falseValue = false;

            View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepots.Behaviors.OutDepotBehavior");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.SaveOutDepotDetailCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.ResetOutDepotCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly(true).Show();
                View.Property(p => p.OutDepotType).UseEnumEditor(p => { p.FilterCategoery = "Create"; }).Show();

                View.Property(p => p.WarehouseId)
                    .UseDataSource((source, pagingInfo, keyword) =>
                    {
                        return RT.Service.Resolve<WarehouseController>().GetWarehouses( pagingInfo,keyword);
                    })
                    .Cascade(p => p.StorageLocationId, null)
                    .Cascade(p => p.StorageLocationNum, null)
                    .Readonly(p => (p.IsBarcode == falseValue && p.ScanValue != "") || p.IsExistDetail == trueValue).Show();

                View.Property(p => p.GetDepartmentId).UseDataSource((e, pagingInfo, keyword)=>
                {
                    EnterpriseController enterpriseController = RT.Service.Resolve<EnterpriseController>();
                    return enterpriseController.GetDepartmentsWithParent(pagingInfo, keyword);
                }).Readonly(p => p.WarehouseId == null).Show();

                View.Property(p => p.QualityStatus)
                    .Cascade(p => p.StorageLocationId, null)
                    .Cascade(p => p.StorageLocationNum, null)
                    .Readonly(p => p.WarehouseId == null || (p.IsBarcode == falseValue && p.ScanValue != "") || p.IsExistDetail == trueValue).Show();

                View.Property(p => p.SparePartId)
                    .UseDataSource((e,p,o) =>
                    { 
                        return RT.Service.Resolve<SparePartController>().GetSparePartByEquipModelId(p, o, null); 
                    }).UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                        keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                        m.DicLinkField = keyValues;
                    })
                    .Cascade(p => p.StorageLocationId, null)
                    .Cascade(p => p.StorageLocationNum, null).Readonly(p => p.WarehouseId == null || p.QualityStatus == null || (p.IsBarcode == falseValue && p.ScanValue != "")).Show().HasLabel("备件编码".L10N()+"*");
                
                View.Property(p => p.SparePartName).Readonly(true).Show();
                View.Property(p => p.ControlMethod).Readonly(true).Show();

                View.Property(p => p.StorageLocationId).UseDataSource((e, p, o)=>
                {
                    OutDepot outDepot = e as OutDepot;
                    return RT.Service.Resolve<StoreSummaryController>().GetStorageLocationForOutDepot((double)outDepot.WarehouseId, (QualityStatus)outDepot.QualityStatus, outDepot.SparePartId, outDepot.ControlMethod,o,p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.StorageLocationNum), "RoutewayId");
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.WarehouseId == null || p.QualityStatus == null).Show().HasLabel("库位".L10N()+"*");

                View.Property(p => p.StorageLocationNum).ShowInDetail("33.3%", columnSpan: 3).Readonly(true).Show();

                View.Property(p => p.Message)
                    .DefaultValue("请先维护【出库仓库】/【质量状态】！".L10N())
                    .ShowInDetail(columnSpan:4).Readonly(true).Show().HasLabel("消息框");
                View.Property(p => p.ScanValue)
                    .UseDisplayEditor(p=>{p.XType = "OutDepotScanValueEditor";})
                    .ShowInDetail(columnSpan: 4).Show().HasLabel("扫描框");

                View.Property(p => p.AdviceStorageLocation).ShowInDetail(columnSpan: 4).Readonly(true).Show();
                View.ChildrenProperty(p => p.OutDepotDetailList).HasLabel("申请明细").Show(ChildShowInWhere.All).ViewGroup = "AddOutDepotDetailViewGroup";
                View.ChildrenProperty(p => p.PartOutDepotDetailList).HasLabel("出库明细").Show(ChildShowInWhere.All).ViewGroup = "AddPartOutDepotDetailViewGroup";
                View.AttachDetailChildrenProperty(typeof(OutDepot), (c) =>
                {
                    var item = c.Parent as OutDepot;
                    return item;
                }, AddSupplierInfoViewGroup).HasLabel("供应商信息").Show(ChildShowInWhere.All).HasOrderNo(30);
            }
        }

        /// <summary>
        /// 供应商信息添加视图
        /// </summary>
        protected void ConfigAddSupplierInfoView()
        {
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

            View.UseDetail(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    keyValues.Add(nameof(e.Contacts), nameof(e.Supplier.Contacts));
                    keyValues.Add(nameof(e.ContactNumber), nameof(e.Supplier.ContactNumber));
                    keyValues.Add(nameof(e.ContactAddress), nameof(e.Supplier.ContactAddress));
                    m.DicLinkField = keyValues;
                }).Readonly(p=>p.RepairOutDepotType != OutDepotType.DgMaintain).Show();
                View.Property(p => p.SupplierName).Readonly().Show();
                View.Property(p => p.Contacts).Readonly().Show();
                View.Property(p => p.ContactNumber).Readonly().Show();
                View.Property(p => p.ContactAddress).Readonly().ShowInDetail(columnSpan: 4).Show();
                View.Property(p => p.DeliveryWay).Readonly(p => p.RepairOutDepotType != OutDepotType.DgMaintain).Show();
                View.Property(p => p.DepotRetDate).UseDateEditor(m => { m.Format = "Y/m/d"; }).Readonly(p => p.RepairOutDepotType != OutDepotType.DgMaintain).Show();
                View.Property(p => p.OutState).Readonly().Show();
            }
        }

        /// <summary>
        /// 供应商信息视图
        /// </summary>
        protected void ConfigSupplierInfoView()
        {
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

            View.UseDetail(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    keyValues.Add(nameof(e.Contacts), nameof(e.Supplier.Contacts));
                    keyValues.Add(nameof(e.ContactNumber), nameof(e.Supplier.ContactNumber));
                    keyValues.Add(nameof(e.ContactAddress), nameof(e.Supplier.ContactAddress));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.OutDepotType != OutDepotType.DgMaintain || p.OutState == OutDepotState.Ed).Show();
                View.Property(p => p.SupplierName).Readonly().Show();
                View.Property(p => p.Contacts).Readonly().Show();
                View.Property(p => p.ContactNumber).Readonly().Show();
                View.Property(p => p.ContactAddress).Readonly().ShowInDetail(columnSpan: 4).Show();
                View.Property(p => p.DeliveryWay).Readonly(p => p.OutDepotType != OutDepotType.DgMaintain || p.OutState == OutDepotState.Ed).Show();
                View.Property(p => p.DepotRetDate).UseDateEditor(m => { m.Format = "Y/m/d"; }).Readonly(p => p.OutDepotType != OutDepotType.DgMaintain || p.OutState == OutDepotState.Ed).Show();
                View.Property(p => p.OutState).Readonly().Show();
            }
        }

        /// <summary>
        /// 拣货视图配置
        /// </summary>
        protected void ConfigPickOutDepotDetailsView()
        {
            bool falseValue = false;

            View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepots.Behaviors.PickOutDepotBehavior");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.SaveOutDepotDetailCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.ResetPickOutDepotCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);

            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().Show();
                View.Property(p => p.OutDepotType).Readonly().Show();
                View.Property(p => p.WarehouseId).Readonly().Show();
                View.Property(p => p.GetDepartmentId).Readonly().Show();
                View.Property(p => p.QualityStatus).Readonly().Show();

                View.Property(p => p.SparePartId)
                    .UseDataSource((e, p, o) =>
                    {
                        OutDepot outDepot = e as OutDepot;
                        return RT.Service.Resolve<OutDepotController>().GetSparePartByOutDepot(p, outDepot, o);
                    }).UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                        keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                        m.DicLinkField = keyValues;
                    })
                    .Cascade(p => p.StorageLocationId, null)
                    .Cascade(p => p.StorageLocationNum, null).Readonly(p => (p.IsBarcode == falseValue && p.ScanValue != "")).Show();

                View.Property(p => p.SparePartName).Readonly().Show();
                View.Property(p => p.ControlMethod).Readonly().Show();

                View.Property(p => p.StorageLocationId).UseDataSource((e, p, o) =>
                {
                    OutDepot outDepot = e as OutDepot;
                    return RT.Service.Resolve<StoreSummaryController>().GetStorageLocationForOutDepot((double)outDepot.WarehouseId, (QualityStatus)outDepot.QualityStatus, outDepot.SparePartId, outDepot.ControlMethod);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.StorageLocationNum), "RoutewayId");
                    m.DicLinkField = keyValues;
                }).Show();

                View.Property(p => p.StorageLocationNum).ShowInDetail("33.3%", columnSpan: 3).Readonly().Show();

                View.Property(p => p.Message)
                    .DefaultValue("请扫描序列号/批次号/备件编码！".L10N())
                    .ShowInDetail(columnSpan: 4).Readonly().Show().HasLabel("消息框");
                View.Property(p => p.ScanValue)
                    .UseDisplayEditor(p => { p.XType = "OutDepotScanValueEditor"; })
                    .ShowInDetail(columnSpan: 4).Show().HasLabel("扫描框");

                View.ChildrenProperty(p => p.OutDepotDetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.PartOutDepotDetailList).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(OutDepotDetail), (e) =>
                {
                    EntityList<OutDepotDetail> applyDetailList = new EntityList<OutDepotDetail>();
                    var args = e as ChildPagingDataWithParentEntityArgs;
                    OutDepot entity = null;

                    if (args.ParentEntity != null)
                    {
                        entity = args.ParentEntity.ToJsonObject<OutDepot>();
                    }
                    else 
                    {
                        entity = RF.GetById<OutDepot>(args.Parent.GetId());
                    }

                    if (entity != null) 
                    {
                        applyDetailList.AddRange(RT.Service.Resolve<OutDepotController>().GetOutDepotDetailList(args.SortInfo, args.PagingInfo, entity));
                    }
                    return applyDetailList;
                }, "PickOutDepotDetailViewGroup").HasLabel("申请单明细").Show(ChildShowInWhere.All);
                View.AttachChildrenProperty(typeof(PartOutDepotDetail), (e) =>
                {
                    EntityList<PartOutDepotDetail> outDepotDetailList = new EntityList<PartOutDepotDetail>();
                    var args = e as ChildPagingDataWithParentEntityArgs;
                    OutDepot entity = null;

                    if (args.ParentEntity != null)
                    {
                        entity = args.ParentEntity.ToJsonObject<OutDepot>();
                    }
                    else
                    {
                        entity = RF.GetById<OutDepot>(args.Parent.GetId());
                    }

                    if (entity != null)
                    {
                        outDepotDetailList.AddRange(RT.Service.Resolve<OutDepotController>().GetPartOutDepotDetailList(args.SortInfo, args.PagingInfo, entity));
                    }
                    return outDepotDetailList;
                }, "PickPartOutDepotDetailViewGroup").HasLabel("出库单明细").Show(ChildShowInWhere.All);
                View.AttachDetailChildrenProperty(typeof(OutDepot), (c) =>
                {
                    OutDepot outDepot = c.Parent as OutDepot;
                    return RF.GetById<OutDepot>(outDepot.Id, new EagerLoadOptions().LoadWithViewProperty());
                }, SupplierInfoViewGroup).HasLabel("供应商信息").Show(ChildShowInWhere.All).HasOrderNo(30);
            }
        }

        /// <summary>
        /// 发货视图配置
        /// </summary>
        protected void ConfigSendOutDepotDetailsView()
        {
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().Show();
                View.Property(p => p.OutDepotType).Readonly().Show();
                View.Property(p => p.GetDepartmentId).Readonly().Show();
                View.Property(p => p.OutDepotState).Readonly().Show();
                View.Property(p => p.OutDepotDate).UseDateEditor().DefaultValue(DateTime.Now.ToString()).Show();
                View.AssociateChildrenProperty(OutDepot.PartOutDepotDetailListProperty,
                e =>
                {
                    var arg = e as ChildPagingDataWithParentEntityArgs;
                    var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<OutDepot>();

                    if (parent == null)
                    {
                        return new EntityList<PartOutDepotDetail>();
                    }
                    else
                    {
                        var keyword = parent.OutDepotDetailKeyWord;//UI工具栏附加查询输入栏值
                        return RT.Service.Resolve<OutDepotController>().GetSendOutDepotDetails(parent.Id, keyword, arg.SortInfo, arg.PagingInfo);
                    }
                }, "SendPartOutDepotDetailViewGroup").HasLabel("待发货明细").Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 关单视图配置
        /// </summary>
        protected void ConfigCloseOutDepotView()
        {
            View.Property(p => p.CloseReason).UseMemoEditor().Show();
        }
    }
}
