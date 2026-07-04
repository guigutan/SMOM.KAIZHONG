using SIE.MES.BarcodeProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses
{
    /// <summary>
    /// 工序视图配置
    /// </summary>
    public class SingleProcessViewConfig : WebViewConfig<SingleProcess>
    {
        /// <summary>
        /// 选择单体工序视图
        /// </summary>
        public const string SelectSingleProcessViewStr = "SelectSingleProcessViewStr";

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(BarcodeProcess));
            View.DeclareExtendViewGroup(SelectSingleProcessViewStr);
            if (ViewGroup == SelectSingleProcessViewStr)
            {
                SelectSingleProcessView();
            }
        }

        /// <summary>
        /// 选择单体工序视图
        /// </summary>
        private void SelectSingleProcessView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().ShowInList(150);
                View.Property(p => p.Name).Readonly().ShowInList(150);
                View.Property(p => p.ProductFamily).Readonly().ShowInList(150);
                View.Property(p => p.CategoryName).Readonly().ShowInList(150);
                View.Property(p => p.Type).UseEnumEditor().Readonly().ShowInList();
                View.Property(p => p.Segment).Readonly().ShowInList();
                View.Property(p => p.EnableMoveInControl).Readonly().ShowInList();
                View.Property(p => p.TransferType).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.IsOutsourcing).Readonly().ShowInList();
            }
        }
    }
}
