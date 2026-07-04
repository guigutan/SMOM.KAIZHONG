using SIE.Domain;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯类型维护保存命令
    /// </summary>
    public class AndonTypeSaveCommand : SaveCommand
    {
        protected override void DoSave(EntityList data)
        {
            base.DoSave(data);
        }
    }
}
