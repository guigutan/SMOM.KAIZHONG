using SIE.Common.Properties;
using SIE.DIST;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.ObjectModel;

namespace SIE.Web.DIST.Distribution
{
    /// <summary>
    /// 配送单退料标签打印
    /// </summary>
    [RootEntity]
    public class BillLabelPrintViewModel : ViewModel
    {

        #region 配送单 Bill
        /// <summary>
        /// 配送单Id
        /// </summary>
        public static readonly IRefIdProperty BillIdProperty =
            P<BillLabelPrintViewModel>.RegisterRefId(e => e.BillId, ReferenceType.Normal);

        /// <summary>
        /// 配送单Id
        /// </summary>
        public double BillId
        {
            get { return (double)this.GetRefId(BillIdProperty); }
            set { this.SetRefId(BillIdProperty, value); }
        }

        /// <summary>
        /// 配送单
        /// </summary>
        public static readonly RefEntityProperty<DistributionBill> BillProperty =
            P<BillLabelPrintViewModel>.RegisterRef(e => e.Bill, BillIdProperty);

        /// <summary>
        /// 配送单
        /// </summary>
        public DistributionBill Bill
        {
            get { return this.GetRefEntity(BillProperty); }
            set { this.SetRefEntity(BillProperty, value); }
        }
        #endregion

        #region 打印机 Printer
        /// <summary>
        /// 打印机
        /// </summary>
        public static readonly Property<string> PrinterProperty = P<BillLabelPrintViewModel>.Register(e => e.Printer, new RegisterRefIdArgs<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as BillLabelPrintViewModel).OnPrinterChanged()
        });

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }

        /// <summary>
        /// 打印机变更事件
        /// </summary>
        void OnPrinterChanged()
        {
            Settings.Default.PrinterName = Printer;
            Settings.Default.Save();
        }
        #endregion

        #region 标签号 LabelNumber
        /// <summary>
        /// 标签号
        /// </summary>
        public static readonly Property<string> LabelNumberProperty = P<BillLabelPrintViewModel>.Register(e => e.LabelNumber);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNumber
        {
            get { return this.GetProperty(LabelNumberProperty); }
            set { this.SetProperty(LabelNumberProperty, value); }
        }
        #endregion

        #region 不良标签号 NGLabelNumber
        /// <summary>
        /// 不良标签号
        /// </summary>
        public static readonly Property<string> NGLabelNumberProperty = P<BillLabelPrintViewModel>.Register(e => e.NGLabelNumber);

        /// <summary>
        /// 不良标签号
        /// </summary>
        public string NGLabelNumber
        {
            get { return this.GetProperty(NGLabelNumberProperty); }
            set { this.SetProperty(NGLabelNumberProperty, value); }
        }
        #endregion

        #region 注册视图
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BillLabelPrintViewModel>.RegisterView(e => e.ItemCode, p => p.Bill.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 名称 ItemName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> ItemNameProperty = P<BillLabelPrintViewModel>.RegisterView(e => e.ItemName, p => p.Bill.Item.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 正常退料数量 ReturnQty
        /// <summary>
        ///  正常退料数量
        /// </summary>
        [Label("正常退料数量")]
        public static readonly Property<decimal> ReturnQtyProperty = P<BillLabelPrintViewModel>.RegisterView(e => e.ReturnQty, p => p.Bill.ReturnQty);

        /// <summary>
        ///  正常退料数量
        /// </summary>
        public decimal ReturnQty
        {
            get { return this.GetProperty(ReturnQtyProperty); }
        }
        #endregion

        #region 不良退料数量 NgReturnQty
        /// <summary>
        /// 不良退料数量
        /// </summary>
        [Label("不良退料数量")]
        public static readonly Property<decimal> NgReturnQtyProperty = P<BillLabelPrintViewModel>.RegisterView(e => e.NgReturnQty, p => p.Bill.NgReturnQty);

        /// <summary>
        /// 不良退料数量
        /// </summary>
        public decimal NgReturnQty
        {
            get { return this.GetProperty(NgReturnQtyProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 配送单退料标签打印视图配置
    /// </summary>
    internal class BillLabelPrintViewModelViewConfig : WebViewConfig<BillLabelPrintViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("产线退货标签打印");
            View.UseDetail(columnCount: 2);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).HasLabel("物料编码").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.ItemName).HasLabel("名称").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.LabelNumber).HasLabel("正常退料标签条码").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.NGLabelNumber).HasLabel("不良退料标签条码").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.ReturnQty).HasLabel("正常退料数量").Show(ShowInWhere.All).Readonly(true);
                View.Property(p => p.NgReturnQty).HasLabel("不良退料数量").Show(ShowInWhere.All).Readonly(true);
            }
        }
    }
}
