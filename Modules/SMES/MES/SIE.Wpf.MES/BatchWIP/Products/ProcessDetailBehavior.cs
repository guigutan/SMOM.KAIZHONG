using SIE.MES.BatchWIP.Products;
using SIE.Wpf;
using System.Linq;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 出站明细视图行为
    /// </summary>
    public class ProcessDetailBehavior : ViewBehavior
    {
        /// <summary>
        /// 关键件列表逻辑视图
        /// </summary>
        ListLogicalView keyItemView;

        /// <summary>
        /// 附加行为默认排序
        /// </summary>
        protected override void OnAttach()
        {
            View.CurrentChanged += (s, e) =>
            {
                 keyItemView = GetKeyItemView();
                if (keyItemView == null)
                    return;
                var detail = View.Current as BatchWipProductProcessDetail;
                if (detail == null) return;
                keyItemView.Data = detail.KeyItemList;
            };
        }

        /// <summary>
        /// 获取关键件列表逻辑视图
        /// </summary>
        /// <returns>关键件列表逻辑视图</returns>
        ListLogicalView GetKeyItemView()
        {
            if (keyItemView != null)
                return keyItemView;
            keyItemView = View.Parent?.ChildrenViews?.FirstOrDefault(p => p.EntityType == typeof(BatchWipProductProcessKeyItem)) as ListLogicalView;
            return keyItemView;
        }
    }
}