using SIE.Andon.Andons.Enum;
using SIE.Domain;
using SIE.Domain.Caching;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安灯类型维护查询实体")]
    public class AndonTypeCriterial : Criteria
    {
        #region 安灯类型编码 AndonTypeCode
        /// <summary>
        /// 安灯类型编码
        /// </summary>
        [Label("安灯类型编码")]
        public static readonly Property<string> AndonTypeCodeProperty = P<AndonTypeCriterial>.Register(e => e.AndonTypeCode);

        /// <summary>
        /// 安灯类型编码
        /// </summary>
        public string AndonTypeCode
        {
            get { return this.GetProperty(AndonTypeCodeProperty); }
            set { this.SetProperty(AndonTypeCodeProperty, value); }
        }
        #endregion

        #region 安灯类型名称 AndonTypeName
        /// <summary>
        /// 安灯类型名称
        /// </summary>
        [Label("安灯类型名称")]
        public static readonly Property<string> AndonTypeNameProperty = P<AndonTypeCriterial>.Register(e => e.AndonTypeName);

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName
        {
            get { return this.GetProperty(AndonTypeNameProperty); }
            set { this.SetProperty(AndonTypeNameProperty, value); }
        }
        #endregion

        #region 安灯大类 AndonTypeClass
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Label("安灯大类")]
        public static readonly Property<AndonTypeClass?> AndonTypeClassProperty = P<AndonTypeCriterial>.Register(e => e.AndonTypeClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass? AndonTypeClass
        {
            get { return this.GetProperty(AndonTypeClassProperty); }
            set { this.SetProperty(AndonTypeClassProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<AndonTypeCriterial>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 创建日期 CreateTime
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateTimeProperty = P<AndonTypeCriterial>.Register(e => e.CreateTime);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateTime
        {
            get { return this.GetProperty(CreateTimeProperty); }
            set { this.SetProperty(CreateTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonTypeController>().GetAndonTypes(this);
        }
    }
}
