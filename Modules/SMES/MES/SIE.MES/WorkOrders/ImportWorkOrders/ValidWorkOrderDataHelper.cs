using SIE.Common.ImportHelper;
using SIE.Core.WorkOrders;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;

namespace SIE.MES.WorkOrders.ImportWorkOrders
{
    /// <summary>
    /// 工单数据帮助类
    /// </summary>
    public static class ValidWorkOrderDataHelper
    {
        private const string NOT_EXISTS = "不存在于系统";

        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="productCodeDic">产品范围</param>
        /// <param name="context">产品编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidProduct(ref Dictionary<string, double> productCodeDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (productCodeDic == null)
            {
                productCodeDic = new Dictionary<string, double>();
            }

            if (!productCodeDic.ContainsKey(context))
            {
                Item product = RT.Service.Resolve<ItemController>().GetItemFromCode(context);
                if (product != null)
                {
                    productCodeDic.Add(context, product.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10N();
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证工单类型
        /// </summary>
        /// <param name="workOrderTypeRange">工单类型范围</param>
        /// <param name="context">工单类型</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidWorkOrderType(ref Dictionary<string, Enum> workOrderTypeRange, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (workOrderTypeRange.Count == 0)
            {
                workOrderTypeRange = ImportExtension.GetEnumLabel(typeof(WorkOrderType), string.Empty);
            }

            if (!workOrderTypeRange.ContainsKey(context))
            {
                messageTip = "只能选择".L10N() + string.Join("、", workOrderTypeRange);
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证车间
        /// </summary>
        /// <param name="workShopCodeDic">车间范围</param>
        /// <param name="context">车间编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidWorkShop(ref Dictionary<string, double> workShopCodeDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (workShopCodeDic == null)
            {
                workShopCodeDic = new Dictionary<string, double>();
            }

            if (!workShopCodeDic.ContainsKey(context))
            {
                Enterprise workShop = RT.Service.Resolve<EnterpriseController>().GetEnterprises(context, EnterpriseType.Shop);
                if (workShop != null)
                {
                    workShopCodeDic.Add(context, workShop.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10N();
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="resourceCodeDic">资源范围</param>
        /// <param name="context">资源编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidResource(ref Dictionary<string, double> resourceCodeDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (resourceCodeDic == null)
            {
                resourceCodeDic = new Dictionary<string, double>();
            }

            if (!resourceCodeDic.ContainsKey(context))
            {
                WipResource resource = RT.Service.Resolve<EnterpriseController>().GetResources(context);
                if (resource != null)
                {
                    resourceCodeDic.Add(context, resource.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10N();
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证上级工单
        /// </summary>
        /// <param name="parentWOCodeDic">上级工单范围</param>
        /// <param name="context">上级工单编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回验证是否通过</returns>
        public static bool ValidParentWorkOrder(ref Dictionary<string, double> parentWOCodeDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (parentWOCodeDic == null)
            {
                parentWOCodeDic = new Dictionary<string, double>();
            }

            if (!parentWOCodeDic.ContainsKey(context))
            {
                WorkOrder wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(context);
                if (wo != null)
                {
                    parentWOCodeDic.Add(context, wo.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10N();
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证ERP工单
        /// </summary>
        /// <param name="erpWorkOrderDic">ERP工单范围</param>
        /// <param name="context">ERP工单编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回验证是否通过</returns>
        public static bool ValidERPWorkOrder(ref Dictionary<string, double> erpWorkOrderDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (erpWorkOrderDic == null)
            {
                erpWorkOrderDic = new Dictionary<string, double>();
            }

            if (!erpWorkOrderDic.ContainsKey(context))
            {
                ErpWorkOrder erpWorkOrder = RT.Service.Resolve<WorkOrderController>().GetErpWorkOrder(context);
                if (erpWorkOrder != null)
                {
                    erpWorkOrderDic.Add(context, erpWorkOrder.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10N();
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}