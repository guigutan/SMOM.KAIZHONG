using SIE.Domain;
using SIE.Resources.Enterprises;

namespace SIE.Wpf.Resources.Enterprises.ViewBehaviors
{
    /// <summary>
    /// 企业模型属性变更行为
    /// </summary>
    public class EnterpriseChangeBehavior : ViewBehavior
    {
        /// <summary>
        /// 是否正在更改企业模型
        /// </summary>
        private bool isRun;

        /// <summary>
        /// 当前实体
        /// </summary>
        private Entity Current;

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            if (view != null)
            {
                view.CurrentChanged -= Enterprise_CurrentChanged;
                view.CurrentChanged += Enterprise_CurrentChanged;
            }
        }

        /// <summary>
        /// 企业模型对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void Enterprise_CurrentChanged(object sender, System.EventArgs e)
        {
            ListLogicalView logicalView = sender as ListLogicalView;
            if (logicalView != null)
            {
                if (Current != null)
                {
                    Current.PropertyChanged -= Enterprise_PropertyChanged;
                }

                Enterprise enterprise = logicalView.Current as Enterprise;
                Current = enterprise;
                if (enterprise != null)
                {
                    enterprise.PropertyChanged -= Enterprise_PropertyChanged;
                    enterprise.PropertyChanged += Enterprise_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// 企业模型变更
        /// </summary>
        /// <param name="sender">变更对象</param>
        /// <param name="e">变更事件</param>
        private void Enterprise_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!isRun)
            {
                isRun = true;
                try
                {
                    if (e.PropertyName == Enterprise.LevelProperty.Name)
                    {
                        Enterprise enterprise = sender as Enterprise;
                        if (enterprise != null)
                        {
                            if (enterprise.LevelId > 0)
                            {
                                enterprise.IsResource = enterprise.Level.IsResource;
                            }
                        }
                    }
                }
                finally
                {
                    isRun = false;
                }
            }
        }
    }
}
