using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.InventoryPlans.ImportHandles
{
    /// <summary>
    /// 备件清单导入处理类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportSpareListHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportSpareListHandle : IDisposable, IBusinessImport
    {

        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "备件编码", "备件名称" };

        #region 私有属性
        /// <summary>
        ///备件基础数据"备件编码"-"备件ID"
        /// </summary>
        private Dictionary<string, double> sparePartDic = new Dictionary<string, double>();

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
                { "备件编码", new ValidColumn(ImportDataType._Custom, true, ValidSparePart) },
                { "备件名称", new ValidColumn(ImportDataType._String, true, 80) },
            };

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            sparePartDic.Clear();
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
            var spareParts = new EntityList<SparePartList>();
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var mainDataItem in drs)
                {
                    var spareCode = mainDataItem.Field<string>(ColIndex("备件编码"));
                    var spareName = mainDataItem.Field<string>(ColIndex("备件名称"));
                
                    //如果不能新增记录错误信息
                    try
                    {
                        var data = sparePartDic[spareCode];
                        var spare = new SparePartList();
                        spare.InventoryPlanId = inventoryPlanId;
                        spare.SparePartId = sparePartDic[spareCode];
                        spareParts.Add(spare);
                    }
                    catch (Exception exc)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                        mainDataItem[ImportDataHandle.MessageColumnName] = mainDataItem[ImportDataHandle.MessageColumnName] + strMsg;
                        continue;
                    }

                }

                DB.Delete<SparePartList>().Where(p => p.InventoryPlanId == inventoryPlanId).Execute();
                RF.Save(spareParts);
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
        private bool ValidSparePart(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (sparePartDic == null)
            {
                sparePartDic = new Dictionary<string, double>();
            }

            if (!sparePartDic.ContainsKey(obj.ToString()))
            {
                var sparePart = RT.Service.Resolve<SparePartController>().GetSparePartByCode(obj.ToString());
                if (sparePart != null)
                {
                    sparePartDic.Add(obj.ToString(), sparePart.Id);
                }
                else
                {
                    messageTip = "备件不存在于系统".L10N();
                    isValid = false;
                }
            }

            return isValid;
        }
        #endregion      

    }
}
