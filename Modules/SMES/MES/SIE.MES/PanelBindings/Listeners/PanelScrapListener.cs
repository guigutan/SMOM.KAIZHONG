using SIE.EventMessages.MES.Panels;

namespace SIE.MES.PanelBindings.Listeners
{
    /// <summary>
    /// 拼板码报废监听
    /// </summary>
    public class PanelScrapListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static PanelScrapListener Instance { get; } = new PanelScrapListener();

        /// <summary>
        /// 订阅事件总线
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<PanelScrapEvent>(this, e =>
            {
                RT.Service.Resolve<PanelBindingController>().PanelScrap(e);
            });
        }
    }
}