using SIE.Domain;
using SIE.MES.Fixture;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Checker
{
    /// <summary>
    /// 检具维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("检具维护查询实体")]
    public class CheckerUpholdCriterial : Criteria
    {
        #region 检具编码 CheckerCode
        /// <summary>
        /// 检具编码
        /// </summary>
        [Required]
        [Label("检具编码")]
        public static readonly Property<string> CheckerCodeProperty = P<CheckerUpholdCriterial>.Register(e => e.CheckerCode);

        /// <summary>
        /// 检具编码
        /// </summary>
        public string CheckerCode
        {
            get { return this.GetProperty(CheckerCodeProperty); }
            set { this.SetProperty(CheckerCodeProperty, value); }
        }
        #endregion

        #region 检具名称 CheckerName
        /// <summary>
        /// 检具名称
        /// </summary>
        [Required]
        [Label("检具名称")]
        public static readonly Property<string> CheckerNameProperty = P<CheckerUpholdCriterial>.Register(e => e.CheckerName);

        /// <summary>
        /// 检具名称
        /// </summary>
        public string CheckerName
        {
            get { return this.GetProperty(CheckerNameProperty); }
            set { this.SetProperty(CheckerNameProperty, value); }
        }
        #endregion

        #region 有效日期 EffectiveDate
        /// <summary>
        /// 有效日期
        /// </summary>
        [Label("有效日期")]
        public static readonly Property<DateRange> EffectiveDateProperty = P<CheckerUpholdCriterial>.Register(e => e.EffectiveDate);

        /// <summary>
        /// 有效日期
        /// </summary>
        public DateRange EffectiveDate
        {
            get { return this.GetProperty(EffectiveDateProperty); }
            set { this.SetProperty(EffectiveDateProperty, value); }
        }
        #endregion

        #region 检具类型 CheckerType
        /// <summary>
        /// 检具类型
        /// </summary>
        [Required]
        [Label("检具类型")]
        public static readonly Property<string> CheckerTypeProperty = P<CheckerUpholdCriterial>.Register(e => e.CheckerType);

        /// <summary>
        /// 检具状态
        /// </summary>
        public string CheckerType
        {
            get { return this.GetProperty(CheckerTypeProperty); }
            set { this.SetProperty(CheckerTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<CheckerUpholdCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<CheckerUpholdCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<CheckerUpholdCriterial>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<CheckerUpholdCriterial>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingNoProperty = P<CheckerUpholdCriterial>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return this.GetProperty(DrawingNoProperty); }
            set { this.SetProperty(DrawingNoProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CheckerUpholdController>().CriterialCheckerUphold(this);
        }
    }
}
