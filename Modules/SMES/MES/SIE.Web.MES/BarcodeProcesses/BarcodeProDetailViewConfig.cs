using SIE.Defects;
using SIE.MES.BarcodeProcesses;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Web.MES.BarcodeProcesses.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派明细视图配置
    /// </summary>
    public class BarcodeProDetailViewConfig : WebViewConfig<BarcodeProDetail>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.BarcodeProcesses.Commands.SynWoProcessListCommand", typeof(AfterAddProcessDetailCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, typeof(WoProcessDetailSaveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberIndex).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; }).ShowInList();
                View.Property(p => p.Process).HasLabel("工序编码").Readonly().ShowInList();
                View.Property(p => p.ProcessName).Readonly().ShowInList();
                View.Property(p => p.EmployeeJoinNames).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Employee).FullName;
                    p.LinkField = BarcodeProDetail.EmployeeIdsProperty.Name;
                    p.DisplayField = Employee.NameProperty.Name;
                    p.XType = "EmployeeMultiComboPopup";
                    p.Editable = false;
                    p.Separator = ",";
                }).ShowInList(150);
                View.Property(p => p.IsCheck).ShowInList();
            }
        }
    }
}
