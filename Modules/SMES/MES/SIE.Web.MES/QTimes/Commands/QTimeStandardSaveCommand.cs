using SIE.Domain;
using SIE.MES.QTimes.Services;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.QTimes.Commands
{
    /// <summary>
    /// QT标准维护保存命令
    /// </summary>
    public class QTimeStandardSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            RT.Service.Resolve<QTimeStandardService>().QTimeStandardSaveCommand(data);
            base.OnSaving(data);
        }
    }
}
