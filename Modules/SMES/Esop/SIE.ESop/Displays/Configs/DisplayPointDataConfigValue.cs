using SIE.Common.Configs;
using SIE.Domain;
using SIE.ESop.Displays.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.Displays.Configs
{
    /// <summary>
    /// ESOP文档来源
    /// </summary>
    [RootEntity, Serializable]
    [Label("ESOP文档来源")]
    public class DisplayPointDataConfigValue : ConfigValue
    {
        #region 数据来源 DataFrom
        /// <summary>
        /// 数据来源
        /// </summary>
        [Label("数据来源")]
        public static readonly Property<DisplayDataSource> DataFromProperty = P<DisplayPointDataConfigValue>.Register(e => e.DataFrom);

        /// <summary>
        /// 数据来源
        /// </summary>
        public DisplayDataSource DataFrom
        {
            get { return this.GetProperty(DataFromProperty); }
            set { this.SetProperty(DataFromProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>返回显示内容</returns>
        public override string Display()
        {
            string dataFrom = DataFrom == DisplayDataSource.Document ? "文档集".L10N() : "工程文件维护".L10N();
            return "{0}:根据工单/产品匹配文档集(数据源为{1})".L10nFormat(DataFrom.ToLabel().L10N(), dataFrom);
        }
    }
}
