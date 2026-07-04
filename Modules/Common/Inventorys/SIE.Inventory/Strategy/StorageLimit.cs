using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 储存限制
    /// </summary>
    public enum StorageLimit
    {
        /// <summary>
        /// 保税库位
        /// </summary>
        [Label("保税库位")]
        Bonded = 10,

        /// <summary>
        /// 专储物料库位
        /// </summary>
        [Label("专储物料库位")]
        SpecialItem = 20,

        /// <summary>
        /// 单一SKU储存库位
        /// </summary>
        [Label("单一SKU储存库位")]
        SingleSku = 30,

        /// <summary>
        /// 禁止混放货主库位
        /// </summary>
        [Label("禁止混放货主库位")]
        BanMixed = 40,

        /// <summary>
        /// 湿敏器件库位
        /// </summary>
        [Label("湿敏器件库位")]
        HumSenGrade = 50,

        /// <summary>
        /// RoHS等级匹配
        /// </summary>
        [Label("RoHS等级匹配")]
        RoHsGrade = 60,

        /// <summary>
        /// 静电敏感物料库位
        /// </summary>
        [Label("静电敏感物料库位")]
        ElecSenGrade = 70,

        /// <summary>
        /// 禁止批属性09混放库位
        /// </summary>
        [Label("禁止批属性09混放库位")]
        BanMixedBatch09 = 80,

        /// <summary>
        /// 禁止批属性10混放库位
        /// </summary>
        [Label("禁止批属性10混放库位")]
        BanMixedBatch10 = 90,
    }
}