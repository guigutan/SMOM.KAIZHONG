using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    #region 产线直通率设置验证规则
    /// <summary>
    /// 车间直通率非重复验证
    /// </summary>
    [DisplayName("车间直通率非重复验证")]
    [Description("车间不能相同")]
    public class ShopFpySettingNotDuplicateRule : NotDuplicateRule<ShopFpySetting>
    {
        /// <summary>
        /// 配置验证信息
        /// </summary>
        public ShopFpySettingNotDuplicateRule()
        {
            Properties.Add(ShopFpySetting.ShopIdProperty);
            MessageBuilder = (e) =>
            {
                var setting = e as ShopFpySetting;
                return "已存在车间[{0}]的直通率设置".L10nFormat(setting.Shop?.Code);
            };
        }
    }

    /// <summary>
    /// 产线直通率非重复验证
    /// </summary>
    [DisplayName("产线直通率非重复验证")]
    [Description("产线不能相同")]
    public class LineFpySettingNotDuplicateRule : NotDuplicateRule<LineFpySetting>
    {
        /// <summary>
        /// 配置验证信息
        /// </summary>
        public LineFpySettingNotDuplicateRule()
        {
            Properties.Add(LineFpySetting.ShopFpySettingIdProperty);
            Properties.Add(LineFpySetting.ResourceIdProperty);
            MessageBuilder = (e) =>
            {
                var setting = e as LineFpySetting;
                return "已存在车间[{0}],产线[{1}]的直通率设置".L10nFormat(setting.ShopFpySetting?.Shop?.Code, setting.Resource?.Code);
            };
        }
    }

    /// <summary>
    /// 车间直通率删除验证
    /// </summary>
    [DisplayName("车间直通率删除验证")]
    [Description("车间直通率被产线直通率引用不允许删除")]
    public class ShopFpySettingDeleteRule : NoReferencedRule<ShopFpySetting>
    {
        /// <summary>
        /// 配置验证信息
        /// </summary>
        public ShopFpySettingDeleteRule()
        {
            Properties.Add(LineFpySetting.ShopFpySettingIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "存在产线直通率设置不允许删除".L10N();
            };
        }
    }
    #endregion

    #region 产品直通率设置验证规则 
    /// <summary>
    /// 产品机型直通率非重复验证
    /// </summary>
    [DisplayName("产品机型直通率非重复验证")]
    [Description("产品机型不能相同")]
    public class ProductModelFpySettingNotDuplicateRule : NotDuplicateRule<ProductModelFpySetting>
    {
        /// <summary>
        /// 配置验证信息
        /// </summary>
        public ProductModelFpySettingNotDuplicateRule()
        {
            Properties.Add(ProductModelFpySetting.ModelIdProperty);
            MessageBuilder = (e) =>
            {
                var setting = e as ProductModelFpySetting;
                return "已存在产品机型[{0}]的直通率设置".L10nFormat(setting.Model?.Code);
            };
        }
    }

    /// <summary>
    /// 产品直通率非重复验证
    /// </summary>
    [DisplayName("产品直通率非重复验证")]
    [Description("产品不能相同")]
    public class ProductFpySettingNotDuplicateRule : NotDuplicateRule<ProductFpySetting>
    {
        /// <summary>
        /// 配置验证信息
        /// </summary>
        public ProductFpySettingNotDuplicateRule()
        {
            Properties.Add(ProductFpySetting.ProductModelFpySettingIdProperty);
            Properties.Add(ProductFpySetting.ProductIdProperty);
            MessageBuilder = (e) =>
            {
                var setting = e as ProductFpySetting;
                return "已存在产品机型[{0}],产品[{1}]的直通率设置".L10nFormat(setting.ProductModelFpySetting?.Model?.Code, setting.Product?.Code);
            };
        }
    }

    /// <summary>
    /// 产品机型直通率删除验证
    /// </summary>
    [DisplayName("产品机型直通率删除验证")]
    [Description("产品机型直通率被产品直通率引用不允许删除")]
    public class ProductModelFpySettingDeleteRule : NoReferencedRule<ProductModelFpySetting>
    {
        /// <summary>
        /// 配置验证信息
        /// </summary>
        public ProductModelFpySettingDeleteRule()
        {
            Properties.Add(ProductFpySetting.ProductModelFpySettingIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "存在产品直通率设置不允许删除".L10N();
            };
        }
    }
    #endregion

    #region 期望值和预警值验证规则

    /// <summary>
    /// 直通率期望值和预警值验证
    /// </summary>
    [DisplayName("直通率预警值验证")]
    [Description("预警值不能大于期望值")]
    public class FpySettingAlarmValueRule : PropertyRule<FpySetting>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return FpySetting.AlarmProperty;
            }
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">直通率设置</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var setting = entity as FpySetting;
            if (setting.Alarm != 0 && setting.Alarm > setting.Desired)
                e.BrokenDescription = "预警值不能大于期望值".L10N();
        }
    }
    #endregion 

    /// <summary>
    /// 生产资源删除验证规则
    /// </summary>
    [DisplayName("生产资源删除验证规则")]
    [Description("产线直通率设置引用的生产资源不能删除")]
    public class WipResourceDeleteRuleLineFpySetting : EntityRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceDeleteRuleLineFpySetting()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 根据生产资源是否被产线直通率设置引用，判断是否能被删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var wipResource = entity as WipResource;
            var flag = RT.Service.Resolve<FpySettingController>().LineFpySettingHasUsedWipResource(wipResource.Id);
            if (flag)
            {
                e.BrokenDescription = "生产资源 [{0}] 被产线直通率设置引用, 不能删除!".L10nFormat(wipResource.Code);
            }
        }
    }
}