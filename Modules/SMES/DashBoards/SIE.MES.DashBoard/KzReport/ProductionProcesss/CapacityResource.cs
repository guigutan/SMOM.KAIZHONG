using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MES.DashBoard.KzReport.ProductionLineProcesss;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.ProductionProcesss
{
    /// <summary>
    /// 产能资源工序
    /// </summary>
    [RootEntity, Serializable]
    [Label("产能资源工序")]
    [CriteriaQuery]
    public class CapacityResource : DataEntity
    {
        #region 资源 Line
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<CapacityResource>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> LineProperty = P<CapacityResource>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(LineProperty); }
            set { SetRefEntity(LineProperty, value); }
        }
        #endregion

        #region 资源类型 ResourceType
        /// <summary>
        /// 资源类型
        /// </summary>
        [Label("资源类型")]
        public static readonly Property<string> ResourceTypeProperty = P<CapacityResource>.Register(e => e.ResourceType);

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResourceType
        {
            get { return this.GetProperty(ResourceTypeProperty); }
            set { this.SetProperty(ResourceTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<CapacityResource>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<CapacityResource>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region UPH UPH
        /// <summary>
        /// UPH
        /// </summary>
        [Label("UPH")]
        public static readonly Property<decimal> UphProperty = P<CapacityResource>.Register(e => e.Uph);

        /// <summary>
        /// UPH
        /// </summary>
        public decimal Uph
        {
            get { return this.GetProperty(UphProperty); }
            set { this.SetProperty(UphProperty, value); }
        }
        #endregion


        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<CapacityResource>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion


    }
    internal class CapacityResourceConfig : EntityConfig<CapacityResource>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    CapacityResource.ResourceIdProperty,
                    CapacityResource.ProcessIdProperty,
                },
                MessageBuilder = (e) =>
                {
                    return "同产线工序不能重复!".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("CAPACITY_RESOURCE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
