using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.InspectionStandards;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验项目实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("检验项目")]
    public class InspectionItemViewModel : ViewModel
    {
        #region 检验项目 ModelInspecitonItem
        /// <summary>
        /// 机型检验项目ID
        /// </summary>
        [Label("检验项目")]
        public static readonly IRefIdProperty ModelInspecitonItemIdProperty =
            P<InspectionItemViewModel>.RegisterRefId(e => e.ModelInspecitonItemId, ReferenceType.Normal);

        /// <summary>
        /// 机型检验项目ID
        /// </summary>
        public double ModelInspecitonItemId
        {
            get { return (double)this.GetRefId(ModelInspecitonItemIdProperty); }
            set { this.SetRefId(ModelInspecitonItemIdProperty, value); }
        }

        /// <summary>
        /// 机型检验项目
        /// </summary>
        public static readonly RefEntityProperty<ModelInspectionItem> ModelInspecitonItemProperty =
            P<InspectionItemViewModel>.RegisterRef(e => e.ModelInspecitonItem, ModelInspecitonItemIdProperty);

        /// <summary>
        /// 机型检验项目
        /// </summary>
        public ModelInspectionItem ModelInspecitonItem
        {
            get { return this.GetRefEntity(ModelInspecitonItemProperty); }
            set { this.SetRefEntity(ModelInspecitonItemProperty, value); }
        }
        #endregion

        #region 规范下限 LimitLow
        /// <summary>
        /// 规范下限
        /// </summary>
        [Label("规范下限")]
        public static readonly Property<decimal?> LimitLowProperty = P<InspectionItemViewModel>.RegisterView(e => e.LimitLow, p => p.ModelInspecitonItem.LimitLow);

        /// <summary>
        /// 规范下限
        /// </summary>
        public decimal? LimitLow
        {
            get { return this.GetProperty(LimitLowProperty); }
        }
        #endregion

        #region 规范上限 LimitMax
        /// <summary>
        /// 规范上限
        /// </summary>
        [Label("规范上限")]
        public static readonly Property<string> LimitMaxProperty = P<InspectionItemViewModel>.RegisterView(e => e.LimitMax, p => p.ModelInspecitonItem.LimitMax);

        /// <summary>
        /// 规范上限
        /// </summary>
        public string LimitMax
        {
            get { return this.GetProperty(LimitMaxProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<InspectionItemViewModel>.RegisterView(e => e.UnitName, p => p.ModelInspecitonItem.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion


        #region 测试值 InspectionValue
        /// <summary>
        /// 测试值
        /// </summary>
        [Label("测试值")]
        public static readonly Property<decimal?> InspectionValueProperty = P<InspectionItemViewModel>.Register(e => e.InspectionValue);

        /// <summary>
        /// 测试值
        /// </summary>
        public decimal? InspectionValue
        {
            get { return this.GetProperty(InspectionValueProperty); }
            set { this.SetProperty(InspectionValueProperty, value); }
        }
        #endregion

        #region 合格 OK
        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        public static readonly Property<bool> IsOkProperty = P<InspectionItemViewModel>.Register(e => e.IsOk);

        /// <summary>
        /// 合格
        /// </summary>
        public bool IsOk
        {
            get { return this.GetProperty(IsOkProperty); }
            set { this.SetProperty(IsOkProperty, value); }
        }
        #endregion

        #region 不合格 NG
        /// <summary>
        /// 不合格
        /// </summary>
        [Label("不合格")]
        public static readonly Property<bool> IsNgProperty = P<InspectionItemViewModel>.Register(e => e.IsNg);

        /// <summary>
        /// 不合格
        /// </summary>
        public bool IsNg
        {
            get { return this.GetProperty(IsNgProperty); }
            set { this.SetProperty(IsNgProperty, value); }
        }
        #endregion

        #region 备注 Remarks
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarksProperty = P<InspectionItemViewModel>.Register(e => e.Remarks);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get { return this.GetProperty(RemarksProperty); }
            set { this.SetProperty(RemarksProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == IsOkProperty)
            {
                IsNg = !IsOk && IsNg;
            }
            if (e.Property == IsNgProperty)
            {
                IsOk = !IsNg && IsOk;
            }
            if (e.Property == InspectionValueProperty && ModelInspecitonItem != null && InspectionValue != null && this.ModelInspecitonItem.CheckTag == CheckTag.Quantitative)
            {
                bool isng = false;
                if (this.ModelInspecitonItem.LimitLowCompare == CompareType.GreaterThan)
                {
                    if (this.InspectionValue <= this.ModelInspecitonItem.LimitLow)
                        isng = true;
                }
                else
                {
                    if (this.InspectionValue < this.ModelInspecitonItem.LimitLow)
                        isng = true;
                }

                if (this.ModelInspecitonItem.LimitMaxCompare == CompareType.LessThan)
                {
                    if (this.InspectionValue >= this.ModelInspecitonItem.LimitMax)
                        isng = true;
                }
                else
                {
                    if (this.InspectionValue > this.ModelInspecitonItem.LimitMax)
                        isng = true;
                }

                if (isng)
                {
                    this.IsNg = true;
                }
                else
                {
                    this.IsOk = true;
                }

                //if (this.InspectionValue < this.ModelInspecitonItem.LimitLow || this.InspectionValue > this.ModelInspecitonItem.LimitMax)
                //{
                //    this.IsNg = true;
                //}
                //else
                //{
                //    this.IsOk = true;
                //}
            }
        }
    }
}
