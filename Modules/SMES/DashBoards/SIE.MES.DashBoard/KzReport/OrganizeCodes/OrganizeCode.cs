using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.OrganizeCodes
{
    /// <summary>
    /// 组织代码
    /// </summary>
    [RootEntity, Serializable]
    [Label("组织代码")]
    [CriteriaQuery]
    public partial  class OrganizeCode : DataEntity
    {
        #region 园区编码 ParkCode
        /// <summary>
        /// 园区编码
        /// </summary>
        [Label("园区编码")]
        public static readonly Property<string> ParkCodeProperty = P<OrganizeCode>.Register(e => e.ParkCode);

        /// <summary>
        /// 园区编码
        /// </summary>
        public string ParkCode
        {
            get { return this.GetProperty(ParkCodeProperty); }
            set { this.SetProperty(ParkCodeProperty, value); }
        }
        #endregion

        #region 园区名称 ParkName
        /// <summary>
        /// 园区名称
        /// </summary>
        [Label("园区名称")]
        public static readonly Property<string> ParkNameProperty = P<OrganizeCode>.Register(e => e.ParkName);

        /// <summary>
        /// 园区名称
        /// </summary>
        public string ParkName
        {
            get { return this.GetProperty(ParkNameProperty); }
            set { this.SetProperty(ParkNameProperty, value); }
        }
        #endregion

        #region 产品线 ProductLine
        /// <summary>
        /// 产品线
        /// </summary>
        [Label("产品线")]
        public static readonly Property<string> ProductLineProperty = P<OrganizeCode>.Register(e => e.ProductLine);

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine
        {
            get { return this.GetProperty(ProductLineProperty); }
            set { this.SetProperty(ProductLineProperty, value); }
        }
        #endregion

        #region 工厂编码
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<OrganizeCode>.Register(e => e.FactoryCode);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
            set { this.SetProperty(FactoryCodeProperty, value); }
        }
        #endregion

        #region 工厂名称
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<OrganizeCode>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { this.SetProperty(FactoryNameProperty, value); }
        }
        #endregion

        #region 厂部编码
        /// <summary>
        /// 厂部编码
        /// </summary>
        [Label("厂部编码")]
        public static readonly Property<string> PlantCodeProperty = P<OrganizeCode>.Register(e => e.PlantCode);

        /// <summary>
        /// 厂部编码
        /// </summary>
        public string PlantCode
        {
            get { return this.GetProperty(PlantCodeProperty); }
            set { this.SetProperty(PlantCodeProperty, value); }
        }
        #endregion

        #region 厂部名称
        /// <summary>
        /// 厂部名称
        /// </summary>
        [Label("厂部名称")]
        public static readonly Property<string> PlantNameProperty = P<OrganizeCode>.Register(e => e.PlantName);

        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName
        {
            get { return this.GetProperty(PlantNameProperty); }
            set { this.SetProperty(PlantNameProperty, value); }
        }
        #endregion

        #region 车间代码
        /// <summary>
        /// 车间代码
        /// </summary>
        [Label("车间代码")]
        public static readonly Property<string> WorkshopCodeProperty = P<OrganizeCode>.Register(e => e.WorkshopCode);

        /// <summary>
        /// 车间代码
        /// </summary>
        public string WorkshopCode
        {
            get { return this.GetProperty(WorkshopCodeProperty); }
            set { this.SetProperty(WorkshopCodeProperty, value); }
        }
        #endregion

        #region 车间名称
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkshopNameProperty = P<OrganizeCode>.Register(e => e.WorkshopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName
        {
            get { return this.GetProperty(WorkshopNameProperty); }
            set { this.SetProperty(WorkshopNameProperty, value); }
        }
        #endregion

        #region 车间描述
        /// <summary>
        /// 车间描述(可空)
        /// </summary>
        [Label("车间描述")]
        public static readonly Property<string> WorkshopDescriptionProperty = P<OrganizeCode>.Register(e => e.WorkshopDescription);

        /// <summary>
        /// 车间描述(可空)
        /// </summary>
        public string WorkshopDescription
        {
            get { return this.GetProperty(WorkshopDescriptionProperty); }
            set { this.SetProperty(WorkshopDescriptionProperty, value); }
        }
        #endregion

        #region MRP控制者
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<OrganizeCode>.Register(e => e.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
            set { this.SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        #region SAP生产调度员/工作中心负责人
        /// <summary>
        /// SAP生产调度员/工作中心负责人
        /// </summary>
        [Label("SAP生产调度员/工作中心负责人")]
        public static readonly Property<string> SapSchedulerProperty = P<OrganizeCode>.Register(e => e.SapScheduler);

        /// <summary>
        /// SAP生产调度员/工作中心负责人
        /// </summary>
        public string SapScheduler
        {
            get { return this.GetProperty(SapSchedulerProperty); }
            set { this.SetProperty(SapSchedulerProperty, value); }
        }

        public static explicit operator OrganizeCode(List<object> v)
        {
            throw new NotImplementedException();
        }
        #endregion

    }


    internal class OrganizeCodeConfig : EntityConfig<OrganizeCode>
    {
        //protected override void AddValidations(IValidationDeclarer rules)
        //{
        //    rules.AddRule(RegionBoard.RegionProperty, new NotDuplicateRule());
        //    base.AddValidations(rules);
        //}

        protected override void ConfigMeta()
        {
            Meta.MapTable("ORGANIZE_CODE").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
