using SIE.Domain;
using SIE.MES.Fixture;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Andon
{
    /// <summary>
    /// 维护安灯区域查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("维护安灯区域查询实体")]
    public class AndonUpholdCriterial:Criteria
    {
        #region 区域描述 AndonDesc
        /// <summary>
        /// 区域描述
        /// </summary>
        [Required]
        [Label("区域描述")]
        public static readonly Property<string> AndonDescProperty = P<AndonUpholdCriterial>.Register(e => e.AndonDesc);

        /// <summary>
        /// 区域描述
        /// </summary>
        public string AndonDesc
        {
            get { return this.GetProperty(AndonDescProperty); }
            set { this.SetProperty(AndonDescProperty, value); }
        }
        #endregion

        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Required]
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<AndonUpholdCriterial>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion

        #region 厂部编码 PlantCode
        /// <summary>
        /// 厂部编码
        /// </summary>
        [Required]
        [Label("厂部编码")]
        public static readonly Property<string> PlantCodeProperty = P<AndonUpholdCriterial>.Register(e => e.PlantCode);

        /// <summary>
        /// 厂部编码
        /// </summary>
        public string PlantCode
        {
            get { return this.GetProperty(PlantCodeProperty); }
            set { this.SetProperty(PlantCodeProperty, value); }
        }
        #endregion

        #region 厂部名称 PlantName
        /// <summary>
        /// 厂部名称
        /// </summary>
        [Required]
        [Label("厂部名称")]
        public static readonly Property<string> PlantNameProperty = P<AndonUpholdCriterial>.Register(e => e.PlantName);

        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName
        {
            get { return this.GetProperty(PlantNameProperty); }
            set { this.SetProperty(PlantNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonUpholdController>().CriterialAndonUphold(this);
        }
    }
}
