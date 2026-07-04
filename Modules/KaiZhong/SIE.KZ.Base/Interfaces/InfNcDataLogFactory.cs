using SIE.Common.Configs;
using SIE.Domain;
using SIE.KZ.Base.Configs;
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
    /// 总控与工厂接口日志
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InfNcDataLogFactoryCriteria))]
    [EntityWithConfig(typeof(InfLogFacFailConfig))]
    [Label("总控与工厂接口日志")]
    public class InfNcDataLogFactory : InfNcDataLogGroupBase
    {
        #region 失败次数 FailCount
        /// <summary>
        /// 失败次数
        /// </summary>
        [Label("失败次数")]
        public static readonly Property<int> FailCountProperty = P<InfNcDataLogFactory>.Register(e => e.FailCount);

        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailCount
        {
            get { return this.GetProperty(FailCountProperty); }
            set { this.SetProperty(FailCountProperty, value); }
        }
        #endregion

        #region 总控Guid GroupGuid
        /// <summary>
        /// 总控Guid
        /// </summary>
        [Label("总控Guid")]
        public static readonly Property<string> GroupGuidProperty = P<InfNcDataLogFactory>.Register(e => e.GroupGuid);

        /// <summary>
        /// 总控Guid
        /// </summary>
        public string GroupGuid
        {
            get { return this.GetProperty(GroupGuidProperty); }
            set { this.SetProperty(GroupGuidProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 主数据NC接口日志 实体配置
    /// </summary>
    internal class InfNcDataLogFactoryConfig : EntityConfig<InfNcDataLogFactory>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("Inf_Nc_LogFactory").MapAllProperties();
            Meta.Property(InfNcDataLogFactory.DataJsonsProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogFactory.ResponseContentProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogFactory.ErrorMsgProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogFactory.InvOrgProperty).ColumnMeta.HasIndex();
            Meta.Property(InfNcDataLogFactory.InfTypeProperty).ColumnMeta.HasIndex();
            Meta.Property(InfNcDataLogFactory.KeyMsgoneProperty).ColumnMeta.HasLength("MAX").HasIndex();
            Meta.Property(InfNcDataLogFactory.KeyMsgtwoProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogFactory.KeyMsgthreeProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogFactory.KeyMsgfourProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(InfNcDataLogFactory.KeyMsgfiveProperty).ColumnMeta.HasLength("MAX");
            Meta.DisablePhantoms();
        }
    }

}
