using SIE.Common.Configs;
using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.DevicePurs;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.AlarmStates;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Lubrications;
using SIE.EMS.Projects;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.EMS.Tpms;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources;
using SIE.Web.ClientMetaModel;
using SIE.Web.Common;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.EMS.EquipRepair.EquipRepairs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修视图配置
    /// </summary>
    public class EquipRepairViewConfig : WebViewConfig<EquipRepairBill>
    {
        #region 扩展视图
        /// <summary>
        /// 错误信息视图
        /// </summary>
        private const string ErrorInfoGroup = "ErrorInfoGroup";

        /// <summary>
        /// 维修报告
        /// </summary>
        private const string RepairRepoterViewGroup = "RepairRepoterViewGroup";

        /// <summary>
        /// 接单视图
        /// </summary>
        private const string TakeOrderViewGroup = "TakeOrderViewGroup";

        /// <summary>
        /// 派工视图
        /// </summary>
        private const string DispatchRepairViewGroup = "DispatchRepairViewGroup";

        /// <summary>
        /// 转派视图
        /// </summary>
        private const string TransferRepairViewGroup = "TransferRepairViewGroup";

        /// <summary>
        /// 交机确认视图
        /// </summary>
        public static readonly string HandoverConfirmViewGroup = "HandoverConfirmViewGroup";

        /// <summary>
        /// 工程确认视图
        /// </summary>
        public static readonly string EngineerConfirmViewGroup = "EngineerConfirmViewGroup";

        /// <summary>
        /// 故障信息视图
        /// </summary>
        private const string AbnormalInfoViewGroup = "AbnormalInfoViewGroup";

        /// <summary>
        /// 设备台账维修单视图
        /// </summary>
        public static readonly string EquipAccountRepairView = "EquipAccountRepairView";

        /// <summary>
        /// 创建维修单（报修）视图
        /// </summary>
        public static readonly string CreateRepairBillView = "CreateRepairBillView";
        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(EquipRepairBill.RepairNoProperty);
            View.FormEdit();
            View.AssignAuthorize(typeof(EquipRepairBill));
            View.DeclareExtendViewGroup(new string[] { ErrorInfoGroup,TakeOrderViewGroup, DispatchRepairViewGroup, EquipAccountRepairView
                , HandoverConfirmViewGroup, AbnormalInfoViewGroup , EngineerConfirmViewGroup,RepairRepoterViewGroup });
            if (ViewGroup == ErrorInfoGroup)
            {
                View.DomainName("故障信息录入");
                ErrorInfo();
            }
            if (ViewGroup == EquipAccountRepairView)
            {
                ConfigEquipAccountRepair();
            }
            if (ViewGroup == TakeOrderViewGroup)
            {
                TakeOrderView();
            }
            if (ViewGroup == DispatchRepairViewGroup)
            {
                DispatchRepairView();
            }
            if (ViewGroup == TransferRepairViewGroup)
            {
                TransferRepairView();
            }
            if (ViewGroup == HandoverConfirmViewGroup)
            {
                HandoverConfirmView();
            }
            if (ViewGroup == EngineerConfirmViewGroup)
            {
                EngineerConfirmView();
            }
            if (ViewGroup == AbnormalInfoViewGroup)
            {
                AbnormalInfoView();
            }
            if (ViewGroup == "FaultInfoView")
            {
                FaultInfoView();
            }

            if (ViewGroup == RepairRepoterViewGroup)
            {
                RepairRepoterView();
            }

            if (ViewGroup == CreateRepairBillView)
            {
                ConfigCreateRepairBillView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();

            View.UseCommands("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddEquipRepairCommand",
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.RepairStartCommand",//开始
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.FinishEquipRepairBillCommand",//完成
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SaveCommand",
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.CancelCommand",//取消
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.DispatchEquipRepairCommand",//派工
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.TakeOrderCommand",//接单
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.TransferEquipRepairCommand",//转派
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SuspendCommand",//暂停
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ContinueCommand",//继续
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.HandoverConfirmCommand",//交机确认
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.EngineerConfirmCommand",//工程确认
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.CompelCloseCommand"//强制关闭
                );
            View.UseCommands(WebCommandNames.ExportXls,WebCommandNames.ExportXlsAll);


            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairListBehavior");

            View.Property(p => p.RepairState);
            View.Property(p => p.RepairNo).ShowInList(width: 150);
            View.Property(p => p.SourceNo).ShowInList(width: 150);
            View.Property(p => p.SourceType);
            View.Property(p => p.RepairType);
            View.Property(p => p.RepairWay);
            View.Property(p => p.EquipAccountCode).ShowInList(width: 120);
            View.Property(p => p.EquipAccountName).ShowInList(width: 120);
            View.Property(p => p.EquipModelCode).HasLabel("设备型号");
            View.Property(p => p.EquipAccountMode);
            View.Property(p => p.SparePartCode).ShowInList(width: 120);
            View.Property(p => p.SparePartName).ShowInList(width: 120);
            View.Property(p => p.WorkShopName).ShowInList(width: 60);
            View.Property(p => p.ResourceName).ShowInList(width: 60);
            View.Property(p => p.ProcessName).ShowInList(width: 80);
            View.Property(p => p.UrgentDegree).ShowInList(width: 80);
            View.Property(p => p.ProduceState).ShowInList(width: 80);

            View.Property(p => p.ApplyRepairEmployee);
            View.Property(p => p.ApplyRepairDate).ShowInList(width: 150);
            View.Property(p => p.RepairMaster).ShowInList(width: 120);
            View.Property(p => p.RepairEmployees).ShowInList(width: 150);
            View.Property(p => p.ReceiveOrderDate).ShowInList(width: 150);
            View.Property(p => p.TransferOrderDate).ShowInList(width: 150);
            View.Property(p => p.EstimateFinishDate).ShowInList(width: 150);
            View.Property(p => p.RepairBeginDate).ShowInList(width: 150);
            View.Property(p => p.RepairFinishDate).ShowInList(width: 150);
            View.Property(p => p.HandoverConfirmResult);
            View.Property(p => p.EngineerConfirmResult);
            View.Property(p => p.RepairTime);
            View.Property(p => p.IsSupplement).Readonly();
            View.Property(p => p.CancelReason);

            View.ChildrenProperty(p => p.HandoverConfirmDetailList)
              .Show(ChildShowInWhere.All).OrderNo = 6;

            View.ChildrenProperty(p => p.EngineerConfirmDetailList)
                .Show(ChildShowInWhere.All).OrderNo = 7;
            AddChildrenProperty();
        }

        /// <summary>
        /// 添加子标签
        /// </summary>
        protected void AddChildrenProperty()
        {
            View.ChildrenProperty(p => p.EquipRepairSparePartAplList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipRepairSparePartChgList).Show(ChildShowInWhere.Hide);

            View.ChildrenProperty(p => p.EquipRepairBillProjectList).HasLabel("维修规程").Show(ChildShowInWhere.All).OrderNo = 0;
            View.AttachDetailChildrenProperty(typeof(EquipRepairBill), (c) =>
            {
                var bill = c.Parent as EquipRepairBill;
                bill = RF.GetById<EquipRepairBill>(bill.Id, new EagerLoadOptions().LoadWith(EquipRepairBill.SupplierProperty).LoadWithViewProperty());
                return bill;
            }, "FaultInfoView").HasLabel("故障信息").OrderNo = 1;

            View.AttachDetailChildrenProperty(typeof(EquipRepairBill), (c) =>
            {
                var bill = c.Parent as EquipRepairBill;
                bill = RF.GetById<EquipRepairBill>(bill.Id, new EagerLoadOptions().LoadWithViewProperty());
                return bill;
            }, RepairRepoterViewGroup).HasLabel("维修报告").OrderNo = 2;

            View.AttachChildrenProperty(typeof(EquipRepairSparePartApl), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                if (args.Parent is not EquipRepairBill parent)
                {
                    return new EntityList<EquipRepairSparePartApl>();
                }
                var spareParts = RT.Service.Resolve<RepairController>().GetEquipRepairSparePartApls(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return spareParts;
            }, directionParentPropertyName: EquipRepairBill.EquipRepairSparePartAplListProperty.Name).HasLabel("备件申请").Show(ChildShowInWhere.All).OrderNo = 3;
            View.AttachChildrenProperty(typeof(EquipRepairSparePartChg), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                if (args.Parent is not EquipRepairBill parent)
                {
                    return new EntityList<EquipRepairSparePartChg>();
                }
                var spareParts = RT.Service.Resolve<RepairController>().GetEquipRepairSpareParts(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return spareParts;
            }, directionParentPropertyName: EquipRepairBill.EquipRepairSparePartChgListProperty.Name).HasLabel("备件更换").Show(ChildShowInWhere.All).OrderNo = 4;
            View.ChildrenProperty(p => p.EquipRepairWorkingHoursList).Show(ChildShowInWhere.All).OrderNo = 5;


            View.ChildrenProperty(p => p.EquipRepairOperationRecList).Show(ChildShowInWhere.All).OrderNo = 8;
            View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.All).OrderNo = 9;

        }

        /// <summary>
        /// 查询条件定义
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.SparePartCode);
            View.Property(p => p.SparePartName);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairBehavior");
            View.AssignAuthorize(typeof(EquipRepairBill));
            View.DefineFormChildSaveMode(FormChildSaveMode.Save);
            View.UseCommands(typeof(SaveEquipRepairCommand).FullName);
            View.RemoveCommands(ConfigCommands.ModuleConfigEditCommand);
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(4);
                View.Property(p => p.RepairNo).Readonly();
                View.Property(p => p.RepairType).DefaultValue(EquipRepairType.EquipRepair);

                #region 设备
                View.Property(p => p.EquipAccountId).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EquipController>().GetEquipAccountsAuth(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                     {
                         Dictionary<string, string> keyValues = new Dictionary<string, string>();
                         keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                         keyValues.Add(nameof(e.EquipAccountMode), "ModelName");
                         keyValues.Add(nameof(e.EquipAccountTypeName), "EquipTypeName");
                         keyValues.Add(nameof(e.ResourceName), "ResourceName");
                         keyValues.Add(nameof(e.UseDepartment), "UseDepartmentName");
                         keyValues.Add(nameof(e.InstallationLocation), nameof(e.EquipAccount.InstallationLocation));
                         keyValues.Add(nameof(e.ProcessName), "ProcessName");
                         keyValues.Add(nameof(e.WorkShopName), "WorkShopName");
                         m.DicLinkField = keyValues;
                     }).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).HasLabel("设备编码".L10N()+"*");
                View.Property(p => p.EquipAccountName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                View.Property(p => p.EquipAccountMode).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                View.Property(p => p.EquipAccountTypeName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                View.Property(p => p.ResourceName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                View.Property(p => p.UseDepartment).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                View.Property(p => p.InstallationLocation).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                View.Property(p => p.ProcessName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                View.Property(p => p.WorkShopName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly();
                #endregion

                #region 备件

                View.Property(p => p.SparePart).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    m.DicLinkField = keyValues;
                }).Visibility(p => p.RepairType == EquipRepairType.SparePartRepair).HasLabel("备件编码");
                View.Property(p => p.SparePartName).Readonly().Visibility(p => p.RepairType == EquipRepairType.SparePartRepair);
                #endregion

                View.Property(p => p.ApplyRepairEmployeeId).HasLabel("报修人").Readonly();
                View.Property(p => p.ApplyRepairDate).Readonly();

                View.AttachDetailChildrenProperty(typeof(EquipRepairBill), (c) =>
                {
                    var item = c.Parent as EquipRepairBill;
                    item = RF.GetById<EquipRepairBill>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return item;
                }, ErrorInfoGroup).HasLabel("故障信息录入").Show(ChildShowInWhere.Detail);
                View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(30)
                .Show(ChildShowInWhere.All).ViewGroup = "ApplyRepairView";

            }
        }

        ///<summary>
        /// 配置明细视图(其他功能的报修按钮）
        /// </summary>
        private void ConfigCreateRepairBillView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.CreateEquipRepairBillBehavior");
            View.AssignAuthorize(typeof(EquipRepairBill), typeof(RegularInspection), typeof(Lubrication), typeof(EquipAlarmRecord));
            View.UseCommands(typeof(SaveEquipRepairCommand).FullName);
            View.RemoveCommands(ConfigCommands.ModuleConfigEditCommand);

            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(4);
                View.Property(p => p.RepairNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.RepairType).Show(ShowInWhere.All).Readonly().DefaultValue(EquipRepairType.EquipRepair);

                #region 设备
                View.Property(p => p.EquipAccountId).Show(ShowInWhere.All).Readonly().HasLabel("设备编码");
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountMode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountTypeName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ResourceName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UseDepartment).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.InstallationLocation).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WorkShopName).Show(ShowInWhere.All).Readonly();
                #endregion

                View.Property(p => p.ApplyRepairEmployeeId).Show(ShowInWhere.All).HasLabel("报修人").Readonly();
                View.Property(p => p.ApplyRepairDate).Show(ShowInWhere.All).Readonly();

                View.AttachDetailChildrenProperty(typeof(EquipRepairBill), (c) =>
                {
                    var item = c.Parent as EquipRepairBill;
                    item = RF.GetById<EquipRepairBill>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return item;
                }, ErrorInfoGroup).HasLabel("故障信息录入").Show(ChildShowInWhere.Detail);
                View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(30)
                    .Show(ChildShowInWhere.All).ViewGroup = "ApplyRepairView";

            }
        }

        #region 子页签
        /// <summary>
        /// 故障信息录入
        /// </summary>
        protected void ErrorInfo()
        {
            View.AssignAuthorize(typeof(EquipRepairBill), typeof(RegularInspection), typeof(Lubrication), typeof(EquipAlarmRecord), typeof(CheckPlan));
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProduceState).Show(ShowInWhere.All).DefaultValue(ProduceState.Produce);
                View.Property(p => p.DeviceAbnormalId).UseDataSource((e, p, k) =>
                {
                    var entity = e as EquipRepairBill;
                    var result = RT.Service.Resolve<RepairController>().GetDeviceAbnormalsByRepairBill(entity, AbnormalType.Unusual, k, p);
                    return result;
                }).Show(ShowInWhere.All).HasLabel("故障现象".L10N()+"*");
                View.Property(p => p.UrgentDegree).Show(ShowInWhere.All).DefaultValue(UrgentDegree.Common);
                View.Property(p => p.DeviceAbnormalRemark).Show(ShowInWhere.All).ShowInDetail(columnSpan: 1, rowSpan: 2).UseMemoEditor();
                View.Property(p => p.DeviceAbnormalCode).Show(ShowInWhere.Hide);
            }
        }
        #endregion

        /// <summary>
        /// 设备台账维修单视图
        /// </summary>
        public void ConfigEquipAccountRepair()
        {
            View.AssignAuthorize(typeof(EquipAccount));
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairToolBarBehavior");
            View.UseCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.OpenRepairBillViewCommand");
            View.UseCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.EquipSearchRepairCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigEditCommand);

            using (View.OrderProperties())
            {
                View.Property(p => p.RepairState).Show().Readonly();
                View.Property(p => p.RepairNo).Show().Readonly();
                View.Property(p => p.SourceNo).Show().Readonly();
                View.Property(p => p.SourceType).Show().Readonly();
                View.Property(p => p.RepairType).Show().Readonly();
                View.Property(p => p.UrgentDegree).Show().Readonly();
                View.Property(p => p.ApplyRepairEmployeeId).Show().Readonly();
                View.Property(p => p.ApplyRepairDate).Show().Readonly();
                View.Property(p => p.RepairMasterId).Show().Readonly();
                View.Property(p => p.ReceiveOrderDate).Show().Readonly();
                View.Property(p => p.TransferOrderDate).Show().Readonly();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairBillProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairOperationRecList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairWorkingHoursList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairSparePartChgList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairSparePartAplList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.HandoverConfirmDetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EngineerConfirmDetailList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 设备维修接单视图
        /// </summary>
        protected void TakeOrderView()
        {
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccount).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>()
                    {
                        { nameof(e.EquipAccountName), nameof(e.EquipAccount.Name) },
                        { nameof(e.WarrantyPeriod), nameof(e.EquipAccount.WarrantyPeriod) }
                    };
                    m.DicLinkField = keyValues;
                }).Readonly().Visibility(p => p.RepairType == EquipRepairType.EquipRepair).HasLabel("设备编码").Show();
                View.Property(p => p.EquipAccountName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().Show();
                View.Property(p => p.WarrantyPeriod).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().Show();
                View.Property(p => p.EquipWarrantyState).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().HasLabel("保修状态").Show();

                View.Property(p => p.SparePartCode).Visibility(p => p.RepairType == EquipRepairType.SparePartRepair).Readonly().Show();
                View.Property(p => p.SparePart).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>()
                    {
                        { nameof(e.SparePartCode), nameof(e.SparePart.SparePartCode) }
                    };
                    m.DicLinkField = keyValues;
                }).Readonly().Visibility(p => p.RepairType == EquipRepairType.SparePartRepair).HasLabel("备件名称").Show();

                View.Property(p => p.RepairMaster).ShowInDetail(columnSpan: 2).Show().Readonly();
                View.Property(p => p.RepairEmployees).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(EquipEmployee).FullName;
                    p.XType = "MultiEmployeeComboPopup";
                    var dic = new Dictionary<string, string>
                    {
                        { EquipRepairBill.RepairEmployeeIdsProperty.Name, Employee.IdProperty.Name },
                        { EquipRepairBill.RepairEmployeesProperty.Name, Employee.NameProperty.Name }
                    };
                    p.MutiLinkField = dic.ToJsonString();
                    p.ValueField = Employee.IdProperty.Name;
                    p.DisplayField = Employee.NameProperty.Name;
                    p.Editable = false;
                    p.Separator = ",";
                }).ShowInDetail(columnSpan: 2).Show();
                View.Property(p => p.RepairWay).ShowInDetail(columnSpan: 2).Show().Readonly();
                View.Property(p => p.EstimateFinishDate).HasLabel("预计完成时间".L10N()+"*").ShowInDetail(columnSpan: 2).Show();
            }
        }

        /// <summary>
        /// 设备维修派工视图
        /// </summary>
        protected void DispatchRepairView()
        {
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                #region 设备
                View.Property(p => p.EquipAccount).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).HasLabel("设备编码").Readonly().Show();
                View.Property(p => p.EquipAccountName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).HasLabel("设备名称").Readonly().Show();
                View.Property(p => p.WarrantyPeriod).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().Show();
                View.Property(p => p.EquipWarrantyState).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().Show();
                #endregion

                #region 备件
                View.Property(p => p.SparePartCode).Visibility(p => p.RepairType == EquipRepairType.SparePartRepair).HasLabel("备件编码").Readonly().Show();
                View.Property(p => p.SparePartName).Visibility(p => p.RepairType == EquipRepairType.SparePartRepair).HasLabel("备件名称").Readonly().Show();

                #endregion

                View.Property(p => p.SourceType).Readonly().ShowInDetail();
                View.Property(p => p.RepairMaster).UseDataSource((e, c, r) =>
                {
                    EquipRepairBill bill = (EquipRepairBill)e;
                    if (bill.EquipAccountId == null || bill.EquipAccountId == 0)
                    {
                        return RT.Service.Resolve<DevicePurController>().GetSparePartDevicePurRepairs(r, c);
                    }
                    else
                    {
                        return RT.Service.Resolve<DevicePurController>().GetDevicePurRepairs((double)bill.EquipAccountId, r, c);
                    }
                }).HasLabel("维修责任人".L10N()+"*").ShowInDetail();

                View.Property(p => p.RepairEmployees).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(EquipEmployee).FullName;
                    p.XType = "MultiEmployeeComboPopup";
                    var dic = new Dictionary<string, string>
                    {
                        { EquipRepairBill.RepairEmployeeIdsProperty.Name, Employee.IdProperty.Name },
                        { EquipRepairBill.RepairEmployeesProperty.Name, Employee.NameProperty.Name }
                    };
                    p.MutiLinkField = dic.ToJsonString();
                    p.ValueField = Employee.IdProperty.Name;
                    p.DisplayField = Employee.NameProperty.Name;
                    p.Editable = false;
                    p.Separator = ",";
                }).ShowInDetail(columnSpan: 2);


                View.Property(p => p.RepairWay).ShowInDetail(columnSpan: 2).Show()
                    .Cascade(p => p.SupplierId, null)
                    .Cascade(p => p.Supplier, null)
                    .Cascade(p => p.SupplierName, null)
                    .Cascade(p => p.SendRepairWay, null)
                    .Cascade(p => p.DeliveryNo, null)
                    .Cascade(p => p.ContactPerson, null)
                    .Cascade(p => p.ContactPhone, null)
                    .Cascade(p => p.SendRepairDate, null)
                    .Cascade(p => p.PredictBackDate, null);
                View.Property(p => p.EstimateFinishDate).UseDateTimeEditor(p =>
                {
                    p.MinValue = DateTime.Now.AddDays(0);     
                }).HasLabel("预计完成时间".L10N()+"*").ShowInDetail(columnSpan: 2).Show();

                View.Property(p => p.Supplier).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>()
                    {
                        { nameof(e.SupplierName), nameof(e.Supplier.Name) }
                    };
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show();
                View.Property(p => p.SupplierName).Readonly().Show();
                View.Property(p => p.SendRepairWay).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All);
                View.Property(p => p.DeliveryNo).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All);
                View.Property(p => p.ContactPerson).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All);
                View.Property(p => p.ContactPhone).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All);
                View.Property(p => p.SendRepairDate).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All);
                View.Property(p => p.PredictBackDate).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 设备维修转派视图
        /// </summary>
        protected void TransferRepairView()
        {
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccount).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    keyValues.Add(nameof(e.WarrantyPeriod), nameof(e.EquipAccount.WarrantyPeriod));
                    m.DicLinkField = keyValues;
                }).Readonly().Visibility(p => p.RepairType == EquipRepairType.EquipRepair).HasLabel("设备编码").Show();
                View.Property(p => p.EquipAccountName).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().Show();
                View.Property(p => p.WarrantyPeriod).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().Show();
                View.Property(p => p.EquipWarrantyState).Visibility(p => p.RepairType == EquipRepairType.EquipRepair).Readonly().HasLabel("保修状态").Show();

                View.Property(p => p.SparePartCode).Visibility(p => p.RepairType == EquipRepairType.SparePartRepair).Readonly().Show();
                View.Property(p => p.SparePart).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>()
                    {
                        { nameof(e.SparePartCode), nameof(e.SparePart.SparePartCode) }
                    };
                    m.DicLinkField = keyValues;
                }).Readonly().Visibility(p => p.RepairType == EquipRepairType.SparePartRepair).HasLabel("备件名称").Show();
                View.Property(p => p.RepairMaster).UseDataSource((e, c, r) =>
                {
                    EquipRepairBill bill = (EquipRepairBill)e;
                    if (bill.EquipAccountId == null || bill.EquipAccountId == 0)
                    {
                        return RT.Service.Resolve<DevicePurController>().GetSparePartDevicePurRepairs(r, c);
                    }
                    else
                    {
                        return RT.Service.Resolve<DevicePurController>().GetDevicePurRepairs((double)bill.EquipAccountId, r, c);
                    }
                }).ShowInDetail(columnSpan: 2).Show().HasLabel("维修责任人".L10N()+"*");
                View.Property(p => p.RepairEmployees).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(EquipEmployee).FullName;
                    p.XType = "MultiEmployeeComboPopup";
                    var dic = new Dictionary<string, string>
                    {
                        { EquipRepairBill.RepairEmployeeIdsProperty.Name, Employee.IdProperty.Name },
                        { EquipRepairBill.RepairEmployeesProperty.Name, Employee.NameProperty.Name }
                    };
                    p.MutiLinkField = dic.ToJsonString();
                    p.ValueField = Employee.IdProperty.Name;
                    p.DisplayField = Employee.NameProperty.Name;
                    p.Editable = false;
                    p.Separator = ",";
                }).ShowInDetail(columnSpan: 2).Show();

                View.Property(p => p.RepairWay).ShowInDetail(columnSpan: 2).Show().HasLabel("派工类型".L10N() + "*")
                    .Cascade(p => p.SupplierId, null)
                    .Cascade(p => p.Supplier, null)
                    .Cascade(p => p.SupplierName, null)
                    .Cascade(p => p.SendRepairWay, null)
                    .Cascade(p => p.DeliveryNo, null)
                    .Cascade(p => p.ContactPerson, null)
                    .Cascade(p => p.ContactPhone, null)
                    .Cascade(p => p.SendRepairDate, null)
                    .Cascade(p => p.PredictBackDate, null);
                View.Property(p => p.TransferReason).HasLabel("转派原因".L10N()+"*").ShowInDetail(columnSpan: 2, rowSpan: 2).Show();
                View.Property(p => p.EstimateFinishDate).ShowInDetail(columnSpan: 2).Show().HasLabel("预计完成时间".L10N() + "*");

                View.Property(p => p.Supplier).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>()
                    {
                        { nameof(e.SupplierName), nameof(e.Supplier.Name) }
                    };
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show().HasLabel("供应商".L10N()+"*");
                View.Property(p => p.SupplierName).Readonly().Show().HasLabel("供应商名称".L10N() + "*");
                View.Property(p => p.SendRepairWay).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All).HasLabel("送修方式".L10N() + "*");
                View.Property(p => p.DeliveryNo).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All);
                View.Property(p => p.ContactPerson).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All).HasLabel("联系人".L10N() + "*");
                View.Property(p => p.ContactPhone).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All).HasLabel("联系电话".L10N() + "*");
                View.Property(p => p.SendRepairDate).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All).HasLabel("外修时间".L10N() + "*");
                View.Property(p => p.PredictBackDate).Readonly(p => p.RepairWay == EquipRepairWay.InnerRepair).Show(ShowInWhere.All).HasLabel("预计返厂时间".L10N() + "*");
            }
        }

        /// <summary>
        /// 设备维修交机确认视图
        /// </summary>
        protected void HandoverConfirmView()
        {
            View.AssignAuthorize(typeof(HandoverConfirmDetail));
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.HandoverConfirmResult).HasLabel("交换机确认结果*").Show();
                View.Property(p => p.HandoverConfirmEmployee).Readonly().Show();
            }
            View.AttachChildrenProperty(typeof(HandoverConfirmDetail), (e) =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity.ToJsonObject<EquipRepairBill>();
                if (parent == null)
                {
                    return new EntityList<HandoverConfirmDetail>();
                }
                else
                {
                    List<TpmWeekInspectScore> tpmlist = RF.GetAll<TpmWeekInspectScore>().Where(p => p.ScoreType == ScoreType.Repair).ToList();
                    EntityList<HandoverConfirmDetail> confirmList = new EntityList<HandoverConfirmDetail>();
                    tpmlist.ForEach(tpm =>
                    {
                        HandoverConfirmDetail detail = new HandoverConfirmDetail()
                        {
                            TpmWeekInspectScoreId = tpm.Id,
                            TpmWeekInspectScore = tpm,
                            ProjectName = tpm.ProjectName,
                        };
                        confirmList.Add(detail);
                    });
                    return confirmList;
                }
            }, HandoverConfirmViewGroup)
            .Show(ChildShowInWhere.All).HasLabel("评分项");

            View.AttachDetailChildrenProperty(typeof(EquipRepairBill), (c) =>
            {
                var item = c.Parent as EquipRepairBill;
                item = RF.GetById<EquipRepairBill>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                item.HandoverConfirmAbnormal = HandoverConfirmAbnormal.NotResolve;
                item.HandoverDeviceAbnormalId = null;
                item.HandoverDeviceAbnormal = null;
                item.HandoverDeviceAbnormalRem = "";
                item.HandoverAttachment = "";
                return item;
            }, AbnormalInfoViewGroup).HasLabel("故障信息输入").Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 设备维修工程确认视图
        /// </summary>
        protected void EngineerConfirmView()
        {
            View.AssignAuthorize(typeof(EngineerConfirmDetail));
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.RespondTime).Readonly().Show();
                View.Property(p => p.ExecuteTime).Readonly().Show();
                View.Property(p => p.RepairTotalTime).Readonly().Show();
            }
            View.AttachChildrenProperty(typeof(EngineerConfirmDetail), (e) =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity.ToJsonObject<EquipRepairBill>();
                if (parent == null)
                {
                    return new EntityList<EngineerConfirmDetail>();
                }
                else
                {
                    List<TpmWeekInspectScore> tpmlist = RF.GetAll<TpmWeekInspectScore>().Where(p => p.ScoreType == ScoreType.Repair).ToList();
                    EntityList<EngineerConfirmDetail> confirmList = new EntityList<EngineerConfirmDetail>();
                    tpmlist.ForEach(tpm =>
                    {
                        EngineerConfirmDetail detail = new EngineerConfirmDetail()
                        {
                            TpmWeekInspectScoreId = tpm.Id,
                            TpmWeekInspectScore = tpm,
                            ProjectName = tpm.ProjectName,
                        };
                        confirmList.Add(detail);
                    });
                    return confirmList;
                }
            }, EngineerConfirmViewGroup)
            .Show(ChildShowInWhere.All).HasLabel("评分项");
        }

        /// <summary>
        /// 交机确认故障信息视图
        /// </summary>
        protected void AbnormalInfoView()
        {
            View.AssignAuthorize(typeof(HandoverConfirmDetail));
            View.HasDetailColumnsCount(1);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccountId).Show(ShowInWhere.Hide);
                View.Property(p => p.HandoverConfirmAbnormal).HasLabel("异常情况".L10N()+"*").Show();
                View.Property(p => p.HandoverDeviceAbnormal).HasLabel("故障现象".L10N() + "*").UseDataSource((e, p, k) =>
                {
                    var equipRepairBill = e as EquipRepairBill;
                    var result = RT.Service.Resolve<RepairController>().GetDeviceAbnormalsByRepairBill(equipRepairBill, AbnormalType.Unusual, k, p);
                    return result;
                }).Show();
                View.Property(p => p.HandoverDeviceAbnormalRem).UseMemoEditor().Show();
                View.Property(p => p.HandoverAttachment).UseConfigValueEditor(p =>
                {
                    p.XType = "uploadfileeditor_abnormal";
                    p.AllowBlank = false;
                    p.TriggerCls = "iconfont icon-ArrowUpBold1";
                    p.AllowAsterisk = AllowAsterisks.close;
                }).Show().HasLabel("文件路径(上传)");
            }
        }

        /// <summary>
        /// 故障信息视图
        /// </summary>
        public void FaultInfoView()
        {
            View.AssignAuthorize(typeof(HandoverConfirmDetail));
            View.FormEdit();
            View.ClearCommands();
            View.HasDetailColumnsCount(6);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProduceState).ShowInDetail(columnSpan: 3).Readonly();
                View.Property(p => p.UrgentDegree).ShowInDetail(columnSpan: 3).Readonly();
                View.Property(p => p.DeviceAbnormal).ShowInDetail(columnSpan: 3).Readonly();
                
                View.Property(p => p.DeviceAbnormalCode).Show(ShowInWhere.Hide).UseMemoEditor();
                View.Property(p => p.RepairWay).ShowInDetail(columnSpan: 3).Readonly();
                View.Property(p => p.DeviceAbnormalRemark).ShowInDetail(columnSpan: 6).UseMemoEditor().Readonly();
                View.Property(p => p.Supplier).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.SupplierName).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ContactPerson).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.SendRepairWay).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.DeliveryNo).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.ContactPhone).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.SendRepairDate).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.PredictBackDate).ShowInDetail(columnSpan: 2).Readonly();
            }
        }

        /// <summary>
        /// 维修报告视图
        /// </summary>
        public void RepairRepoterView()
        {
            View.DomainName("维修报告");
            View.FormEdit();
            View.HasDetailColumnsCount(3);
            
            View.ClearCommands();
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Scripts.EquipRepairReportBehavior");
            View.UseCommands("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ShowExperienceDepotCommand"
                , "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddExperienceDepotCommand"
                , "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.ReportDownloadCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.RepairWay).ShowInDetail(columnSpan: 1).Readonly();
                View.Property(p => p.OutsourcedMaintenanceReport).UseConfigValueEditor(p =>
                {
                    p.XType = "uploadfileeditor_repoter";
                    p.AllowBlank = true;
                    p.TriggerCls = "iconfont icon-ArrowUpBold1";
                }).Readonly(p => p.RepairWay != EquipRepairWay.OuterRepair).ShowInDetail(columnSpan: 1);

                View.Property(p => p.SourceType).ShowInDetail(columnSpan: 1);


                View.Property(p => p.FaultDescription).UseDataSource((e, p, k) =>
                {
                    var equipRepairBill = e as EquipRepairBill;
                    var result = RT.Service.Resolve<RepairController>().GetDeviceAbnormalsByRepairBill(equipRepairBill, AbnormalType.Fault, k, p);
                    return result;
                }).UsePagingLookUpEditor(p => p.AllowBlank = false).ShowInDetail(columnSpan: 3);

                View.Property(p => p.ProjectId).Show(ShowInWhere.Hide);
                View.Property(p => p.ProjectKeyItemId).Show(ShowInWhere.Hide);


                View.Property(p => p.FaultDescriptionRemark).ShowInDetail( columnSpan: 3).UseMemoEditor();
                View.Property(p => p.DeviceAbnormalCode).Show(ShowInWhere.Hide);
                View.Property(p => p.FaultReason).UseCatalogEditor(p => { p.CatalogType = EquipRepairBill.CatalogExpFaultReson; p.AllowBlank = false; p.CatalogReloadData = true; }).ShowInDetail(columnSpan: 1);
                View.Property(p => p.FaultPart).Show(ShowInWhere.Hide);
                View.Property(p => p.FaultCategory).UsePagingLookUpEditor(p => p.AllowBlank = false).ShowInDetail(columnSpan: 1);
                View.Property(p => p.FaultLevel).UseEnumEditor(p => p.AllowBlank = false).ShowInDetail(columnSpan: 1);
                View.Property(p => p.RepairCategory).UseEnumEditor(p => p.AllowBlank = false).ShowInDetail(columnSpan: 1);
                View.Property(p => p.RepairLevel).UseEnumEditor(p => p.AllowBlank = false).ShowInDetail(columnSpan: 1);
                View.Property(p => p.RepairCosts).UseSpinEditor(e => e.DecimalPrecision = 2).Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartsCost).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.MinValue = 0;
                    p.DecimalPrecision = 2;
                }).Show(ShowInWhere.Hide);

                View.Property(p => p.RepairDowntime).ShowInDetail(columnSpan: 1).Show(ShowInWhere.Hide);

                View.Property(p => p.RepairHours).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.MinValue = 0;
                    p.DecimalPrecision = 1;
                }).UseSpinEditor().ShowInDetail(columnSpan: 1).Readonly();
                View.Property(p => p.RepairMethod).ShowInDetail(columnSpan: 3)
                   .UseMemoEditor(p => p.AllowBlank = false);
                View.Property(p => p.PreventionAdvice).ShowInDetail(columnSpan: 3)
                   .UseMemoEditor();
            }

        }
    }

    /// <summary>
    /// 员工视图
    /// </summary>
    public class EmployeeExtensionWebViewConfig : WebViewConfig<EquipEmployeeCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.EquipAccountId)
                .UsePagingLookUpEditor((m, e) =>
                {
                    m.BindDisplayField = EquipEmployeeCriteria.EquipAccountNameProperty.Name;
                    m.ReloadDataOnPopping = true;
                    var keyValues = new Dictionary<string, string>
                    {
                        { nameof(e.EquipAccountName), nameof(e.EquipAccount.Name) }
                    };
                    m.DicLinkField = keyValues;
                });
        }
    }

    /// <summary>
    /// 扩展员工弹框列表
    /// </summary>
    public class EquipEmployeeViewConfig : WebViewConfig<EquipEmployee>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).HasLabel("编码").Show();
            View.Property(p => p.Name).HasLabel("名称").Show();
            View.Property(p => p.Sex).Show(ShowInWhere.Hide);
            View.Property(p => p.EmployeeGroup).Show(ShowInWhere.Hide);
            View.Property(p => p.WorkGroup).Show(ShowInWhere.Hide);
            View.Property(p => p.HireDate).Show(ShowInWhere.Hide);
            View.Property(p => p.EmployeeStatus).Show(ShowInWhere.Hide);
            View.Property(p => p.Phone).Show(ShowInWhere.Hide);
            View.Property(p => p.Email).Show(ShowInWhere.Hide);
            View.Property(p => p.Remark).Show(ShowInWhere.Hide);
            View.Property(p => p.User).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
