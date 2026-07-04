using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System.Linq;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品维修记录视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class BatchWipProductRepaireViewConfig : WPFViewConfig<BatchWipProductRepaire>
    {
        #region 缺陷代码 DefectCode
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        public static readonly Property<string> DefectCodeProperty = P<BatchWipProductRepaire>.RegisterExtensionReadOnly("DefectCode", typeof(BatchWipProductRepaireViewConfig),
            GetDefectCode, BatchWipProductRepaire.DefectProperty);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public static string GetDefectCode(BatchWipProductRepaire me)
        {
            var defects = me.Defect?.DetailList.Select(p => p.Defect.Code);
            return string.Join(";", defects);
        }
        #endregion

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion), typeof(BatchWipProductRouting));
            View.ClearCommands();
            View.Property(p => p.BatchNo);
            View.Property(p => p.SubBatchNo);
            View.Property(DefectCodeProperty);
            View.Property(p => p.Qty);
            View.Property(p => p.RepairQty);
            View.Property(p => p.ScrapQty);
            View.Property(p => p.ScrapReason);
            View.Property(p => p.RepaireTime).ShowInList(gridWidth: 150);
            View.Property(p => p.ReparieByName);
            View.Property(p => p.StationName);
            View.Property(p => p.ProcessName);
            View.Property(p => p.ResourceName);
            View.Property(p => p.ShiftName);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}