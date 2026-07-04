using DevExpress.XtraRichEdit.Model;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.DockMaintains;
using SIE.Dock.ViewModels;
using SIE.Dock.YardZones;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using SIE.Web.Dock.DockAppoints.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.Dock.DockAppoints
{
    /// <summary>
	/// 月台预约视图配置
	/// </summary>
	internal class DockAppointViewConfig : WebViewConfig<DockAppoint>
    {
        public const string ReadOnlyView = "ReadOnlyView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadOnlyView);
            View.AssignAuthorize(typeof(DockAppoint));
            if (ViewGroup == ReadOnlyView)
                ConfigReadOnlyView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(AddDockAppointCommand).FullName, typeof(EditDockAppointCommand).FullName, typeof(CancelAppointCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.UseCommands(typeof(PrintDockAppointCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 160);
                View.Property(p => p.DockMaintainCode);
                View.Property(p => p.DockMaintainName);
                View.Property(p => p.YardZoneId);
                View.Property(p => p.AppointType);
                View.Property(p => p.BillNo);
                View.Property(p => p.AppointStartDate).UseDateTimeEditor().ShowInList(width: 150);
                View.Property(p => p.AppointEndDate).UseDateTimeEditor().ShowInList(width: 150);
                View.Property(p => p.UseHours);
                View.Property(p => p.CompanyName);
                View.Property(p => p.CarNum);
                View.Property(p => p.Contacts);
                View.Property(p => p.ContactNum);
                View.Property(p => p.IDNumber);
                View.Property(p => p.Remark);
                View.Property(p => p.IsCancelAppoint).Readonly();
                View.Property(p => p.CancelAppointBy);
                View.Property(p => p.CancelAppointDate).UseDateTimeEditor();
                View.Property(p => p.CancelReason);
                View.Property(p => p.DockSourceType);
                View.Property(p => p.WeChatID);
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
                View.Property(p => p.No).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.AppointType).HasLabel("预约类型".L10N() + "*").ShowInDetail(columnSpan: 2).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.BillNo).UseTextEditor(x => x.AllowAsterisk = AllowAsterisks.enable).HasLabel("单据号").ShowInDetail(columnSpan: 2);
                View.Property(p => p.YardZoneId).UseDataSource((o, c, r) =>
                {
                    var dockAppoint = o as DockAppoint;
                    if (dockAppoint == null)
                    {
                        return new EntityList<YardZone>();
                    }

                    return RT.Service.Resolve<YardZoneService>().GetEnableYardZones(r, c);
                }).HasLabel("预约地点").ShowInDetail(columnSpan: 2).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.CompanyName).ShowInDetail(columnSpan: 2);
                View.Property(p => p.CarNum).HasLabel("车牌号".L10N()+"*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.Contacts).HasLabel("联系人".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.ContactNum).HasLabel("联系电话".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.IDNumber).HasLabel("身份证号".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.AppointDate).UseDateEditor(p => p.MinValue = DateTime.Now.Date.ToString()).HasLabel("预约日期".L10N() + "*").ShowInDetail(columnSpan: 2).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.AppointDock).HasLabel("预约时段".L10N() + "*").UseSelectAppointDockEditor((p, m) =>
                {
                    p.Editable = false;
                    p.DisplayField = SelectAppointDockViewModel.AppointTimeDisplayProperty.Name;
                    p.ValueField = SelectAppointDockViewModel.AppointTimeDisplayProperty.Name;
                    p.ReloadDataOnPopping = true;
                    var keyValues = new Dictionary<string, string>
                    {
                        { nameof(m.AppointStartDate), SelectAppointDockViewModel.StartDateProperty.Name },
                        { nameof(m.AppointEndDate), SelectAppointDockViewModel.EndDateProperty.Name },
                        { nameof(m.UseHours), SelectAppointDockViewModel.AppointUseTimeProperty.Name }
                    };
                    p.DicLinkField = keyValues;
                }).ShowInDetail(columnSpan: 2).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.UseHours).HasLabel("预计占用(H)".L10N() + "*").UseSelectUseHoursEditor().Visibility(p => p.CreateBy <= 0).ShowInDetail(columnSpan: 2);
                View.Property(p => p.UseHoursDisplay).HasLabel("预计占用(H)".L10N() + "*").Visibility(p => p.CreateBy > 0).ShowInDetail(columnSpan: 2).Readonly(p => p.CreateBy > 0);
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected void ConfigReadOnlyView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(6);
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.AppointType).HasLabel("预约类型".L10N() + "*").ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.BillNo).HasLabel("单据号".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.YardZoneId).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.CompanyName).ShowInDetail(columnSpan: 2);
                View.Property(p => p.CarNum).HasLabel("车牌号".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.Contacts).HasLabel("联系人".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.ContactNum).HasLabel("联系电话".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.IDNumber).HasLabel("身份证号".L10N() + "*").ShowInDetail(columnSpan: 2);
                View.Property(p => p.AppointDate).UseDateEditor().HasLabel("预约日期".L10N() + "*").ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.AppointDock).HasLabel("预约时段".L10N() + "*").ShowInDetail(columnSpan: 2).Visibility(p => p.CreateBy > 0).Readonly();
                View.Property(p => p.UseHours).HasLabel("预计占用(H)".L10N() + "*").ShowInDetail(columnSpan: 2).Readonly();
            }
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 160);
                View.Property(p => p.DockMaintainId);
                View.Property(p => p.DockMaintainName);
                View.Property(p => p.YardZoneId);
                View.Property(p => p.AppointType);
                View.Property(p => p.BillNo);
                View.Property(p => p.AppointStartDate).UseDateTimeEditor().ShowInList(width: 150);
                View.Property(p => p.AppointEndDate).UseDateTimeEditor().ShowInList(width: 150);
                View.Property(p => p.CompanyName);
                View.Property(p => p.CarNum);
                View.Property(p => p.Contacts);
                View.Property(p => p.ContactNum);
                View.Property(p => p.IDNumber);
            }
        }
    }
}