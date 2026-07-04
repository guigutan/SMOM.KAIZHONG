using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.PackRule;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.MtartProcessLookups.Configs
{
    /// <summary>
    /// 物料分类与工序关系对照表配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料分类与工序关系对照表配置值")]
    public class MtartProcessLookupConfigValue: ConfigValue
    {
        #region 是否只生成首工序BOM(多个工序用英文逗号隔开) GenrateStartProcessBom
        /// <summary>
        /// 是否只生成首工序BOM(多个工序用英文逗号隔开)
        /// </summary>
        [Label("是否只生成首工序BOM(多个工序用英文逗号隔开)")]
        public static readonly Property<string> GenrateStartProcessBomProperty = P<MtartProcessLookupConfigValue>.Register(e => e.GenrateStartProcessBom);

        /// <summary>
        /// 是否只生成首工序BOM(多个工序用英文逗号隔开)
        /// </summary>
        public string GenrateStartProcessBom
        {
            get { return this.GetProperty(GenrateStartProcessBomProperty); }
            set { this.SetProperty(GenrateStartProcessBomProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示 
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (GenrateStartProcessBom == null)
                return "NIL";
            return GenrateStartProcessBom;
        }
    }
}
