using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LoadItems;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区被引用不能删除规则
    /// </summary>
    [DisplayName("工位货区被引用不能删除规则")]
    class StorageAreaRefStorageLocationRule : NoReferencedRule<StorageArea>
    {
        /// <summary>
        /// 初始化需要验证的属性
        /// </summary>
        public StorageAreaRefStorageLocationRule()
        {
            Properties.Add(StorageLocation.StorageAreaIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "[工位货区:{0}]已经被[产线货区货位]引用了{1}次".L10nFormat((e as StorageArea).Name, i);
            };
        }
    }

    /// <summary>
    /// 产线货区被引用不能删除规则
    /// </summary>
    [DisplayName("工位货区被引用不能删除规则")]
    class StorageAreaRefStorageSaftyRule : NoReferencedRule<StorageArea>
    {
        /// <summary>
        /// 初始化需要验证的属性
        /// </summary>
        public StorageAreaRefStorageSaftyRule()
        {
            Properties.Add(StorageSafty.StorageAreaIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "[工位货区:{0}]已经被[产线库存]引用了{1}次".L10nFormat((e as StorageArea).Name, i);
            };
        }
    }

    /// <summary>
    /// 产线货区被引用不能删除规则
    /// </summary>
    [DisplayName("工位货区被引用不能删除规则")]
    class StorageAreaRefStationStorageAreaRule : NoReferencedRule<StorageArea>
    {
        /// <summary>
        /// 初始化需要验证的属性
        /// </summary>
        public StorageAreaRefStationStorageAreaRule()
        {
            Properties.Add(StationStorageArea.StorageAreaIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "[工位货区:{0}]已经被[产线货区工位]引用了{1}次".L10nFormat((e as StorageArea).Name, i);
            };
        }
    }

    /// <summary>
    /// 产线货位被引用不能删除规则
    /// </summary>
    [DisplayName("[产线货区货位被引用不能删除规则")]
    class StorageLocationReferencedRule : NoReferencedRule<StorageLocation>
    {
        /// <summary>
        /// 初始化需要验证的属性
        /// </summary>
        public StorageLocationReferencedRule()
        {
            Properties.Add(ItemStorage.StorageLocationIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "[产线货区货位:{0}]已经被[产线物料货位]引用了{1}次".L10nFormat((e as StorageLocation).Name, i);
            };
        }
    }

    /// <summary>
    /// 产线货位的物料非重校验规则
    /// </summary>
    [DisplayName("产线货位的物料非重校验规则")]
    [Description("产线货位的物料不能相同")]
    public class ItemStorageRule : NotDuplicateRule<ItemStorage>
    {
        /// <summary>
        /// 初始化需要验证的属性
        /// </summary>
        public ItemStorageRule()
        {
            Properties.Add(ItemStorage.ItemIdProperty);
            Properties.Add(ItemStorage.StorageLocationIdProperty);
            MessageBuilder = (e) =>
            {
                var itemStorage = e as ItemStorage;
                return "已存在物料[{0}]".L10nFormat(itemStorage.Item.Name);
            };
        }
    }

    /// <summary>
    /// 添加工位库区关联物料库存不允许物料重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("工位库区关联物料库存验证规则")]
    [System.ComponentModel.Description("添加工位库区关联物料库存不允许物料重复验证")]
    public class NotDuplicateStorageSafty : NotDuplicateRule<StorageSafty>
    {
        /// <summary>
        /// 初始化需要验证的属性
        /// </summary>
        public NotDuplicateStorageSafty()
        {
            Properties.Add(StorageSafty.StorageAreaIdProperty);
            Properties.Add(StorageSafty.ItemIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = (e) as StorageSafty;
                return "该库区已存在物料[{0}]，不允许重复".L10nFormat(entity.Item.Name);
            };
        }
    }

    /// <summary>
    /// 工位货区工位的验证规则类
    /// </summary>
    [DisplayName("工位货区工位验证规则")]
    [Description("工位货区工位验证规则")]
    public class StationStorageAreaRule : EntityRule<StationStorageArea>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationStorageAreaRule()
        {
            Scope = MetaModel.EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">工位货区工位实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var stationStorageArea = entity as StationStorageArea;
            var loadItemCtl = RT.Service.Resolve<LoadItemController>();
            var check = loadItemCtl.CheckStationItems(stationStorageArea.StationId);
            if (check)
            {
                e.BrokenDescription = "不能删除已经上料的工位".L10N();
            }
        }
    }

    /// <summary>
    /// 产线物料货位验证规则
    /// </summary>
    [DisplayName("产线物料货位验证规则")]
    [Description("产线物料货位验证规则")]
    public class ItemStorageEntityRule : EntityRule<ItemStorage>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">产线物料货位实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var itemStorage = entity as ItemStorage;

            var itemConsumeMode = itemStorage.Item?.ConsumeMode;

            if (itemConsumeMode == Items.ConsumeMode.Pull && itemStorage.CallMaterialBatch <= 0)
            {
                e.BrokenDescription = "拉式物料-叫料批量必须大于0！".L10N();
            }

            if (itemConsumeMode == Items.ConsumeMode.Pull &&
                RT.Service.Resolve<StorageController>().IsStorageMixItem(itemStorage.StorageLocationId, itemStorage.ItemId))
            {
                e.BrokenDescription = "货位【{0}】不支持混放，只能存放一种拉式物料！！".L10nFormat(itemStorage.StorageLocation.Code);
            }
        }
    }

    /// <summary>
    /// 工位货区验证规则
    /// </summary>
    [DisplayName("工位货区验证规则")]
    [Description("工位货区验证规则")]
    public class StorageAreaEntityRule : EntityRule<StorageArea>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">工位货区实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var storageArea = entity as StorageArea;
            if (storageArea.IsMixItem)
                return;
            if (!storageArea.IsMixItem && storageArea.PersistenceStatus != PersistenceStatus.New
                && RT.Service.Resolve<StorageController>().IsNoUpdateMixItem(storageArea.Id))
            {
                e.BrokenDescription = "此工位货区货位存在多个物料，不能取消混放".L10N();
            }
        }
    }
}