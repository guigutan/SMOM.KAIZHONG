using SIE.Domain;
using SIE.Kit.MES.CallMaterials;
using SIE.Kit.MES.Storages;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using SIE.Web.Kit.MES.CallMaterials.Commands;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料单视图配置
    /// </summary>
    public class CallMaterialBillViewConfig : WebViewConfig<CallMaterialBill>
    {
        /// <summary>
        /// 转移工位
        /// </summary>
        public const string ChangeStationView = "ChangeStationView";

        /// <summary>
        /// 发料模拟（后续wms发料功能完善则去除）
        /// </summary>
        public const string IssueView = "ChangeStationView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CallMaterialWorkOrder));
            View.DeclareExtendViewGroup(ChangeStationView, IssueView);
            if (ViewGroup == ChangeStationView)
            {
                ChangeStationConfigView();
            }
            if (ViewGroup == IssueView)
                ConfigIssueView();
        }
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Kit.MES.CallMaterials.Scripts.CallMaterialBehavior");
            View.RemoveCommands();
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(SetUrgencyCommand).FullName, typeof(CancelCallMaterialCommand).FullName, typeof(ChangeStationCommand).FullName, WebCommandNames.ExportXls);
                View.Property(p => p.Status).Readonly().UseEnumEditor();
                View.Property(p => p.No).Readonly();
                View.Property(p => p.Priority).Readonly().UseEnumEditor();
                View.Property(p => p.Station).Readonly();
                View.Property(p => p.StorageArea).Readonly().UseDataSource((e, p, r) =>
                {
                    var bill = e as CallMaterialBill;
                    var rst = new EntityList<StorageArea>();
                    if (e != null && bill.Station != null && bill.StationId > 0)
                    {
                        rst.Add(RT.Service.Resolve<StorageController>().GetInputAreaByStationId(bill.StationId));
                    }

                    return rst;
                }).UseListSetting(e => { e.HelpInfo = "显示当前配送工位下的工位货区"; });
                View.Property(p => p.RequiredTime).Readonly().HasLabel("预计需求时间");
                View.Property(p => p.SendingTime).Readonly();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
            View.ChildrenProperty(p => p.DetailList);
            View.ChildrenProperty(p => p.ReasonList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 客制视图
        /// </summary>
        void ChangeStationConfigView()
        {
            View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
            {
                var entity = e as CallMaterialBill;
                var bill = RF.GetById<CallMaterialBill>(entity.Id);
                if (bill?.Station == null) return new EntityList<WipResource>();
                return RT.Service.Resolve<StationController>().GetWipResourceByStation(bill.ProcessId, c, r);
            }).UsePagingLookUpEditor();

            View.Property(p => p.Station).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
            {
                var entity = e as CallMaterialBill;
                var bill = RF.GetById<CallMaterialBill>(entity.Id);
                if (bill?.Station == null || bill?.Resource == null) return new EntityList<Station>();
                return RT.Service.Resolve<StationController>().GetStations(bill.ResourceId, bill.ProcessId, c, r);
            }).UsePagingLookUpEditor();
        }

        /// <summary>
        /// 发料模拟（后续wms发料功能完善则去除）
        /// </summary>
        private void ConfigIssueView()
        {
            View.AssignAuthorize(typeof(CallMaterialReceive));
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).HasLabel("工单号").ShowInList();
                View.Property(p => p.No).ShowInList();
                View.Property(p => p.Status).UseEnumEditor().ShowInList();
                View.Property(p => p.Priority).UseEnumEditor().ShowInList();
                View.Property(p => p.Station).ShowInList();
                View.Property(p => p.Resource).ShowInList();
                View.Property(p => p.RequiredTime).ShowInList();
                View.Property(p => p.SendingTime).ShowInList();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DetailList).UseViewGroup(IssueView);
                View.ChildrenProperty(p => p.ReasonList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
