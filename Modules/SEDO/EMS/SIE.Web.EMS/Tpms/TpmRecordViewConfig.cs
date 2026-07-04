using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Tpms;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.EMS.Tpms.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Tpms
{
    /// <summary>
    /// TPM操作记录视图配置
    /// </summary>
    public class TpmRecordViewConfig : WebViewConfig<TpmRecord>
    {
        /// <summary>
        /// 添加TPM操作记录视图
        /// </summary>
        public const string TpmRecordAddView = "TpmRecordAddView";

        /// <summary>
        /// 查看TPM操作记录视图
        /// </summary>
        public const string TpmRecordLookView = "TpmRecordLookView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(TpmRecordAddView, TpmRecordLookView);
            if (ViewGroup == TpmRecordAddView)
                AddView();
            else if (ViewGroup == TpmRecordLookView)
                LookView();
        }

        /// <summary>
        /// 添加数据时 列表显示视图
        /// </summary>
        protected void AddView()
        {
            View.HasDetailColumnsCount(3);
            View.UseCommands(typeof(SaveRecordCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.Tpms.Scripts.AddRecordBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.TpmNo).Readonly().Show(ShowInWhere.Detail).HasLabel("单号");
                View.Property(p => p.Equipment).Show(ShowInWhere.Detail).HasLabel("设备编号").UsePagingLookUpEditor((m, e) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.MachineNo), nameof(e.Equipment.Name));
                    keyValues.Add(nameof(e.WorkShopName), "WorkShopId_Display");
                    keyValues.Add(nameof(e.ProcessName), "ProcessId_Display");
                    keyValues.Add(nameof(e.WorkGroup), null);
                    keyValues.Add(nameof(e.WorkGroupId), null);
                    m.DicLinkField = keyValues;
                }).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<EquipAccountController>().GetAllEquipAccounts(c, r);
                });
                View.Property(p => p.MachineNo).Readonly().Show(ShowInWhere.Detail);
                View.Property(p => p.WorkShopName).Readonly().Show(ShowInWhere.Detail);
                View.Property(p => p.ProcessName).Readonly().Show(ShowInWhere.Detail);
                View.Property(p => p.WorkGroup).Show(ShowInWhere.Detail).UseDataSource((e, page, code) =>
                {
                    var tmpActionRecord = e as TpmRecord;
                    if (tmpActionRecord != null && tmpActionRecord.EquipmentId != 0 && tmpActionRecord.Equipment.WorkShopId.HasValue)
                    {
                        return RT.Service.Resolve<TpmController>().QueryWorkGroupByWorkshopId(tmpActionRecord.Equipment.WorkShopId.Value, code, page);
                    }
                    return new EntityList<WorkGroup>();
                });
                View.AttachChildrenProperty(typeof(TpmRecordDetail), (e) =>
                {
                    return RT.Service.Resolve<TpmController>().GetTpmRecordDetails();
                }).HasLabel("操作明细").Show(ChildShowInWhere.Detail);
            }
        }

        /// <summary>
        /// 添加数据时 列表显示视图
        /// </summary>
        protected void LookView()
        {
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.TpmNo).Readonly().Show(ShowInWhere.All).HasLabel("单号");
                View.Property(p => p.Equipment).Show(ShowInWhere.All).HasLabel("设备编号").Readonly();
                View.Property(p => p.MachineNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WorkShopName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ProcessName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WorkGroup).Show(ShowInWhere.All).Readonly();
                View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.All);
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(AddRecordCommand).FullName, "SIE.Web.EMS.Tpms.Commands.SearchScoreDetailCommand", ConfigCommands.ModuleConfigCommand, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);

            View.Property(p => p.ExecutionTime);
            View.Property(p => p.TpmNo).Readonly();
            View.Property(p => p.EquipTypeCode).Readonly();
            View.Property(p => p.MachineNo).Readonly();
            View.Property(p => p.WorkShopName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.WorkGroup).Readonly();
            View.Property(p => p.TotalScore).Readonly();
            View.Property(p => p.ScorerName).Readonly();
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);//隐藏
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);//隐藏
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);//隐藏
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);//隐藏
            View.ChildrenProperty(p => p.DetailList).IsVisible = false;
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(3);
            View.Property(p => p.TpmNo).Readonly().Show(ShowInWhere.All).HasLabel("单号");
            View.Property(p => p.Equipment).Show(ShowInWhere.All).HasLabel("设备编号").Readonly();
            View.Property(p => p.MachineNo).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.WorkShopName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.ProcessName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.WorkGroup).Show(ShowInWhere.All).Readonly();
            View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.All);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipTypeCode);
            View.Property(p => p.WorkShopName);
            View.Property(p => p.ProcessName);
            View.Property(p => p.ExecutionTime);
        }
    }
}
