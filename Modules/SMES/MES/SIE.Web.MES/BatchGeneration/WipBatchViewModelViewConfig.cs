using Org.BouncyCastle.Asn1.Cms.Ecc;
using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.BatchGeneration;
using SIE.Web.Barcodes.WipBatchs.Commands;

namespace SIE.Web.MES.BatchGeneration
{
    /// <summary>
    /// 生产批次视图配置
    /// </summary>
    public class WipBatchViewModelViewConfig : WebViewConfig<WipBatchViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WOBatchGeneration));
            View.UseCommand(typeof(ReprintBatchCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).ShowInList(150).HasLabel("批次号").FixColumn().Readonly();
                View.Property(p => p.Qty).HasLabel("批次数量").Readonly();
                View.Property(p => p.ScrapQty).HasLabel("报废数").Readonly(); 
                View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Readonly();
                View.Property(p => p.PrintDate).ShowInList(150).HasLabel("生成时间").Readonly();
                View.Property(p => p.PrintTimes).ShowInList(150).Readonly();
                View.Property(p => p.PrintByName).HasLabel("打印人").Readonly();
                View.Property(p => p.PrintedState).HasLabel("打印状态").UseEnumEditor().Readonly();
                View.Property(p => p.CreateDate).ShowInList(width: 150);
                View.Property(p => p.CreateBy).ShowInList(width: 150);
                View.Property(p => p.UpdateDate).ShowInList(width: 150);
                View.Property(p => p.UpdateBy).ShowInList(width: 150);
            }
        }
    }
}