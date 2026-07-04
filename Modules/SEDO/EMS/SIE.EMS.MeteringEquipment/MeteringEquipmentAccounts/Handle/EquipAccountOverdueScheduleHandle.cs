using SIE.Core.Enums;
using SIE.Domain;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Handle
{
    /// <summary>
    /// 自动更新计量设备台账超期停用
    /// </summary>
    [Services.Service(FallbackType = typeof(EquipAccountOverdueScheduleHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class EquipAccountOverdueScheduleHandle
    {
        /// <summary>
        /// 计量设备台账控制器
        /// </summary>
        private MeteringEquipmentAccountController MeterEquipmentConter { get; set; }

        #region 定义

        /// <summary>
        /// 保存的数据实体
        /// </summary>

        private EntityList<MeteringEquipmentAccount> MeteringEquipmentAccountList { get; set; }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipAccountOverdueScheduleHandle()
        {
            MeterEquipmentConter = RT.Service.Resolve<MeteringEquipmentAccountController>();
            MeteringEquipmentAccountList = new EntityList<MeteringEquipmentAccount>();
        }

        /// <summary>
        ///  入口
        /// </summary>
        public virtual void DoSchedule()
        {
            //加载基础数据
            LoadBase();
            //生成实体数据
            InitDate();
            //保存
            SaveDate();
        }

        /// <summary>
        /// 加载基础数据
        /// </summary>
        public virtual void LoadBase()
        {
            MeteringEquipmentAccountList = MeterEquipmentConter.GetMeteringEquipmentAccountList();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void InitDate()
        {
            foreach (var account in MeteringEquipmentAccountList)
            {
                account.UseState = AccountUseState.Overdue;
            }
        }

        /// <summary>
        /// 保存数据d
        /// </summary>
        public virtual void SaveDate()
        {
            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                if (MeteringEquipmentAccountList.Count > 0)
                {
                    RF.Save(MeteringEquipmentAccountList);
                }
                tran.Complete();
            }
        }
    }
}
