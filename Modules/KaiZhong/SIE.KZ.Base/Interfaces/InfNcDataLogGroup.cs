using SIE.Domain;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    /// <summary>
    /// 主数据NC与总控的接口日志
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InfNcDataLogGroupCriteria))]
    [Label("总控与NC接口日志")]
    public class InfNcDataLogGroup : InfNcDataLogGroupBase
    {
        #region 同步结果 SysncResult
        /// <summary>
        /// 同步结果
        /// </summary>
        [Label("同步结果")]
        public static readonly Property<string> SysncResultProperty = P<InfNcDataLogGroup>.Register(e => e.SysncResult);

        /// <summary>
        /// 同步结果
        /// </summary>
        public string SysncResult
        {
            get { return this.GetProperty(SysncResultProperty); }
            set { this.SetProperty(SysncResultProperty, value); }
        }
        #endregion

        #region 成功数据 SuccessJson
        /// <summary>
        /// 成功数据
        /// </summary>
        [Label("成功数据")]
        public static readonly Property<string> SuccessJsonProperty = P<InfNcDataLogGroup>.Register(e => e.SuccessJson);

        /// <summary>
        /// 成功数据
        /// </summary>
        public string SuccessJson
        {
            get { return this.GetProperty(SuccessJsonProperty); }
            set { this.SetProperty(SuccessJsonProperty, value); }
        }
        #endregion

        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<InfNcDataLogGroup>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }

        #endregion 开始时间 BeginDate

        #region 结束时间 EndDate

        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime?> EndDateProperty = P<InfNcDataLogGroup>.Register(e => e.EndDate);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }

        #endregion 结束时间 EndDate

        #region 执行结果 CallResult

        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<CallResult> CallResultProperty = P<InfNcDataLogGroup>.Register(e => e.CallResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public CallResult CallResult
        {
            get { return GetProperty(CallResultProperty); }
            set { SetProperty(CallResultProperty, value); }
        }

        #endregion 执行结果 CallResult

        #region 工厂错误信息 FactoryErrorMsg
        /// <summary>
        /// 工厂错误信息
        /// </summary>
        [Label("工厂错误信息")]
        public static readonly Property<string> FactoryErrorMsgProperty = P<InfNcDataLogGroup>.Register(e => e.FactoryErrorMsg);

        /// <summary>
        /// 工厂错误信息
        /// </summary>
        public string FactoryErrorMsg
        {
            get { return this.GetProperty(FactoryErrorMsgProperty); }
            set { this.SetProperty(FactoryErrorMsgProperty, value); }
        }
        #endregion

        #region 由原工厂分发 IsDistribute
        /// <summary>
        /// 由原工厂分发
        /// </summary>
        [Label("由原工厂分发")]
        public static readonly Property<bool?> IsDistributeProperty = P<InfNcDataLogGroup>.Register(e => e.IsDistribute);

        /// <summary>
        /// 由原工厂分发
        /// </summary>
        public bool? IsDistribute
        {
            get { return this.GetProperty(IsDistributeProperty); }
            set { this.SetProperty(IsDistributeProperty, value); }
        }
        #endregion

    }


    /// <summary>
    /// 主数据NC接口日志 实体配置
    /// </summary>
    internal class InfNcDataLogGroupConfig : EntityConfig<InfNcDataLogGroup>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("Inf_Nc_LogGroup").MapAllProperties();
            Meta.Property(InfNcDataLogGroup.DataJsonsProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogGroup.ResponseContentProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogGroup.SuccessJsonProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogGroup.ErrorMsgProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogGroup.FactoryErrorMsgProperty).ColumnMeta.HasLength("MAX");

            Meta.Property(InfNcDataLogGroup.BeginDateProperty).ColumnMeta.HasIndex();
            Meta.Property(InfNcDataLogGroup.EndDateProperty).ColumnMeta.HasIndex();
            Meta.Property(InfNcDataLogGroup.InvOrgProperty).ColumnMeta.HasIndex();
            Meta.Property(InfNcDataLogGroup.InfTypeProperty).ColumnMeta.HasIndex();
            Meta.Property(InfNcDataLogGroup.KeyMsgoneProperty).ColumnMeta.HasLength("MAX").HasIndex();
            Meta.Property(InfNcDataLogGroup.KeyMsgtwoProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogGroup.KeyMsgthreeProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogGroup.KeyMsgfourProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogGroup.KeyMsgfiveProperty).ColumnMeta.HasLength("MAX");
            Meta.DisablePhantoms();
        }
    }

}
