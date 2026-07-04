using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 周转工具控制器
    /// </summary>
    public partial class KitTurnoverToolController : DomainController
    {
        #region 周转工具
        /// <summary>
        /// 修改周转工具状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="state"><see cref="TurnoverToolState"/>周转工具状态</param>
        public virtual void ChangeState(double id, TurnoverToolState state)
        {
            var model = RF.GetById<TurnoverTool>(id);
            model.State = state;
            RF.Save(model);
        }

        /// <summary>
        /// 获取周转工具
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>周转工具</returns>
        public virtual TurnoverTool GetTurnoverTool(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ValidationException("周转工具编码不能为空".L10N());
            }

            return Query<TurnoverTool>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取周转工具
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual TurnoverTool GetTurnoverTool(Expression<Func<TurnoverTool, bool>> exp)
        {
            var query = Query<TurnoverTool>();
            if (exp != null)
            {
                query.Where(exp);
            }

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取周转工具列表
        /// </summary>
        /// <param name="codes">编码列表</param>
        /// <returns>周转工具列表</returns>
        public virtual EntityList<TurnoverTool> GetTurnoverToolByCodes(List<string> codes)
        {
            return codes.SplitContains(code => Query<TurnoverTool>().Where(p => code.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }


        /// <summary>
        /// 编码是否是周转工具编码
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>是返回true，否则返回false</returns>
        public virtual bool IsTurnoverTool(string code)
        {
            return Query<TurnoverTool>().Where(p => p.Code == code).Count() > 0;
        }

        /// <summary>
        /// 编码是否是周转工具编码
        /// </summary>
        /// <param name="codes">编码列表</param>
        /// <returns>是返回true，否则返回false</returns>
        public virtual List<string> GetTurnoverToolCodes(List<string> codes)
        {
            var tools = Query<TurnoverTool>().Where(p => codes.Contains(p.Code)).ToList();
            return tools.Select(p => p.Code).Distinct().ToList();
        }

        /// <summary>
        /// 判断周转工具是否在周转工具类型范围内
        /// </summary>
        /// <param name="toolCode">周转工具编码</param>
        /// <param name="toolTypes">周转工具类型范围</param>
        /// <returns>是返回true，否则返回false</returns>
        public virtual bool IsTurnoverToolInToolTypes(string toolCode, List<string> toolTypes)
        {
            return Query<TurnoverTool>().Where(p => p.Code == toolCode && toolTypes.Contains(p.ToolType)).Count() > 0;
        }

        /// <summary>
        /// 验证周转工具是否可删除
        /// </summary>
        /// <param name="ids">周转工具ID列表</param>
        public virtual void CheckTurnoverToolDeleteState(List<double> ids)
        {
            string msg = string.Empty;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.IsNeedCount = true;

            StringBuilder sbMessage = new StringBuilder();
            foreach (var id in ids)
            {
                var turnoverTool = GetById<TurnoverTool>(id);
                if (turnoverTool.State != TurnoverToolState.Unused)
                {
                    sbMessage.Append("周转工具：【{0}】状态非【闲置】，不允许删除！".L10nFormat(turnoverTool.Code));
                    continue;
                }
                var bind = GetTurnoverTools(id, true, pagingInfo, null);
                if (bind.Count > 0)
                    sbMessage.Append("周转工具：【{0}】有绑定历史记录，不允许删除！".L10nFormat(turnoverTool.Code));
            }

            if (!msg.IsNullOrEmpty())
            {
                throw new ValidationException(sbMessage.ToString());
            }
        }

        /// <summary>
        /// 获取周转工具默认容量
        /// </summary>
        /// <param name="code">工具编码</param>
        /// <returns>返回周转工具默认容量，找不到周转工具返回null</returns>
        public virtual int? GetTurnoverToolDefaultCapacity(string code)
        {
            return Query<TurnoverToolModel>()
                .Join<TurnoverTool>((m, t) => m.Id == t.ModelId && t.Code == code)
                .FirstOrDefault()?.DefaultCapacity;
        }

        /// <summary>
        /// 目检采集获取周转工具容量
        /// </summary>
        /// <param name="code">工具编码</param>
        /// <returns>周转工具容量</returns>
        public virtual int? GetTurnoverToolCapacity(string code)
        {
            var tool = GetTurnoverTool(code);

            if (tool == null)
            {
                throw new ValidationException("周转工具[{0}]不存在".L10nFormat(code));
            }

            //1、周转工具未绑定条码，返回空 
            var bindingRecord = GetTurnoverToolBinding(p => p.TurnoverTool.Code == code && !p.IsUnbinding);

            if (bindingRecord == null)
                return null;

            //2、绑定产品，取周转工具的型号对应产品的容量，不存在特定产品关系，取型号默认容量
            var modelInProduct = Query<TurnoverToolModelInProduct>()
                .Join<TurnoverToolModel>((p, m) => p.ModelId == m.Id && p.ProductId == bindingRecord.ItemId)
                .Join<TurnoverToolModel, TurnoverTool>((m, t) => m.Id == t.ModelId && t.Code == code)
                .FirstOrDefault();

            if (modelInProduct != null)
            {
                return modelInProduct.Capacity;
            }
            else
            {
                return tool.ModelCapacity;
            }
        }

        /// <summary>
        /// 周转工具状态变更：回收\维修\报废
        /// </summary>
        /// <param name="TurnoverToolId">周转工具ID</param>
        /// <param name="state">周转工治具状态</param>
        /// <param name="action">周转工具操作</param>
        public virtual void TurnoverToolStateAction(double TurnoverToolId, TurnoverToolState state, TurnoverToolAction action)
        {
            using (var tran = DB.TransactionScope(MesEntityDataProvider.ConnectionStringName))
            {
                ChangeState(TurnoverToolId, state);
                SaveActionLog(TurnoverToolId, action, null);
                tran.Complete();
            }
        }
        #endregion

        #region 周转工具型号  
        /// <summary>
        /// 获取周转工具型号列表
        /// </summary>
        /// <returns>周转工具列表</returns>
        public virtual Dictionary<double, string> GetTurnoverToolModelCodes()
        {
            return Query<TurnoverToolModel>().Select(c => new { c.Id, c.Code }).ToList()
                .ToDictionary(c => c.Id, c => c.Code);
        }

        /// <summary>
        /// 设置周转工具型号专用容器
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isDedicated"></param>
        public virtual void SetTurnoverToolModelDedicated(double[] ids, bool isDedicated)
        {
            if (isDedicated)
            {
                foreach (var item in ids)
                {
                    var model = GetById<TurnoverToolModel>(item);
                    if (model == null)
                        throw new ValidationException("选中的数据中有未保存数据无法设为专用容器！".L10N());
                    if (model.ProductList.Count == 0)
                        throw new ValidationException("周转工具型号[{0}]未维护产品容量不能设为专用容器！".L10nFormat(model.Code + "[" + model.Name + "]"));
                }
            }
            DB.Update<TurnoverToolModel>()
                .Set(p => p.IsDedicated, isDedicated)
                .Where(p => ids.Contains(p.Id))
                .Execute();

        }

        /// <summary>
        /// 获取周转工具型号下对应产品
        /// </summary>
        /// <param name="turnoverToolModelId">周转工具型号ID</param>
        /// /// <param name="productId">产品ID</param>
        /// <returns>周转工具型号产品明细记录</returns>
        public virtual TurnoverToolModelInProduct GetTurnoverToolModelInProduct(double turnoverToolModelId, double productId)
        {
            return Query<TurnoverToolModelInProduct>().Where(p => p.ModelId == turnoverToolModelId && p.ProductId == productId)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过周转工具类型获取周转工具型号
        /// </summary>
        /// <param name="toolType">周转工具类型</param>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>周转工具列表</returns>
        public virtual EntityList<TurnoverToolModel> GetTurnoverToolModels(string toolType, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<TurnoverToolModel>();
            if (!toolType.IsNullOrEmpty())
                query = query.Where(p => p.ToolType == toolType);
            if (!keyword.IsNullOrEmpty())
                query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 周转工具绑定
        /// <summary>
        /// 获取周转工具绑定明细列表
        /// </summary>
        /// <param name="toolId">周转工具ID</param>
        /// <param name="isUnbinding">是否已解绑</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <param name="orderInfos">排序条件</param>
        /// <returns>绑定明细列表</returns>
        public virtual EntityList<TurnoverToolBinding> GetTurnoverTools(double toolId, bool? isUnbinding, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var query = Query<TurnoverToolBinding>()
                .Where(p => p.TurnoverToolId == toolId);
            if (isUnbinding != null)
                query.Where(p => p.IsUnbinding == isUnbinding);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据条码号列表获取绑定明细列表
        /// </summary>
        /// <param name="sns">条码号列表</param>
        /// <returns>绑定明细列表</returns>
        public virtual EntityList<TurnoverToolBinding> GetTurnoverToolBindingsBySns(List<string> sns)
        {
            return Query<TurnoverToolBinding>()
                .Where(p => !p.IsUnbinding && sns.Contains(p.Sn))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取周转工具绑定明细列表
        /// </summary>
        /// <param name="toolIds">周转工具ID列表</param>>
        /// <returns>绑定明细列表</returns>
        public virtual EntityList<TurnoverToolBinding> GetTurnoverToolBindings(List<double> toolIds)
        {
            return Query<TurnoverToolBinding>()
                .Where(p => toolIds.Contains(p.TurnoverToolId) && !p.IsUnbinding)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取一条绑定数据
        /// </summary>
        /// <param name="exp">查询表达式</param>
        /// <returns></returns>
        public virtual TurnoverToolBinding GetTurnoverToolBinding(Expression<Func<TurnoverToolBinding, bool>> exp)
        {
            var query = Query<TurnoverToolBinding>();
            if (exp != null)
                query.Where(exp);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品条码获取绑定数据信息
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <param name="IsUnbinding">是否解绑</param>
        /// <returns></returns>
        public virtual TurnoverToolBinding GetTurnoverToolBindingBySn(string sn, bool IsUnbinding = false)
        {
            return GetTurnoverToolBinding(c => c.Sn == sn && c.IsUnbinding == IsUnbinding);
        }

        /// <summary>
        /// 根据载具id获取绑定的数量
        /// </summary>
        /// <param name="turnoverToolId"></param>
        /// <returns>载具id</returns>
        public virtual int GetToolBindingNum(double turnoverToolId)
        {
            return Query<TurnoverToolBinding>().Where(c => c.TurnoverToolId == turnoverToolId && !c.IsUnbinding).Count();
        }

        /// <summary>
        /// 根据载具编码获取绑定SN的数量
        /// </summary>
        /// <param name="code">周转工具编码</param>
        /// <returns>绑定SN的数量</returns>
        public virtual int GetToolBindingNum(string code)
        {
            return Query<TurnoverToolBinding>()
                .Join<TurnoverTool>((b, t) => b.TurnoverToolId == t.Id && t.Code == code)
                .Where(c => !c.IsUnbinding)
                .Count();
        }

        /// <summary>
        /// 根据产品条码获取绑定的数量
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <returns></returns>
        public virtual int GetToolBindingNumBySn(string sn)
        {
            return Query<TurnoverToolBinding>().Where(c => c.Sn == sn && !c.IsUnbinding).Count();
        }


        /// <summary>
        /// 判断产品是否已经绑定指定周转工具
        /// </summary>
        /// <param name="turnoverTool">周转工具</param>
        /// <param name="sn">产品条码</param>
        /// <returns>已绑定返回true，否则返回false</returns>
        public virtual bool IsBindingTurnoverTool(string turnoverTool, string sn)
        {
            return Query<TurnoverToolBinding>()
                .Join<TurnoverTool>((b, t) => b.TurnoverToolId == t.Id && t.Code == turnoverTool)
                .Where(p => p.Sn == sn && !p.IsUnbinding)
                .Count() > 0;
        }

        /// <summary>
        /// 设置周转工具绑定-是否绑定完成
        /// </summary>
        /// <param name="turnoverToolId">周转工具id</param>
        /// <param name="isBindFinish">是否绑定完成</param>
        public virtual void SetBindFinish(double turnoverToolId, bool isBindFinish)
        {
            var binds = GetTurnoverTools(turnoverToolId, false, null, null);
            binds.ForEach(p => p.IsBindFinish = isBindFinish);
            RF.Save(binds);
        }


        #endregion

        #region 操作日志
        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="TurnoverToolId">工具id</param>
        /// <param name="state">操作状态</param>
        /// <param name="binding">绑定明细</param>
        public virtual void SaveActionLog(double TurnoverToolId, TurnoverToolAction state, TurnoverToolBinding binding)
        {
            var model = new TurnoverToolActionLog();
            model.TurnoverToolId = TurnoverToolId;
            model.TurnoverToolAction = state;
            if (binding != null)
            {
                model.Sn = binding.Sn;
                model.Qty = binding.Qty;
                model.ItemId = binding.ItemId;
                model.WorkOrderId = binding.WorkOrderId;
            }
            RF.Save(model);
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="toolId">周转工具</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="orderInfos">排序</param>
        /// <returns></returns>
        public virtual EntityList<TurnoverToolActionLog> GetTurnoverToolActionLogs(double toolId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var query = Query<TurnoverToolActionLog>().Where(c => c.TurnoverToolId == toolId);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion
    }
}