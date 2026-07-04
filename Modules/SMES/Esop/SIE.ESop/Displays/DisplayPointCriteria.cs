using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.ESop.Displays
{
    /// <summary>
    /// 显示点
    /// </summary>
    [QueryEntity, Serializable]
    [Label("显示点查询实体")]
    [DisplayMember(nameof(DisplayPointCriteria.Code))]
    public partial class DisplayPointCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<DisplayPointCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<DisplayPointCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<DisplayPointCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<DisplayPointCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<DisplayPointCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<DisplayPointCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>返回查询的显示点数据集</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DisplayPointController>().GetDisplayPointList(this);
        }
    }
}