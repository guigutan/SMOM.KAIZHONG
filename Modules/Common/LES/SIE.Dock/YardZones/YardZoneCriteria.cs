using SIE.Dock.YardMaintains;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardZones
{
    /// <summary>
    /// 园片区维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("园片区维护查询实体")]
    public partial class YardZoneCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<YardZoneCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<YardZoneCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 园区 YardMaintainName
        /// <summary>
        /// 园区Id
        /// </summary>
        [Label("园区")]
        public static readonly IRefIdProperty YardMaintainNameIdProperty =
            P<YardZoneCriteria>.RegisterRefId(e => e.YardMaintainNameId, ReferenceType.Normal);

        /// <summary>
        /// 园区Id
        /// </summary>
        public double? YardMaintainNameId
        {
            get { return (double?)this.GetRefNullableId(YardMaintainNameIdProperty); }
            set { this.SetRefNullableId(YardMaintainNameIdProperty, value); }
        }

        /// <summary>
        /// 园区
        /// </summary>
        public static readonly RefEntityProperty<YardMaintain> YardMaintainNameProperty =
            P<YardZoneCriteria>.RegisterRef(e => e.YardMaintainName, YardMaintainNameIdProperty);

        /// <summary>
        /// 园区
        /// </summary>
        public YardMaintain YardMaintainName
        {
            get { return this.GetRefEntity(YardMaintainNameProperty); }
            set { this.SetRefEntity(YardMaintainNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<YardZoneService>().GetYardZones(this);
        }
    }
}
