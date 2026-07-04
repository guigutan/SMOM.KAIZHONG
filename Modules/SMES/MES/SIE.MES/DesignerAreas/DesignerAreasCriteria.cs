using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DesignerAreas
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("查询实体")]
    public partial class DesignerAreaCriteria : Criteria
    {

        /// <summary>
        /// 看板区域编码
        /// </summary>
        [Label("看板区域编码")]
        public static readonly Property<string> AreaCodeProperty = P<DesignerAreaCriteria>.Register(e => e.AreaCode);
        /// <summary>
        /// 看板区域编码
        /// </summary>
        public string AreaCode
        {
            get { return this.GetProperty(AreaCodeProperty); }
            set { this.SetProperty(AreaCodeProperty, value); }
        }


        /// <summary>
        /// 看板区域名称
        /// </summary>
        [Label("看板区域名称")]
        public static readonly Property<string> AreaNameProperty = P<DesignerAreaCriteria>.Register(e => e.AreaName);
        /// <summary>
        /// 看板区域名称
        /// </summary>
        public string AreaName
        {
            get { return this.GetProperty(AreaNameProperty); }
            set { this.SetProperty(AreaNameProperty, value); }
        }












        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DesignerAreaController>().Fetch(this);
        }





    }
}