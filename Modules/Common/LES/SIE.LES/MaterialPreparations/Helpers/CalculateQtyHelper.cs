using SIE.LES.MaterialMoves;
using SIE.LES.MaterialPreparations.ApiModels;
using SIE.LES.MaterialReceives;
using SIE.LES.MaterialReturnApplys;
using System;
using System.Collections.Generic;

namespace SIE.LES.MaterialPreparations.Helpers
{
    /// <summary>
    /// 计算可备料量
    /// </summary>
    public class CalculateQtyHelper
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        public CalculateQtyHelper(List<double> woIds)
        {
            WoIds = woIds;
        }
        
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="woId">工单Id</param>
        public CalculateQtyHelper(double woId)
        {
            WoIds = new List<double>() { woId };
        }

        /// <summary>
        /// 构造
        /// </summary>
        public CalculateQtyHelper()
        {

        }

        /// <summary>
        /// 备料需求单控制器
        /// </summary>
        protected MaterialPreparationController MpCtl = RT.Service.Resolve<MaterialPreparationController>();

        /// <summary>
        /// 挪料控制器
        /// </summary>
        protected MaterialMoveRecordController MpMoveCtl = RT.Service.Resolve<MaterialMoveRecordController>();

        /// <summary>
        /// 退料控制器
        /// </summary>
        protected MaterialReturnApplyController MpRtCtl = RT.Service.Resolve<MaterialReturnApplyController>();

        /// <summary>
        /// 接收控制器
        /// </summary>
        protected MaterialReceiveController MrecCtl = RT.Service.Resolve<MaterialReceiveController>();

        /// <summary>
        /// 工单Ids
        /// </summary>
        protected List<double> WoIds { get; set; } = new List<double>();

        /// <summary>
        /// 工单bom(key:工单Id,value:工单bom列表)
        /// </summary>
        protected Dictionary<double, List<WoBomInfo>> BomDic { get; set; } = new Dictionary<double, List<WoBomInfo>>();

        /// <summary>
        /// 工单物料-备料明细统计计算(key {工单id,物料id,扩展属性,行号} value:{备料数sum,取消数sum})
        /// </summary>
        protected Dictionary<Tuple<double, double, string, string>, MaterialPreDetailInfo> WoItemMpDetailDic { get; set; } = new Dictionary<Tuple<double, double, string, string>, MaterialPreDetailInfo>();

        /// <summary>
        /// 工单挪料(key: 工单id,物料id,扩展属性， value: 挪出数sum)
        /// </summary>
        protected Dictionary<Tuple<double, double, string>, decimal> WoItemMoveOutDic { get; set; } = new Dictionary<Tuple<double, double, string>, decimal>();

        /// <summary>
        /// 工单挪料(key: 工单id,物料id,扩展属性， value: 挪入数sum)
        /// </summary>
        protected Dictionary<Tuple<double, double, string>, decimal> WoItemMoveInDic { get; set; } = new Dictionary<Tuple<double, double, string>, decimal>();

        /// <summary>
        /// 工单退料(key: 工单id,物料id,扩展属性， value: 退料数sum)
        /// </summary>
        protected Dictionary<Tuple<double, double, string>, decimal> WoItemReturnDic { get; set; } = new Dictionary<Tuple<double, double, string>, decimal>();

        /// <summary>
        /// 物料接收记录(key: 工单id,物料id,扩展属性， value: 接收数sum)
        /// </summary>
        protected Dictionary<Tuple<double, double, string>, decimal> WoItemReceivedDic { get; set; } = new Dictionary<Tuple<double, double, string>, decimal>();

        /// <summary>
        /// 待接收明细(key: 工单id,物料id,扩展属性， value: 待接收发料数sum)
        /// </summary>
        protected Dictionary<Tuple<double, double, string>, decimal> WoItemToReceiveDic { get; set; } = new Dictionary<Tuple<double, double, string>, decimal>();

        /// <summary>
        /// 获取工单bom信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<double, List<WoBomInfo>> GetBomDic()
        {
            return BomDic;
        }

        /// <summary>
        /// 当前工单bom的分页信息
        /// </summary>
        public int SingleWoBomTotalCount { get; protected set; }

        /// <summary>
        /// 获取工单对应bom信息
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public List<WoBomInfo> GetBomList(double woId)
        {
            if (BomDic.TryGetValue(woId, out var list))
            {
                return list;
            }
            else
            {
                return new List<WoBomInfo>();
            }
        }

        /// <summary>
        /// 初始化计算
        /// </summary>
        public void InitDataBase()
        {
            BomDic = MpCtl.GetBomDic(WoIds);
            WoItemMpDetailDic = MpCtl.GetMaterialPreDetailDic(WoIds);
            WoItemMoveOutDic = MpMoveCtl.GetWoItemMoveOutQtyDic(WoIds);
            WoItemMoveInDic = MpMoveCtl.GetWoItemMoveInQtyDic(WoIds);
            WoItemReturnDic = MpRtCtl.GetMaterialReturnQtyDic(WoIds);
        }

        /// <summary>
        /// 备料需求明细统计
        /// </summary>
        public void GetPreDetailDic()
        {
            WoItemMpDetailDic = MpCtl.GetMaterialPreDetailDic(WoIds);
            WoItemMoveOutDic = MpMoveCtl.GetWoItemMoveOutQtyDic(WoIds);
            WoItemMoveInDic = MpMoveCtl.GetWoItemMoveInQtyDic(WoIds);
            WoItemReturnDic = MpRtCtl.GetMaterialReturnQtyDic(WoIds);
            WoItemReceivedDic = MrecCtl.GetReceivedDic(WoIds);
            WoItemToReceiveDic = MrecCtl.GetToReceiveDic(WoIds);
        }

        /// <summary>
        /// 导出获取
        /// </summary>
        public void ExportGetPreDetailDic()
        {
            BomDic = MpCtl.GetBomDic(WoIds);
            WoItemMpDetailDic = MpCtl.GetMaterialPreDetailDic(WoIds);
            WoItemMoveOutDic = MpMoveCtl.GetWoItemMoveOutQtyDic(WoIds);
            WoItemMoveInDic = MpMoveCtl.GetWoItemMoveInQtyDic(WoIds);
            WoItemReturnDic = MpRtCtl.GetMaterialReturnQtyDic(WoIds);
            WoItemReceivedDic = MrecCtl.GetReceivedDic(WoIds);
            WoItemToReceiveDic = MrecCtl.GetToReceiveDic(WoIds);
        }

        /// <summary>
        /// 保存重新计算可备料量(除去编辑数据)
        /// </summary>
        /// <param name="exceptIds"></param>
        public void GetPreDetailDicForSave(List<double> exceptIds)
        {
            WoItemMpDetailDic = MpCtl.GetMaterialPreDetailDic(WoIds, exceptIds);
            WoItemMoveOutDic = MpMoveCtl.GetWoItemMoveOutQtyDic(WoIds);
            WoItemMoveInDic = MpMoveCtl.GetWoItemMoveInQtyDic(WoIds);
            WoItemReturnDic = MpRtCtl.GetMaterialReturnQtyDic(WoIds);
        }

        /// <summary>
        /// 添加获取备料明细
        /// </summary>
        public void InitSelectDataBase(double woId, string itemCode, PagingInfo pagingInfo)
        {
            List<double> woIds = new List<double> { woId };
            (BomDic, SingleWoBomTotalCount) = MpCtl.GetBomDic(woId, itemCode, pagingInfo);
            WoItemMpDetailDic = MpCtl.GetMaterialPreDetailDic(woIds);
            WoItemMoveOutDic = MpMoveCtl.GetWoItemMoveOutQtyDic(woIds);
            WoItemMoveInDic = MpMoveCtl.GetWoItemMoveInQtyDic(woIds);
            WoItemReturnDic = MpRtCtl.GetMaterialReturnQtyDic(woIds);
        }

        /// <summary>
        /// 获取已建备料数
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="bom">工单bom信息</param>
        /// <returns></returns>
        public decimal CalculateHasQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string, string>(woId, bom.ItemId, bom.ItemExtProp, bom.LineNo);
            WoItemMpDetailDic.TryGetValue(key, out var mpDetail);
            decimal hasQty = mpDetail != null ? mpDetail.Qty : 0;
            return hasQty;
        }

        /// <summary>
        /// 获取发料数
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="bom">工单bom信息</param>
        /// <returns></returns>
        public decimal CalculateShippingQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string, string>(woId, bom.ItemId, bom.ItemExtProp, bom.LineNo);
            WoItemMpDetailDic.TryGetValue(key, out var mpDetail);
            decimal shippingQty = mpDetail != null ? mpDetail.ShippingQty : 0;
            return shippingQty;
        }

        /// <summary>
        /// 获取取消数
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="bom">工单bom信息</param>
        /// <returns></returns>
        public decimal CalculateCancelQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string, string>(woId, bom.ItemId, bom.ItemExtProp, bom.LineNo);
            WoItemMpDetailDic.TryGetValue(key, out var mpDetail);
            decimal cancelQty = mpDetail != null ? mpDetail.CancelQty : 0;
            return cancelQty;
        }

        /// <summary>
        /// 获取退料数
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="bom"></param>
        /// <returns></returns>
        public decimal CalculateReturnQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string>(woId, bom.ItemId, bom.ItemExtProp);
            WoItemReturnDic.TryGetValue(key, out var returnQty);
            return returnQty;
        }

        /// <summary>
        /// 获取接收数
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="bom"></param>
        /// <returns></returns>
        public decimal CalculateReceivedQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string>(woId, bom.ItemId, bom.ItemExtProp);
            WoItemReceivedDic.TryGetValue(key, out var receivedQty);
            return receivedQty;
        }

        /// <summary>
        /// 获取待收数
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="bom"></param>
        /// <returns></returns>
        public decimal CalculateToReceiveQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string>(woId, bom.ItemId, bom.ItemExtProp);
            WoItemToReceiveDic.TryGetValue(key, out var toReceiveQty);
            return toReceiveQty;
        }

        /// <summary>
        /// 获取挪入数
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="bom"></param>
        /// <returns></returns>
        public decimal CalculateMoveInQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string>(woId, bom.ItemId, bom.ItemExtProp);
            WoItemMoveInDic.TryGetValue(key, out var moveInQty);
            return moveInQty;
        }

        /// <summary>
        /// 获取挪出数
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="bom"></param>
        /// <returns></returns>
        public decimal CalculateMoveOutQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string>(woId, bom.ItemId, bom.ItemExtProp);
            WoItemMoveOutDic.TryGetValue(key, out var moveOutQty);
            return moveOutQty;
        }

        /// <summary>
        /// 计算可备料量:需求数 - 已建备料数 + 取消数 + 挪出数 - 挪入数 + 退料数
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="bom">工单bom信息</param>
        /// <returns></returns>
        public decimal CalculateCanQty(double woId, WoBomInfo bom)
        {
            var key = new Tuple<double, double, string, string>(woId, bom.ItemId, bom.ItemExtProp, bom.LineNo);
            // 需求数
            decimal bomNeedQty = bom.BomNeedQty;
            // 获取备料需求明细已建备料数和取消数
            WoItemMpDetailDic.TryGetValue(key, out var mpDetail);
            decimal hasQty = mpDetail != null ? mpDetail.Qty : 0;
            decimal cancelQty = mpDetail != null ? mpDetail.CancelQty : 0;

            // 获取挪出数
            var keyy = new Tuple<double, double, string>(woId, bom.ItemId, bom.ItemExtProp);
            WoItemMoveOutDic.TryGetValue(keyy, out var moveOutQty);
            WoItemMoveInDic.TryGetValue(keyy, out var moveInQty);
            WoItemReturnDic.TryGetValue(keyy, out var returnQty);

            //计算可备料量:需求数 - 已建备料数 + 取消数 + 挪出数 - 挪入数 + 退料数
            return bomNeedQty - hasQty + cancelQty + moveOutQty - moveInQty + returnQty;
        }
    }
}
