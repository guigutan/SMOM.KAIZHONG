using SIE.EventMessages.MES.Barcodes;
using SIE.MES.PanelBindings;

namespace SIE.MES.Messages
{
    /// <summary>
    /// 条码事件监听器
    /// </summary>
    public class BarcodeEventListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static BarcodeEventListener Instance { get; } = new BarcodeEventListener();

        /// <summary>
        /// 发布事件总线
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<BarcodeScrapEvent>(this, arg =>
            {
                RT.Service.Resolve<PanelBindingController>().ScrapPanelBarcodes(arg);
            });
        }
    }
}