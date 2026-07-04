using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.EMS.SpareParts.ImportSparePart
{
    /// <summary>
    /// 导入类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportSparePartHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportSparePartHandle : IBusinessImport
    {
        /// <summary>
        /// 备件类型编码
        /// </summary>
        private const string SparePartTypeString = "备件类型编码";
        #region 私有属性       

        /// <summary>
        /// 备件资料--已存在则会被记录
        /// </summary>
        private readonly Dictionary<string, SparePart> SparePartCodeDic = new Dictionary<string, SparePart>();
        #endregion

        /// <summary>
        /// 导入模板的列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
             "编码", "名称", "规格", SparePartTypeString, "是否序列号管控", "原厂料号", "是否以旧换新", "单位", "制造商", "供应商",
             "安全库存", "存放仓库", "库位", "更换周期(天)", "可用时间(小时)", "单价(含税)"
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
            this.ColumnValidList.Add("编码", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("名称", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("规格", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add(SparePartTypeString, new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("是否序列号管控", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("原厂料号", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("是否以旧换新", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("单位", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("制造商", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("供应商", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("安全库存", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("存放仓库", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("库位", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("更换周期(天)", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("可用时间(小时)", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("单价(含税)", new ValidColumn(ImportDataType._String, true, true));
            return this;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length <= 0) return;

            var sparePareItemSmallCategoryDictionary = new Dictionary<string,double>();

            var list = RT.Service.Resolve<ItemController>()
                .GetItemSmallCategory(SIE.Items.Items.CategoryType.Item, ItemType.SparePart, String.Empty, null);

            list.ForEach(p =>
            {
                if (!sparePareItemSmallCategoryDictionary.ContainsKey(p.Name))
                {
                    sparePareItemSmallCategoryDictionary.Add(p.Name, p.Id);
                }
            });

            //循环检验每一行主数据
            foreach (var dr in drs)
            {
                SparePart SparePart = null;

                if (SparePartCodeDic.Count > 0 && SparePartCodeDic.ContainsKey(dr["编码"].ToString()))
                {
                    SparePart = SparePartCodeDic[dr["编码"].ToString()];
                }

                if (SparePart == null)
                {
                    SparePart = new SparePart();                    
                    SparePart.SparePartCode = dr["编码"].ToString();
                    SparePart.SparePartName = dr["名称"].ToString();
                    SparePart.Specification = dr["规格"].ToString();
                    if (sparePareItemSmallCategoryDictionary.ContainsKey(dr[SparePartTypeString].ToString()))
                    {
                        SparePart.ItemCategoryId = sparePareItemSmallCategoryDictionary[dr[SparePartTypeString].ToString()];
                    }                   

                    SparePart.IsReplacement = false;
                    
                    SparePart.UnitId = 7;
                    SparePart.State = State.Disable;
                    SparePart.CreateBy = RT.Identity.Id;
                    SparePart.CreateDate = DateTime.Now;

                    try
                    {
                        RF.Save(SparePart);
                    }
                    catch (Exception ex)
                    {
                        string strMsg = ex.GetBaseException()?.Message;
                        ImportExtension.BatchAppendText(null, ImportDataHandle.MessageColumnName, strMsg);
                    }
                }
            }
        }
    }
}
