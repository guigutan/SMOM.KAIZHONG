using SIE.Common;
using SIE.Domain;
using SIE.Resources.WipResources;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Resources.WipResources.Commands
{
    /// <summary>
    /// 资源同步设置命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.WipResources.Commands.SynWipResSettingCommand")]
    public class SynWipResSettingCommand : ViewCommand
    {
        /// <summary>
        /// 命令执行逻辑
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>成功</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var settings = args.Data.ToJsonObject<List<SynWipResSetting>>();
            if (settings == null)
                throw new ArgumentNullException("同步设置数据为空".L10N());
            if (settings.Count > 0)
            {
                RF.Save(settings.AsEntityList());
                RT.Service.Resolve<WipResourceController>().RunSync();
            }
            return true;
        }
    }
}