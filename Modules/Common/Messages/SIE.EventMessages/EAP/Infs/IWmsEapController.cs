using SIE.EventMessages.EAP.Infs.Datas;

namespace SIE.EventMessages.EAP.Infs
{
    /// <summary>
    /// 调用wms相关的agv任务回传处理接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultWmsEapController))]
    public interface IWmsEapController
    {
        /// <summary>
        /// agv任务回传时调用wms的接口进行
        /// </summary>
        /// <param name="taskParam"></param>
        /// <returns></returns>
        bool FinishWmsAgvTask(ReturnTaskParam taskParam);
    }

    /// <summary>
    /// 接口默认实现类
    /// </summary>
    public class DefaultWmsEapController : IWmsEapController
    {
        public bool FinishWmsAgvTask(ReturnTaskParam taskParam)
        {
            return false;
        }
    }
}
