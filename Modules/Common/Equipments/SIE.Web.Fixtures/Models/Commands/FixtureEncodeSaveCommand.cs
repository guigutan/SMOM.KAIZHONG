using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Fixtures.Models.Commands
{
    /// <summary>
    /// 工治具编码保存命令
    /// </summary>
    public class FixtureEncodeSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var encodeList = data as EntityList<FixtureEncode>;
            RT.Service.Resolve<CoreFixtureController>().FixtureEncodeBeforeSaveValidate(encodeList);
            base.OnSaving(data);
        }
    }
}
