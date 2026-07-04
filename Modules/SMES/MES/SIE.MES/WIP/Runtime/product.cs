using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using System;
using System.Linq;

namespace SIE.MES.WIP.Runtime
{
    /// <summary>
    /// 采集运行时产品模型, 记录产品在生产过程中的信息, 通过Puid产品全局ID关联生产信息
    /// </summary>
    [Serializable]
    public class product : ICloneable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public product()
        {
            Routing = new routing();
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public routing Routing { get; set; }

        /// <summary>
        /// 产品全局ID
        /// </summary>
        public string Puid { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 上线数量
        /// 空拼板上线数量为0，待绑定SN时赋值
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }

        /// <summary>
        /// 产品是否不良，维修后也是不良
        /// </summary>
        public bool IsNg { get; set; }

        /// <summary>
        /// 缺陷数量
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        WorkOrder workOrder;

        /// <summary>
        /// 工单属性
        /// </summary>
        [JsonIgnore]
        public WorkOrder WorkOrder
        {
            get { return workOrder ?? (workOrder = RF.GetById<WorkOrder>(WorkOrderId)); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        WorkOrderMove workOrderMove;

        /// <summary>
        /// 工单属性
        /// </summary>
        [JsonIgnore]
        public WorkOrderMove WorkOrderMove
        {
            get { return workOrderMove ?? (workOrderMove = RF.GetById<WorkOrderMove>(WorkOrderId)); }
        }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsHold { get; set; }

        /// <summary>
        /// 克隆方法
        /// </summary>
        /// <returns>返回克隆的采集产品模型</returns>
        public product Clone()
        {
            var json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<product>(json);
        }

        /// <summary>
        /// 克隆方法
        /// </summary>
        /// <returns>返回克隆的采集产品模型</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 获取过站记录状态
        /// </summary>
        /// <returns></returns>        
        public WipProductProcessState GetNextWipProductProcessState(double processId)
        {
            //启用入站(Move in)控制，且未执行入站（Move in）时，设置采集状态为开始（即入站（Move in）)
            process process = Routing.GetNext().FirstOrDefault(x => x.ProcessId == processId);

            if (process == null)
            {
                var nextProcess = Routing.GetNext().Select(p => p?.Name).Concat("、");
                throw new ValidationException("采集工序不正确，应该为[{0}]"
                    .L10nFormat(nextProcess));
            }

            if (process.EnableMoveInControl == true
                && process.WipProductProcessState != WipProductProcessState.Start)
            {
                return WipProductProcessState.Start;
            }
            else
            {
                return WipProductProcessState.Finish;
            }
        }
    }
}
