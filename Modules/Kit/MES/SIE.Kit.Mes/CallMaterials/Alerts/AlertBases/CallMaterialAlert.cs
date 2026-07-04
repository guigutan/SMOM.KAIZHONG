using MimeKit;
using SIE.Common.Alert;
using System;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 物料呼叫预警插件基类
    /// </summary>
    [Serializable]
    public class CallMaterialAlert : AlertBase
    {
        /// <summary>
        ///  执行物料预警逻辑
        /// </summary>
        /// <returns>预警参数</returns>
        public override AlertResultBase Run()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 物料预警结果处理
        /// </summary>
        /// <param name="alretResult">预警参数</param>
        /// <returns>bool</returns>
        public override bool AlertResultProcess(AlertResultBase alretResult)
        {
            return base.AlertResultProcess(alretResult);
        }

        /// <summary>
        /// 创建物料预警邮件附件
        /// </summary>
        /// <param name="result">预警参数</param>
        /// <returns>邮件附件</returns>
        public override AttachmentCollection CreateEmailAttachments(AlertResultBase result)
        {
            return base.CreateEmailAttachments(result);
        }
    }
}
