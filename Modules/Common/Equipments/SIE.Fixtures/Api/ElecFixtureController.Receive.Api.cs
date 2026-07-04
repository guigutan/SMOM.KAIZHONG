using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures.ApiModels;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.TurnoverTools.TurnoverTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具领用
    /// </summary>
    public partial class ElecFixtureController : CoreFixtureController
    {
        #region 工治具治具领用接口  

        /// <summary>
        /// 获取工治具领用清单列表
        /// </summary>
        /// <param name="receiveQueryInfo">入库任务查询信息</param>
        /// <returns>分页工治具领用清单信息</returns>
        [ApiService("获取工治具领用清单列表")]
        [return: ApiReturn("分页工治具领用清单信息  ReceiveDataInfo")]
        public virtual ReceiveDataInfo GetPagingFixtureReceiveInfos([ApiParameter("入库任务查询信息")] ReceiveQueryInfo receiveQueryInfo)
        {
            if (receiveQueryInfo == null)
            {
                throw new ValidationException("前端传入参数有误！".L10N());
            }
            var pagingInfo = GetPagingInfo(receiveQueryInfo.PageNumber, receiveQueryInfo.PageSize);
            var fixtureDemands = GetReceiveFixtureDemands(pagingInfo);
            var fixtureDemandIds = fixtureDemands.Select(p => p.Id).ToList();
            var unloads = GetUnloadsByDemandIds(fixtureDemandIds, ReceiveState.None);
            var receiveDataInfo = new ReceiveDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize
            };
            foreach (var fixtureDemand in fixtureDemands)
            {
                var qty = 0;
                var typeNums = new List<string>();
                var demandUnloads = unloads.Where(p => p.FixtureDemandId == fixtureDemand.Id).ToList();
                if (!demandUnloads.Any())
                    continue;
                foreach (var unload in demandUnloads)
                {
                    qty += unload.UnloadQty;
                    qty -= unload.NgQty;
                    typeNums.Add(unload.FixtureType);
                }
                var fixtureReceiveInfo = new FixtureReceiveInfo();
                fixtureReceiveInfo.No = fixtureDemand.No;
                fixtureReceiveInfo.ResourceName = fixtureDemand.ResourceName;
                fixtureReceiveInfo.WorkOrderNo = fixtureDemand.WorkOrderNo;
                fixtureReceiveInfo.TypeNum = typeNums.Distinct().Count();
                fixtureReceiveInfo.Qty = qty;
                fixtureReceiveInfo.CreateDate = fixtureDemand.CreateDate;
                receiveDataInfo.FixtureReceiveInfos.Add(fixtureReceiveInfo);
            }
            receiveDataInfo.TotalCount = receiveDataInfo.FixtureReceiveInfos.Count;
            return receiveDataInfo;
        }

        /// <summary>
        /// 按单号或者Id编码获取领用需求信息
        /// </summary>
        /// <param name="no">ID编码或者需求单号</param>
        /// <returns>领用需求信息</returns>
        [ApiService("按单号或者Id编码获取领用需求信息")]
        [return: ApiReturn("领用需求信息  ReceiveSearchInfo")]
        public virtual ReceiveSearchInfo GetReceiveSearchInfo([ApiParameter("ID编码或者需求单号")] string no)
        {
            var info = new ReceiveSearchInfo();
            info.IsReceive = false;
            var fixDemand = GetFixtureDemand(no);
            if (fixDemand != null)
            {
                var unloadsByDemand = GetUnloadsByDemandIds(new List<double> { fixDemand.Id }, ReceiveState.None);
                if (!unloadsByDemand.Any())
                {
                    throw new ValidationException("查询的需求单号无可领用数据！".L10N());
                }
                var unloadQty = unloadsByDemand.Sum(p => p.UnloadQty) - unloadsByDemand.Sum(p => p.NgQty);
                GetFixtureReceiveInfo(info, fixDemand, unloadsByDemand.Select(p => p.FixtureType).Distinct().Count(), unloadQty);
                return info;
            }
            var account = GetFixtureAccountByCodeOrRFID(no);
            if (account == null)
            { throw new ValidationException("查询的需求单号或者ID编码不存在！".L10N()); }
            var unloads = GetUnloadsByAccountId(account.Id);
            if (!unloads.Any())
            { throw new ValidationException("查询的ID编码无可领用数据！".L10N()); }
            if (account.ManageMode == ManageMode.Number)
            {
                fixDemand = unloads.FirstOrDefault().FixtureDemand;
                if (fixDemand == null)
                { throw new ValidationException("查询的ID编码数据异常！".L10N()); }
                var unloadQty = unloads.Sum(p => p.UnloadQty) - unloads.Sum(p => p.NgQty);
                GetFixtureReceiveInfo(info, fixDemand, unloads.Select(p => p.FixtureType).Distinct().Count(), unloadQty);
                return info;
            }
            else
            {
                var unloadGroups = unloads.GroupBy(p => p.FixtureDemandId);
                foreach (var group in unloadGroups)
                {
                    var fixtureDemand = RF.GetById<FixtureDemand>(group.Key);
                    var unloadQty = group.Sum(p => p.UnloadQty) - group.Sum(p => p.NgQty);
                    GetFixtureReceiveInfo(info, fixtureDemand, group.Select(p => p.FixtureType).Distinct().Count(), unloadQty);
                }
                return info;
            }
        }

        /// <summary>
        /// 创建工治具领用清单信息
        /// </summary>
        /// <param name="info">工治具领用清单信息</param>
        /// <param name="fixDemand">需求单</param>
        /// <param name="typeNum">类型数量</param>
        /// <param name="unloadQty">出库数量</param>
        private void GetFixtureReceiveInfo(ReceiveSearchInfo info, FixtureDemand fixDemand, int typeNum, int unloadQty)
        {
            var fixtureReceiveInfo = new FixtureReceiveInfo();
            fixtureReceiveInfo.No = fixDemand.No;
            fixtureReceiveInfo.ResourceName = fixDemand.Resource.Name;
            fixtureReceiveInfo.WorkOrderNo = fixDemand.WorkOrder.No;
            fixtureReceiveInfo.TypeNum = typeNum;
            fixtureReceiveInfo.Qty = unloadQty;
            fixtureReceiveInfo.CreateDate = fixDemand.CreateDate;
            info.FixtureReceiveInfos.Add(fixtureReceiveInfo);
        }

        /// <summary>
        /// 获取【工治具治具需求清单】中【出库状态】不为【未出库】，【领用状态】不为【领用完成】的数据
        /// 并且【工治具治具需求清单】对应的【保养任务】的【保养状态】为【保养完成】的治具；
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>工治具治具需求清单</returns>
        private List<FixtureDemand> GetReceiveFixtureDemands(PagingInfo pagingInfo)
        {
            var fixtureDemands = GetReceiveDemandsByStateAndNo(null);
            var nos = fixtureDemands.Select(p => p.No).ToList<string>();
            var relatedNos = RT.Service.Resolve<MaintainTaskController>().GetRelatedNosByMaintainState(nos);
            var resultFixtureDemands = fixtureDemands.Where(p => !relatedNos.Contains(p.No)).ToList();
            return resultFixtureDemands;
        }

        /// <summary>
        /// 获取领用需求明细列表 
        /// </summary>
        /// <param name="receiveNoQueryInfo">入库任务查询信息</param>
        /// <returns>分页领用需求明细信息</returns>
        [ApiService("获取领用需求明细列表")]
        [return: ApiReturn("分页领用需求明细信息  ReceiveDetailDataInfo")]
        public virtual ReceiveDetailDataInfo GetPagingReceiveDetailInfosByNo([ApiParameter("入库任务查询信息")] ReceiveNoQueryInfo receiveNoQueryInfo)
        {
            if (receiveNoQueryInfo == null)
            {
                throw new ValidationException("前端传入参数有误！".L10N());
            }
            var pagingInfo = GetPagingInfo(receiveNoQueryInfo.PageNumber, receiveNoQueryInfo.PageSize);
            var fixDemand = GetFixtureDemand(receiveNoQueryInfo.No);
            if (fixDemand == null)
            { throw new ValidationException("查询的需求单号不存在！".L10N()); }
            var unloads = GetUnloadsByDemandIds(new List<double> { fixDemand.Id }, ReceiveState.None, pagingInfo);
            var receiveDetailDataInfo = new ReceiveDetailDataInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = unloads.Count()
            };
            foreach (var unload in unloads)
            {
                if (unload.UnloadQty <= unload.NgQty)
                {
                    continue;
                }
                var receiveDetailInfo = new ReceiveDetailInfo();
                receiveDetailInfo.IsSelect = 0;
                receiveDetailInfo.No = fixDemand.No;
                receiveDetailInfo.ToolCode = unload.TurnoverToolCode;
                receiveDetailInfo.Code = unload.AccountCode;
                receiveDetailInfo.EncodeCode = unload.EncodeCode;
                receiveDetailInfo.ModelName = unload.ModelName;
                receiveDetailInfo.FixtureType = unload.FixtureType;
                receiveDetailInfo.RFID = unload.FixtureAccount.Rfid;
                receiveDetailInfo.Qty = unload.UnloadQty - unload.NgQty;
                receiveDetailInfo.MaintainState =unload.MaintainTaskState.ToLabel().L10N();
                receiveDetailDataInfo.ReceiveDetailInfos.Add(receiveDetailInfo);
            }
            return receiveDetailDataInfo;
        }

        /// <summary>
        /// 设置满足载具编号的领用需求明细列表  
        /// </summary>
        /// <param name="toolCode">载具编号</param>
        /// <param name="receiveDetailInfos">工治具领用需求明细列表</param>
        /// <returns>分页领用需求明细信息</returns>
        [ApiService("设置满足载具编号的领用需求明细列表")]
        [return: ApiReturn("分页领用需求明细信息  ReceiveDetailDataInfoByTool")]
        public virtual ReceiveDetailDataInfoByTool GetReceiveDetailInfosByTool([ApiParameter("载具编号")] string toolCode,
            [ApiParameter("工治具领用需求明细列表")] List<ReceiveDetailInfo> receiveDetailInfos)
        {
            var receivesByTool = new ReceiveDetailDataInfoByTool();
            var isExist = RT.Service.Resolve<KitTurnoverToolController>().IsTurnoverTool(toolCode);
            receivesByTool.IsSuccess = isExist;
            if (!isExist)
            {
                receivesByTool.Message = "无此载具：{0}".L10nFormat(toolCode);
                return receivesByTool;
            }
            if (receiveDetailInfos == null)
            {
                receivesByTool.Message = "前端传值错误".L10nFormat(toolCode);
                return receivesByTool;
            }
            if (receiveDetailInfos.All(p => p.ToolCode != toolCode))
            {
                receivesByTool.IsSuccess = false;
                receivesByTool.Message = "此载具:{0}未装载本单据治具".L10nFormat(toolCode);
                return receivesByTool;
            }
            foreach (var receiveDetailInfo in receiveDetailInfos)
            {
                if (receiveDetailInfo.ToolCode == toolCode)
                {
                    receiveDetailInfo.IsSelect = 1;
                    receivesByTool.ReceiveDetailInfos.Add(receiveDetailInfo);
                }
            }
            return receivesByTool;
        }

        /// <summary>
        /// 设置满足ID编号的领用需求明细列表 
        /// </summary>
        /// <param name="code">ID编码</param>
        /// <param name="receiveDetailInfos">工治具领用需求明细列表</param>
        /// <returns>分页领用需求明细信息</returns>
        [ApiService("设置满足ID编号的领用需求明细列表")]
        [return: ApiReturn("分页领用需求明细信息  ReceiveDetailDataInfoByTool")]
        public virtual ReceiveDetailDataInfoByTool GetReceiveDetailInfos([ApiParameter("ID编码")] string code, [ApiParameter("工治具领用需求明细列表")] List<ReceiveDetailInfo> receiveDetailInfos)
        {

            var receivesByTool = new ReceiveDetailDataInfoByTool();
            if (receiveDetailInfos == null)
            {
                return receivesByTool;
            }
            var account = GetFixtureAccountByCodeOrRFID(code);
            if (account == null)
            {
                receivesByTool.IsSuccess = false;
                receivesByTool.Message = "无此ID编码/RFID：{0}".L10nFormat(code);
                return receivesByTool;
            }
            if (receiveDetailInfos.All(p => p.Code != code) && receiveDetailInfos.All(p => p.EncodeCode != code) && receiveDetailInfos.All(p => p.RFID != code))
            {
                receivesByTool.IsSuccess = false;
                receivesByTool.Message = "此单无【{0}】治具ID/编码/RFID".L10nFormat(code);
                return receivesByTool;
            }
            foreach (var receiveDetailInfo in receiveDetailInfos)
            {
                if (receiveDetailInfo.Code == code || receiveDetailInfo.RFID == code || receiveDetailInfo.EncodeCode == code)
                {
                    receiveDetailInfo.IsSelect = 1;
                    receivesByTool.ReceiveDetailInfos.Add(receiveDetailInfo);
                }
            }
            receivesByTool.IsSuccess = true;
            return receivesByTool;
        }

        /// <summary>
        /// 提交工治具治具领用列表
        /// </summary>
        /// <param name="receiveDetailInfos">领用需求明细信息</param>
        [ApiService("提交工治具治具领用列表")]
        public virtual void SubmitReceiveDetailInfo([ApiParameter("领用需求明细信息")] List<ReceiveDetailInfo> receiveDetailInfos)
        {
            if (receiveDetailInfos == null)
            {
                throw new ValidationException("前端参数错误，请检查!".L10N());
            }
            var idCodes = receiveDetailInfos.Select(p => p.Code).Distinct().ToList();
            var nos = receiveDetailInfos.Select(p => p.No).Distinct().ToList();
            var accounts = GetFixtureAccountsByCodes(idCodes);
            var demands = GetFixtureDemandsByNos(nos);
            var fixtureDemandIds = demands.Select(p => p.Id).ToList();
            var noneUnloads = GetUnloadsByDemandIds(fixtureDemandIds, ReceiveState.None);
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                foreach (var receiveInfo in receiveDetailInfos)
                {
                    //更新治具台账
                    var account = accounts.FirstOrDefault(p => p.Code == receiveInfo.Code);
                    ReceiveUpdateAccount(account, receiveInfo);
                    //子表【出库明细】的【领用状态】变为【已领用】
                    var demand = demands.FirstOrDefault(p => p.No == receiveInfo.No);
                    if (demand == null)
                    { throw new ValidationException("找不到需求单号为{0}的工治具治具需求！".L10nFormat(receiveInfo.No)); }
                    var unload = noneUnloads.FirstOrDefault(p => p.FixtureDemandId == demand.Id && p.FixtureAccountId == account.Id && p.TurnoverToolCode == receiveInfo.ToolCode && p.State == ReceiveState.None && p.UnloadQty - p.NgQty == receiveInfo.Qty);
                    unload = unload ?? noneUnloads.FirstOrDefault(p => p.FixtureDemandId == demand.Id && p.FixtureAccountId == account.Id && p.TurnoverToolCode == receiveInfo.ToolCode && p.State == ReceiveState.None);
                    if (unload == null)
                    { throw new ValidationException("找不到需求单号{0},ID编码{1},关联载具{2}的未领用的出库明细！".L10nFormat(demand.No, account.Code, receiveInfo.ToolCode)); }
                    if (unload.UnloadQty - unload.NgQty != receiveInfo.Qty)
                    {
                        throw new ValidationException("数据异常！需求单号{0},ID编码{1},关联载具{2}的未领用的出库明细,可领用数量为{3}，不等于领用数量{4}"
                              .L10nFormat(demand.No, account.Code, receiveInfo.ToolCode, unload.UnloadQty - unload.NgQty, receiveInfo.Qty));
                    }
                    unload.State = ReceiveState.Finish;
                    RF.Save(unload);
                    // 创建使用履历
                    CreateSaveAccountUseResume(account.Id, demand.ResourceId, demand.WorkOrderId, UseResumeType.Receive, receiveInfo.Qty);
                }
                //主表【领用状态】进行更新
                var finishUnloads = GetUnloadsByDemandIds(fixtureDemandIds, ReceiveState.Finish);
                var details = GetDetailsByDemandIds(fixtureDemandIds);
                foreach (var fixtureDemand in demands)
                {
                    var demandFinishUnloads = finishUnloads.Where(p => p.FixtureDemandId == fixtureDemand.Id);
                    var unloadQty = demandFinishUnloads.Sum(p => p.UnloadQty);
                    var ngQty = demandFinishUnloads.Sum(p => p.NgQty);
                    var demandQty = details.Where(p => p.FixtureDemandId == fixtureDemand.Id).Sum(p => p.DemandQty);
                    var receiveQty = unloadQty - ngQty;
                    fixtureDemand.ReceiveState = receiveQty >= demandQty ? ReceiveState.Finish : ReceiveState.Part;
                }
                RF.Save(demands);
                tran.Complete();
            }
        }

        /// <summary>
        /// 提交工治具治具领用列表-更新治具台账
        /// </summary>
        /// <param name="account">治具台账</param>
        /// <param name="receiveInfo">工治具领用清单信息</param>
        private void ReceiveUpdateAccount(FixtureAccount account, ReceiveDetailInfo receiveInfo)
        {
            if (account == null)
                throw new ValidationException("找不到ID编码为{0}的工治具治具台账！".L10nFormat(receiveInfo.Code));
            if (account.ManageMode == ManageMode.Number)
            {
                if (account.AccountState != FixtureAccountState.WaitReceive)
                    throw new ValidationException("ID编码为{0}的工治具治具台账不是待领用状态，无法领用".L10nFormat(receiveInfo.Code));
                account.AccountState = FixtureAccountState.Online;
            }
            else
            {
                if (account.WaitReceive < receiveInfo.Qty)
                    throw new ValidationException("领用失败！工治具台账{0}的待领用数量为{1}，不足{2}！".L10nFormat(account.Code, account.WaitReceive, receiveInfo.Qty));
                account.WaitReceive -= receiveInfo.Qty;
                account.OnlineQty += receiveInfo.Qty;
            }
            RF.Save(account);
        }
        #endregion
    }
}
