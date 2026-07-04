using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.EMS.Equipments.Boms.ImportEquipBom
{
    /// <summary>
    /// 导入类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipBomHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipBomHandle : IDisposable, IBusinessImport
    {
        private const string UP_LEVEL_SPARE_PART_LINE_NO = "上层备件行号";
        #region 私有属性

        /// <summary>
        /// 备件资料--已存在则会被记录
        /// </summary>
        private readonly Dictionary<string, EquipBom> EquipBomCodeDic = new Dictionary<string, EquipBom>();

        /// <summary>
        /// 设备型号缓存
        /// </summary>
        private readonly Dictionary<string, EquipModel> EquipModelCache = new Dictionary<string, EquipModel>();
        /// <summary>
        /// 导入明细
        /// </summary>
        private readonly List<EquipBomDetailTemp> equipBomDetailTempList = new List<EquipBomDetailTemp>();
        #endregion

        /// <summary>
        /// 导入模板的列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "行号*",
            "设备型号编码*",
            "备件编码*",
            "数量*",
            "部位",
            UP_LEVEL_SPARE_PART_LINE_NO
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入备件基础数据标准对象
        /// </summary>
        /// <returns>返回料号检验标准对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("行号*", new ValidColumn(ImportDataType._Int, true, true));
            this.ColumnValidList.Add("设备型号编码*", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("备件编码*", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("数量*", new ValidColumn(ImportDataType._Int, true, true));
            this.ColumnValidList.Add("部位", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add(UP_LEVEL_SPARE_PART_LINE_NO, new ValidColumn(ImportDataType._Int, false, true));
            return this;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length <= 0) return;

            //循环检验每一行主数据
            foreach (var dr in drs)
            {
                EquipBom equipBom = null;//主表数据
                var equipModleCode = dr["设备型号编码*"].ToString();
                if (equipModleCode.IsNullOrEmpty())
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "设备型号编码不允许空");
                    continue;
                }
                SIE.Equipments.EquipModels.EquipModel equipModel = null;

                if (!EquipModelCache.ContainsKey(equipModleCode))
                {
                    equipModel = RT.Service.Resolve<EquipModelController>().GetEquipModelByCode(equipModleCode);
                    EquipModelCache.Add(equipModleCode, equipModel);
                }
                else
                    equipModel = EquipModelCache[equipModleCode];
                if (equipModel == null)
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "设备型号系统不存在");
                    continue;
                }

                if (EquipBomCodeDic.Count > 0 && EquipBomCodeDic.ContainsKey(equipModleCode))
                {
                    equipBom = EquipBomCodeDic[equipModleCode];
                }
                if (equipBom == null)//主表数据
                {
                    var exsitedEquipBom = RT.Service.Resolve<EquipBomController>().GetEquipBomByModelId(equipModel.Id);
                    if (exsitedEquipBom == null)
                    {
                        equipBom = new EquipBom();
                        equipBom.GenerateId();
                        equipBom.EquipModelId = equipModel.Id;
                        equipBom.PersistenceStatus = PersistenceStatus.New;
                    }
                    else equipBom = exsitedEquipBom;
                    EquipBomCodeDic.Add(equipModleCode, equipBom);
                    var equipBomDetail = SetChilds(dr, equipBom);//1.判断是否使用数据ID和数据父ID 如存在则需要处理  2.记录关系 
                    if (equipBomDetail == null) continue;
                    equipBomDetailTempList.Add(equipBomDetail);


                }
                else
                {
                    var equipBomDetail = SetChilds(dr, equipBom);//1.判断是否使用数据ID和数据父ID 如存在则需要处理  2.记录关系 
                    if (equipBomDetail == null) continue;
                    equipBomDetailTempList.Add(equipBomDetail);
                }
            }

            //备件层次关系修正
            if (equipBomDetailTempList.Any())
            {
                var bomDetailTempListHasParent = equipBomDetailTempList.OrderBy(m => m.ExcleRowIndex).ThenBy(m => m.ExcleParentRowIndex).Where(m => m.ExcleParentRowIndex > 0);
                foreach (var bomDetail in bomDetailTempListHasParent)
                {
                    var pid = equipBomDetailTempList.FirstOrDefault(m => m.ExcleRowIndex == bomDetail.ExcleParentRowIndex && m.ExcleRowIndex != bomDetail.ExcleRowIndex && m.EqBomDetail.EquipBomId == bomDetail.EqBomDetail.EquipBomId);
                    if (pid != null)
                    {
                        bomDetail.EqBomDetail.SetTreePId(pid.EqBomDetail.Id);
                    }
                }

            }
            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {

                if (equipBomDetailTempList.Any())
                {
                    var newEquipBoms = EquipBomCodeDic.Values.Where(m => m.PersistenceStatus == PersistenceStatus.New).ToList();
                    var updateEquipBoms = EquipBomCodeDic.Values.Where(m => m.PersistenceStatus != PersistenceStatus.New).ToList();
                    if (updateEquipBoms.Any())//清除已有数据后重新添加
                    {
                        var deleteIds = updateEquipBoms.Select(m => m.Id).ToList();
                        RT.Service.Resolve<EquipBomController>().RemoveByEquipBomIds(deleteIds);
                    }

                    var saveBoms = new EntityList<EquipBom>();
                    var saveEquipBomDetails = new EntityList<EquipBomDetail>();
                    try
                    {
                        saveBoms.AddRange(newEquipBoms);
                        saveEquipBomDetails.AddRange(equipBomDetailTempList.Select(m => m.EqBomDetail).ToList());
                        if (saveBoms.Any())
                            RF.Save(saveBoms);
                        RF.Save(saveEquipBomDetails);
                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        string strMsg = ex.GetBaseException()?.Message;
                        throw new ValidationException(strMsg);
                    }
                }
            }
        }

        /// <summary>
        /// 设置子级数据
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="equipBom"></param>
        private EquipBomDetailTemp SetChilds(DataRow dr, EquipBom equipBom)
        {
            var code = dr["备件编码*"].ToString();
            if (code.IsNullOrEmpty())
            {
                ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "备件编码不允许空");
                return null;
            }
            var sparePart = RT.Service.Resolve<EquipBomController>().GetSparePart(code);
            if (sparePart == null)
            {
                ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "备件编码系统不存在");
                return null;
            }

            var partSite = dr["部位"].ToString();
            var num = dr["数量*"].ToString();

            if (!ValidInt(dr["数量*"].ToString(), out string messageTip))
            {
                ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, messageTip);
                return null;
            }
            var rowIndex = dr["行号*"].ToString();
            var message = "";
            if (rowIndex.IsNullOrEmpty() || !ValidInt(rowIndex, out message))
            {
                ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "行号必须是正整数");
                return null;
            }


            var equipBomDetail = new EquipBomDetail();
            equipBomDetail.GenerateId();
            equipBomDetail.EquipBomId = equipBom.Id;
            equipBomDetail.SparePartId = sparePart.Id;
            equipBomDetail.SparePartQty = int.Parse(num);
            equipBomDetail.SparePartSite = partSite;


            EquipBomDetailTemp equipBomDetailTemp = new EquipBomDetailTemp();
            equipBomDetailTemp.ExcleRowIndex = int.Parse(rowIndex);//行号
            equipBomDetailTemp.EqBomDetail = equipBomDetail;

            var parebtRowIndex = dr[UP_LEVEL_SPARE_PART_LINE_NO].ToString();
            if (!parebtRowIndex.IsNullOrEmpty())
            {
                if (!ValidInt(parebtRowIndex, out message))
                {
                    ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, UP_LEVEL_SPARE_PART_LINE_NO + message);
                    return null;
                }
                equipBomDetailTemp.ExcleParentRowIndex = int.Parse(parebtRowIndex);//父级行号
            }
            return equipBomDetailTemp;

        }

        /// <summary>
        /// 导入临时对象
        /// </summary>
        internal class EquipBomDetailTemp
        {
            /// <summary>
            /// 数据库对象
            /// </summary>
            internal EquipBomDetail EqBomDetail { get; set; }

            /// <summary>
            /// 行号
            /// </summary>
            internal int ExcleRowIndex { get; set; }

            /// <summary>
            /// 父级行号
            /// </summary>
            internal int ExcleParentRowIndex { get; set; }

        }



        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="qty">验证数量</param>
        /// <param name="messageTip">错误信息</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidInt(string qty, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (!string.IsNullOrEmpty(qty))
            {
                var reg = new Regex(@"^[0-9]\d*$");
                var result = reg.IsMatch(qty);
                if (!result)
                {
                    messageTip = "必须是正整数".L10N();
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
