using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 固定资产查询
    /// </summary>
    [QueryEntity, Serializable]
    public class FixedAssetsAccountCriteria : Criteria
    {
        #region 资产编码 Code
        /// <summary>
        /// 资产编码
        /// </summary>
        [Label("资产编码")]
        public static readonly Property<string> CodeProperty = P<FixedAssetsAccountCriteria>.Register(e => e.Code);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 资产名称 Name
        /// <summary>
        /// 资产名称
        /// </summary>
        [Label("资产名称")]
        public static readonly Property<string> NameProperty = P<FixedAssetsAccountCriteria>.Register(e => e.Name);

        /// <summary>
        /// 资产名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 资产分类 AssetsType
        /// <summary>
        /// 资产分类(快码)
        /// </summary>
        [Label("资产分类")]
        [Required]
        public static readonly Property<string> AssetsCategoryProperty = P<FixedAssetsAccountCriteria>.Register(e => e.AssetsCategory);

        /// <summary>
        /// 资产分类
        /// </summary>
        public string AssetsCategory
        {
            get { return GetProperty(AssetsCategoryProperty); }
            set { SetProperty(AssetsCategoryProperty, value); }
        }
        #endregion

        #region 资产来源 AssetsSource
        /// <summary>
        /// 资产来源
        /// </summary>
        [Label("资产来源")]
        public static readonly Property<AssetsSource?> AssetsSourceProperty = P<FixedAssetsAccountCriteria>.Register(e => e.AssetsSource);

        /// <summary>
        /// 资产来源
        /// </summary>
        public AssetsSource? AssetsSource
        {
            get { return GetProperty(AssetsSourceProperty); }
            set { SetProperty(AssetsSourceProperty, value); }
        }
        #endregion

        #region 管理状态 ManageStatus
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<ManageState?> ManageStatusProperty = P<FixedAssetsAccountCriteria>.Register(e => e.ManageStatus);

        /// <summary>
        /// 管理状态
        /// </summary>
        public ManageState? ManageStatus
        {
            get { return GetProperty(ManageStatusProperty); }
            set { SetProperty(ManageStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreationDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreationDateProperty = P<FixedAssetsAccountCriteria>.Register(e => e.CreationDate,
            new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.DateTime, });

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreationDate
        {
            get { return GetProperty(CreationDateProperty); }
            set { SetProperty(CreationDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FixedAssetsAccountController>().Fetch(this);

        }
    }
}
