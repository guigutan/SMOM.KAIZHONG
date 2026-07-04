namespace SIE.EventMessages.WCS
{
    /// <summary>
    /// WCS请求接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitWcsRequest))]
    public interface IWcsRequest
    {
        /// <summary>
        /// 下发指令
        /// </summary>
        /// <param name="instruct">指令参数</param>
        void SendInstruct(InstructData instruct);

        /// <summary>
        /// 取消指令
        /// </summary>
        /// <param name="cancelInstruct"></param>
        void SendCancelInstruct(CancelInstructData cancelInstruct);
    }

    /// <summary>
    /// 获取WCS指令等数据
    /// </summary>
    public class DefalitWcsRequest : IWcsRequest
    {
        /// <summary>
        /// 下发指令
        /// </summary>
        /// <param name="instruct">指令参数</param>
        public void SendInstruct(InstructData instruct)
        {
            //
        }

        /// <summary>
        /// 取消指令
        /// </summary>
        /// <param name="cancelInstruct">指令参数</param>
        public void SendCancelInstruct(CancelInstructData cancelInstruct)
        {
            //
        }      
    }
}
