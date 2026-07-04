using SIE.Domain;
using SIE.LES.LesStockCounts;
using SIE.ObjectModel;
using System;

namespace SIE.Web.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 输入实盘ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class LesStockCountDetailQtyViewModel : ViewModel
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<LesStockCountDetailQtyViewModel>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal?> QtyProperty = P<LesStockCountDetailQtyViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 标签 LabelNo
        /// <summary>
        /// 标签
        /// </summary>
        [Label("标签")]
        public static readonly Property<string> LabelNoProperty = P<LesStockCountDetailQtyViewModel>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion

        #region 物料信息 ItemMessage
        /// <summary>
        /// 物料信息
        /// </summary>
        [Label("物料信息")]
        public static readonly Property<string> ItemMessageProperty = P<LesStockCountDetailQtyViewModel>.Register(e => e.ItemMessage);

        /// <summary>
        /// 物料信息
        /// </summary>
        public string ItemMessage
        {
            get { return this.GetProperty(ItemMessageProperty); }
            set { this.SetProperty(ItemMessageProperty, value); }
        }
        #endregion

        #region 处理消息 Message
        /// <summary>
        /// 处理消息
        /// </summary>
        [Label("处理消息")]
        public static readonly Property<string> MessageProperty = P<LesStockCountDetailQtyViewModel>.Register(e => e.Message);

        /// <summary>
        /// 处理消息
        /// </summary>
        public string Message
        {
            get { return this.GetProperty(MessageProperty); }
            set { this.SetProperty(MessageProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 输入实盘 视图配置
    /// </summary>
    public class StockCountDetailQtyViewModelViewConfig : WebViewConfig<LesStockCountDetailQtyViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LesStockCountDetail));
        }

        /// <summary>
        /// 配置明细试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Message).UseDisplayEditor().ShowInDetail(hideLabel: true).Readonly();
            View.Property(p => p.LineNo).UseSpinEditor(p =>
            {
                p.XType = "LesLineNoNumberfield";
            });

            View.Property(p => p.Qty).UseSpinEditor(p =>
            {
                p.XType = "LesActualQtyNumberfield";
            });
            View.Property(p => p.LabelNo).Readonly();
            View.Property(p => p.ItemMessage).Readonly();
        }
    }
}
