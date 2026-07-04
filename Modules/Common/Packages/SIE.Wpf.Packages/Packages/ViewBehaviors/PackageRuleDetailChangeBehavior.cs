using SIE.Domain;
using SIE.Packages;
using System.Linq;

namespace SIE.Wpf.Packages.Packages.ViewBehaviors
{
    /// <summary>
    /// 包装规则明细视图行为类
    /// </summary>
    public class PackageRuleDetailChangeBehavior : ViewBehavior
    {
        /// <summary>
        /// 是否正在切换ASN类型
        /// </summary>
        private bool isRun = false;

        /// <summary>
        /// 当前的包装规则明细
        /// </summary>
        private PackageRuleDetail currPackageDetail = null;

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            view.Control.View.EnableImmediatePosting = true;

            if (view != null)
            {
                view.CurrentChanged -= Asn_CurrentChanged;
                view.CurrentChanged += Asn_CurrentChanged;
            }
        }

        /// <summary>
        /// 当前的包装规则明细对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void Asn_CurrentChanged(object sender, System.EventArgs e)
        {
            ListLogicalView logicalView = sender as ListLogicalView;
            if (currPackageDetail != null)
            {
                currPackageDetail.PropertyChanged -= PackageDetail_PropertyChanged;
            }

            currPackageDetail = logicalView.Current as PackageRuleDetail;
            if (currPackageDetail != null)
            {
                currPackageDetail.PropertyChanged -= PackageDetail_PropertyChanged;
                currPackageDetail.PropertyChanged += PackageDetail_PropertyChanged;
            }
        }

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="sender">变更的对象</param>
        /// <param name="e">事件参数</param>
        private void PackageDetail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!isRun)
            {
                isRun = true;
                try
                {
                    EntityList<PackageRuleDetail> allData = View.Data as EntityList<PackageRuleDetail>;
                    if (e.PropertyName == PackageRuleDetail.IsSequenceProperty.Name)
                    {
                        if (allData != null)
                        {
                            allData.Where(p => p.Id != currPackageDetail.Id).ForEach(p => p.IsSequence = false);
                        }
                    }
                    else if (e.PropertyName == PackageRuleDetail.IsInStockLabelProperty.Name)
                    {
                        if (allData != null)
                        {
                            allData.Where(p => p.Id != currPackageDetail.Id).ForEach(p => p.IsInStockLabel = false);
                        }
                    }
                    else if (e.PropertyName == PackageRuleDetail.IsOutStockLabelProperty.Name && allData != null)
                    {
                        allData.Where(p => p.Id != currPackageDetail.Id).ForEach(p => p.IsOutStockLabel = false);
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
