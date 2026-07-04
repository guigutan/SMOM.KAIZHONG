using System;

namespace SIE.WorkBenchCommon
{
    /// <summary>
    /// 异常辅助类
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// 捕获断网异常，不处理
        /// </summary>
        /// <param name="action">执行逻辑</param>
        public static void CatchRemoteException(Action action)
        {
            try
            {
                action();
            }
            catch (SIE.Services.RemoteServiceProxyException ex)
            {
                //// 断网不处理
                SIE.Logging.LogManager.Logger.Error(ex.Message);
            }
        }
    }
}
