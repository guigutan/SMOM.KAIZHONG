using SIE.EventMessages.WMS.IqcPick.Model;
using SIE.Services;

namespace SIE.EventMessages.WMS.IqcPick
{
    /// <summary>
    /// 来料挑选接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultIqcPick))]
    public interface IIqcPickService
    {
        /// <summary>
        /// 挑选来料
        /// </summary>
        /// <param name="doPickEvent"></param>
        void DoPick(DoPickEvent doPickEvent);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultIqcPick : IIqcPickService
    {
        /// <summary>
        /// 挑选来料
        /// </summary>
        /// <param name="doPickEvent"></param>
        public void DoPick(DoPickEvent doPickEvent)
        {
          //
        }
    }
}
