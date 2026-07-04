using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.Experiences
{
    /// <summary>
    /// 物料查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class ExperienceItemCriteria : Criteria
    {
        #region 编码 Code 
        /// <summary>
        /// 编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<ExperienceItemCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name 
        /// <summary>
        /// 名称
        /// </summary> 
        [Label("物料名称")]
        public static readonly Property<string> NameProperty = P<ExperienceItemCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ItemType?> TypeProperty = P<ExperienceItemCriteria>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemType? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  查询物料
        /// </summary>
        /// <returns>物料列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemController>().GetItems(Code, Name, Type, this.PagingInfo);
        }
    }
}
