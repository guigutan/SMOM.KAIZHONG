using SIE.MES.BarcodeProcesses;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派
    /// </summary>
    public class BarcodeProcessViewConfig : WebViewConfig<BarcodeProcess>
    {
        /// <summary>
        /// 菜单主视图
        /// </summary>
        public const string MainListViewStr = "MainListViewStr";

        #region 条码范围 SnRange
        /// <summary>
        /// 条码范围
        /// </summary>
        public static readonly Property<string> SnRangeProperty = P<BarcodeProcess>.RegisterExtensionReadOnly("SnRange", typeof(BarcodeProcessViewConfig),
            GetSnRangeProperty, BarcodeProcess.RangeIdProperty);

        /// <summary>
        /// 获取条码范围
        /// </summary>
        /// <param name="me">条码</param>
        /// <returns>条码范围</returns>
        public static string GetSnRangeProperty(BarcodeProcess me)
        {
            return $"{me.StartSn}-{me.EndSn}";
        }
        #endregion

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(MainListViewStr);
            if (ViewGroup == MainListViewStr)
            {
                MainListView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        private void MainListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrder).Readonly().ShowInList(150);
                View.Property(p => p.Sn).Readonly().ShowInList(150);
                View.Property(p => p.AssignState).UseEnumEditor().Readonly().ShowInList();
                View.Property(SnRangeProperty).HasLabel("条码范围").Readonly().ShowInList(300);
                View.Property(p => p.Qty).Readonly().ShowInList();
                View.Property(p => p.BoxesQty).Readonly().ShowInList();
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Readonly().ShowInList();
                View.Property(p => p.PrintDate).Readonly().ShowInList(150);
                View.Property(p => p.PrinterName).HasLabel("打印人").Readonly().ShowInList();
                View.Property(p => p.PrintedState).UseEnumEditor().Readonly().ShowInList();
                View.Property(p => p.IsPending).Readonly().ShowInList();
                View.Property(p => p.CreateByName).Readonly().ShowInList();
                View.Property(p => p.CreateDate).Readonly().ShowInList();
                View.Property(p => p.UpdateByName).Readonly().ShowInList();
                View.Property(p => p.UpdateDate).Readonly().ShowInList();
                View.AttachChildrenProperty(typeof(BarcodeProDetail), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<BarcodeProcess>();
                    if (parent == null)
                    {
                        return new EntityList<BarcodeProDetail>();
                    }
                    return RT.Service.Resolve<BarcodeProcessController>().GetProDetails(parent.Id, args.PagingInfo);
                }).HasLabel("工序明细");
                View.AttachChildrenProperty(typeof(BarcodeProOptLog), e =>
                {
                    var args = e as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<BarcodeProcess>();
                    if (parent == null)
                    {
                        return new EntityList<BarcodeProOptLog>();
                    }
                    return RT.Service.Resolve<BarcodeProcessController>().GetProLogs(parent.Id, args.PagingInfo);
                }).HasLabel("操作日志");
            }
        }
    }
}
