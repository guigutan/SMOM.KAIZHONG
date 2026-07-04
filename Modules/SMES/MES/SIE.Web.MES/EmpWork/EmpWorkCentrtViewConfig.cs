using SIE.MES.EmpWork;
using SIE.MES.Fixture;
using SIE.MetaModel.View;
using SIE.Web.MES.Fixture.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.EmpWork
{
    /// <summary>
    /// 人员与工作中心视图配置
    /// </summary>
    public class EmpWorkCentrtViewConfig : WebViewConfig<EmpWorkCentrt>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.Employee).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EmpNo), nameof(e.Employee.Code));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.EmpNo).ShowInList(width: 150);
                View.Property(p => p.WorkCenter).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.WorkName), nameof(e.WorkCenter.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200).HasLabel("工作中心编码");
                View.Property(p => p.WorkName).ShowInList(width: 150);
              
            }
        }

        /// <summary>
        /// 下拉配置视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p=>p.Employee).ShowInList(width: 200);
            View.Property(p=>p.EmpNo).ShowInList(width:200);
        }
    }
}
