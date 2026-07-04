using SIE.Barcodes.Panels.Configs;
using SIE.Barcodes.Panels.ViewModels;
using SIE.Common.Configs;
using SIE.Common.Domain;
using SIE.Common.InvOrg;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.EventMessages.MES.Panels;
using SIE.Security;
using SIE.Security.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码控制器
    /// </summary>
    public partial class PanelController : DomainController
    {
        /// <summary>
        /// 根据拼板码明细id获取拼板码明细(贪婪加载工单)
        /// </summary>
        /// <param name="id">拼板码明细id</param>
        /// <returns>拼板码明细</returns>
        public virtual Panel GetPanel(double id)
        {
            return Query<Panel>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWith(Panel.WorkOrderProperty));
        }

        /// <summary>
        /// 根据拼板码编码获取拼板码(贪婪加载工单)
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>拼板码</returns>
        public virtual Panel GetPanel(string code)
        {
            return Query<Panel>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWith(Panel.WorkOrderProperty));
        }

        /// <summary>
        /// 判断条码是否为拼板码
        /// </summary>
        /// <param name="code">条码</param>
        /// <returns>条码为拼板码返回true，否则返回false</returns>
        public virtual bool IsExistPanel(string code)
        {
            return Query<Panel>().Where(p => p.Code == code).Count() > 0;
        }

        /// <summary>
        /// 拼板码打印界面-查询工单
        /// </summary>
        /// <param name="criteria">工单查询实体</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<PanelWorkOrder> GetWorkOrders(PanelWorkOrderCriteria criteria)
        {
            var q = Query<PanelWorkOrder>().Where(p => p.State != WorkOrderState.CancelRelease);
            if (criteria.No.IsNotEmpty())
                q.Where(p => p.No.Contains(criteria.No));
            if (criteria.PlanBeginDate.BeginValue.HasValue)
                q.Where(p => p.PlanBeginDate >= criteria.PlanBeginDate.BeginValue);
            if (criteria.PlanBeginDate.EndValue.HasValue)
                q.Where(p => p.PlanBeginDate <= criteria.PlanBeginDate.EndValue);
            q.Where(p => p.PanelWorkOrderId == null);
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// Get panel
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<PanelRange> GetPanelRanges(PanelRangeCriteria criteria)
        {
            var q = Query<PanelRange>();
            if (criteria.WorkOrderId.HasValue)
                q.Where(p => p.WorkOrderId==criteria.WorkOrderId);
            if (!criteria.PanelCode.IsNullOrEmpty())
            {
                q.Exists<Panel>((a,b)=>b.Where(p=>p.RangeId==a.Id&&p.Code== criteria.PanelCode));
            }
            if (criteria.ReceiveById.HasValue)
                q.Where(p => p.ReceiveById == criteria.ReceiveById);
            if (criteria.ReceiveDate.BeginValue.HasValue)
                q.Where(p => p.ReceiveDate >= criteria.ReceiveDate.BeginValue);
            if (criteria.ReceiveDate.EndValue.HasValue)
                q.Where(p => p.ReceiveDate <= criteria.ReceiveDate.EndValue);
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// Range receive
        /// </summary>
        /// <param name="rangeId"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        public virtual void PanelRangeReceive(double rangeId, string userName, string pwd)
        {
            if (userName.IsNullOrEmpty())
                throw new ValidationException("用户名不能为空".L10N());
            var range = RF.GetById<PanelRange>(rangeId);
            if (range == null)
                throw new EntityNotFoundException(typeof(PanelRange), rangeId);
            if (range.State == ReceiveState.Received)
                throw new ValidationException("领用失败，拼板码状态为[{0}]".L10nFormat(ReceiveState.Received.ToLabel()));
            LoginUser user = AuthenticateManager.Current.Authenticate(userName, pwd);
            range.ReceiveById = user.EmployeeId;
            range.ReceiveDate = RF.Find<PanelRange>().GetDbTime();
            range.State = ReceiveState.Received;
            RF.Save(range);
        }
        /// <summary>
        /// 通过工单Id获取拼板码打印
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>拼板码打印</returns>
        public virtual PanelWorkOrder GetPanelWorkOrder(double workOrderId)
        {
            return Query<PanelWorkOrder>().Where(p => p.Id == workOrderId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty().LoadWith(PanelWorkOrder.TemplateProperty));
        }

        /// <summary>
        /// 根据工单号获取拼板码列表
        /// </summary>
        /// <param name="workOrderId">工单号</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="sortInfo">排序参数</param>
        /// <returns>拼板码列表</returns>
        public virtual EntityList<Panel> GetPanelsByWorkOrderId(double workOrderId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<Panel>().Where(p => p.WorkOrderId == workOrderId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取拼板条码Id所对应的所有拼板码信息列表
        /// </summary>
        /// <param name="panelIds">拼板码Ids列表</param>
        /// <returns>拼板码信息列表</returns>
        public virtual EntityList<Panel> GetPanelsByIds(List<double> panelIds)
        {
            return Query<Panel>().Where(p => panelIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取工单报废拼板码数量
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>报废拼板码数量</returns>
        public virtual int GetScrapPanelCount(double workOrderId)
        {
            return Query<Panel>().Where(p => p.WorkOrderId == workOrderId && p.IsScrap).Count();
        }

        #region 拼板码打印、补打、报废
        /// <summary>
        /// 创建工单拼板码
        /// </summary>
        /// <param name="info">打印信息</param>
        /// <returns>错误信息和拼板码列表</returns>
        public virtual Tuple<string, EntityList<Panel>> PrintPanels(PrinterInfo info)
        {
            string errMsg = string.Empty;
            EntityList<Panel> panels = new EntityList<Panel>();
            try
            {
                panels.AddRange(Print(info));
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }
            return new Tuple<string, EntityList<Panel>>(errMsg, panels);
        }

        /// <summary>
        /// 创建工单拼板码
        /// </summary>
        /// <param name="info">打印信息</param>
        /// <returns>拼板码列表</returns>
        public virtual EntityList<Panel> Print(PrinterInfo info)
        {
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                var workOrder = GetById<PanelWorkOrder>(info.WorkOrderId);
                if (workOrder == null)
                    throw new EntityNotFoundException(typeof(PanelWorkOrder), info.WorkOrderId);
                var numberRule = GetById<NumberRule>(info.NumberRuleId);
                if (numberRule == null)
                    throw new EntityNotFoundException(typeof(NumberRule), info.NumberRuleId);
                var template = GetById<PrintTemplate>(info.PrintTemplateId);
                if (template == null)
                    throw new EntityNotFoundException(typeof(PrintTemplate), info.PrintTemplateId);
                if (info.PrintQty <= 0)
                    throw new ValidationException("打印数量必须大于0".L10N());
                var panels = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRule.Id, info.PrintQty, workOrder);
                var range = new PanelRange()
                {
                    StartNo = panels.FirstOrDefault(),
                    EndNo = panels.LastOrDefault(),
                    WorkOrder = workOrder,
                    PrintQty= info.PrintQty
                };
                RF.Save(range);
                var panelPrintQty = workOrder.PanelPrintQty;
                var newPanelPrintQty = panelPrintQty + info.PrintQty;
                workOrder.PanelPrintQty = newPanelPrintQty;
                RF.Save(workOrder);
                var panelList = new EntityList<Panel>();
                var now = RF.Find<Panel>().GetDbTime();
                foreach (var sn in panels)
                {
                    var panel = new Panel()
                    {
                        Code = sn,
                        PrintDate = now,
                        PrintQty = 1,
                        IsScrap = false,
                        Range = range,
                        WorkOrder = workOrder,
                        State = PanelState.Printed,
                        PrintById = AppRuntime.IdentityId,
                        //Qty = workOrder.PanelQty
                    };

                    panel.CreateBy = RT.IdentityId;
                    panel.CreateDate = now;
                    panel.UpdateBy = RT.IdentityId;
                    panel.UpdateDate = now;
                    InvOrgIdExtension.SetInvOrgId(panel, RT.InvOrg);
                    PhantomEntityExtension.SetIsPhantom(panel, false);
                    panelList.Add(panel);
                }
                var Logger = Logging.LogManager.GetLogger("startup_logger");
                using (Diagnostics.PerformenceWatcher.Start(Logger, "批量保存拼板码列表"))
                {
                    var existSns = panels.SplitContains(codes => Query<Panel>().Where(p => codes.Contains(p.Code)).ToList()).Select(p => p.Code).ToList();
                    if (existSns.Count > 0)
                        throw new ValidationException("已经存在拼板码：{0}".L10nFormat(string.Join(";", existSns)));
                    BulkSaver.SetBatchEntityId(panelList);
                    RF.BatchInsert(panelList);
                }
                tran.Complete();
                return panelList;
            }
        }

        /// <summary>
        /// 拼板码补打
        /// </summary>
        /// <param name="panelList">选中条码列表</param>
        /// <param name="reason">补打原因</param>
        /// <param name="times">补打次数</param> 
        /// <returns>补打结果</returns>
        public virtual void ReprintPanel(EntityList<Panel> panelList, string reason, int times)
        {
            if (reason.IsNullOrWhiteSpace())
                throw new ValidationException("补打原因不允许为空.".L10N());
            if (panelList.Count == 0)
                throw new ArgumentNullException(nameof(panelList));
            if (panelList.Any(p => p.IsScrap))
                throw new ValidationException("选择条码存在已报废条码".L10N());
            if (times < 1)
                throw new ValidationException("打印次数：{0} 必须大于等于 1".L10nFormat(times));
            foreach (var panel in panelList)
            {
                panel.PrintQty += times;
                panel.PrintDate = RF.Find<Panel>().GetDbTime();
                panel.PrintById = AppRuntime.IdentityId;
                panel.State = PanelState.Printed;
                RF.Save(panel);
            }
        }

        /// <summary>
        /// 拼板码报废
        /// </summary>
        /// <param name="panelList">待报废拼板码列表</param>
        /// <param name="reason">报废原因</param>
        public virtual void PanelScrap(EntityList<Panel> panelList, string reason)
        {
            ValidatePanelScrap(panelList, reason);
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                foreach (var panel in panelList)
                {
                    panel.IsScrap = true;
                    panel.ScrapReason = reason;
                    RF.Save(panel);
                    var range = GetById<PanelRange>(panel.RangeId);
                    if (range != null)
                    {
                        range.ScrapedQty += 1;
                        RF.Save(range);
                    }
                    var workorder = RF.GetById<PanelWorkOrder>(panel.WorkOrderId);
                    if (workorder == null)
                        throw new EntityNotFoundException(typeof(PanelWorkOrder), panel.WorkOrderId);
                    var panelPrintQty = workorder.PanelPrintQty;
                    workorder.PanelPrintQty = panelPrintQty - 1;
                    RF.Save(workorder);
                }
                var scrapBarcodes = panelList.Select(p => new BarcodeScrapInfo { Sn = p.Code, workOrderId = p.WorkOrderId }).ToList();
                RT.EventBus.Publish(new PanelManualScrapEvent(scrapBarcodes));
                tran.Complete();
            }
        }


        /// <summary>
        /// 拼板码报废
        /// </summary>
        /// <param name="panelCode">拼板码编码</param>
        /// <param name="reason">报废原因</param>
        /// <exception cref="ArgumentException">拼板码为空、报废原因为空</exception>
        /// <exception cref="ValidationException">拼板码为空、拼板码已报废</exception>
        public virtual void PanelScrap(string panelCode, string reason)
        {
            Check.NotNullOrEmpty(panelCode, "拼板码不能为空".L10N());
            Check.NotNullOrEmpty(reason, "报废原因不能为空".L10N());
            var panel = GetPanel(panelCode);
            if (panel == null)
                throw new ValidationException("拼板码不能为空".L10N());
            if (panel.IsScrap)
                throw new ValidationException("报废失败，拼板码已报废".L10N());
            panel.IsScrap = true;
            RF.Save(panel);
            var range = GetById<PanelRange>(panel.RangeId);
            if (range != null)
            {
                range.ScrapedQty += 1;
                RF.Save(range);
            }
            RT.EventBus.Publish(new PanelScrapEvent(panel.WorkOrderId, panelCode, reason));
        }

        /// <summary>
        /// 拼板码挂起
        /// </summary>
        /// <param name="panelList">待挂起拼板码列表</param>
        public virtual void PanelPending(EntityList<Panel> panelList)
        {
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                foreach (var panel in panelList)
                {
                    panel.IsPending = true;
                    RF.Save(panel);
                }
                tran.Complete();
            }
        }
        /// <summary>
        /// 挂起恢复
        /// </summary>
        /// <param name="panelList">待恢复拼板码列表</param>
        public virtual void PanelUnPending(EntityList<Panel> panelList)
        {
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                foreach (var panel in panelList)
                {
                    panel.IsPending =false;
                    RF.Save(panel);
                }
                tran.Complete();
            }
        }
        /// <summary>
        /// 验证证拼板码报废
        /// </summary>
        /// <param name="panelList">待报废拼板码列表</param>
        /// <param name="reason">报废原因</param>
        private void ValidatePanelScrap(EntityList<Panel> panelList, string reason)
        {
            if (reason.IsNullOrEmpty())
                throw new ValidationException("报废原因不允许为空.".L10N());
            if (panelList.Count == 0)
                throw new ValidationException("报废拼板码不能为空".L10N());
            var scrapList = panelList.Where(p => p.IsScrap);
            if (scrapList.Any())
                throw new ValidationException("条码：{0} 是报废拼板码不允许多次报废".L10nFormat(string.Join(",", scrapList.Select(p => p.Code).ToArray())));
        }
        #endregion

        /// <summary>
        /// 获取拼板码打印配置
        /// </summary>
        /// <returns>拼板码打印配置值</returns>
        public virtual PanelPrintConfigValue GetPanelPrintConfig()
        {
            var config = ConfigService.GetConfig(new PanelPrintConfig(), typeof(PanelWorkOrder));
            if (config == null || config.BacodeRule == null || config.LabelTemplate == null)
                throw new ValidationException("未配置拼板码打印规则和模板,请检查规则配置！".L10N());
            return config;
        }

        /// <summary>
        /// 拼板码归属
        /// </summary>
        /// <param name="panelBelongVM">拼板码归属ViewModel</param>
        /// <returns>错误信息</returns>
        public virtual string PanelBelong(PanelBelongViewModel panelBelongVM)
        {
            string errMsg = string.Empty;
            try
            {
                //var panel = GetPanel(panelBelongVM.PanelId);
                //PanelBelong(panel, panelBelongVM.WorkOrderId);

                if (!panelBelongVM.WorkOrderId.HasValue)
                {
                    throw new ValidationException("归属工单Id没有找到值，执行拼板码归属失败".L10N());
                }

                RT.Service.Resolve<IPanelBelongWorkOrder>()
                    .PanelBelongWorkOrder(panelBelongVM.WorkOrderId.Value, panelBelongVM.PanelId);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 拼板码归属
        /// </summary>
        /// <param name="panel">拼板码</param>
        /// <param name="belongWorkOrderId">归属工单ID</param>
        public virtual void PanelBelong(Panel panel, double? belongWorkOrderId)
        {
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                Check.NotNull(panel, nameof(panel));

                var orgWo = RF.GetById<WorkOrder>(panel.WorkOrderId);

                if (orgWo == null)
                    throw new ValidationException("原工单不存在，拼板码归属失败!".L10N());

                if (panel.IsScrap)
                    throw new ValidationException("工单[{0}]拼板码[{1}]已报废，拼板码归属失败!".L10nFormat(panel.WONo, panel.Code));

                if (!belongWorkOrderId.HasValue)
                    throw new ValidationException("归属工单必填，拼板码归属失败!".L10N());

                var belongWo = RF.GetById<WorkOrder>(belongWorkOrderId);
                var printingQty = belongWo.PlanQty - belongWo.PanelPrintQty;
                if (printingQty <= 0)
                    throw new ValidationException("归属工单[{0}]的剩余数量为0，拼板码归属失败!".L10nFormat(belongWo.No));
                panel.WorkOrderId = belongWo.Id;
                RF.Save(panel);
                belongWo.PanelPrintQty += 1;
                RF.Save(belongWo);
                orgWo.PanelPrintQty -= 1;
                RF.Save(orgWo);
                CreatePanelBelongLog(panel, orgWo, belongWo);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建保存拼板码归属日志
        /// </summary>
        /// <param name="panel">拼板码</param>
        /// <param name="orgWo">原工单</param>
        /// <param name="belongWo">归属工单</param>
        private void CreatePanelBelongLog(Panel panel, WorkOrder orgWo, WorkOrder belongWo)
        {
            var log = new PanelBelongLog()
            {
                OrgWorkOrderId = orgWo.Id,
                WorkOrderId = belongWo.Id,
                Sn = panel.Code,
                OperatorId = RT.IdentityId,
                OperatDate = RF.Find<PanelBelongLog>().GetDbTime(),
            };
            RF.Save(log);
        }
    }
}