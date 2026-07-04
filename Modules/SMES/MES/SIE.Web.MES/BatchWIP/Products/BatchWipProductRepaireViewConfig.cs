using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System.Linq;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品维修记录视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BatchWipProductRepaireViewConfig : WebViewConfig<BatchWipProductRepaire>
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
            if (defects == null)
                return "";
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
            View.Property(p => p.BatchNo).Readonly();
            View.Property(p => p.SubBatchNo).Readonly().Show(ShowInWhere.Hide);
            View.Property(DefectCodeProperty);
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.RepairQty).Readonly();
            View.Property(p => p.ScrapQty).Readonly();
            View.Property(p => p.ScrapReason).Readonly();
            View.Property(p => p.RepaireTime).Readonly().ShowInList(width: 150);
            View.Property(p => p.ReparieByName).Readonly();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.ShiftName).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}
