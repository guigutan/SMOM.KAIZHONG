using SIE.EventMessages;
using SIE.EventMessages.AbnormalInfo;

namespace SIE.RedCardManagment.RedcardEvent
{
	/// <summary>
	/// 来料检验单EventBus监听类
	/// </summary>
	public class RedcardEventListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static RedcardEventListener Instance { get; set; } = new RedcardEventListener();

        /// <summary>
        /// 订阅WMS来料接收信息
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<TaskCallEvent>(this, e =>
            {
                RT.Service.Resolve<RedcardEventsController>().SysRedcardData(e);
            });

            RT.EventBus.Subscribe<TaskhandleEvent>(this, e =>
            {
                RT.Service.Resolve<RedcardEventsController>().RedcardTaskHandel(e);
            });
        }
    }
}
