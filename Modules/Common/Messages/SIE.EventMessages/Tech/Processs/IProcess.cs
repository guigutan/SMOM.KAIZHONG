using SIE.EventMessages.WCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Tech.Processs
{
    [Services.Service(FallbackType = typeof(DefaultIProcess))]
    public interface IProcess
    {
        /// <summary>
        /// 用户组删除用户，同步到员工
        /// </summary>
        /// <param name="UserInUserGroupId"></param>
        void DeleteUserInUserGroupSyncEmployeeProcess(double userId, double userGroupId);

        /// <summary>
        /// 用户组新增用户，同步到员工
        /// </summary>
        /// <param name="UserInUserGroupId"></param>
        void InsertUserInUserGroupSyncEmployeeProcess(double UserInUserGroupId);
    }

    public class DefaultIProcess : IProcess
    {
        /// <summary>
        /// 用户组删除用户，同步到员工
        /// </summary>
        /// <param name="UserInUserGroupId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteUserInUserGroupSyncEmployeeProcess(double userId, double userGroupId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户组新增用户，同步到员工
        /// </summary>
        /// <param name="UserInUserGroupId"></param>
        public void InsertUserInUserGroupSyncEmployeeProcess(double UserInUserGroupId)
        {
            throw new NotImplementedException();
        }
    }
}
