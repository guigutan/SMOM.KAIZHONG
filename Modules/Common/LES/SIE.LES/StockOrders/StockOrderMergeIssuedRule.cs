using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单合并下发规则验证
    /// </summary>
    [DisplayName("备料单合并下发规则验证")]
    [Description("备料单合并下发规则验证")]
    public class StockOrderMergeIssuedRule : EntityRule<StockOrderMergeIssued>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public StockOrderMergeIssuedRule()
        {
            Scope = EntityStatusScopes.Add;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (entity is StockOrderMergeIssued issued)
            {
                bool flg = true;
                if (issued.LinesideWarehouse == null)
                {
                    e.BrokenDescription = "生产资源不能为空".L10N();
                    flg = false;
                }
                if (issued.StockModel == null)
                {
                    e.BrokenDescription = "备料模式不能为空".L10N();
                    flg = false;
                }
                if (flg)
                {
                    var ri = RT.Service.Resolve<StockOrderMergeIssuedController>().Repeated(issued);
                    if (ri != null)
                    {
                        e.BrokenDescription = "已存在生产资源为【{0}】备料模式为【{1}】接收仓库为【{2}】的数据".L10nFormat(ri.LinesideWarehouse.WipResouce.Name, ri.StockModel.ToLabel().L10N(), ri.LinesideWarehouse.Warehouse.Code);
                    }
                }
            }
        }
    }
}
