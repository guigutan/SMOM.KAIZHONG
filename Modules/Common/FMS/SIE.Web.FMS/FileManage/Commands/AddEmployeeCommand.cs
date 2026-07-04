using SIE.Domain;
using SIE.FMS;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.FMS.FileManage.Commands
{
    /// <summary>
    /// 添加员工命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.AddEmployeeCommand")]
    public class AddEmployeeCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">scope</param>
        /// <returns>返回结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var savedData = new EntityList<UserInFileUserGroup>();
            var userInFileUserGroupList = args.Data.ToJsonObject<List<UserInFileUserGroup>>();
            Check.NotNullOrEmpty(userInFileUserGroupList, nameof(userInFileUserGroupList));
            foreach (var item in userInFileUserGroupList)
            {
                var userInFileUserGroup = new UserInFileUserGroup();
                userInFileUserGroup.FileUserGroupId = item.FileUserGroupId;
                userInFileUserGroup.EmployeeId = item.EmployeeId;
                savedData.Add(userInFileUserGroup);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
