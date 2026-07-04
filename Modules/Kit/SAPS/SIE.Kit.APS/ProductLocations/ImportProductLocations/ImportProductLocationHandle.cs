using SIE.APS;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Kit.APS.ImportInspection;
using SIE.Kit.APS.ProductLocations;
using SIE.SO.SaleOrders;
using SIE.Resources.Enterprises;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Kit.APS.ProductLocations.ImportProductLocations
{
    /// <summary>
    /// 导入产品定位 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportProductLocationHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportProductLocationHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "工厂编码", "分类", "分类值", "最小值", "最大值", "备注" };

        #region 私有属性

        /// <summary>
        /// 工厂
        /// </summary>
        private Dictionary<string, Enterprise> EnterpriseCodeDic = new Dictionary<string, Enterprise>();

        /// <summary>
        /// 分类值快码
        /// </summary>
        private Dictionary<string, string> TypeValueDic = null;

        /// <summary>
        /// 检验分类范围 字典
        /// </summary>
        private Dictionary<string, Enum> ClassificationDic = null;

        /// <summary>
        /// 检验参考值范围 字典
        /// </summary>
        private Dictionary<string, Enum> ReferenceValueDic = null;

        /// <summary>
        /// 行业类型 字典(转行)
        /// </summary>
        private Dictionary<string, string> IndustryKeyDic = null;
        /// <summary>
        /// 产品类型 字典(转行)
        /// </summary>
        private Dictionary<string, string> ProductKeyDic = null;
        /// <summary>
        /// 特殊工艺 字典(转行)
        /// </summary>
        private Dictionary<string, Enum> SpecialProcessKeyDic = null;

        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get { return columnNameList; }
            set { columnNameList = value; }
        }

        /// <summary>
        /// 列的标准验证(列名 列对应验证)
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
                { "工厂编码", new ValidColumn(ImportDataType._String, true, ValidEnterprise,true) },
                { "分类", new ValidColumn(ImportDataType._String, true,ValidClassification,true) },
                { "分类值", new ValidColumn(ImportDataType._String, true, ValidTypeValue,true) },
                { "最小值", new ValidColumn(ImportDataType._Double, false, 80) },
                { "最大值", new ValidColumn(ImportDataType._Double, false, 80) },
                { "备注", new ValidColumn(ImportDataType._String, false,true) },
            };
            return this;
        }

        #region 基础验证

        /// <summary>
        /// 验证工厂
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns></returns>
        private bool ValidEnterprise(object obj, out string messageTip, DataRow dr)
        {
            return BussinessDataValid.ValidEnterprise(ref EnterpriseCodeDic, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="obj">数量</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidQty(object obj, out string messageTip, DataRow dr)
        {
            return BussinessDataValid.ValidInt(obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证分类值
        /// </summary>
        /// <param name="obj">验证订单名称</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidTypeValue(object obj, out string messageTip, DataRow dr)
        {
            Classification c = (Classification)EnumViewModel.LabelToEnum(dr.Field<string>(ColIndex("分类")).Trim(), typeof(Classification));
            switch (c)
            {
                case Classification.Industry:
                    return BussinessDataValid.ValidCategory(ref IndustryKeyDic, obj.ToString(), out messageTip, SaleOrderDetail.INDUSTRYTYPE);
                case Classification.Product:
                    return BussinessDataValid.ValidCategory(ref ProductKeyDic, obj.ToString(), out messageTip, SaleOrderDetail.PRODUCTTYPE);
                //case Classification.SpecialProcess:
                //    return BussinessDataValid.ValidProcess(ref SpecialProcessKeyDic, obj.ToString(), out messageTip);
                default:
                    messageTip = "非系统类型".L10N();
                    return false;
            }
        }

        /// <summary>
        /// 验证分类
        /// </summary>
        /// <param name="obj">验证分类名称</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidClassification(object obj, out string messageTip, DataRow dr)
        {
            return BussinessDataValid.ValidClassification(ref ClassificationDic, obj.ToString(), out messageTip);
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            IndustryKeyDic.Clear();
            ProductKeyDic.Clear();
            SpecialProcessKeyDic.Clear();
            EnterpriseCodeDic.Clear();
            TypeValueDic.Clear();
            ClassificationDic.Clear();
            ReferenceValueDic.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            // 循环检验每一行主数据
            var ctrl = RT.Service.Resolve<ProductLocationController>();
            foreach (var dataItem in drs)
            {
                ProductLocation product = InitProductLocation(dataItem);
                using (var tran = DB.AutonomousTransactionScope(ApsCoreEntityDataProvider.ConnectionStringName))
                {
                    try
                    {
                        RF.Save(product);
                    }
                    catch (Exception exc)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                        dataItem[ImportDataHandle.MessageColumnName] = dataItem[ImportDataHandle.MessageColumnName] + strMsg;
                        continue;
                    }
                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 产品定位初始化
        /// </summary>
        /// <param name="dataItem">产品定位数据行</param>
        /// <returns>产品定位</returns>
        private ProductLocation InitProductLocation(DataRow dataItem)
        {
            var EnterpriseCode = dataItem.Field<string>(ColIndex("工厂编码"));
            var classification = dataItem.Field<string>(ColIndex("分类"));
            var typevalue = dataItem.Field<string>(ColIndex("分类值"));
            var minvalue = dataItem.Field<string>(ColIndex("最小值"));
            var maxvalue = dataItem.Field<string>(ColIndex("最大值"));
            var remark = dataItem.Field<string>(ColIndex("备注"));
            //库存组织(企业)
            Enterprise enterprise = GetEnterpriseCode(EnterpriseCode);
            var PL = new ProductLocation();
            PL.Enterprise = enterprise;
            PL.Classification = (Classification)ClassificationDic[classification];
            switch (PL.Classification)
            {
                case Classification.Industry:
                    PL.TypeValue = GetIndustryName(typevalue);
                    break;
                case Classification.Product:
                    PL.TypeValue = GetProductName(typevalue);
                    break;
                //case Classification.SpecialProcess:
                //    PL.TypeValue = GetSpecialProcessName(typevalue).ToString();
                //    break;
                default:
                    PL.TypeValue = typevalue;
                    break;
            }
            if (!string.IsNullOrWhiteSpace(minvalue))
            {
                PL.MinValue = Convert.ToDecimal(minvalue);
            }
            if (!string.IsNullOrWhiteSpace(maxvalue))
            {
                PL.MaxValue = Convert.ToDecimal(maxvalue);
            }
            PL.Remark = remark;
            return PL;
        }

        #region 引用类型的属性从字典中取值
        /// <summary>
        /// 根据工厂编码取值
        /// </summary>
        /// <param name="key">物料编码</param>
        /// <returns>返回物料</returns>
        private Enterprise GetEnterpriseCode(string key)
        {
            if (EnterpriseCodeDic.ContainsKey(key))
                return EnterpriseCodeDic[key];
            return null;
        }

        /// <summary>
        /// 根据行业类型名称取值
        /// </summary>
        /// <param name="key">行业类型</param>
        /// <returns>返回行业类型</returns>
        private String GetIndustryName(string key)
        {
            if (IndustryKeyDic.ContainsKey(key))
                return IndustryKeyDic[key];
            return null;
        }
        /// <summary>
        /// 根据产品类型名称取值
        /// </summary>
        /// <param name="key">物料编码</param>
        /// <returns>返回物料</returns>
        private String GetProductName(string key)
        {
            if (ProductKeyDic.ContainsKey(key))
                return ProductKeyDic[key];
            return null;
        }
        /// <summary>
        /// 根据特殊工艺名称取值
        /// </summary>
        /// <param name="key">特殊工艺名称</param>
        /// <returns>返回物料</returns>
        private Enum GetSpecialProcessName(string key)
        {
            if (SpecialProcessKeyDic.ContainsKey(key))
                return SpecialProcessKeyDic[key];
            return null;
        }

        #endregion

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }
    }
}

