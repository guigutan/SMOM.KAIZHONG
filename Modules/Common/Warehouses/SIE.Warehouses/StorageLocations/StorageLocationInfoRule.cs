using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;

namespace SIE.Warehouses
{
    #region 基本资料
    /// <summary>
    /// 长(M)必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("长(M)必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationInfoLengthRule : EntityRule<StorageLocationInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationInfoLengthRule()
        {
            Property = StorageLocationInfo.LengthProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationInfo = entity as StorageLocationInfo;
            if (storageLocationInfo.Length.HasValue && storageLocationInfo.Length < 0)
                e.BrokenDescription = "长(M)请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 宽(M)必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("宽(M)必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationInfoWidthRule : EntityRule<StorageLocationInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationInfoWidthRule()
        {
            Property = StorageLocationInfo.WidthProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationInfo = entity as StorageLocationInfo;
            if (storageLocationInfo.Width.HasValue && storageLocationInfo.Width < 0)
                e.BrokenDescription = "宽(M)请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 高(M)必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("高(M)必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationInfoHeightRule : EntityRule<StorageLocationInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationInfoHeightRule()
        {
            Property = StorageLocationInfo.HeightProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationInfo = entity as StorageLocationInfo;
            if (storageLocationInfo.Height.HasValue && storageLocationInfo.Height < 0)
                e.BrokenDescription = "高(M)请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 库位层数必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("库位层数必须大于 0")]
    [System.ComponentModel.Description("请输入大于 0 的数字")]
    public class StorageLocationInfoLayerCountRule : EntityRule<StorageLocationInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationInfoLayerCountRule()
        {
            Property = StorageLocationInfo.LayerCountProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationInfo = entity as StorageLocationInfo;
            if (storageLocationInfo.LayerCount <= 0)
                e.BrokenDescription = "库位层数请输入大于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 同一个仓库库位编码不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("同一个仓库库位编码不能重复")]
    [System.ComponentModel.Description("同一个仓库库位编码不能重复")]
    public class StorageLocationCodeNotDuplicateRule : NotDuplicateRule<StorageLocation>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public StorageLocationCodeNotDuplicateRule()
        {
            Properties.Add(StorageLocation.WarehouseIdProperty);
            Properties.Add(StorageLocation.CodeProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = e =>
            {
                var storageLocation = e as StorageLocation;
                return string.Format("仓库已经存在相同的库位编码[{0}]".L10nFormat(storageLocation.Code));
            };
        }
    }

    /// <summary>
    /// 同一个仓库库位名称不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("同一个仓库库位名称不能重复")]
    [System.ComponentModel.Description("同一个仓库库位名称不能重复")]
    public class LocationNameNotDuplicateRule : NotDuplicateRule<StorageLocation>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public LocationNameNotDuplicateRule()
        {
            Properties.Add(StorageLocation.WarehouseIdProperty);
            Properties.Add(StorageLocation.NameProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = e =>
            {
                var storageLocation = e as StorageLocation;
                return string.Format("仓库已经存在相同的库位名称[{0}]".L10nFormat(storageLocation.Name));
            };
        }
    }
    #endregion

    #region 操作管理
    /// <summary>
    /// 上架顺序必须大于0
    /// </summary>
    [System.ComponentModel.DisplayName("上架顺序必须大于0")]
    [System.ComponentModel.Description("请输入大于 0 的数字")]
    public class StorageLocationOperationUpOrderRule : EntityRule<StorageLocationOperation>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationOperationUpOrderRule()
        {
            Property = StorageLocationOperation.UpOrderIndexProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationOperation = entity as StorageLocationOperation;
            if (storageLocationOperation.UpOrderIndex <= 0)
                e.BrokenDescription = "上架顺序请输入大于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 拣货顺序必须大于0
    /// </summary>
    [System.ComponentModel.DisplayName("拣货顺序必须大于0")]
    [System.ComponentModel.Description("请输入大于 0 的数字")]
    public class StorageLocationOperationPickOrderRule : EntityRule<StorageLocationOperation>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationOperationPickOrderRule()
        {
            Property = StorageLocationOperation.PickOrderIndexProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationOperation = entity as StorageLocationOperation;
            if (storageLocationOperation.PickOrderIndex <= 0)
                e.BrokenDescription = "拣货顺序请输入大于 0 的数字".L10N();
        }
    }
    #endregion

    #region 仓储资料
    /// <summary>
    /// 储存湿度下限必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("储存湿度下限必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationLayinInfoHumidityLowerRule : PropertyRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return StorageLocationLayinInfo.HumidityLowerProperty;
            }
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.HumidityLower.HasValue && storageLocationLayinInfo.HumidityLower < 0)
                e.BrokenDescription = "储存湿度下限请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 储存湿度上限必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("储存湿度上限必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationLayinInfoHumidityUpperRule : PropertyRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return StorageLocationLayinInfo.HumidityUpperProperty;
            }
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.HumidityUpper.HasValue && storageLocationLayinInfo.HumidityUpper < 0)
                e.BrokenDescription = "储存湿度上限请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 重量限制必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("重量限制(KG)必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationLayinInfoWeightRule : PropertyRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return StorageLocationLayinInfo.WeightLimitProperty;
            }
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.WeightLimit.HasValue && storageLocationLayinInfo.WeightLimit < 0)
                e.BrokenDescription = "重量限制(KG)请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 体积限制(CBM)必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("体积限制(CBM)必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationLayinInfoVolumeRule : PropertyRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return StorageLocationLayinInfo.VolumeLimitProperty;
            }
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.VolumeLimit.HasValue && storageLocationLayinInfo.VolumeLimit < 0)
                e.BrokenDescription = "体积限制(CBM)请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 箱数限制必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("箱数限制必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationLayinInfoBoxCountRule : PropertyRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return StorageLocationLayinInfo.BoxCountLimitProperty;
            }
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.BoxCountLimit.HasValue && storageLocationLayinInfo.BoxCountLimit < 0)
                e.BrokenDescription = "箱数限制请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 托数限制必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("托数限制必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationLayinInfoTrayCountRule : PropertyRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return StorageLocationLayinInfo.TrayCountLimitProperty;
            }
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.TrayCountLimit.HasValue && storageLocationLayinInfo.TrayCountLimit < 0)
                e.BrokenDescription = "托数限制请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 数量限制必须为非负数
    /// </summary>
    [System.ComponentModel.DisplayName("数量限制必须为非负数")]
    [System.ComponentModel.Description("请输入大于等于 0 的数字")]
    public class StorageLocationLayinInfoAmountRule : PropertyRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return StorageLocationLayinInfo.AmountLimitProperty;
            }
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.AmountLimit.HasValue && storageLocationLayinInfo.AmountLimit < 0)
                e.BrokenDescription = "数量限制请输入大于等于 0 的数字".L10N();
        }
    }

    /// <summary>
    /// 库位存储温度验证
    /// </summary>
    [System.ComponentModel.DisplayName("库位存储温度验证")]
    [System.ComponentModel.Description("库存存储温度从小到大")]
    public class StorageLocationTemperatureRule : EntityRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationTemperatureRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.TemperatureLower.HasValue && storageLocationLayinInfo.TemperatureUpper.HasValue &&
                storageLocationLayinInfo.TemperatureLower > storageLocationLayinInfo.TemperatureUpper)
                e.BrokenDescription = "库存存储温度必须从小到大".L10N();
        }
    }

    /// <summary>
    /// 库位存储湿度验证
    /// </summary>
    [System.ComponentModel.DisplayName("库位存储湿度验证")]
    [System.ComponentModel.Description("库存存储湿度从小到大")]
    public class StorageLocationHumidityRule : EntityRule<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationHumidityRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocationLayinInfo = entity as StorageLocationLayinInfo;
            if (storageLocationLayinInfo.HumidityLower.HasValue && storageLocationLayinInfo.HumidityUpper.HasValue &&
                storageLocationLayinInfo.HumidityLower > storageLocationLayinInfo.HumidityUpper)
                e.BrokenDescription = "库存存储湿度必须从小到大".L10N();
        }
    }
    #endregion

    #region 验证被库位使用的基础数据不能删除
    /// <summary>
    /// 库区已经关联库位，不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("库区已经关联库位，不能删除")]
    public class UndeleteInvolveStorageLocationArea : NoReferencedRule<StorageArea>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteInvolveStorageLocationArea()
        {
            Properties.Add(StorageLocation.AreaIdProperty);
            MessageBuilder = (o, e) =>
            {
                var storageArea = o as StorageArea;
                return "库区[{0}]已经被[{1}]引用，不能删除".L10nFormat(storageArea.Code, "库位".L10N());
            };
        }
    }

    /// <summary>
    /// 库区已经关联库位，不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("NoReferencedRule验证规则")]
    [System.ComponentModel.Description("物料已经关联库位，不能删除")]
    public class UndeleteInvolveStorageLocationItem : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteInvolveStorageLocationItem()
        {
            Properties.Add(StorageLocationItemList.ItemIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已经被[{1}]引用，不能删除".L10nFormat(item.Code, "库位".L10N());
            };
        }
    }
    #endregion

    #region 验证库位中仓库与库区的关系
    /// <summary>
    /// 验证库位中仓库与库区的关系
    /// </summary>
    [System.ComponentModel.DisplayName("验证库位中仓库与库区的关系")]
    [System.ComponentModel.Description("验证库位中仓库与库区是否合法")]
    public class StorageLocationSuperiorRule : EntityRule<StorageLocation>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageLocationSuperiorRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageLocation = entity as StorageLocation;
            if (storageLocation == null || storageLocation.Area == null)
                return;
            StorageArea newestStorageArea = RF.GetById<StorageArea>(storageLocation.AreaId);
            if (newestStorageArea.WarehouseId != storageLocation.WarehouseId)
            {
                e.BrokenDescription = string.Format("仓库[{0}]没有包含库区[{1}]，请重新检查".L10nFormat(storageLocation.Warehouse.Code, storageLocation.Area.Code));
            }
        }
    }
    #endregion
}
