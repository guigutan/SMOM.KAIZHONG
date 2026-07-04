using SIE.Core.Enums;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.Inventory.Transactions;
using SIE.Web.Inventory.Transactions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Inventory.Transactions.DataQueryer
{
    /// <summary>
    /// 查询
    /// </summary>
    public class FunctionDataQuery : SIE.Web.Data.DataQueryer
    {
        /// <summary>
        /// 保存编码规则
        /// </summary>
        /// <param name="functionId">大类Id</param>
        /// <param name="ruleId">编码规则Id</param>
        public void SaveFunctionRuleId(double functionId, double ruleId)
        {
            var function = RF.GetById<Function>(functionId);
            if (function != null)
            {
                function.NumberRuleId = ruleId;
                RF.Save(function);
            }
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="functionId">大类Id</param>
        /// <param name="tempId">模板Id</param>
        public void SaveBillTemplateId(double functionId, double tempId)
        {
            var function = RF.GetById<Function>(functionId);
            if (function != null)
            {
                function.BillTemplateId = tempId;
                RF.Save(function);
            }
        }

        /// <summary>
        /// 获取初始化单据大类集合
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <returns>单据大类集合</returns>
        public EntityList GetInitFunctions(string code, string name)
        {
            var result = new EntityList<FunctionModel>();
            var list = Enum.GetValues(typeof(OrderType)).Cast<OrderType>();
            if (!string.IsNullOrEmpty(code))
                list = list.Where(p => p.ToString().Contains(code));
            if (!string.IsNullOrEmpty(name))
                list = list.Where(p => p.ToLabel().Contains(name));
            foreach (OrderType item in list.ToList())
            {
                var model = new FunctionModel();
                model.Code = item.ToString().L10N();
                model.Name = item.ToLabel().L10N();
                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 获取默认单据小类
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public object GetTransaction(OrderType orderType)
        {
            TranData td = new TranData();

            var tr = RT.Service.Resolve<TransactionController>().GetDefaultTransactions(orderType);
            if (tr != null)
            {
                td.Id = tr.Id;
                td.Name = tr.Name;
            }
            return td;
        }

        [Serializable]
        public class TranData
        {
            public double Id { get; set; }

            public string Name { get; set; }

        }
    }
}
