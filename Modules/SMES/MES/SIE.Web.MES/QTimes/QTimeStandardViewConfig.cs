using SIE.Items;
using SIE.MES.QTimes;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Web.MES.QTimes.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.QTimes
{
    /// <summary>
    /// QT标准维护
    /// </summary>
    public class QTimeStandardViewConfig : WebViewConfig<QTimeStandard>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Copy, "SIE.Web.MES.QTimes.Commands.QTimeStandardEditCommand", WebCommandNames.Delete, typeof(QTimeStandardSaveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Product).UseDataSource((e,p,k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemDatas(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    m.DicLinkField = keyValuePairs;
                }).HasLabel("产品编码").ShowInList(width:120);
                View.Property(p => p.ProductName).Readonly().ShowInList(width:120);
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(p, k);
                }).ShowInList(width:120);
                View.Property(p => p.Factory).Readonly().ShowInList(width:120);
                View.Property(p => p.StartProcess).UseDataSource((e,p,k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add(nameof(e.StartProcessType), nameof(e.StartProcess.Type));
                    m.DicLinkField = keyValuePairs;
                }).ShowInList(width:120);
                View.Property(p => p.StartState).UseEnumEditor(p => p.XType = "QTStartStateComboList").ShowInList(width:120);
                View.Property(p => p.EndProcess).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add(nameof(e.EndProcessType), nameof(e.EndProcess.Type));
                    m.DicLinkField = keyValuePairs;
                }).ShowInList(width:120);
                View.Property(p => p.EndState).UseEnumEditor(p => p.XType = "QTEndStateComboList").ShowInList(width:120);
                View.Property(p => p.Time).ShowInList(width:120);
                View.Property(p => p.TimeUnit).ShowInList(width:120);
                View.Property(p => p.PushPlug).ShowInList(width: 120);
                View.Property(p => p.MessageTemplate).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.MES.QTimes.Scripts.QTimeStandardMessageEditor"; p.Editable = false; }).ShowInList(width: 120);
                View.Property(p => p.IsAlert).Show(ShowInWhere.Hide);
                View.Property(p => p.State).ShowInList(width:120);
                View.ChildrenProperty(p => p.QTPushObjectList).HasLabel("推送对象");
            }
        }
    }
}
