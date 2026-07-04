using DocumentFormat.OpenXml.ExtendedProperties;
using Org.BouncyCastle.Asn1.Ocsp;
using SIE.Api;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Smom.Download.KaiZhong;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.MES.Outsourcing.Datas;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.Enterprises;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Api.WebApi.KaiZhong
{
    /// <summary>
    /// 基础数据下载控制器
    /// </summary>
    public class KZWebApiController : KzLoginController
    {
        #region 跨库存组织物料标签同步

        /// <summary>
        /// 跨库存组织物料标签同步发起
        /// </summary>
        /// <param name="itemCodes"></param>
        /// <param name="SourceFactory"></param>
        /// <param name="ToFactory"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("跨库存组织物料标签同步发起")]
        public virtual OuterSystemRetVO SyncItemLabel(List<string> itemCodes,string SourceFactory,string ToFactory)
        {
            OuterSystemRetVO result = new OuterSystemRetVO();
            result.success = true;
            try
            {
                if (itemCodes == null || itemCodes.Count < 1)
                    throw new ValidationException("同步物料不能为空".L10N());
                if (SourceFactory.IsNullOrEmpty())
                    throw new ValidationException("来源工厂不能为空".L10N());
                if (ToFactory.IsNullOrEmpty())
                    throw new ValidationException("目标工厂不能为空".L10N());

                Login(SourceFactory);
                var itemLabels = itemCodes.SplitContains(codes =>
                {
                    return Query<ItemLabel>().Where(p => codes.Contains(p.Item.Code) && (p.ToFactory == null || !p.ToFactory.Contains("%" + ToFactory + "|%"))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });

                if (itemLabels.Count < 1)
                    return result;

                var smomControlSetting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(ToFactory);
                try
                {
                    if (smomControlSetting == null || smomControlSetting.FactoryUrl.IsNullOrEmpty())
                        throw new ValidationException("未配置总控Url地址!".L10N());

                    var smomParam = new List<SmomParam>(){
                    new SmomParam { Value =itemLabels },
                                    new SmomParam { Value =ToFactory }
                                 }.ToArray();
                    result = SmomControlHepler.SmomPost<OuterSystemRetVO>("KZWebApiController", "GetSyncItemLabelInfo", smomControlSetting.FactoryUrl, smomParam);
                }
                catch (Exception ex)
                {
                    result.success = false;
                    throw new ValidationException(ex.GetBaseException().Message);
                }
                finally
                {
                    if (result.success == true)
                    {
                        //找出失败的id
                        var failIds = new List<double>();
                        if (result.errorList != null && result.errorList.Count > 0)
                            failIds = result.errorList.Select(p => (double)p).ToList();
                        foreach (var itemLabel in itemLabels)
                        {
                            //但存在失败的id的时候，就直接跳过
                            if (failIds.Count > 0 && failIds.Contains(itemLabel.Id))
                                continue;

                            if (itemLabel.ToFactory.IsNullOrEmpty())
                                itemLabel.ToFactory = ToFactory + "|";
                            else
                                itemLabel.ToFactory += ToFactory + "|";
                        }
                        RF.Save(itemLabels);
                    }
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMsg = ex.GetBaseException()?.Message;
            }
            return result;
        }

        /// <summary>
        /// 跨库存组织物料标签同步接收
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("跨库存组织物料标签同步接收")]
        public virtual OuterSystemRetVO GetSyncItemLabelInfo(List<ItemLabel> itemLabels,string invOrg)
        {
            Login(invOrg);

            OuterSystemRetVO result = new OuterSystemRetVO();
            result.errorMsg = "";
            result.success = true;

            try
            {
                var itemCodes = itemLabels.Select(p => p.ItemCode).Distinct().ToList();
                var items = RT.Service.Resolve<ItemController>().GetItemDataList(itemCodes, new EagerLoadOptions().LoadWithViewProperty());

                //var WorkOrderNos = itemLabels.Select(p => p.WorkOrderNo).Distinct().ToList();
                //var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(WorkOrderNos);

                var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(invOrg);
                foreach (var itemLabel in itemLabels)
                {
                    try
                    {
                        var item = items.FirstOrDefault(p => p.Code == itemLabel.ItemCode);
                        if (item == null)
                            throw new ValidationException("物料{0}在工厂{1}中不存在".L10nFormat(itemLabel.ItemCode, invOrg));
                        //var workOrder = workOrders.FirstOrDefault(p => p.No == itemLabel.WorkOrderNo);
                        //if (workOrder == null)
                        //    throw new ValidationException("工单{0}在工厂{1}中不存在".L10nFormat(itemLabel.WorkOrderNo, invOrg));
                        if (factorys == null)
                            throw new ValidationException("在工厂{0}的企业模型中，找不到编码为{1}的工厂信息".L10nFormat(invOrg, invOrg));

                        var label = new ItemLabel
                        {
                            Label = itemLabel.Label,//data.EXIDV,
                            Qty = itemLabel.Qty,
                            InitialQty = itemLabel.InitialQty,
                            ItemId = item.Id,
                            Item = item,
                            SourceType = itemLabel.SourceType
                        };
                        label.Exidv = itemLabel.Exidv;
                        label.Exidv2 = itemLabel.Exidv2;
                        label.Lgort = itemLabel.Lgort;
                        label.Lot = itemLabel.Lot;
                        label.FactoryId = factorys?.Id;
                        label.Factory = factorys;
                        label.IsSerialNumber = itemLabel.IsSerialNumber;
                        label.ItemLabelState = ItemLabelState.Receive;

                        //label.Clone(itemLabel, new CloneOptions(CloneActions.NormalProperties));
                        //label.GenerateId();
                        //label.ItemId = item.Id;
                        //label.WorkOrderId = workOrder.Id;
                        //label.FactoryId = factorys?.Id;
                        RF.Save(label);

                        result.sucessList.Add(itemLabel.Id);
                    }
                    catch (Exception ex)
                    {
                        var errMsg = ex.GetBaseException()?.Message;
                        //如果是相同标签号存在就认为是成功，不在同步
                        if (errMsg.Contains("标签号") && errMsg.Contains("已经存在"))
                            result.sucessList.Add(itemLabel.Id);
                        else
                            result.errorList.Add(itemLabel.Id);

                        result.errorMsg += errMsg + ";";
                    }
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMsg += ex.GetBaseException()?.Message + ";";
            }
            return result;
        }

        #endregion

    }
}
