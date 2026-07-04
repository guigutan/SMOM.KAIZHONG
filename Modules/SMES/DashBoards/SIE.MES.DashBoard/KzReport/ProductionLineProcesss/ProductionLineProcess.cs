using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.DashBoard.KzReport.OrganizeCodes;
using SIE.MES.DashBoard.KzReport.ProductionProcesss;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.ProductionLineProcesss
{
    /// <summary>
    /// 产品线工序
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品线工序")]
    [CriteriaQuery]
    public class ProductionLineProcess : DataEntity
    {
        #region 产品线 ProductLine
        /// <summary>
        /// 产品线
        /// </summary>
        [Label("产品线")]
        public static readonly Property<string> ProductLineProperty = P<ProductionLineProcess>.Register(e => e.ProductLine);

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine
        {
            get { return this.GetProperty(ProductLineProperty); }
            set { this.SetProperty(ProductLineProperty, value); }
        }
        #endregion

        #region 厂部编码
        /// <summary>
        /// 厂部编码
        /// </summary>
        [Label("厂部编码")]
        public static readonly Property<string> PlantCodeProperty = P<ProductionLineProcess>.Register(e => e.PlantCode);

        /// <summary>
        /// 厂部编码
        /// </summary>
        public string PlantCode
        {
            get { return this.GetProperty(PlantCodeProperty); }
            set { this.SetProperty(PlantCodeProperty, value); }
        }
        #endregion

        #region 库存组织
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<string> InventoryCodeProperty = P<ProductionLineProcess>.Register(e => e.InventoryCode);

        /// <summary>
        /// 库存组织
        /// </summary>
        public string InventoryCode
        {
            get { return this.GetProperty(InventoryCodeProperty); }
            set { this.SetProperty(InventoryCodeProperty, value); }
        }
        #endregion

        #region 厂部名称
        /// <summary>
        /// 厂部名称
        /// </summary>
        [Label("厂部名称")]
        public static readonly Property<string> PlantNameProperty = P<ProductionLineProcess>.Register(e => e.PlantName);

        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName
        {
            get { return this.GetProperty(PlantNameProperty); }
            set { this.SetProperty(PlantNameProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ProductionLineProcess>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<ProductionLineProcess>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序编码
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProductionLineProcess>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion
    }
    internal class ProductionLineProcessConfig : EntityConfig<ProductionLineProcess>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    ProductionLineProcess.InventoryCodeProperty,
                    ProductionLineProcess.ProcessCodeProperty,
                },
                MessageBuilder = (e) =>
                {
                    return "同库存组织工序不能重复!".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("PRODUCTION_LINE_PROCESS").MapAllProperties();
            Meta.DisableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
