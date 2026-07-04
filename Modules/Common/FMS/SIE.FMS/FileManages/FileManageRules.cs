using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 同一文件夹下文件夹名称不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("同一文件夹下文件夹名称不能重复")]
    [System.ComponentModel.Description("同一文件夹下文件夹名称不能重复")]
    public class FolderNotDuplicateRule : NotDuplicateRule<Folder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FolderNotDuplicateRule()
        {
            Properties.Add(Folder.PreFolderIdProperty);
            Properties.Add(Folder.NameProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = (e) =>
            {
                var entity = (e) as Folder;
                return "该文件夹[{0}]已经存在,不允许重复".L10nFormat(entity.Name);
            };
        }
    }
}
