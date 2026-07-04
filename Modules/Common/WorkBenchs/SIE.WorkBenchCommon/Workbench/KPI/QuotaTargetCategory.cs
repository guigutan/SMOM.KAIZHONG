using Castle.MicroKernel.Registration;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// kpi指标分类、指标名称
    /// </summary>
    public static class QuotaTargetCategoryHelper
    {
        private static List<QuotaTargetCategory> source = new List<QuotaTargetCategory>()
        {
            new QuotaTargetCategory(){ Code="效率类",Name="日均达成率"},
            new QuotaTargetCategory(){ Code="效率类",Name="月度总量达成"},
            new QuotaTargetCategory(){ Code="效率类",Name="生产计划达成率"},
            new QuotaTargetCategory(){ Code="效率类",Name="生产工单完工率"},
            new QuotaTargetCategory(){ Code="效率类",Name="日均直通率"},

            new QuotaTargetCategory(){ Code="品质类",Name="成品检验批次合格率"},
            new QuotaTargetCategory(){ Code="品质类",Name="来料检验批次合格率"},
            new QuotaTargetCategory(){ Code="品质类",Name="每日首检直通率"},
            new QuotaTargetCategory(){ Code="品质类",Name="生产物料下线率"},
            new QuotaTargetCategory(){ Code="品质类",Name="客诉批退率"},

             new QuotaTargetCategory(){ Code="持续改进",Name="改善关闭率"},
        };

        /// <summary>
        /// 获取指标分类字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetCodeDic()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var entity in source)
            {
                if (!result.ContainsKey(entity.Code))
                {
                    result.Add(entity.Code,entity.Code);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指标名称字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetNameDic(string code)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var entity in source)
            {
                if (entity.Code==code&&!result.ContainsKey(entity.Name))
                {
                    result.Add(entity.Name, entity.Name);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 指标类
    /// </summary>
    public class QuotaTargetCategory
    {
        /// <summary>
        /// 指标分类
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public string Name { get; set; }
    }
}
