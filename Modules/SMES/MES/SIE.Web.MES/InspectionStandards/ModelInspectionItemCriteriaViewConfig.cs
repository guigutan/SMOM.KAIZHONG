using Microsoft.Scripting.Actions.Calls;
using SIE.Domain;
using SIE.Items;
using SIE.MES.InspectionStandards;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.InspectionStandards
{
    /// <summary>
    /// 项目建议查询实体视图配置
    /// </summary>
    public class ModelInspectionItemCriteriaViewConfig : WebViewConfig<ModelInspectionItemCriteria>
    {
         
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Model);
            View.Property(p => p.Process).UseDataSource((s, p, k) =>
            {
                var source = s as ModelInspectionItemCriteria;
                if (source != null)
                {
                    return RT.Service.Resolve<ModelInspectionItemController>().GetPqcProcess(p, k);
                }
                else
                {
                    return new EntityList<Process>();
                }
            });
            View.Property(p => p.ProductItem).UseDataSource((s, p, k) =>
            {
                var source = s as ModelInspectionItemCriteria;
                if (source != null)
                {
                    return RT.Service.Resolve<ModelInspectionItemController>().GetTargetItems(p, k);
                }
                else
                {
                    return new EntityList<Item>();
                }
            });
        }
    }
}
