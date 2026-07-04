using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.DockQueues;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.Dock.DockQueues.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.Dock.DockQueues
{
    /// <summary>
    /// 月台排队视图配置
    /// </summary>
    internal class DockQueueViewConfig : WebViewConfig<DockQueue>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(AddSceneDockQueueCommand).FullName, typeof(AddAppointDockQueueCommand).FullName, typeof(CancelDockQueueCommand).FullName, typeof(AssignDockCommand).FullName);
            View.UseCommands(typeof(DelayDockQueueCommand).FullName, typeof(CheckInQueueCommand).FullName, typeof(CheckOutQueueCommand).FullName, typeof(UpDockQueueCommand).FullName, 
                typeof(DownDockQueueCommand).FullName, typeof(PrintDockQueueCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.QueueState);
                View.Property(p => p.AppointType);
                View.Property(p => p.TakeNoWay);
                View.Property(p => p.QueuePriority);
                View.Property(p => p.DockAppointId).ShowInList(width: 150);
                View.Property(p => p.AppointDockCode);
                View.Property(p => p.DelayNum);
                View.Property(p => p.YardZoneId);
                View.Property(p => p.AssignDockId);
                View.Property(p => p.AssignDockName);
                View.Property(p => p.DistributionTime);
                View.Property(p => p.LastDistriTime);
                View.Property(p => p.BillNo);
                View.Property(p => p.CompanyName);
                View.Property(p => p.CarNum);
                View.Property(p => p.Contacts);
                View.Property(p => p.ContactNum);
                View.Property(p => p.IDNumber);
                View.Property(p => p.Remark);
                View.Property(p => p.CheckInTime);
                View.Property(p => p.CheckOutTime);
                View.Property(p => p.JobTime);
                View.Property(p => p.WeChatID);
                View.Property(p => p.CancelByName);
                View.Property(p => p.CancelTime);
                View.Property(p => p.CancelReason);
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(6);
            using (View.OrderProperties())
            {
                View.Property(p => p.DockAppointId).UseDataSource((o, c, r) =>
                {
                    var dockQueue = o as DockQueue;
                    if (dockQueue == null)
                    {
                        return new EntityList<DockAppoint>();
                    }

                    return RT.Service.Resolve<DockAppointService>().GetSelectDockAppoints(r, c);
                }).UsePagingLookUpEditor((c, p) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(p.BillNo), nameof(p.DockAppoint.BillNo));
                    dic.Add(nameof(p.AppointType), nameof(p.DockAppoint.AppointType));
                    dic.Add(nameof(p.YardZoneId), nameof(p.DockAppoint.YardZoneId));
                    dic.Add(nameof(p.YardZoneName), nameof(p.DockAppoint.YardZoneName));
                    dic.Add(nameof(p.CompanyName), nameof(p.DockAppoint.CompanyName));
                    dic.Add(nameof(p.CarNum), nameof(p.DockAppoint.CarNum));
                    dic.Add(nameof(p.Contacts), nameof(p.DockAppoint.Contacts));
                    dic.Add(nameof(p.ContactNum), nameof(p.DockAppoint.ContactNum));
                    dic.Add(nameof(p.IDNumber), nameof(p.DockAppoint.IDNumber));
                    c.DicLinkField = dic;
                }).HasLabel("预约号".L10N() + "*").ShowInDetail(columnSpan: 6).Visibility(p => p.TakeNoWay == TakeNoWay.Appoint);
                View.Property(p => p.No).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.AppointType).HasLabel("排队类型".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.BillNo).HasLabel("单据号".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.YardZone).UseDataSource((o, c, r) =>
                {
                    var dockQueue = o as DockQueue;
                    if (dockQueue == null)
                    {
                        return new EntityList<YardZone>();
                    }

                    return RT.Service.Resolve<YardZoneService>().GetEnableYardZones(r, c);
                }).UsePagingLookUpEditor(p =>
                {
                    p.DisplayField = YardZone.NameProperty.Name;
                    p.BindDisplayField = "YardZoneName";
                }).HasLabel("排队地点".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.CompanyName).ShowInDetail(columnSpan: 2);
                View.Property(p => p.CarNum).HasLabel("车牌号".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.Contacts).HasLabel("联系人".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.ContactNum).HasLabel("联系电话".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.IDNumber).HasLabel("身份证号".L10N() + "*").ShowInDetail(columnSpan: 2);
            }
        }
    }
}