using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ViewModels
{
    /// <summary>
    /// 项目号需求设计-工艺卡基本属性
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号需求设计-工艺卡基本属性")]
    public class ProjectDesignCardProperty : ViewModel
    {
        #region 属性 PropertyName
        /// <summary>
        /// 属性
        /// </summary>
        [Label("属性")]
        public static readonly Property<string> PropertyNameProperty = P<ProjectDesignCardProperty>.Register(e => e.PropertyName);

        /// <summary>
        /// 属性
        /// </summary>
        public string PropertyName
        {
            get { return this.GetProperty(PropertyNameProperty); }
            set { this.SetProperty(PropertyNameProperty, value); }
        }
        #endregion

        #region 属性值 PropertyValue
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> PropertyValueProperty = P<ProjectDesignCardProperty>.Register(e => e.PropertyValue);

        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue
        {
            get { return this.GetProperty(PropertyValueProperty); }
            set { this.SetProperty(PropertyValueProperty, value); }
        }
        #endregion

        #region 单位 PropertyUnit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> PropertyUnitProperty = P<ProjectDesignCardProperty>.Register(e => e.PropertyUnit);

        /// <summary>
        /// 单位
        /// </summary>
        public string PropertyUnit
        {
            get { return this.GetProperty(PropertyUnitProperty); }
            set { this.SetProperty(PropertyUnitProperty, value); }
        }
        #endregion

    }
}
