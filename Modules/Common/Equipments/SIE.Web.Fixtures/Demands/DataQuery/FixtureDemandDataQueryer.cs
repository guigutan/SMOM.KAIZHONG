using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureDemands.ViewModels;
using SIE.Fixtures.Models;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Demands.DataQuery
{
    /// <summary>
    /// 工治具治具需求清单查询器
    /// </summary>
    public class FixtureDemandDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取工治具治具需求清单信息
        /// </summary>
        /// <returns>工治具治具需求清单信息</returns>
        public AddDemandInfo GetDemandInfo()
        {
            AddDemandInfo demandInfo = new AddDemandInfo();
            demandInfo.ErrMsg = string.Empty;
            try
            {
                demandInfo = RT.Service.Resolve<ElecFixtureController>().CreateDemandInfo();
            }
            catch (Exception ex)
            {
                demandInfo.ErrMsg = ex.Message;
            }

            return demandInfo;
        }

        /// <summary>
        /// 获取工治具治具上架信息
        /// </summary>
        /// <param name="no">需求单号</param>
        /// <returns>工治具治具上架信息</returns>
        public UnloadDemandInfo GetUnloadDemandInfo(string no)
        {
            return RT.Service.Resolve<ElecFixtureController>().GetUnloadDemandInfo(no);
        }

        /// <summary>
        /// 获取库存情况ViewModel列表
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="demandDetail">工治具治具需求明细</param>
        /// <param name="fixUnloadVMs">出库明细ViewModel列表</param>
        /// <returns>库存情况ViewModel列表</returns>
        public List<UnloadStockViewModel> GetUnloadStockVMs(double? warehouseId, FixtureDemandDetail demandDetail, List<FixtureUnloadViewModel> fixUnloadVMs)
        {
            return RT.Service.Resolve<ElecFixtureController>().GetUnloadStockVMs(warehouseId, demandDetail, fixUnloadVMs);
        }

        /// <summary>
        /// 验证出库数量是否合法
        /// </summary>
        /// <param name="editUnloadInfo">编辑后的出库信息</param>
        /// <returns>出库信息</returns>
        public string ValidateUnloadQty(UnloadInfo editUnloadInfo)
        {
            return RT.Service.Resolve<ElecFixtureController>().ValidateUnloadQty(editUnloadInfo);
        }

        /// <summary>
        /// 获取出库信息
        /// </summary>
        /// <param name="editUnloadInfo">编辑后的出库信息</param>
        /// <returns>出库信息</returns>
        public UnloadInfo GetUnloadInfo(UnloadInfo editUnloadInfo)
        {
            return RT.Service.Resolve<ElecFixtureController>().UpdateUnloadStockInfo(editUnloadInfo);
        }

        /// <summary>
        /// 根据工单的物料和工艺面获取工治具治具编码列表
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="fixtureType">工治具类型</param>
        /// <param name="modelId">工治具型号Id</param>
        /// <param name="processStegmentId">工段</param>
        /// <param name="pageIndex">分页Index</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工治具治具编码列表</returns>
        public EntityList<FixtureEncode> GetFixtureEncodeList(double? woId, double? fixtureType, double? modelId, double? processStegmentId,int pageIndex, int pageSize, string keyword)
        {
            var pagingInfo = new PagingInfo(pageIndex, pageSize, true);
            var fixEncodeList = new EntityList<FixtureEncode>();
            if (woId != null && woId > 0)
                fixEncodeList = RT.Service.Resolve<ElecFixtureController>().GetFixtureEncodeList(woId.Value,  fixtureType, modelId, processStegmentId, keyword, pagingInfo);
            return fixEncodeList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="procesegmentId"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public EntityList<FixtureDemandDetail> GetFixtureDemandDetailList(double? woId, double procesegmentId, double pid)
        {
            return RT.Service.Resolve<ElecFixtureController>().GetFixtureDemandDetailList(woId, procesegmentId, pid);
        }


        /// <summary>
        /// 获取工艺面信息
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="encodeId">工治具治具编码Id</param>
        /// <returns>工艺面信息</returns>
        public DeckInfo GetDeckInfo(double woId, double encodeId)
        {
            var deckInfo = new DeckInfo();
            deckInfo.FixtureTypeId = null;
            deckInfo.FixtureModelId = null;
            deckInfo.ModelCode = string.Empty;
            deckInfo.ModelName = string.Empty;
            deckInfo.ErrMsg = string.Empty;
            try
            {
                if (encodeId > 0)
                    deckInfo = RT.Service.Resolve<ElecFixtureController>().GetDeck(woId, encodeId);
            }
            catch (Exception ex)
            {
                deckInfo.ErrMsg = ex.Message;
            }

            return deckInfo;
        }

        /// <summary>
        /// 获取审批按钮是否可用
        /// </summary>
        /// <returns></returns>
        public ApprovalWay? GetEnableApproval()
        {
            var fixtureDemandsConfigValue = RT.Service.Resolve<ElecFixtureController>().GetFixtureDemandsConfigValue();
            if (fixtureDemandsConfigValue != null && fixtureDemandsConfigValue.SwitchApproval)
                return fixtureDemandsConfigValue.ApprovalWay;
            else return null;
        }

        /// <summary>
        /// 获取绑定工单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BindWoInfo GetBindWoInfo(double id)
        {
            return RT.Service.Resolve<ElecFixtureController>().GetBindWoInfo(id);
        }
    }


}
