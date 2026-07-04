using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SIE.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收 API控制器
    /// </summary>
    public partial class MaterialReceiveController
    {
        #region 物料接收API-按单接收

        /// <summary>
        /// 获取待接收的单据数据
        /// </summary>
        /// <returns></returns>
        [ApiService("获取待接收的单据数据")]
        [return: ApiReturn("返回待接收的单据数据集合：List<MaterialReceiveViewModel>")]
        public virtual List<MaterialReceive> GetReceiveOrders([ApiParameter("查询参数")] QueryShipDataParam param)
        {
            //通过接口查询待接收单据
            param.InvOrgId = RT.InvOrg.Value;
            //var list = RT.Service.Resolve<ILesShippingOrder>().GetShipData(param);
            //var datas = ConvertShipData(list).ToList();

            PagingInfo pagingInfo = null;
            if (param.PageNumber > 0 && param.PageSize > 0)
                pagingInfo = new PagingInfo(param.PageNumber, param.PageSize);
            var query = DB.Query<MaterialReceive>().Where(p => p.State == ReceiveState.TobeReceived || p.State == ReceiveState.PartReceived);
            if (param.Keyword.IsNotEmpty())
            {
                query.Where(p => p.SoNo == param.Keyword || p.SourceNo == param.Keyword || p.WorkOrder.No == param.Keyword);
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty()).OrderByDescending(p => p.UpdateDate).ToList();
        }
        /// <summary>
        /// 获取单据明细数据
        /// </summary>
        /// <param name="soNo"></param>
        /// <returns></returns>
        [ApiService("获取单据明细数据")]
        [return: ApiReturn("返回单据明细数据集合：List<MaterialReceiveDetail>")]
        public virtual List<MaterialReceiveDetail> GetReceiveOrderDetails([ApiParameter("发运单号")] string soNo)
        {
            var list = DB.Query<MaterialReceiveDetail>().Where(p => p.MaterialReceive.SoNo == soNo && p.State == ReceiveState.TobeReceived).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //自动带出非序列号标签
            var labels = DB.Query<MaterialReceiveLabel>().Where(p => p.MaterialReceive.SoNo == soNo && p.State == ReceiveState.TobeReceived && p.IsSerialNumber == false).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            labels.ForEach(p => p.ReceivedQty = p.IssuedQty);
            list.ForEach(dtl =>
            {
                dtl.LabelList.AddRange(labels.Where(p => p.MaterialReceiveDetailId == dtl.Id));
            });


            return list.ToList();
        }

        /// <summary>
        /// 部分接收扫码
        /// </summary>
        /// <param name="soNo"></param>
        /// <param name="soLineNo"></param>
        /// <param name="labelNo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("部分接收扫码")]
        [return: ApiReturn("返回扫码标签数据：MaterialReceiveLabel")]
        public virtual MaterialReceiveLabel GetReceiveOrderLabel([ApiParameter("发运单号")] string soNo, [ApiParameter("发运单行号")] string soLineNo, [ApiParameter("标签号")] string labelNo)
        {
            if (soNo.IsNullOrEmpty()|| soLineNo.IsNullOrEmpty())
                throw new ValidationException("发运单号或行号不能为空".L10N());
            if (labelNo.IsNullOrEmpty())
                throw new ValidationException("标签号不能为空".L10N());
            var label = Query<MaterialReceiveLabel>().Where(p => p.LabelNo == labelNo && p.SoLineNo == soLineNo && p.MaterialReceive.SoNo == soNo && p.State == ReceiveState.TobeReceived).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (label == null)
                throw new ValidationException("发运单[{0}]行[{1}]不存在该标签[{2}]".L10nFormat(soNo, soLineNo, labelNo));
            label.ReceivedQty = label.IssuedQty;
            return label;
        }

        /// <summary>
        /// 提交按单接收数据
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="isRejected"></param>
        [ApiService("提交按单接收数据")]
        public virtual void SubmitReceiveOrders([ApiParameter("需要接收的单据数据")] List<MaterialReceiveDetail> datas, bool isRejected)
        {
            if (isRejected) {
                datas.ForEach(p => p.LabelList.Clear());
            }
            MaterialReceive(datas, isRejected);
        }
        #endregion

        #region 物料接收API-扫码接收

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labelNo"></param>
        /// <returns></returns>
        [ApiService("获取待接收的扫码数据")]
        [return: ApiReturn("返回待接收的扫码数据：MaterialReceiveDetail")]
        public virtual List<MaterialReceiveDetail> GetReceiveScanData([ApiParameter("扫码参数")] string labelNo)
        {
            if (labelNo.IsNullOrEmpty())
                throw new ValidationException("标签号不能为空".L10N());
            var ret = new List<MaterialReceiveDetail>();
            var label = DB.Query<MaterialReceiveLabel>().Where(p => p.LabelNo == labelNo && p.State == ReceiveState.TobeReceived).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (label != null)
            {
                //待接收标签,取对就的接收明细数据
                label.ReceivedQty = label.IssuedQty;
                var detail = DB.Query<MaterialReceiveDetail>().Where(p => label.MaterialReceiveDetailId == p.Id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                detail.LabelList.Add(label);
                detail.ReceivedQty = detail.LabelList.Sum(p => p.ReceivedQty);
                ret.Add(detail);
            }
            else
            {
                //非待接收标签, 通过标签查询接口出对应物料,再匹配相关待接收明细数据
                ret = GetReceiveDetailByOther(labelNo);
            }

            return ret;
        }

        /// <summary>
        /// 通过接口匹配数据
        /// </summary>
        /// <param name="labelNo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        List<MaterialReceiveDetail> GetReceiveDetailByOther(string labelNo)
        {
            var data = RT.Service.Resolve<ILesShippingOrder>().ScanLabel(RT.InvOrg.Value, labelNo);
            if (data.Count == 0)
                throw new ValidationException("标签[{0}]无法识别,请确认".L10nFormat(labelNo));
            var scanLabel = data[0];
            var itemCode = data[0].ItemCode;
            var lotCode = data[0].LotCode;
            var list = DB.Query<MaterialReceiveDetail>().Where(p => p.Item.Code == itemCode && p.LotCode == lotCode && p.State == ReceiveState.TobeReceived).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (list.Count == 0)
                throw new ValidationException("标签[{0}]未匹配到待接收的物料明细,请确认".L10nFormat(labelNo));

            //按交货时间优先匹配
            var ret = list.OrderBy(p => p.DeliveryDate).ToList();
            var detail = ret[0];

            var label = new MaterialReceiveLabel()
            {
                MaterialReceiveId = detail.MaterialReceiveId,
                MaterialReceiveDetailId = detail.Id,
                Item = detail.Item,
                ItemId = detail.ItemId,
                ItemCode = detail.Item?.Code,
                ItemName = detail.Item?.Name,
                ItemUnitName = detail.ItemUnitName,
                ItemExtPropName = detail.ItemExtPropName,
                ItemExtProp = detail.ItemExtProp,
                ProjectNo = detail.ProjectNo,
                IsSerialNumber = false,
                LabelNo = labelNo,
                IssuedQty = detail.IssuedQty,
                ReceivedQty = scanLabel.Qty,
                LotCode = scanLabel.LotCode,
                SoLineNo = detail.SoLineNo,
                State = ReceiveState.TobeReceived
            };
            label.LoadProperty(MaterialReceiveLabel.SourceNoProperty, detail.SourceNo);
            label.LoadProperty(MaterialReceiveLabel.SoNoProperty, detail.SoNo);
            detail.ReceivedQty = label.ReceivedQty;
            detail.LabelList.Add(label);
            return ret;
        }

        /// <summary>
        /// 提交行明细接收数据
        /// </summary>
        /// <param name="datas"></param>
        [ApiService("提交行明细接收数据")]
        public virtual void SubmitReceiveScanDatas([ApiParameter("需要接收的数据")] List<MaterialReceiveDetail> datas)
        {
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                //保存新增的扫码数据
                foreach (var data in datas)
                {
                    data.LabelList.Where(p => p.Id == 0).ForEach(p =>
                    {
                        RF.Save(p);
                    });
                }
                //提交数据
                SubmitReceiveOrders(datas, false);
                trans.Complete();
            }
        }
        #endregion
    }
}
