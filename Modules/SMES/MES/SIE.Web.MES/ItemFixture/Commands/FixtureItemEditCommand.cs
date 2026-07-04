using SIE.Domain;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemFixture.Commands
{
    /// <summary>
    /// 工装与产品关系的保存命令
    /// </summary>
    public class FixtureItemEditCommand : SaveCommand
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
