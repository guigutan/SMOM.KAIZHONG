using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Common;
using SIE.MES.Common.HomeMenusConfigs;
using SIE.Rbac.Roles;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.Common.HomeMenusConfigs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [JsCommand("SIE.Web.MES.Common.HomeMenusConfigs.Commands.EditPermissionsCommand")]
    public class EditPermissionsCommand : ViewCommand
    {
        /// <summary>
        ///  web 服务器
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var postData = args.Data.ToJsonObject<MenPostData>();
            var selectModelKeys = postData.CheckedCmds.Where(m => m.ModuleKey != "").Select(m => m.ModuleKey).ToList();
            return  RT.Service.Resolve<HomeMenusConfigsController>().SaveMenPostData(postData, selectModelKeys);
        }

        
    }
}
