using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using SIE.LES.Distributions.Datas;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送管理API
    /// </summary>
    public partial class DistributionController
    {
        #region 配送公共接口
        /// <summary>
        /// 配送管理:扫描识别
        /// </summary>
        /// <param name="scanLabel"></param>
        /// <param name="type">1-扫描配送 2-配送送达</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("配送管理:扫描识别")]
        [return: ApiReturn("返回扫描数据")]
        public virtual ScanData GetScanData([ApiParameter("发运单Id")] string scanLabel, [ApiParameter("类型")] int type)
        {
            /// <summary>
            /// 扫描类型 1-容器编码 2-配送单号 3-标签 4-发运单 5-物料编码 6-产线
            /// </summary>
            int orderType = 1;
            if (type == 2)
            {
                orderType = 2;
                //配送送达如果
                if (RT.Service.Resolve<DistributionController>().CheckIsNoDistribution() == Configs.IsNoDistributionType.NoScan)
                {
                    orderType = 3;
                }
            }

            ScanData rst = new ScanData();
            rst.DistributionNoList = new List<DistributionNoList>();
            var lpnDistributions = RT.Service.Resolve<DistributionController>().GetDistributionsByNos("", scanLabel, "", orderType);
            if (lpnDistributions.Count > 0)
            {

                rst.type = 1;
                rst.Code = scanLabel;
                rst.Lpn = scanLabel;
                return rst;
            }
            var noDistributions = RT.Service.Resolve<DistributionController>().GetDistributionsByNos(scanLabel, "", "", orderType);
            if (noDistributions.Count > 0)
            {
                rst.type = 2;
                rst.Code = scanLabel;
                rst.BillNo = scanLabel;
                return rst;
            }
            var labelDistributions = RT.Service.Resolve<DistributionController>().GetDistributionsByLabel(scanLabel, orderType);
            if (labelDistributions.Count > 0)
            {
                var DistributionsNos = labelDistributions.Select(p => p.No).Distinct().ToList();
                rst.type = 3;
                rst.Code = scanLabel;
                DistributionsNos.ForEach(p =>
                {
                    rst.DistributionNoList.Add(new DistributionNoList()
                    {
                        BillNo = p
                    });
                });
                return rst;
            }
            var OrderDistributions = RT.Service.Resolve<DistributionController>().GetDistributionsByNos("", "", scanLabel, orderType);
            if (OrderDistributions.Count > 0)
            {
                var DistributionsNos = OrderDistributions.Select(p => p.No).Distinct().ToList();
                rst.type = 4;
                rst.Code = scanLabel;
                DistributionsNos.ForEach(p =>
                {
                    rst.DistributionNoList.Add(new DistributionNoList()
                    {
                        BillNo = p
                    });
                });
                return rst;
            }
            var ItemDistributions = RT.Service.Resolve<DistributionController>().GetDistributionsByItemCode(scanLabel, orderType);
            if (ItemDistributions.Count > 0)
            {
                var DistributionsNos = ItemDistributions.Select(p => p.No).Distinct().ToList();
                rst.type = 5;
                rst.Code = scanLabel;
                DistributionsNos.ForEach(p =>
                {
                    rst.DistributionNoList.Add(new DistributionNoList()
                    {
                        BillNo = p
                    });
                });
                return rst;
            }
            if (type == 2)
            {
                var productLine = RT.Service.Resolve<WipResourceController>().GetScheResourceByResCode(scanLabel);
                if (productLine != null)
                {
                    rst.type = 6;
                    rst.Code = productLine.Code;
                }
                return rst;
            }
            throw new ValidationException("扫描的内容有误或无关联的配送单!".L10N());
        }
        #endregion

        #region 扫描配送
        /// <summary>
        /// 根据配送单/LPN获取
        /// </summary>
        /// <param name="keyWord">配送单或者容器编码</param>
        /// <param name="type">1-扫描配送 2-配送送达</param>
        /// <returns></returns>
        [ApiService("扫描配送：获取配送单信息和配送明细信息")]
        [return: ApiReturn("配送单信息和配送明细信息：DistributionScanData")]
        public virtual DistributionScanData GetDistributionData([ApiParameter("容器/配送单号")] string keyWord, [ApiParameter("类型")] int type)
        {
            var rst = new DistributionScanData();
            int orderType = 1;
            if (type == 2)
            {
                orderType = 2;
                //配送送达如果
                if (RT.Service.Resolve<DistributionController>().CheckIsNoDistribution() == Configs.IsNoDistributionType.NoScan)
                {
                    orderType = 3;
                }
            }
            rst.DistributionList = new List<DistributionDetailData>();
            var distributions = RT.Service.Resolve<DistributionController>().GetDistributionsByNosOrLpn(keyWord, orderType, new EagerLoadOptions().LoadWithViewProperty());
            if (distributions.Count == 0)
            {
                throw new ValidationException("扫描的内容有误或无关联的配送单!".L10N());
            }
            foreach (var item in distributions)
            {
                var distriItem = new DistributionDetailData();
                distriItem.BillID = item.Id;
                distriItem.Lpn = item.Lpn;
                distriItem.DistributionDetailList = new List<DistributionDetailList>();
                distriItem.ProductCode = item.ProductLineCode;
                distriItem.ProductName = item.ProductLineName;
                distriItem.BillNo = item.No;
                item.DistributionDetailList.ForEach(p =>
                {
                    var detail = new DistributionDetailList()
                    {
                        ItemName = p.ItemName,
                        ItemCode = p.ItemCode,
                        ItemExtProp = p.ItemExtProp,
                        ItemExtPropName = p.ItemExtPropName,
                        Unit = p.UnitName,
                        Qty = p.Qty
                    };
                    distriItem.DistributionDetailList.Add(detail);
                });
                rst.DistributionList.Add(distriItem);
            }
            return rst;
        }

        /// <summary>
        /// 配送单提交
        /// </summary>
        /// <param name="submitData">提交的数据</param>
        /// <returns></returns>
        [ApiService("扫描配送：提交")]
        [return: ApiReturn("扫描配送：提交")]
        public virtual void SubmitDistributionList(List<DistributionNoList> submitData)
        {
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                var ids = submitData.Select(p => p.BillID).Distinct().ToList();
                if (ids.Count == 0)
                {
                    throw new ValidationException("前端传入的配送ID有误!".L10N());
                }
                var distributionList = RT.Service.Resolve<DistributionController>().GetDistributionsByIds(ids);
                if (distributionList.Any(p => p.OrderState != OrderState.WaitDelivery))
                {
                    throw new ValidationException("配送单状态已发生改变，请检查数据".L10N());
                }
                if (RT.Service.Resolve<DistributionController>().CheckIsNoDistribution() == Configs.IsNoDistributionType.NoRecive)
                {
                    //是否跳过扫描配送-跳过扫描配送直接变为已接受-更新接受人接受时间
                    distributionList.ForEach(p =>
                    {
                        p.DeliverymanId = RT.IdentityId;
                        p.DeliveryDate = DateTime.Now;
                        p.ReceiverId = RT.IdentityId;
                        p.ReceiveDate = DateTime.Now;
                        p.OrderState = OrderState.Receipt;
                    });
                    var dids = distributionList.Select(p => p.Id).Distinct().ToList();
                    //更新备料单信息
                    UpdateStockOrder(dids);
                }
                else
                {
                    //更新发起配送时间
                    distributionList.ForEach(p =>
                    {
                        p.OrderState = OrderState.Delivery;
                        p.DeliverymanId = RT.IdentityId;
                        p.DeliveryDate = DateTime.Now;
                    });
                }
                RF.Save(distributionList);
                tran.Complete();
            }
        }
        #endregion

        #region 配送送达
        /// <summary>
        /// 配送送达-提交
        /// </summary>
        /// <param name="submitData">提交的数据</param>
        /// <returns></returns>
        [ApiService("配送送达：配送送达-提交")]
        [return: ApiReturn("配送送达-提交")]
        public virtual void SubmitDistributionListByArrive(List<DistributionNoList> submitData)
        {
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                var ids = submitData.Select(p => p.BillID).Distinct().ToList();
                if (ids.Count == 0 || ids.Count != submitData.Count)
                {
                    throw new ValidationException("前端传入的配送ID有误!".L10N());
                }
                var distributionList = RT.Service.Resolve<DistributionController>().GetDistributionsByIds(ids);
                if (RT.Service.Resolve<DistributionController>().CheckIsNoDistribution() == Configs.IsNoDistributionType.NoScan)
                {
                    if (distributionList.Any(p => p.OrderState != OrderState.WaitDelivery && p.OrderState != OrderState.Delivery))
                    {
                        throw new ValidationException("配送单状态已发生改变，请检查数据".L10N());
                    }
                }
                else
                {
                    if (distributionList.Any(p => p.OrderState != OrderState.Delivery))
                    {
                        throw new ValidationException("配送单状态已发生改变，请检查数据".L10N());
                    }
                }
                distributionList.ForEach(p =>
                {
                    p.OrderState = OrderState.Receipt;
                    p.ReceiverId = RT.IdentityId;
                    p.ReceiveDate = DateTime.Now;
                });
                RF.Save(distributionList);
                var dids = distributionList.Select(p => p.Id).Distinct().ToList();
                //更新备料单信息
                UpdateStockOrder(dids);
                tran.Complete();
            }
        }
        #endregion

        /// <summary>
        /// 配送完成更新备料单信息
        /// </summary>
        /// <param name="DistributionIds"></param>
        public virtual void UpdateStockOrder(List<double> DistributionIds)
        {
            var distributionController = RT.Service.Resolve<DistributionController>();
            var labelData = new List<SoLabelData>();
            var DistributionLabels = distributionController.GetDistributionLabelsByOrderIds(DistributionIds, new EagerLoadOptions().LoadWithViewProperty());
            if (DistributionLabels.Count == 0)
            {
                throw new ValidationException("配送单扫描记录为空".L10N());
            }
            //var DistributionDtls = new EntityList<DistributionDetail>();
            //DistributionLabels.GroupBy(p => new { p.DistributionId, p.AssignId }).ForEach(f => {
            //    DistributionDtls.AddRange(distributionController.GetDistributionDetailByAssignId(f.Key.DistributionId, f.Key.AssignId));
            //});
            DistributionLabels.ForEach(p =>
            {
                var labelItem = new SoLabelData()
                {
                    No = p.LabelNo,
                    HighestNo = p.HighestNo,
                    LotCode = p.LotCode,
                    IsSerialNumber = p.IsSerialNumber,
                    ItemId = p.ItemId,
                    SoNo = p.OrderNo,
                    Qty = p.Qty,
                    SoLineNo = p.OrderLineNo,
                    OrderNo = p.StockOrderNo,
                    OrderLineNo = p.StockOrderLineNo,
                    WarehouseId = p.WarehouseId,
                    DistributionNo = p.DistributionNo
                };
                labelData.Add(labelItem);
            });
            RT.Service.Resolve<ISoUpdateStock>().UpdateStockOrderBySo(labelData);
        }
    }
}
