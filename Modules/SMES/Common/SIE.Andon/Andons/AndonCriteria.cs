using RazorEngine.Compilation.ImpromptuInterface.Dynamic;
using SIE.Andon.Andons.Enum;
using SIE.Authority;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    [QueryEntity, Serializable]
    public class AndonCriteria : Criteria
    {
        #region 安灯编码 Code
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        public static readonly Property<string> CodeProperty = P<AndonCriteria>.Register(e => e.Code);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 安灯名称 Name
        /// <summary>
        /// 安灯名称
        /// </summary>
        [Label("安灯名称")]
        public static readonly Property<string> NameProperty = P<AndonCriteria>.Register(e => e.Name);

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 安灯类型 AndonType
        /// <summary>
        /// 安灯类型Id
        /// </summary>
        [Label("安灯类型")]
        public static readonly IRefIdProperty AndonTypeIdProperty =
            P<AndonCriteria>.RegisterRefId(e => e.AndonTypeId, ReferenceType.Normal);

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double? AndonTypeId
        {
            get { return (double?)this.GetRefNullableId(AndonTypeIdProperty); }
            set { this.SetRefNullableId(AndonTypeIdProperty, value); }
        }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public static readonly RefEntityProperty<AndonType> AndonTypeProperty =
            P<AndonCriteria>.RegisterRef(e => e.AndonType, AndonTypeIdProperty);

        /// <summary>
        /// 安灯类型
        /// </summary>
        public AndonType AndonType
        {
            get { return this.GetRefEntity(AndonTypeProperty); }
            set { this.SetRefEntity(AndonTypeProperty, value); }
        }
        #endregion

        #region 安灯大类 AndonClass
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Label("安灯大类")]
        public static readonly Property<AndonTypeClass?> AndonClassProperty = P<AndonCriteria>.Register(e => e.AndonClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass? AndonClass
        {
            get { return this.GetProperty(AndonClassProperty); }
            set { this.SetProperty(AndonClassProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<AndonCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 创建时间 CreateTime
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateTimeProperty = P<AndonCriteria>.Register(e => e.CreateTime);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateTime
        {
            get { return this.GetProperty(CreateTimeProperty); }
            set { this.SetProperty(CreateTimeProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonController>().GetAndons(this); ;
        }
    }
}
