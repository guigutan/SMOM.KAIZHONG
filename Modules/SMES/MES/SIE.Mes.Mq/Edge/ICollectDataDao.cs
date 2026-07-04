using SIE.MES.WIP.Products;

namespace SIE.Mes.Mq.Edge
{
    /// <summary>
    /// 采集Dao接口
    /// </summary>
    public interface ICollectDataDao
    {
        /// <summary>
        /// 保存采集数据到数据库
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="version"></param>
        /// <param name="pd"></param>
        void SaveCollectData(WipProductProcess pp, WipProductVersion version, WipProduct pd);


        /// <summary>
        /// 获取产品生产版本
        /// </summary>
        /// <param name="puid">产品ID</param>
        /// <returns>产品生产版本</returns>
        WipProductVersion GetWipProductVersion(string puid);


        /// <summary>
        /// 创建产品在制信息
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="pd"></param>
        void SaveWipProductVersion(WipProductVersion pv, WipProduct pd);


        /// <summary>
        /// 取员工的登录用户ID
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        double? GetUserId(double employeeId);

        /// <summary>
        /// 更新物料标签
        /// </summary>
        /// <param name="decreaseQty"></param>
        /// <param name="label"></param>
        void UpdateItemLabel(decimal decreaseQty, string label);

        /// <summary>
        /// 更新工单状态
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="state">状态</param>
        void UpdateWorkOrderState(double woId, Core.WorkOrders.WorkOrderState state);

        /// <summary>
        /// 更新工单完工数
        /// </summary>
        /// <param name="woId">标签号</param>
        /// <param name="qty">完工数量</param>
        void UpdateWorkOrderQty(double woId,decimal qty);
    }
}
