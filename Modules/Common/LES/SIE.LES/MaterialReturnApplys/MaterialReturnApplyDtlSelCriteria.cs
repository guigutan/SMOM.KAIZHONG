using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 选择退料申请明细查询视图
    /// </summary>
    [QueryEntity, Serializable]
    [Label("选择退料申请明细查询视图")]
    public class MaterialReturnApplyDtlSelCriteria : Criteria
    {
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialReturnApplyDtlSelCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 工单Id WoId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double?> WoIdProperty = P<MaterialReturnApplyDtlSelCriteria>.Register(e => e.WoId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WoId
        {
            get { return this.GetProperty(WoIdProperty); }
            set { this.SetProperty(WoIdProperty, value); }
        }
        #endregion

        #region 仓库Id WareId
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库Id")]
        public static readonly Property<double?> WareIdProperty = P<MaterialReturnApplyDtlSelCriteria>.Register(e => e.WareId);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WareId
        {
            get { return this.GetProperty(WareIdProperty); }
            set { this.SetProperty(WareIdProperty, value); }
        }
        #endregion

        #region 库位Id StorageId
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位Id")]
        public static readonly Property<double?> StorageIdProperty = P<MaterialReturnApplyDtlSelCriteria>.Register(e => e.StorageId);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageId
        {
            get { return this.GetProperty(StorageIdProperty); }
            set { this.SetProperty(StorageIdProperty, value); }
        }
        #endregion

        #region 车间 WorkShopId
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<double?> WorkShopIdProperty = P<MaterialReturnApplyDtlSelCriteria>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间
        /// </summary>
        public double? WorkShopId
        {
            get { return this.GetProperty(WorkShopIdProperty); }
            set { this.SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 批次号 Lot
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotProperty = P<MaterialReturnApplyDtlSelCriteria>.Register(e => e.Lot);

        /// <summary>
        /// 批次号
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
            set { this.SetProperty(LotProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<MaterialReturnApplyDtlSelCriteria>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaterialReturnApplyController>().GetMaterialReturnApplyDetailSelects(this);
        }
    }
}
