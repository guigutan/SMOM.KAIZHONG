using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.ERPJobCloseRules
{
    /// <summary>
    /// 交易期关闭日
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("交易期关闭日")]
    public class ErpJobCloseRule : DataEntity
    {
        #region 期间 During
        /// <summary>
        /// 期间
        /// </summary>
        [Label("期间")]
        public static readonly Property<string> DuringProperty = P<ErpJobCloseRule>.Register(e => e.During);

        /// <summary>
        /// 期间
        /// </summary>
        public string During
        {
            get { return this.GetProperty(DuringProperty); }
            set { this.SetProperty(DuringProperty, value); }
        }
        #endregion

        #region 交易期关闭开始时间 StartTime
        /// <summary>
        /// 交易期关闭开始时间
        /// </summary>
        [Label("交易期关闭开始时间")]
        public static readonly Property<DateTime?> StartTimeProperty = P<ErpJobCloseRule>.Register(e => e.StartTime);

        /// <summary>
        /// 交易期关闭开始时间
        /// </summary>
        public DateTime? StartTime
        {
            get { return this.GetProperty(StartTimeProperty); }
            set { this.SetProperty(StartTimeProperty, value); }
        }
        #endregion

        #region 交易期关闭结束时间 EndTime
        /// <summary>
        /// 交易期关闭结束时间
        /// </summary>
        [Label("交易期关闭结束时间")]
        public static readonly Property<DateTime?> EndTimeProperty = P<ErpJobCloseRule>.Register(e => e.EndTime);

        /// <summary>
        /// 交易期关闭结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class ErpJobCloseRuleConfig : EntityConfig<ErpJobCloseRule>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ERP_CLOSE_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
