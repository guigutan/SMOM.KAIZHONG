using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Tech.Processs.Commands
{
    public class SelectProcessUserGroupCommand: ViewCommand
    {
        /// <summary>
        /// 选择工序命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var processUserGroupList = args.Data.ToJsonObject<List<ProcessUserGroup>>();
            Check.NotNullOrEmpty(processUserGroupList, nameof(processUserGroupList));
            if (processUserGroupList == null || processUserGroupList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(processUserGroupList)));
            }
            RT.Service.Resolve<ProcessController>().SelectSaveProcessUserGroup(processUserGroupList);
            //foreach (var item in processUserGroupList)
            //{
            //    var processUserGroup = new ProcessUserGroup();
            //    processUserGroup.UserGroupId = item.UserGroupId;
            //    processUserGroup.ProcessId = item.ProcessId;
            //    RF.Save(processUserGroup);
            //}

            return true;
        }
    }
}
