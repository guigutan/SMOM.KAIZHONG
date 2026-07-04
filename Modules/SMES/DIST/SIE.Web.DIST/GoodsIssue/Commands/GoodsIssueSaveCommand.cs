using SIE.DIST;
using SIE.Domain;
using SIE.Web.Command;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 配送管理表单保存命令
    /// </summary>
    [JsCommand("SIE.Web.DIST.GoodsIssueSaveCommand")]
    public class GoodsIssueSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return base.Excute(args, scope);
        }

        /// <summary>
        /// 保存时验证物料属性以及初始化剩余数量等于出货数量
        /// </summary>
        /// <param name="entity">配送管理实体</param>
        protected override void OnSaving(Entity entity)
        {
            GoodsIssue goodsIssue = entity as GoodsIssue;
            SetPropertyValues(goodsIssue);
            base.OnSaving(entity);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="goodsIssue">配送管理实体</param>
        private static void SetPropertyValues(GoodsIssue goodsIssue)
        { 
            if (goodsIssue.Qty.HasValue)//添加保存的时候剩余数量初始化等于发货数量
            {
                goodsIssue.RemainderQty = goodsIssue.Qty.Value;
            }
        }
    }
}
