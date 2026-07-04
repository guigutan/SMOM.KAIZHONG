using SIE.Domain;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System.Windows.Controls;

namespace SIE.Wpf.Tech.Routings.DesignPropertys
{
    /// <summary>
    /// 工序BOM控件
    /// </summary>
    public class ProcessBomControl : Grid
    {
        /// <summary>
        /// 控件结果
        /// </summary>
        ControlResult _ui;

        /// <summary>
        /// 初始化工序BOM信息
        /// </summary>
        /// <param name="activity">活动接口</param>
        /// <param name="state">工艺流程状态</param>
        public void InitProcessBom(IActivity activity, RoutingState? state)
        {
            if (activity == null)
            {
                Children.Clear();
                return;
            }

            if (_ui == null)
            {
                var template = new ListUITemplate(typeof(Item), ProcessBomViewConfig.ProcessBomViewGroup);
                template.BlocksDefined += Template_BlocksDefined;
                _ui = template.CreateUI();
            }

            if (Children.Count == 0)
            {
                Children.Add(_ui.Control);
            }

            _ui.MainView.Data = activity.Bom;
            var process = RF.GetById<Process>(activity.ProcessId);
            if (state != RoutingState.Save || activity.Type != ActivityType.Interaction || (process != null && process.Type != ProcessType.Assembly && process.Type != ProcessType.BatchAssembly))
            {
                _ui.MainView["CanExecute"] = false;
            }
            else
            {
                _ui.MainView["CanExecute"] = true;
            }
        }

        /// <summary>
        /// 模板定义，移除查询块
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Template_BlocksDefined(object sender, MetaModel.View.CodeBlocksDefinedEventArgs e)
        {
            var criteriaBlock = e.Blocks.Surrounders.Find(typeof(ItemCriteria));
            if (criteriaBlock != null)
            {
                e.Blocks.Surrounders.Remove(criteriaBlock);
            }

            e.Blocks.Children.Clear();
        }
    }
}
