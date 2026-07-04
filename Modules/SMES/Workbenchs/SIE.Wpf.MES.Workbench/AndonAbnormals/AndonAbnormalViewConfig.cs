using SIE.Domain;
using SIE.MES.Workbench.AndonAbnormals;
using SIE.ObjectModel;
using SIE.Wpf.MES.Workbench.AndonAbnormals.Commands;
using System.Linq;

namespace SIE.Wpf.MES.Workbench.AndonAbnormals
{
    /// <summary>
    /// 安灯异常视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class AndonAbnormalViewConfig : WPFViewConfig<AndonAbnormal>
    {
        #region 接收人员列表 HandersValue
        /// <summary>
        /// 接收人员列表
        /// </summary>
        [Label("接收人员列表")]
        public static readonly Property<string> HandersValueProperty = P<AndonAbnormal>.RegisterExtensionReadOnly("HandersValue", typeof(AndonAbnormalViewConfig),
            GetHandersValue, AndonAbnormal.AlertLightHandersProperty);

        /// <summary>
        /// 接收人员列表
        /// </summary>
        public static string GetHandersValue(Entity me)
        {
            var andonAbnormal = me as AndonAbnormal;
            return string.Join(",", andonAbnormal.AlertLightHanders.Select(x => x.HandlerName));
        }
        #endregion

        /// <summary>
        /// 安灯异常视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("安灯异常");
        }

        /// <summary>
        /// 安灯异常列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.InlineEdit();
            View.UseCommands(typeof(AndonSignCommand));

            using (View.OrderProperties())
            {
                View.Property(p => p.ProductLine).Readonly();
                View.Property(p => p.Station).Readonly();
                View.Property(p => p.AlertType).Readonly();
                //View.Property(p => p.ExceptionType).Readonly();
                View.Property(p => p.ExpTypeName).Readonly();
                View.Property(p => p.TriggerTime).Readonly();
                View.Property(p => p.RestoreTime).Readonly();
                View.Property(p => p.CallEmployee).Readonly();
                View.Property(p => p.ProcessEmployee).Readonly();
                View.Property(p => p.SignTime).Readonly();
                View.Property(p => p.ProcessStatus).Readonly();
                View.Property(HandersValueProperty).Readonly();
                View.ChildrenProperty(p => p.AlertLightHanders).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 安灯异常查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ProductLine);
            View.Property(p => p.AlertType);
            View.Property(p => p.ExceptionType);
            View.Property(p => p.ProcessStatus);
        }
    }
}
