using SIE.Barcodes;
using SIE.Core.Items;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SIE.MES.WorkOrders.ImportWorkOrders
{
    /// <summary>
    /// 导入工单条码帮助类
    /// </summary>
    public static class ValidBarcodeDataHelper
    {
        /// <summary>
        /// 验证工单编号
        /// </summary>
        /// <param name="workOrderNoDic">工单字典</param>
        /// <param name="retrospectTypeDic">工单物料追溯方式字典</param>
        /// <param name="itemDic">物料字典</param>
        /// <param name="context">工单编号</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidWorkOrder(ref Dictionary<string, WorkOrder> workOrderNoDic, ref Dictionary<double, RetrospectType?> retrospectTypeDic, ref Dictionary<double, string> itemDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (workOrderNoDic == null)
            {
                workOrderNoDic = new Dictionary<string, WorkOrder>();
            }

            if (itemDic == null)
            {
                itemDic = new Dictionary<double, string>();
            }

            if (retrospectTypeDic == null)
            {
                retrospectTypeDic = new Dictionary<double, RetrospectType?>();
            }

            if (!workOrderNoDic.ContainsKey(context))
            {
                var woCt = RT.Service.Resolve<WorkOrderController>();
                WorkOrder wo = woCt.GetWorkOrder(context);

                if (wo != null)
                {
                    if (!itemDic.ContainsKey(wo.ProductId))
                        itemDic.Add(wo.ProductId, wo.Product.Code);
                    workOrderNoDic.Add(context, wo);

                    if (!retrospectTypeDic.ContainsKey(wo.ProductId))
                    {
                        var restrospectType = woCt.GetRetrospectType(wo.ProductId);
                        retrospectTypeDic.Add(wo.ProductId, restrospectType);
                    }
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary> 
        /// 验证条码
        /// </summary>
        /// <param name="context">条码序号</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        public static bool ValidBarcode(string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (!string.IsNullOrEmpty(context))
            {
                Regex regex = new Regex(@"[\u4e00-\u9fa5]");
                if (regex.IsMatch(context))
                {
                    messageTip = "：{0} 带有中文字符！".L10nFormat(context);
                    isValid = false;
                }
                else
                {
                    if (RT.Service.Resolve<BarcodeController>().UinqueExists(context))
                    {
                        messageTip = "：{0} 已存在条码表".L10nFormat(context);
                        isValid = false;
                    }
                }
            }
            else
            {
                messageTip = "必填！".L10N();
                isValid = false;
            }

            return isValid;
        }
    }
}
