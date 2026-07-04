using System;

namespace SIE.EventMessages.EMS.Furnace
{
    /// <summary>
    /// 炉温管理接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultFurnaceInterface))]
    public interface IFurnace
    {
        /// <summary>
        /// 投产提醒
        /// </summary>
        /// <param name="changeInfo">投产信息</param>
        void ChangeProductNotify(ChangeProdcutInfo changeInfo);
    }

    /// <summary>
    /// 炉温接口的默认实现
    /// </summary>
    class DefaultFurnaceInterface : IFurnace
    {
        public void ChangeProductNotify(ChangeProdcutInfo changeInfo)
        {
            Logging.LogManager.Logger.Warn("炉温接口未有具体实现。".L10N());
        }
    }

    /// <summary>
    /// 投产信息
    /// </summary>
    [Serializable]
    public class ChangeProdcutInfo
    {
        /// <summary>
        /// 投产工单Id
        /// </summary>
        public double WorkOrderId { get; set; }
    }
}
