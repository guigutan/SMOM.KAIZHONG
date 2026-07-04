using SIE.Kit.APS.FactoryConfirms;
using System;
using System.Collections.Generic;
namespace SIE.Web.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 智能分厂 基类
    /// </summary>
    public class DispatchBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lstData"></param>
        public DispatchBase(List<FactoryConfirmsViewModel> lstData)
        {
            this.LstData = lstData;
        }

        /// <summary>
        /// 分厂确认的数据（ 销售单行 ）
        /// </summary>
        protected List<FactoryConfirmsViewModel> LstData { get; set; }

        /// <summary>
        /// 数据初始化
        /// </summary>
        public virtual void DataInit()
        {
        }

        /// <summary>
        /// 分派完在怕刷新基础数据
        /// </summary>
        /// <param name="data">已分派的数据</param>

        public virtual void RefreshData(FactoryConfirmsViewModel data)
        {
        }

        /// <summary>
        /// 根据日期分组
        /// </summary>
        /// <param name="promiseDelivery">承诺交期</param>
        /// <param name="requireDelivery">客户交期</param>
        /// <returns></returns>
        protected DateTime GroupByDate(DateTime? promiseDelivery, DateTime requireDelivery)
        {
            if (promiseDelivery != null && promiseDelivery.Value > DateTime.MinValue)
                return promiseDelivery.Value;
            return requireDelivery;
        }

        /// <summary>
        /// 工厂分配
        /// </summary>
        /// <param name="data">销售单行</param>
        /// <param name="lstFo">可分配的工厂（库存组织）</param>
        /// <returns></returns>
        public virtual List<double> Dispatch(FactoryConfirmsViewModel data, List<double> lstFo)
        {
            return lstFo;
        }
    }
}

