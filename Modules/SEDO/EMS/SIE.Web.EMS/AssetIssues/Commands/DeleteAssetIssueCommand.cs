using SIE.Domain;
using SIE.EMS.AssetIssues;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetIssues.Commands
{
    /// <summary>
    ///  带判断审核状态的立即删除
    /// </summary>
    public class DeleteAssetIssueCommand : DeleteCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            var updatelist = new EntityList<AssetIssue>();
            updatelist.AddRange(list.DeletedList);
            RT.Service.Resolve<AssetIssueController>().DeleteAssetIssues(list, updatelist);
            return true;
        }
    }
}
