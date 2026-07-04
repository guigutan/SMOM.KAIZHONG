using SIE.Barcodes;
using SIE.MetaModel;
using SIE.MetaModel.View;
using System.Windows.Controls;

namespace SIE.Wpf.MES.ProductRoutings
{
    /// <summary>
    /// 产品工艺路线模板
    /// </summary>
    public class ProductRoutingTemplate : ListUITemplate
    {
        /// <summary>
        /// 初始化实体和视图
        /// </summary>
        public ProductRoutingTemplate()
            : base(typeof(Barcode), BarcodeViewConfig.BarcodeViewGroup)
        {
        }

        /// <summary>
        /// 重写生成聚合块的方法
        /// </summary>
        /// <returns>返回新的聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(DockLayout));
            return blocks;
        }

        /// <summary>
        /// 重写定义查询聚合块的方法
        /// </summary>
        /// <param name="em">实体元数据</param>
        /// <param name="result">聚合块</param>
        protected override void DefineQueryBlocks(EntityMeta em, AggtBlocks result)
        {
            if (em.EntityType == typeof(Barcode)) //指定查询界面
            {
                var conditionBlock = new ConditionBlock(typeof(BarcodeCriteria), "ProductRouting");
                var entityMeta = CommonModel.Entities.Get(conditionBlock.EntityType);
                var aggtBlocks = this.DefineAggtBlocks(entityMeta, conditionBlock);
                result.Surrounders.Add(aggtBlocks);
            }
        }
    }

    /// <summary>
    /// Dock布局
    /// </summary>
    public class DockLayout : DockPanel, ILayoutControl
    {
        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">UI组件</param>
        public void Arrange(UIComponents components)
        {
            var toolBar = components.Condition;
            if (toolBar != null)
            {
                toolBar.Control.Margin = new System.Windows.Thickness(5);
                SetDock(toolBar.Control, Dock.Top);
                Children.Add(toolBar.Control);
            }

            var control = components.Main;
            if (control != null)
            {
                control.Control.Margin = new System.Windows.Thickness(5, -10, 5, 5);
                SetDock(control.Control, Dock.Top);
                Children.Add(control.Control);
            }
        }
    }
}
