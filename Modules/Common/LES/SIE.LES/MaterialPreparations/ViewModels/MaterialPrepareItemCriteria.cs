using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.ViewModels
{
    /// <summary>
    /// 选择物料查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("选择物料查询实体")]
    public class MaterialPrepareItemCriteria : Criteria
    {
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialPrepareItemCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialPrepareItemCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 消耗方式 ConsumeMode
        /// <summary>
        /// 消耗方式
        /// </summary>
        [Label("消耗方式")]
        public static readonly Property<ConsumeMode?> ConsumeModeProperty = P<MaterialPrepareItemCriteria>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 消耗方式
        /// </summary>
        public ConsumeMode? ConsumeMode
        {
            get { return this.GetProperty(ConsumeModeProperty); }
            set { this.SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaterialPreparationController>().WorkShopSelectItemQuery(this);
        }
    }
}
