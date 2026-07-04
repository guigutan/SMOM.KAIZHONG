using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Resources.Employees;

namespace SIE.Wpf.Resources.Employees.ViewModels
{
    /// <summary>
    /// EmployeeGroupModule
    /// </summary>
    public class EmployeeGroupModule : UITemplate
    {
        /// <summary>
        /// 聚合定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var block = new AggtBlocks
            {
                MainBlock = new Block(typeof(EmployeeGroup), ViewConfig.ListView),
                Layout = new LayoutMeta()
                {
                    IsLayoutChildrenHorizonal = false,
                },
            };
            var surBlock = new ConditionBlock(typeof(EmployeeLinkUserlCriteria), WPFViewConfig.QueryView);

            //用通用模板去获取 条件面板块 查询到的结果
            var surEM = CommonModel.Entities.Get(surBlock.EntityType);

            //定义聚合块
            var surAggt = this.DefineAggtBlocks(surEM, surBlock);
            block.Surrounders.Add(surAggt);
            return block;
        }
    }
}
