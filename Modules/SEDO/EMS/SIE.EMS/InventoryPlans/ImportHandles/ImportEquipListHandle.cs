using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Tech.Stations;
using SIE.Tech;
using SIE.Tech.Stations.ImportStationItem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Equipments.EquipAccounts;
using Irony.Parsing;
using SIE.Domain.Validation;
using SIE.Core.Enums;

namespace SIE.EMS.InventoryPlans.ImportHandles
{
    /// <summary>
    /// 设备清单导入处理类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipListHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipListHandle : IDisposable, IBusinessImport
    {

        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "设备编码", "设备名称" };

        #region 私有属性
        /// <summary>
        ///设备台账"设备台账编码"-"设备台账"
        /// </summary>
        private Dictionary<string, EquipAccount> equipAccountDic = new Dictionary<string, EquipAccount>();

        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get
            {
                return columnNameList;
            }

            set
            {
                columnNameList = value;
            }
        }

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <returns>IBusinessImport</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { "设备编码", new ValidColumn(ImportDataType._Custom, true, ValidEquipAccount) },
                { "设备名称", new ValidColumn(ImportDataType._String, true, 80) },
            };

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            equipAccountDic.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0) return;
            //获取主表ID
            var tempId = drs[0][ImportDataHandle.ParentId].ToString().Trim();
            if (string.IsNullOrEmpty(tempId)) return;
            var inventoryPlanId = double.Parse(tempId);

            var inventoryPlan = RF.GetById<InventoryPlan>(inventoryPlanId);
            if (inventoryPlan == null)
            {
                throw new ValidationException("请先保存主表数据".L10N());
            }
            var equipments = new EntityList<EquipmentList>();
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var mainDataItem in drs)
                {
                    var equipCode = mainDataItem.Field<string>(ColIndex("设备编码"));
                    var equipName = mainDataItem.Field<string>(ColIndex("设备名称"));


                    if (inventoryPlan.FactoryId != equipAccountDic[equipCode].FactoryId)
                    {
                        mainDataItem[ImportDataHandle.MessageColumnName] = "导入的设备清单的设备台账编码【{0}】所属与主表工厂不一致，请查看".FormatArgs(equipCode);
                        continue;
                    }
                    //如果不能新增记录错误信息
                    try
                    {
                        var account = equipAccountDic[equipCode];
                        var equipment = new EquipmentList();
                        equipment.InventoryPlanId = inventoryPlanId;
                        equipment.EquipAccoutId = account.Id;
                        equipment.EquipModelId = account.EquipModelId;
                        equipment.EquipTypeId = account.EquipModel.EquipTypeId;
                        equipment.UseDeptId = account.UseDepartmentId;
                        equipment.WorkShopId = account.WorkShopId;
                        equipments.Add(equipment);

                    }
                    catch (Exception exc)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                        mainDataItem[ImportDataHandle.MessageColumnName] = mainDataItem[ImportDataHandle.MessageColumnName] + strMsg;
                        continue;
                    }

                }

                DB.Delete<EquipmentList>().Where(p => p.InventoryPlanId == inventoryPlanId).Execute();
                RF.Save(equipments);
                tran.Complete();
            }
        }


        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }

        #region 基础验证
        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="obj">产品编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidEquipAccount(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (equipAccountDic == null)
            {
                equipAccountDic = new Dictionary<string, EquipAccount>();
            }

            if (!equipAccountDic.ContainsKey(obj.ToString()))
            {
                var equipAccout = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCodeNoAuth(obj.ToString());
                if (equipAccout != null)
                {
                    equipAccountDic.Add(obj.ToString(), equipAccout);
                    if (equipAccout.UseState == AccountUseState.ToAccepted || equipAccout.UseState == AccountUseState.Scrap || equipAccout.UseState == AccountUseState.DisposedOf)
                    {
                        messageTip = "【{0}】设备管理状态为【{1}】时，无法导入".FormatArgs(obj.ToString(),equipAccout.UseState.ToLabel());
                        isValid = false;
                    }
                }
                else
                {
                    messageTip = "设备不存在于系统".L10N();
                    isValid = false;
                }
            }
            else
            {
                messageTip = "重复的设备编码【{0}】不做导入".FormatArgs(obj.ToString());
                isValid = false;
            }
           

            return isValid;
        }
        #endregion      

    }
}
