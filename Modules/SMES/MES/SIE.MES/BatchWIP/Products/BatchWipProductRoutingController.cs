using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Scripting.Utils;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品工艺路线控制器
    /// </summary>
    public partial class BatchWipProductRoutingController : DomainController
    {
        /// <summary>
        /// 获取产品工艺路线修改事件
        /// </summary>
        /// <param name="batchRelationId">批次关系</param>
        /// <returns>产品工艺路线变更事件列表</returns>
        public virtual EntityList<BatchWipProductRoutingEvent> GetWipProductRoutingEvents(double batchRelationId)
        {
            var wipProductRouting = RT.Service.Resolve<BatchWipProductRoutingController>().GetWipProductRouting(batchRelationId);
            if (wipProductRouting == null)
            {
                return new EntityList<BatchWipProductRoutingEvent>();
            }
            return Query<BatchWipProductRoutingEvent>().Where(p => p.RoutingId == wipProductRouting.Id).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次产品工艺路线
        /// </summary>
        /// <param name="relationId">批次关系Id</param>
        /// <returns>批次产品工艺路线</returns>
        public virtual BatchWipProductRouting GetWipProductRouting(double relationId)
        {
            return Query<BatchWipProductRouting>().Where(p => p.RelationId == relationId).FirstOrDefault();
        }

        /// <summary>
        /// 启用批次产品工艺路线
        /// </summary>
        /// <param name="relationId">批次关系Id</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public virtual void EnableProductRouting(double relationId, string oldLayout, string newLayout)
        {
            var relation = ValidationBatchRelation(relationId);
            var product = ValidationProduct(relation.Bid);
            IContainer container = new ContainerModel();
            container.Deserialize(newLayout);
            container.ValidateSave();
            var currentCount = container.Activitys.Count(p => p.ProcessState == ProcessState.Current);
            if (currentCount != 1)
                throw new ValidationException("只可以有一个当前工序，当前工序数量：{0}".L10nFormat(currentCount));
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                CreateBatchRuntimeRouting(relation.Bid, relation.WorkOrderId, newLayout);
                GenerateRouting(container, product);
                RT.Service.Resolve<RuntimeController>().Save(product);
                SaveProductRoutingInfo(relation, oldLayout, newLayout, "启用");
                relation.IsPause = YesNo.No;
                RF.Save(relation);
                tran.Complete();
            }
        }

        /// <summary>
        /// 生成工艺路线数据
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="product">产品</param>
        void GenerateRouting(IContainer container, product product)
        {
            product.Routing.Processes.Clear();
            bool isPassRate = false;
            StringBuilder sb = new StringBuilder();
            foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
            {
                string passRateProcess = GenerateRoutingProcess(product, ref isPassRate, activity);
                sb.Append(passRateProcess);
            }
            ////假如当前工序是开始工序，先给开始指定的工序，再计算其它工序
            var beginActivity = container.Activitys.FirstOrDefault(p => p.Type == ActivityType.Initial && p.ProcessState == ProcessState.Current);
            if (beginActivity != null)
            {
                var nextProcessId = beginActivity.BeginRules.FirstOrDefault().EndActivity.Index;
                product.Routing.Next.Clear();
                var nextProcess = product.Routing.Processes.FirstOrDefault(p => p.Id == nextProcessId);
                product.Routing.Next.Add(nextProcess.Id);
            }
            else//当前工序不是开始工序，直接计算下工序
            {
                if (product.Routing.Current == null && product.Routing.CurrentId != 0)
                {
                    var curActivity = container.Activitys.FirstOrDefault(p => p.ProcessState == ProcessState.Current);
                    if (curActivity != null)
                    {
                        var currentProcess= product.Routing.Processes.Find(n => n.Id == curActivity.Index);
                        if (currentProcess != null)
                        {
                            product.Routing.Current = currentProcess;
                        }
                    }
                }

                RT.Service.Resolve<WipController>().ComputeNextProcess(product, ResultType.Pass, null);
            }
        }

        /// <summary>
        /// 生成工艺路线数据
        /// </summary>
        /// <param name="product"></param>
        /// <param name="isPassRate"></param>
        /// <param name="activity"></param>
        private string GenerateRoutingProcess(product product, ref bool isPassRate, IActivity activity)
        {
            string passRateProcess = string.Empty;
            var process = RF.GetById<Process>(activity.ProcessId);

            var isGroup = ((ActivityModel)activity).IsGroup;
            if (process == null && !isGroup)
            {
                throw new ValidationException("工序：{0} 不存在".L10nFormat(activity.Text));
            }
            if (isPassRate && activity.IsPassRate)
            {
                passRateProcess += activity.Text;
                throw new ValidationException("直通率工序【{0}】最多只能勾选一个".L10nFormat(passRateProcess));
            }
            if (activity.IsPassRate)
            {
                isPassRate = true;
                passRateProcess += activity.Text + ",";
            }
            var runtimeProcess = new process()
            {
                Id = activity.Index,
                ProcessId = activity.ProcessId,
                Optional = activity.IsOptional,
                Repeat = activity.IsRepeat,
                CreateSku = activity.CreateSku,
                IsCalculate = activity.IsCalculate,
                IsGenerateTask = activity.IsGenerateTask,
                IsRequirementTask= activity.IsRequirementTask,
                IsBuckleMaterial = activity.IsBuckleMaterial,
                IsPassRate = activity.IsPassRate,
                IsBinding = activity.IsBinding,
                IsUnBinding = activity.IsUnBinding,
                Overtime = activity.Overtime,
                StartProcess = activity.StartProcess,
                NormalVictory = activity.NormalVictory,
                RepairVictory = activity.RepairVictory,
                IsStricter = activity.IsStricter,
                Index = activity.Index,
                MaxPassNum = activity.MaxPassNum,
                IsNextMoveIn = activity.IsNextMoveIn,
                Outsourcing = activity.IsOutsourcing
            };

            if (process != null)
            {
                runtimeProcess.Name = process.Name;
                runtimeProcess.Type = (ProcessType)process.Type;
            }
            else
            {
                runtimeProcess.Name = activity.Text;
            }
            var activityModel = activity as ActivityModel;
            if (activityModel != null)
            {
                runtimeProcess.IsGroup = activityModel.IsGroup;
                runtimeProcess.GroupId = activityModel.GroupId;
            }
            var beginRule = activity.EndRules.FirstOrDefault(p => p.BeginActivity.Type == ActivityType.Initial);
            if (beginRule != null)
                runtimeProcess.Sign = RoutingProcessSign.Start;
            var endRule = activity.BeginRules.FirstOrDefault(p => p.EndActivity.Type == ActivityType.Completion);
            if (endRule != null)
                runtimeProcess.Sign |= RoutingProcessSign.End;
            if (beginRule == null && endRule == null)
                runtimeProcess.Sign = RoutingProcessSign.Normal;
            foreach (var rule in activity.BeginRules)
            {
                if (rule.EndActivity == null || rule.EndActivity.Type != ActivityType.Interaction)
                    continue;
                var processParameter = RF.GetById<ProcessParameter>(rule.ParameterId);
                if (((ResultType)processParameter.Type & ResultType.Pass) == ResultType.Pass)
                    runtimeProcess.Next.Add(ResultType.Pass, new List<double>() { rule.EndActivity.Index });
                if (((ResultType)processParameter.Type & ResultType.Fail) == ResultType.Fail)
                    runtimeProcess.Next.Add(ResultType.Fail, new List<double>() { rule.EndActivity.Index });
                if (((ResultType)processParameter.Type & ResultType.Custom) == ResultType.Custom)
                {
                    if (runtimeProcess.Next.ContainsKey(ResultType.Custom))
                    {
                        runtimeProcess.Next[ResultType.Custom].Add(rule.EndActivity.Index);
                    }
                    else
                        runtimeProcess.Next.Add(ResultType.Custom, new List<double>() { rule.EndActivity.Index });
                }
            }

            if (activity.ProcessState == ProcessState.Current)
            {
                product.Routing.CurrentId = activity.ProcessId;
                product.Routing.Next.Clear();
                product.Routing.Next.AddRange(runtimeProcess.Next.SelectMany(p => p.Value));
                if (!product.Routing.Next.Any())
                    throw new ValidationException("后工序：{0} 不能是当前工序".L10nFormat(activity.Text));
            }

            foreach (var processBom in activity.ProcessBoms)
            {
                if (processBom.Qty <= 0)
                    throw new ValidationException("工序：{0} 的BOM数量必须大于 0".L10nFormat(activity.Text));
                runtimeProcess.Boms.Add(new bom()
                {
                    ItemId = processBom.ItemId,
                    Qty = processBom.Qty,
                    IsBuckleMaterial = processBom.IsBuckleMaterial,
                    ItemExtProp = processBom.ItemExtProp,
                    ItemExtPropName = processBom.ItemExtPropName
                });
            }

            product.Routing.Processes.Add(runtimeProcess);
            return passRateProcess;
        }

        /// <summary>
        /// 暂停产品工艺路线
        /// </summary>
        /// <param name="relationId">批次关系ID</param>
        /// <param name="oldLayout">工艺路线旧布局</param>
        /// <param name="newLayout">工艺路线新布局</param>
        public virtual void PauseProductRouting(double relationId, string oldLayout, string newLayout)
        {
            var relation = ValidationBatchRelation(relationId, YesNo.Yes);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                relation.IsPause = YesNo.Yes;
                RF.Save(relation);
                SaveProductRoutingInfo(relation, oldLayout, newLayout, "暂停");
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存产品工艺路线
        /// </summary>
        /// <param name="versionId">生产版本</param>
        /// <param name="relationId">批次关系Id</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public virtual void SaveProductRouting(double versionId, double relationId, string oldLayout, string newLayout)
        {
            var relation = ValidationBatchRelation(relationId);
            var version = RF.GetById<BatchWipProductVersion>(versionId);
            var product = ValidationProduct(relation.Bid);
            IContainer container = new ContainerModel();
            container.Deserialize(newLayout);
            container.ValidateSave();



            var currents = container.Activitys.Where(p => p.ProcessState == ProcessState.Current);
            var currentCount = currents.Count();
            if (currentCount > 1)//不允许存在多个
            {
                throw new ValidationException("只可以有一个当前工序，当前工序数量：{0}".L10nFormat(currentCount));
            }
            //如果界面没有设置当前工序则去版本的取
            if (currentCount < 1 && version.CurrentProcessIndex.HasValue)
            {
                var currentProcessActivityIndex = container.Activitys.FindIndex(p => p.Index == (int)version.CurrentProcessIndex);
                if (currentProcessActivityIndex < 0)//如果版本的取不到也报错
                {
                    throw new ValidationException("只可以有一个当前工序，当前工序数量：{0}".L10nFormat(currentCount));
                }
                container.Activitys[currentProcessActivityIndex].ProcessState = ProcessState.Current;
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var current = currents.FirstOrDefault();
                CreateBatchRuntimeRouting(relation.Bid, relation.WorkOrderId, newLayout);
                GenerateRouting(container, product);
                RT.Service.Resolve<RuntimeController>().Save(product);
                SaveNowAndNextProcess(current.ProcessId, product, version);
                SaveProductRoutingInfo(relation, oldLayout, newLayout, "保存");
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存并计算下一工序
        /// </summary>
        /// <param name="currentProIds"></param>
        /// <param name="product"></param>
        /// <param name="version"></param>
        private void SaveNowAndNextProcess(double currentProIds, product product, BatchWipProductVersion version)
        {
            // 更新当前工序
            version.ProcessId = currentProIds;

            //获取工艺路线下一工序
            var nexts = product.Routing.GetNext();
            var nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId != currentProIds);
            if (nextProcess == null)
            {
                nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId == currentProIds);
            }
            if (!version.IsFinish)
            {
                version.NextProcessId = nextProcess?.ProcessId;
                version.NextProcessIndex = nextProcess?.Index;
            }
            RF.Save(version);
        }

        /// <summary>
        /// 验证运行时产品
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <returns>运行时产品</returns>
        product ValidationProduct(string batchNo)
        {
            var product = RT.Service.Resolve<RuntimeController>().FindProduct(batchNo, BarcodeType.BatchBarocde);
            if (product == null)
                throw new ValidationException("未找到批次产品：{0} 的运行时数据".L10nFormat(batchNo));
            return product;
        }

        /// <summary>
        /// 保存产品工艺路线数据
        /// </summary>
        /// <param name="relation">批次关系</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        /// <param name="remark">备注</param>
        internal virtual void SaveProductRoutingInfo(BatchRelation relation, string oldLayout, string newLayout, string remark)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var wipProductRouting = RT.Service.Resolve<BatchWipProductRoutingController>().GetWipProductRouting(relation.Id);
                if (wipProductRouting == null)
                {
                    var layout = new BatchWipProductRoutingLayout() { Layout = newLayout };
                    RF.Save(layout);
                    wipProductRouting = new BatchWipProductRouting()
                    {
                        Relation = relation,
                        WorkOrderId = relation.WorkOrderId,
                        Layout = layout,
                    };
                    RF.Save(wipProductRouting);
                }
                else
                {
                    wipProductRouting.Layout.Layout = newLayout;
                    RF.Save(wipProductRouting.Layout);
                }

                var wipProductRoutingEvent = new BatchWipProductRoutingEvent()
                {
                    Routing = wipProductRouting,
                    OldLayout = oldLayout,
                    NewLayout = newLayout,
                    ChangeDate = DateTime.Now,
                    ChangeUserId = RT.IdentityId,
                    Remark = remark.L10N(),
                };
                RF.Save(wipProductRoutingEvent);
                tran.Complete();
            }
        }

        /// <summary>
        /// 验证产品生产批次
        /// </summary>
        /// <param name="relationId">批次关系Id</param>
        /// <param name="isPause">是否暂停</param>
        /// <returns>产品生产版本</returns>
        BatchRelation ValidationBatchRelation(double relationId, YesNo isPause = YesNo.No)
        {
            var relation = GetById<BatchRelation>(relationId);
            if (relation == null)
                throw new EntityNotFoundException(typeof(BatchRelation), relationId);
            if (relation.IsFinish)
                throw new ValidationException("批次：{0} 已完工下线".L10nFormat(relation.Bid));
            if (relation.IsPause == isPause)
            {
                if (isPause == YesNo.No)
                    throw new ValidationException("批次：{0} 不是暂停状态".L10nFormat(relation.Bid));
                else
                    throw new ValidationException("批次：{0} 已经是暂停状态".L10nFormat(relation.Bid));
            }

            return relation;
        }

        /// <summary>
        /// 获取批次运行时工艺路线布局
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>批次运行时工艺路线布局，找不到返回null</returns>
        public virtual BatchWipRTProductRoutingLayout GetBatchRuntimeRoutingLayout(string batchNo, double workOrderId)
        {
            return GetBatchRuntimeRouting(batchNo, workOrderId)?.Layout;
        }

        /// <summary>
        /// 获取批次运行时工艺路线
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <exception cref="ValidationException">批次号不能为空</exception>
        /// <returns>批次运行时工艺路线,找不到返回null</returns>
        public virtual BatchWipRTProductRouting GetBatchRuntimeRouting(string batchNo, double workOrderId)
        {
            if (batchNo.IsNullOrEmpty())
                throw new ValidationException("批次号不能为空".L10N());
            return Query<BatchWipRTProductRouting>().Where(p => p.BatchNo == batchNo && p.WorkOrderId == workOrderId).FirstOrDefault();
        }

        /// <summary>
        /// 创建批次运行时工艺路线
        /// 存在工艺路线则更新布局，不存在新增
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="strLayout">工艺路线布局</param>
        public virtual void CreateBatchRuntimeRouting(string batchNo, double workOrderId, string strLayout)
        {
            var routing = GetBatchRuntimeRouting(batchNo, workOrderId);
            if (routing != null)
            {
                var oldLayout = routing.Layout;
                oldLayout.Layout = strLayout;
                RF.Save(oldLayout);
                return;
            }

            var layout = new BatchWipRTProductRoutingLayout() { Layout = strLayout };
            layout.GenerateId();
            routing = new BatchWipRTProductRouting()
            {
                BatchNo = batchNo,
                WorkOrderId = workOrderId,
                Layout = layout
            };
            RF.Save(layout);
            RF.Save(routing);
        }
    }
}