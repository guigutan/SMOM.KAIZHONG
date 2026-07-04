using SIE.Common.Security;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.WPF.Common.Templates;
using SIE.WPF.Resources.Employees.ViewModels;

namespace SIE.WPF.CSM.Suppliers.UITemplate
{
    /// <summary>
    /// 供应商选择账户UI界面
    /// </summary>
    public class SearchTemplate : ListUITemplate
    {
        public SearchTemplate() : base(typeof(User)) { }

        protected override void DefineQueryBlocks(EntityMeta em, AggtBlocks result)
        {
            if (em.EntityType == typeof(User))
            {
                //条件面板块
                var surBlock = new ConditionBlock
                {
                    EntityType = typeof(SupplierUserCriteria), //员工关联用户查询实体
                };
                //用通用模板去获取 条件面板块 查询到的结果
                var surEM = CommonModel.Entities.Get(surBlock.EntityType);
                //定义聚合块
                var surAggt = this.DefineAggtBlocks(surEM, surBlock);
                result.Surrounders.Add(surAggt);
            }
            else
                base.DefineQueryBlocks(em, result);
        }
    }
}
