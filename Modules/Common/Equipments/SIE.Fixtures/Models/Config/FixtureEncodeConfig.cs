using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.Models.Config
{

    /// <summary>
    /// 工治具型号配置项
    /// </summary>
    [System.ComponentModel.DisplayName("工治具编码配置项")]
    [System.ComponentModel.Description("工治具编码配置项")]
    public class FixtureEncodeConfig : ModuleConfig<FixtureEncodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly FixtureEncodeConfigValue defaultValue = new FixtureEncodeConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override FixtureEncodeConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 工治具编码配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具仓库分类置项值")]
    public class FixtureEncodeConfigValue : ConfigValue
    {
        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            if (WareHouseTypeIds.IsNullOrEmpty())
            {
                return "工治具仓库分类:NIL".L10N();
            }
            else
            {
                return "工治具仓库分类:{0}".L10nFormat(WareHouseTypeName);
            }
        }


        #region 仓库分类 WareHouseCategorys
        /// <summary>
        /// 仓库分类Id
        /// </summary>
        [Label("仓库分类")]
        public static readonly IRefIdProperty WareHouseCategorysIdProperty =
            P<FixtureEncodeConfigValue>.RegisterRefId(e => e.WareHouseCategorysId, ReferenceType.Normal);

        /// <summary>
        /// 仓库分类Id
        /// </summary>
        public double WareHouseCategorysId
        {
            get { return (double)this.GetRefId(WareHouseCategorysIdProperty); }
            set { this.SetRefId(WareHouseCategorysIdProperty, value); }
        }

        /// <summary>
        /// 仓库分类
        /// </summary>
        public static readonly RefEntityProperty<Catalog> WareHouseCategorysProperty =
            P<FixtureEncodeConfigValue>.RegisterRef(e => e.WareHouseCategory, WareHouseCategorysIdProperty);

        /// <summary>
        /// 仓库分类
        /// </summary>
        public Catalog WareHouseCategory
        {
            get { return this.GetRefEntity(WareHouseCategorysProperty); }
            set { this.SetRefEntity(WareHouseCategorysProperty, value); }
        }
        #endregion

        #region 仓库分类集合 WareHouseTypeIds
        /// <summary>
        /// 仓库分类集合
        /// </summary>
        [Label("仓库分类集合")]
        public static readonly Property<string> WareHouseTypeIdsProperty = P<FixtureEncodeConfigValue>.Register(e => e.WareHouseTypeIds);

        /// <summary>
        /// 仓库分类集合
        /// </summary>
        public string WareHouseTypeIds
        {
            get { return GetProperty(WareHouseTypeIdsProperty); }
            set { SetProperty(WareHouseTypeIdsProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 仓库分类编码 WareHouseTypeCode
        /// <summary>
        /// 仓库分类编码
        /// </summary>
        [Label("仓库分类编码")]
        public static readonly Property<string> WareHouseTypeCodeProperty = P<FixtureEncodeConfigValue>.RegisterView(e => e.WareHouseTypeCode, p => p.WareHouseCategory.Code);

        /// <summary>
        /// 仓库分类编码
        /// </summary>
        public string WareHouseTypeCode
        {
            get { return this.GetProperty(WareHouseTypeCodeProperty); }
        }
        #endregion 

        #region 仓库分类名称 WareHouseTypeName
        /// <summary>
        /// 仓库分类名称
        /// </summary>
        [Label("仓库分类名称")]
        public static readonly Property<string> WareHouseTypeNameProperty = P<FixtureEncodeConfigValue>.RegisterView(e => e.WareHouseTypeName, p => p.WareHouseCategory.Name);

        /// <summary>
        /// 仓库分类名称
        /// </summary>
        public string WareHouseTypeName
        {
            get { return this.GetProperty(WareHouseTypeNameProperty); }
        }
        #endregion

        #endregion
    }
}
