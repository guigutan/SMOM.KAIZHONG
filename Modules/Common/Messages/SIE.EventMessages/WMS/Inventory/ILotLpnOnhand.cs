using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 更新库存操作
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultILotLpnOnhandInterface))]
    public interface ILotLpnOnhand
    {
        /// <summary>
        /// MES上料、下料、倒扣、接收物料、半成品入库线边仓、更新库存数据
        /// </summary>
        /// <param name="mesUpdateOnhandData"></param>
        void MesUpdateOnhand(MesUpdateOnhandData mesUpdateOnhandData);

        /// <summary>
        /// MES挪料更新库存（工单不变）只更新仓库 库位信息 不换工单
        /// </summary>
        /// <param name="mesMoveUpdateOnhandData"></param>
        void MesMoveUpdateOnhand(MesMoveUpdateOnhandData mesMoveUpdateOnhandData);

        /// <summary>
        /// LES线边仓收货，触发发运单发货，接收到线边仓写入库存
        /// </summary>
        /// <param name="lesReciveInUpdateOnhands"></param>
        void LesReciveInUpdateOnhand(List<LesReciveInUpdateOnhandData> lesReciveInUpdateOnhands);
    }

    /// <summary>
    /// 更新库存操作
    /// </summary>
    class DefaultILotLpnOnhandInterface : ILotLpnOnhand
    {
        public void MesUpdateOnhand(MesUpdateOnhandData mesUpdateOnhandData)
        {
            //默认实现
        }

        public void MesMoveUpdateOnhand(MesMoveUpdateOnhandData mesMoveUpdateOnhandData)
        {
            //默认实现
        }

        public void LesReciveInUpdateOnhand(List<LesReciveInUpdateOnhandData> lesReciveInUpdateOnhands)
        {
            //默认实现
        }
    }
}
