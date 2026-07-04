using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Items.ProductModels.ImportProductModels
{
    /// <summary>
    /// 导入产品机型逻辑处理
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportProductModelHandel), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportProductModelHandel : IDisposable, IBusinessImport
    {
        #region 表头        
        /// <summary>
        /// 机型编码
        /// </summary>
        private const string Col_ModelCode = "机型编码";

        /// <summary>
        /// 机型名称
        /// </summary>
        private const string Col_ModelName = "机型名称";

        /// <summary>
        /// 工时（单位/小时）
        /// </summary>
        private const string Col_ModelWorkingHours = "工时（单位/小时）";

        /// <summary>
        /// 配送工时（单位/小时）
        /// </summary>
        private const string Col_ModelSendingHours = "配送工时（单位/小时）";

        /// <summary>
        /// 产线编码
        /// </summary>
        private const string Col_ResourceCode = "产线编码";

        /// <summary>
        /// 产线工时（单位/小时）
        /// </summary>
        private const string Col_RresWorkingHours = "产线工时（单位/小时）";
        #endregion
        /// <summary>
        /// 导入列表头
        /// </summary>
        private List<string> columnNameList = new List<string>
        {
            Col_ModelCode, Col_ModelName, Col_ModelWorkingHours,Col_ModelSendingHours, Col_ResourceCode, Col_RresWorkingHours
        };

        #region 私有属性
        /// <summary>
        /// 产线编码
        /// </summary>
        private Dictionary<string, double> dicProductionCellCode;
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
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入对象
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add(Col_ModelCode, new ValidColumn(ImportDataType._String, true, 40));
            this.ColumnValidList.Add(Col_ModelName, new ValidColumn(ImportDataType._String, false, 240));
            this.ColumnValidList.Add(Col_ModelWorkingHours, new ValidColumn(ImportDataType._Double, true, true));
            this.ColumnValidList.Add(Col_ModelSendingHours, new ValidColumn(ImportDataType._Double, true, true));
            this.ColumnValidList.Add(Col_ResourceCode, new ValidColumn(ImportDataType._Custom, false, VaildLineCode));
            this.ColumnValidList.Add(Col_RresWorkingHours, new ValidColumn(ImportDataType._Double, false, true));

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicProductionCellCode != null)
            {
                dicProductionCellCode.Clear();
                dicProductionCellCode = null;
            }
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            // 1、按"机型编码","工时(单位/小时)"分组
            var mainDataList = from g in drs
                               group g by new
                               {
                                   Code = g.Field<string>(ColIndex(Col_ModelCode)),
                                   Name = g.Field<string>(ColIndex(Col_ModelName)),
                                   WorkingHours = g.Field<string>(ColIndex(Col_ModelWorkingHours)),
                                   SendingHours = g.Field<string>(ColIndex(Col_ModelSendingHours))
                               }
                               into proModel
                               select new
                               {
                                   Code = proModel.Key.Code,
                                   Name = proModel.Key.Name,
                                   WorkingHours = proModel.Key.WorkingHours,
                                   SendingHours = proModel.Key.SendingHours,
                                   DetailInfo = proModel
                               };
            var ctrl = RT.Service.Resolve<ItemController>();
            ///// 循环检验每一行主数据
            foreach (var mainDataItem in mainDataList)
            {
                // 判断主数据是否存在
                ProductModel productModel = ctrl.GetProductModel(mainDataItem.Code);
                if (productModel == null)
                {
                    productModel = new ProductModel();
                    productModel.Code = mainDataItem.Code;
                    productModel.Name = mainDataItem.Name;
                    productModel.WorkingHours = decimal.Parse(mainDataItem.WorkingHours);
                    productModel.SendingHours = decimal.Parse(mainDataItem.SendingHours);
                }
                else
                {
                    productModel.WorkingHours = decimal.Parse(mainDataItem.WorkingHours);
                    productModel.SendingHours = decimal.Parse(mainDataItem.SendingHours);
                }

                // 如果不能新增记录错误信息
                using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
                {
                    try
                    {
                        RF.Save(productModel);
                    }
                    catch (Exception ex)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? ex.Message : ex.InnerException.Message;
                        ImportExtension.BatchAppendText(mainDataItem.DetailInfo.ToList(), ImportDataHandle.MessageColumnName, strMsg);
                        continue;
                    }

                    ProcessDetailData(productModel, mainDataItem.DetailInfo.ToList());
                    tran.Complete();
                }
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

        /// <summary>
        /// 处理产品机型数据的数据
        /// </summary>
        /// <param name="mainData">主表数据对象</param>
        /// <param name="detailRows">明细数据集合</param>
        private void ProcessDetailData(ProductModel mainData, List<DataRow> detailRows)
        {
            foreach (DataRow detailRow in detailRows)
            {
                ProductModelLineCapacity productModelLineCapacity = new ProductModelLineCapacity();
                productModelLineCapacity.ProductModelId = mainData.Id;
                productModelLineCapacity.ResourceId = dicProductionCellCode[detailRow[(Col_ResourceCode)].ToString()];
                string workingHours = detailRow[ColIndex(Col_RresWorkingHours)].ToString();
                if (!string.IsNullOrEmpty(workingHours))
                {
                    productModelLineCapacity.WorkingHours = decimal.Parse(workingHours);
                }

                try
                {
                    RF.Save(productModelLineCapacity);
                }
                catch (Exception ex)
                {
                    string strMsg = AppRuntime.Location.ConnectDataDirectly ? ex.Message : ex.InnerException.Message;
                    detailRow[ImportDataHandle.MessageColumnName] = detailRow[ImportDataHandle.MessageColumnName] + strMsg;
                }
            }
        }

        #region 基础验证

        /// <summary>
        /// 验证产线编码
        /// </summary>
        /// <param name="obj">上下文内容</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">产线编码集合</param>
        /// <returns>是否验证通过</returns>
        private bool VaildLineCode(object obj, out string messageTip, DataRow dr)
        {
            return ValidLineCode(ref dicProductionCellCode, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证产线编码
        /// </summary>
        /// <param name="lineCodeDic">产线编码集合</param>
        /// <param name="context">上下文内容</param>
        /// <param name="messageTip">信息提示</param>
        /// <returns>是否验证通过</returns>
        public bool ValidLineCode(ref Dictionary<string, double> lineCodeDic, string context, out string messageTip)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (lineCodeDic == null)
            {
                lineCodeDic = new Dictionary<string, double>();
            }

            if (!lineCodeDic.ContainsKey(context))
            {
                var resource = RT.Service.Resolve<WipResourceController>().GetScheResourceByResCode(context);
                if (resource != null)
                {
                    lineCodeDic.Add(context, resource.Id);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
                    isValid = false;
                }
            }

            return isValid;
        }
        #endregion
    }
}
