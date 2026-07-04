using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ERPInterface.Logs.Commands
{
    /// <summary>
    /// 事务上传调整交易日期命令
    /// </summary>
    public class AdjustTxnDateCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //反序列化调整后日期
            var dataTime = args.Data?.ToJsonObject<DateTime>();
            if (dataTime == null)
                throw new ValidationException("交易日期不能为空".L10N());

            List<double> idlist = args.SelectedIds.ToList(); //事务Id列表
            RT.Service.Resolve<UploadBaseController>().AdjustTransation(idlist, dataTime.Value);

            return true;
        }
    }
}


