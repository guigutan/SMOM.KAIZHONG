using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.CSM.Common
{
    /// <summary>
    /// 区域信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("区域信息")]
    [DisplayMember(nameof(Region))]
    public class RegionalInfo : DataEntity
    {
        #region 区域 Country
        /// <summary>
        /// 区域
        /// </summary>
        [Label("区域")]
        public static readonly Property<string> RegionProperty = P<RegionalInfo>.Register(e => e.Region);

        /// <summary>
        /// 区域
        /// </summary>
        public string Region
        {
            get { return GetProperty(RegionProperty); }
            set { SetProperty(RegionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 区域信息 实体配置
    /// </summary>
    internal class RegionalInfoConfig : EntityConfig<RegionalInfo>
    {
        /// <summary>
        /// 数据库区域信息表名
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("REGIONAL_INFO").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.DisableDataSync();
            Meta.SupportTree();
            Meta.DisablePhantoms();
        }
    }
}
