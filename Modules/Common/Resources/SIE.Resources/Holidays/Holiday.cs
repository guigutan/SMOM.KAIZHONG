using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Holidays
{
    /// <summary>
    /// 法定假期
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("法定假期")]
    public partial class Holiday : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Holiday()
        {
            BeginDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);
        }
        #region 开始日期 BeginDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateTime> BeginDateProperty = P<Holiday>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 结束日期 EndDate
        /// <summary>
        /// 结束日期
        /// </summary>
        [Label("结束日期")]
        public static readonly Property<DateTime> EndDateProperty = P<Holiday>.Register(e => e.EndDate);

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 假期名称 Remark
        /// <summary>
        /// 假期名称
        /// </summary>
        [Label("假期名称")]
        public static readonly Property<string> RemarkProperty = P<Holiday>.Register(e => e.Remark);

        /// <summary>
        /// 假期名称
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 法定假期 实体配置
    /// </summary>
    internal class HolidayConfig : EntityConfig<Holiday>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_HOLIDAY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}