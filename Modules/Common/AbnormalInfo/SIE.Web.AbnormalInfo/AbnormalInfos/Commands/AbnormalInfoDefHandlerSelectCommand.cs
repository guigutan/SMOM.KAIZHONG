using SIE.Domain;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.AbnormalInfos.Commands
{
    /// <summary>
    /// 异常信息定义选择处理人按钮
    /// </summary>
    public class AbnormalInfoDefHandlerSelectCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = SIE.ClientEntities.Find(args.Type);
            var savedData = RepositoryFactory.Find(meta.EntityType).NewList();
            var list = args.Data.ToJsonObject<List<AbnormalInfoDefinitionEmployee>>();    //转换数据成实体
            Check.NotNullOrEmpty(list, nameof(list));       //检验非空

            if (list == null || list.Count == 0)
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(list)));

            foreach (var item in list)
            {
                var tempInsItem = new AbnormalInfoDefinitionEmployee();
                tempInsItem.HandlerId = item.HandlerId;
                tempInsItem.AbnormalInfoDefinitionId = item.AbnormalInfoDefinitionId;
                savedData.Add(tempInsItem);
            }

            RF.Save(savedData);

            return true;
        }
    }
}
