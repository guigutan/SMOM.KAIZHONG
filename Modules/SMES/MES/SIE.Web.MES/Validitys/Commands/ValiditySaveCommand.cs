using SIE.Domain;
using SIE.MES.Validitys.Helpers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Validitys.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    public class ValiditySaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            ValidityHelper validityHelper = new ValidityHelper(data);
            validityHelper.SaveCommand();
            base.OnSaving(data);
        }
    }
}
