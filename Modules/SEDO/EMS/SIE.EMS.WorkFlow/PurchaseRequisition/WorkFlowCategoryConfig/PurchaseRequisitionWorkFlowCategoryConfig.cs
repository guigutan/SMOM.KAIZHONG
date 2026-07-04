using Newtonsoft.Json;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.WorkFlow.Base.FlowDefinitions.Categorys;
using System;
using System.Text;

namespace SIE.EMS.WorkFlow.PurchaseRequisition.WorkFlowCategoryConfig
{
    /// <summary>
    /// 资产采购申请工作流分类配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("资产采购申请工作流分类配置")]
    public class PurchaseRequisitionWorkFlowCategoryConfig : WorkFlowCategoryConfigBase
    {
        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<PurchaseRequisitionWorkFlowCategoryConfig>.Register(e => e.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType? PurchaseObjectType
        {
            get { return this.GetProperty(PurchaseObjectTypeProperty); }
            set { this.SetProperty(PurchaseObjectTypeProperty, value); }
        }
        #endregion

        #region 采购类型 PurchaseType
        /// <summary>
        /// 采购类型
        /// </summary>
        [Label("采购类型")]
        public static readonly Property<PurchaseType?> PurchaseTypeProperty = P<PurchaseRequisitionWorkFlowCategoryConfig>.Register(e => e.PurchaseType);

        /// <summary>
        /// 采购类型
        /// </summary>
        public PurchaseType? PurchaseType
        {
            get { return GetProperty(PurchaseTypeProperty); }
            set { SetProperty(PurchaseTypeProperty, value); }
        }
        #endregion


        /// <summary>
        /// 转换成Json配置字符串
        /// </summary>
        /// <returns></returns>
        public override string ToConfigJson()
        {
            var model = new
            {
                PurchaseObjectType,
                PurchaseType
            };
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// 获取展示内容
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayConfigStr()
        {            
            return "采购对象：{0};采购类型：{1};".L10nFormat(PurchaseObjectType?.ToLabel(), PurchaseType?.ToLabel());
        }
    }

    /// <summary>
    /// 视图配置
    /// </summary>
    public class PurchaseRequisitionWorkFlowCategoryConfigViewConfig : WebViewConfig<PurchaseRequisitionWorkFlowCategoryConfig>
    {
        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.PurchaseType).ShowInDetail();
            View.Property(p => p.PurchaseObjectType).ShowInDetail();
        }
    }
}
