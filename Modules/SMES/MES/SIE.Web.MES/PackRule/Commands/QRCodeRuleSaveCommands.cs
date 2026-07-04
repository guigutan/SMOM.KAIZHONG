using SIE.Domain;
using SIE.MES.ItemLine;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackRule.Commands
{
    public class QRCodeRuleSaveCommands : SaveCommand 
    {
        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            base.DoSave(data);
        }
    }
}
