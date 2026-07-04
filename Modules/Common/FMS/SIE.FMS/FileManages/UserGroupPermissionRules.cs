using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.FMS.FileManages
{
    /// <summary>
    /// 文件权限：文件夹+文件用户组不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("文件权限非重复规则")]
    [System.ComponentModel.Description("文件权限：文件夹+文件用户组不能重复")]
    public class UserGroupPermissionNotDuplicateRule : NotDuplicateRule<UserGroupPermission>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserGroupPermissionNotDuplicateRule()
        {
            Properties.Add(UserGroupPermission.FolderIdProperty);
            Properties.Add(UserGroupPermission.FileUserGroupIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = (e) =>
            {
                return "文件权限：文件夹+文件用户组不能重复".L10N();
            };
        }
    }
}
