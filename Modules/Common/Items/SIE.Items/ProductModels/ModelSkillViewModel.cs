using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.ProductModels
{
    /// <summary>
    /// 机型技能
    /// </summary>
    [RootEntity, Serializable]
    [Label("机型技能")]
    public class ModelSkillViewModel : ViewModel
    {
        #region 技能编码 SkillCode
        /// <summary>
        /// 技能编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> SkillCodeProperty = P<ModelSkillViewModel>.Register(e => e.SkillCode);

        /// <summary>
        /// 技能编码
        /// </summary>
        public string SkillCode
        {
            get { return this.GetProperty(SkillCodeProperty); }
            set { this.SetProperty(SkillCodeProperty, value); }
        }
        #endregion

        #region 技能名称 SkillName
        /// <summary>
        /// 技能名称
        /// </summary>
        [Label("技能名称")]
        public static readonly Property<string> SkillNameProperty = P<ModelSkillViewModel>.Register(e => e.SkillName);

        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillName
        {
            get { return this.GetProperty(SkillNameProperty); }
            set { this.SetProperty(SkillNameProperty, value); }
        }
        #endregion

        #region 需求人数 DemandQty
        /// <summary>
        /// 需求人数
        /// </summary>
        [Label("需求人数")]
        public static readonly Property<int?> DemandQtyProperty = P<ModelSkillViewModel>.Register(e => e.DemandQty);

        /// <summary>
        /// 需求人数
        /// </summary>
        public int? DemandQty
        {
            get { return this.GetProperty(DemandQtyProperty); }
            set { this.SetProperty(DemandQtyProperty, value); }
        }
        #endregion

        #region 实际人数 ActualQty
        /// <summary>
        /// 实际人数
        /// </summary>
        [Label("实际人数")]
        public static readonly Property<int?> ActualQtyProperty = P<ModelSkillViewModel>.Register(e => e.ActualQty);

        /// <summary>
        /// 实际人数
        /// </summary>
        public int? ActualQty
        {
            get { return this.GetProperty(ActualQtyProperty); }
            set { this.SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #region 缺编数量 LackQty
        /// <summary>
        /// 缺编数量
        /// </summary>
        [Label("缺编数量")]
        public static readonly Property<int?> LackQtyProperty = P<ModelSkillViewModel>.Register(e => e.LackQty);

        /// <summary>
        /// 缺编数量
        /// </summary>
        public int? LackQty
        {
            get { return this.GetProperty(LackQtyProperty); }
            set { this.SetProperty(LackQtyProperty, value); }
        }
        #endregion
    }
}
