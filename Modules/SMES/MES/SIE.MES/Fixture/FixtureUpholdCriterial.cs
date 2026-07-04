using SIE.Domain;
using SIE.MES.Checker;
using SIE.MES.ItemLine;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Fixture
{
    /// <summary>
    /// 工装维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工装维护查询实体")]
    public class FixtureUpholdCriterial : Criteria
    {
        #region 工装编码 FixtureCode
        /// <summary>
        /// 工装编码
        /// </summary>
        [Required]
        [Label("工装编码")]
        public static readonly Property<string> FixtureCodeProperty = P<FixtureUpholdCriterial>.Register(e => e.FixtureCode);

        /// <summary>
        /// 工装编码
        /// </summary>
        public string FixtureCode
        {
            get { return this.GetProperty(FixtureCodeProperty); }
            set { this.SetProperty(FixtureCodeProperty, value); }
        }
        #endregion

        #region 工装名称 FixtureName
        /// <summary>
        /// 工装名称
        /// </summary>
        [Required]
        [Label("工装名称")]
        public static readonly Property<string> FixtureNameProperty = P<FixtureUpholdCriterial>.Register(e => e.FixtureName);

        /// <summary>
        /// 工装名称
        /// </summary>
        public string FixtureName
        {
            get { return this.GetProperty(FixtureNameProperty); }
            set { this.SetProperty(FixtureNameProperty, value); }
        }
        #endregion

        #region 工装状态 FixtureState
        /// <summary>
        /// 工装状态
        /// </summary>
        [Required]
        [Label("工装状态")]
        public static readonly Property<string> FixtureStateProperty = P<FixtureUpholdCriterial>.Register(e => e.FixtureState);

        /// <summary>
        /// 工装状态
        /// </summary>
        public string FixtureState
        {
            get { return this.GetProperty(FixtureStateProperty); }
            set { this.SetProperty(FixtureStateProperty, value); }
        }
        #endregion

        #region 工装类型 FixtureType
        /// <summary>
        /// 工装类型
        /// </summary>
        [Required]
        [Label("工装类型")]
        public static readonly Property<string> FixtureTypeProperty = P<FixtureUpholdCriterial>.Register(e => e.FixtureType);

        /// <summary>
        /// 工装状态
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
            set { this.SetProperty(FixtureTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<FixtureUpholdCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<FixtureUpholdCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

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
            P<FixtureUpholdCriterial>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<FixtureUpholdCriterial>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 图号 Drawn
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawnProperty = P<FixtureUpholdCriterial>.Register(e => e.Drawn);

        /// <summary>
        /// 图号
        /// </summary>
        public string Drawn
        {
            get { return this.GetProperty(DrawnProperty); }
            set { this.SetProperty(DrawnProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FixtureUpholdController>().CriterialFixtureUphold(this);
        }
    }
}
