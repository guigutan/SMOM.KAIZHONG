using SIE.Domain.Validation;
using SIE.Items;
using SIE.Web.Command;
using SIE.Web.Items.ViewModels;
using System;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 添加命令
    /// </summary>
    [JsCommand("SIE.Web.DIST.GoodsIssuePropertyValueCommand")]
    public class GoodsIssuePropertyValueCommand : ListViewCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<PropertyValueViewModel>();
            var result = RT.Service.Resolve<ItemController>().GetItemPropertys(data.ItemId);
            if (result == null || result.Count == 0)
            {
                throw new ValidationException("当前物料没有配置物料属性，请先在物料中配置".L10N());
            }
            return data;
        }
    }
}