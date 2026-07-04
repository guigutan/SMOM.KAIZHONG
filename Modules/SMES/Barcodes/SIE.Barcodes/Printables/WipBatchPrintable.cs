using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Presentation;
using NPOI.HSSF.Record.Aggregates;
using NPOI.SS.Formula.Functions;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using SIE.EventMessages.Items;
using SIE.EventMessages.Items.Datas;
using SIE.EventMessages.MES.Dispatchs;
using SIE.EventMessages.MES.SuspectProductLabel;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.Resources.WipResources;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SIE.Barcodes.Printables
{
    /// <summary>
    /// 生产批次
    /// </summary>
    [Serializable]
    [DisplayName("生产批次")]
    public class WipBatchPrintable : LabelPrintable<WipBatch>
    {

        private Dictionary<double, string> dicP_Layout = new Dictionary<double, string>();
        private Dictionary<double, string> dicP_Layout_Code = new Dictionary<double, string>();
        private Dictionary<double, string> dicP_Unit = new Dictionary<double, string>();
        private Dictionary<double, string> dicresourName = new Dictionary<double, string>();
        private Dictionary<double, decimal> dicP_Zcode = new Dictionary<double, decimal>();
        private Dictionary<string, ParentItemData> dicparentItemData = new Dictionary<string, ParentItemData>();
        private Dictionary<double, ItemCusotmerRelationData> dicitemCusotmer = new Dictionary<double, ItemCusotmerRelationData>();

        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = base.GetPropertys(type).ToList();
            propertys.Add("P_PlanQty");                           //订单数量    
            propertys.Add("P_ShortDescription");                  //旧料号
            propertys.Add("P_ProductCode");                          //物料编码
            propertys.Add("P_ProductName");                       //物料名称/物料描述
            propertys.Add("P_WorkOrderNo");                       //生产订单/工单号
            propertys.Add("P_Layout");                            //工艺路线
            propertys.Add("P_Layout_Code");                       //工艺路线
            propertys.Add("P_Type");                        //报工类型
            propertys.Add("P_WorkOrderBatchNo");                 //工单计划批次
            propertys.Add("P_Unit");                            //单位
            propertys.Add("P_Zmc");                             //名称
            propertys.Add("P_Normt");                           //颜色
            propertys.Add("P_Zkhxhgy");                         //可焊点
            propertys.Add("P_Zjdlx");                           //胶带类型
            propertys.Add("P_Zmodel");                          //型号
            propertys.Add("P_Zgg");                             //规格
            propertys.Add("P_AxisNumber");                      //轴号
            propertys.Add("P_Bn");                              //批号
            propertys.Add("P_Zcode");                           //分单数量
            propertys.Add("P_ParentItemCode");                  //父级物料
            propertys.Add("P_ParentWeight");                    //父级净重
            propertys.Add("P_ParentWeightUnit");                //父级净重单位
            propertys.Add("P_ParentShortDesc");                 //父级旧料号
            propertys.Add("P_Weight");                          //物料净重
            propertys.Add("P_WeightUnit");                      //净重单位
            propertys.Add("P_BatchNo14to22");                       //取Batch_No的14至22位
            propertys.Add("P_ItemCustomer");                    //物料客户编码
            propertys.Add("P_ItemAttribute1");                  //属性1(其他特性字段)
            propertys.Add("P_SerialNumber");                    //序号
            propertys.Add("P_PrintProcess");                    //打印工序
            return propertys;
        }

        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data">对象</param>
        /// <returns>字符串</returns>
        public override string ConverterData(object data)
        {
            var content = base.ConverterData(data);
            WipBatch labeldata = data as WipBatch;
            //工艺路线
            var P_Layout = "";
            var P_Layout_Code = "";
            if (labeldata.WorkOrder != null)
            {
                if (dicP_Layout.ContainsKey(labeldata.WorkOrderId))
                {
                    P_Layout = dicP_Layout[labeldata.WorkOrderId];
                }
                else
                {
                    P_Layout = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderLayoutByWoId(labeldata.WorkOrderId);
                    dicP_Layout.Add(labeldata.WorkOrderId, P_Layout);
                }
                if (dicP_Layout_Code.ContainsKey(labeldata.WorkOrderId))
                {
                    P_Layout_Code = dicP_Layout_Code[labeldata.WorkOrderId];
                }
                else
                {
                    P_Layout_Code = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderLayoutByWoCodeId(labeldata.WorkOrderId);
                    dicP_Layout_Code.Add(labeldata.WorkOrderId, P_Layout_Code);
                }
                //P_Layout = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderLayoutByWoId(labeldata.WorkOrderId);
                //P_Layout_Code = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderLayoutByWoCodeId(labeldata.WorkOrderId);
            }

            var P_Type = RT.Service.Resolve<WipBatchController>().GetWipBatchType(labeldata);

            var P_PlanQty = labeldata?.WorkOrder?.PlanQty == null ? "" : labeldata?.WorkOrder?.PlanQty.ToString();
            var P_ShortDescription = labeldata?.WorkOrder?.Product?.ShortDescription == null ? "" : labeldata?.WorkOrder?.Product?.ShortDescription;
            var P_ProductCode = labeldata?.WorkOrder?.Product.Code == null ? "" : labeldata?.WorkOrder?.Product.Code;
            var P_ProductName = labeldata?.WorkOrder?.Product?.Name == null ? "" : labeldata?.WorkOrder?.Product?.Name;
            var P_WorkOrderNo = labeldata?.WorkOrder?.No == null ? "" : labeldata?.WorkOrder?.No;
            var P_WorkOrderBatchNo = labeldata?.WorkOrder?.BatchNo == null ? "" : labeldata?.WorkOrder?.BatchNo;
            var P_Unit = "";
            if (labeldata != null && labeldata.WorkOrder != null && labeldata.WorkOrder.ProductId > 0)
            {
                if (dicP_Unit.ContainsKey(labeldata.WorkOrder.ProductId))
                {
                    P_Unit = dicP_Unit[labeldata.WorkOrder.ProductId];
                }
                else
                {
                    P_Unit = RT.Service.Resolve<IItem>().GetUnitNameGetItemId(labeldata.WorkOrder.ProductId);
                    dicP_Unit.Add(labeldata.WorkOrder.ProductId, P_Unit);
                }
                //P_Unit = RT.Service.Resolve<IItem>().GetUnitNameGetItemId(labeldata.WorkOrder.ProductId);

            }
            var P_Weight = labeldata?.WorkOrder?.Product?.Weight;
            var P_WeightUnit = labeldata?.WorkOrder?.Product?.WeightUnit;

            var P_Zmc = labeldata?.WorkOrder?.Product?.Zmc;
            var P_Normt = labeldata?.WorkOrder?.Product?.Normt;
            var P_Zkhxhgy = labeldata?.WorkOrder?.Product?.Zkhxhgy;
            var P_Zjdlx = labeldata?.WorkOrder?.Product?.Zjdlx;
            var P_Zmodel = labeldata?.WorkOrder?.Product?.Zmodel;
            var P_Zgg = labeldata?.WorkOrder?.Product?.Zgg;
            var P_AxisNumber = "";
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            if (invOrg.ExternalId == "2810" && labeldata != null && !labeldata.BatchNo.IsNullOrEmpty() && labeldata.BatchNo.Length >= 3)
            {
                P_AxisNumber = labeldata.BatchNo.Substring(labeldata.BatchNo.Length - 3, 3);
                //RT.Service.Resolve<IDispatchs>().GetPrintAxisNumberByTaskId(labeldata.DispatchTaskId.Value);
            }
            //批号（YYYYMMDD+班次A或B+产线名称后两位）
            var P_Bn = RT.Service.Resolve<ItemCusotmerDataController>().GetShiftDate().ToString("yyyyMMdd") + RT.Service.Resolve<ItemCusotmerDataController>().ShiftAlgorithmGetCode();
            if (labeldata != null && labeldata.DispatchTaskId != null)
            {
                var resourName = "";
                if (dicresourName.ContainsKey(labeldata.DispatchTaskId.Value))
                {
                    resourName = dicresourName[labeldata.DispatchTaskId.Value];
                }
                else
                {
                    resourName = RT.Service.Resolve<IDispatchs>().GetResourceNameByTaskId(labeldata.DispatchTaskId.Value);
                    dicresourName.Add(labeldata.DispatchTaskId.Value, resourName);
                }
                //var resourName = RT.Service.Resolve<IDispatchs>().GetResourceNameByTaskId(labeldata.DispatchTaskId.Value);

                // 确保字符串长度至少为2
                if (resourName.Length >= 2)
                {
                    // 从长度减2的位置开始，截取2个字符
                    P_Bn += resourName.Substring(resourName.Length - 2, 2);
                }
            }
            decimal P_Zcode = 0;
            if (labeldata != null && labeldata.DispatchTaskId != null)
            {
                if (dicP_Zcode.ContainsKey(labeldata.DispatchTaskId.Value))
                {
                    P_Zcode = dicP_Zcode[labeldata.DispatchTaskId.Value];
                }
                else
                {
                    P_Zcode = RT.Service.Resolve<IDispatchs>().GetProcessZcodeByTaskId(labeldata.DispatchTaskId.Value);
                    dicP_Zcode.Add(labeldata.DispatchTaskId.Value, P_Zcode);
                }
                //P_Zcode = RT.Service.Resolve<IDispatchs>().GetProcessZcodeByTaskId(labeldata.DispatchTaskId.Value);

            }

            var P_ParentItemCode = "";
            var P_ParentWeight = "";
            var P_ParentWeightUnit = "";
            var P_ParentShortDesc = "";
            if (P_ProductCode != "" && !P_ProductCode.IsNullOrEmpty())
            {
                ParentItemData parentItemData = null;
                if (dicparentItemData.ContainsKey(P_ProductCode))
                {
                    parentItemData = dicparentItemData[P_ProductCode];
                }
                else
                {
                    parentItemData = RT.Service.Resolve<IItem>().GetParentItemData(P_ProductCode);
                    dicparentItemData.Add(P_ProductCode, parentItemData);
                }
                //var parentItemData = RT.Service.Resolve<IItem>().GetParentItemData(P_ProductCode);

                if (parentItemData != null)
                {
                    P_ParentItemCode = parentItemData.ItemCode;

                    if (parentItemData.Weight != null)
                        P_ParentWeight = parentItemData.Weight.Value.ToString();
                    if (!parentItemData.WeightUnit.IsNullOrEmpty())
                        P_ParentWeightUnit = parentItemData.WeightUnit;
                    if (!parentItemData.ShortDescription.IsNullOrEmpty())
                        P_ParentShortDesc = parentItemData.ShortDescription;
                }
            }

            var P_BatchNo14to22 = "";
            //截取BatchNo的14至22位内容
            if (labeldata != null && !labeldata.BatchNo.IsNullOrEmpty() && labeldata.BatchNo.Length >= 14)
            {
                P_BatchNo14to22 = GetBatchNo14to22(labeldata.BatchNo);
            }

            var P_ItemCustomer = "";
            var P_ItemAttribute1 = "";
            //获取任意一个物料客户料码数据
            ItemCusotmerRelationData itemCusotmer = null;
            if (labeldata.WorkOrder != null)
            {
                if (dicitemCusotmer.ContainsKey(labeldata.WorkOrder.ProductId))
                {
                    itemCusotmer = dicitemCusotmer[labeldata.WorkOrder.ProductId];
                }
                else
                {
                    itemCusotmer = RT.Service.Resolve<IItem>().GetItemCusotmerRelationData(labeldata.WorkOrder.ProductId);
                    dicitemCusotmer.Add(labeldata.WorkOrder.ProductId, itemCusotmer);
                }
            }
            //ItemCusotmerRelationData itemCusotmer = RT.Service.Resolve<IItem>().GetItemCusotmerRelationData(labeldata.WorkOrder.ProductId);

            if (itemCusotmer != null)
            {
                P_ItemCustomer = itemCusotmer.Attribute2;
                P_ItemAttribute1 = itemCusotmer.Attribute1;
            }

            int index = 1;
            if (labeldata.WorkOrder != null)
            {
                //按批次生成中批次标签的创建时间和批次号生成，创建时间最早的为1,以此类推，当前条码在什么位置，序号就是多少
                var sortInfo = new List<OrderInfo>();
                sortInfo.Add(new OrderInfo() { Property = "CreateDate", SortIndex = 0, SortOrder = ListSortDirection.Ascending });
                sortInfo.Add(new OrderInfo() { Property = "BatchNo", SortIndex = 0, SortOrder = ListSortDirection.Ascending });
                var wips = RT.Service.Resolve<WipBatchController>().GetWipBatchsByWorkOrder(labeldata.WorkOrderId, sortInfo: sortInfo, pagingInfo: null).ToList();
                foreach (var wip in wips)
                {
                    if (wip.Id == labeldata.Id)
                        break;
                    index += 1;
                }
            }
            var P_SerialNumber = index.ToString();

            //补打时候的工序用原来的工序编码
            var P_PrintProcess = labeldata.ProcessCode;
            if (!labeldata.PrintProcessCode.IsNullOrEmpty())
                P_PrintProcess = labeldata.PrintProcessCode;


            content += P_PlanQty + Separator +
                       P_ShortDescription + Separator +
                       P_ProductCode + Separator +
                       P_ProductName + Separator +
                       P_WorkOrderNo + Separator +
                       P_Layout + Separator +
                       P_Layout_Code + Separator +
                       P_Type + Separator +
                       P_WorkOrderBatchNo + Separator +
                       P_Unit + Separator +
                       P_Zmc + Separator +
                       P_Normt + Separator +
                       P_Zkhxhgy + Separator +
                       P_Zjdlx + Separator +
                       P_Zmodel + Separator +
                       P_Zgg + Separator +
                       P_AxisNumber + Separator +
                       P_Bn + Separator +
                       P_Zcode + Separator +
                       P_ParentItemCode + Separator +
                       P_ParentWeight + Separator +
                       P_ParentWeightUnit + Separator +
                       P_ParentShortDesc + Separator +
                       P_Weight + Separator +
                       P_WeightUnit + Separator +
                       P_BatchNo14to22 + Separator +
                       P_ItemCustomer + Separator +
                       P_ItemAttribute1 + Separator +
                       P_SerialNumber + Separator +
                       P_PrintProcess + Separator;

            return content;
        }

        /// <summary>
        /// 截取BatchNo的14至22位内容
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public string GetBatchNo14to22(string batchNo)
        {
            // 检查字符串是否为null或空
            if (string.IsNullOrEmpty(batchNo))
            {
                return "";
            }

            // 如果长度小于14位，直接返回空字符串
            if (batchNo.Length < 14)
            {
                return "";
            }
            // 计算起始索引和要截取的长度
            int startIndex = 13; // 因为C#字符串索引从0开始，第14位对应索引13
            int length = batchNo.Length - startIndex;

            // 如果总长度超过22位，最多只取到22位（即9个字符）
            if (length > 9)
            {
                length = 9;
            }
            // 截取并返回相应的子字符串
            return batchNo.Substring(startIndex, length);
        }
    }
}