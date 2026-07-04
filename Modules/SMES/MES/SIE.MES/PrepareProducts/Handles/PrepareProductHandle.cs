using NPOI.POIFS.Properties;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Items;
using SIE.MES.PrepareProducts.Services;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.MES.PrepareProducts.Handles
{
    /// <summary>
    /// 产品产前准备设置
    /// </summary>
    [SIE.Services.Service(FallbackType = typeof(PrepareProductHandle), ServiceLifeStyle = SIE.Services.ServiceLifeStyle.Transient)]
    public class PrepareProductHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "产品编码","产品族编码","工序","项目编码",
        };

        /// <summary>
        /// 列标准验证
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get ; set ; }


        /// <summary>
        /// 创建导入数据列验证
        /// </summary>
        /// <returns></returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        protected virtual int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 导入逻辑
        /// </summary>
        /// <param name="drs"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs
                                 select new
                                 {
                                     Product = g.Field<string>(ColIndex("产品编码")).Trim(),
                                     ProFamily = g.Field<string>(ColIndex("产品族编码")).Trim(),
                                     Process = g.Field<string>(ColIndex("工序")).Trim(),
                                     ProCode = g.Field<string>(ColIndex("项目编码")).Trim(),
                                     DetailInfo = g,
                                 };
            List<string> improductCodeList = new List<string>();
            List<string> imfamilyCodeList = new List<string>();
            List<string> improcessCodeList = new List<string>();
            List<string> improjectCodeList = new List<string>();
            importDataList.ForEach(data =>
            {
                improductCodeList.Add(data.Product);
                imfamilyCodeList.Add(data.ProFamily);
                improcessCodeList.Add(data.Process);
                improjectCodeList.Add(data.ProCode);
            });
            var sr = RT.Service.Resolve<PrepareProductService>();
            var productList = sr.ImGetProductByCodes(improductCodeList);
            var familyList = sr.ImGetFamilyByCodes(imfamilyCodeList);
            var processList = sr.ImGetProcessByCodes(improcessCodeList);
            var projectList = sr.ImGetProjectByCodes(improjectCodeList);
            var dbproductList = sr.GetDBPrepareProductList(improductCodeList);
            var dbproductDetailList = sr.GetPrepareProductDetailList(dbproductList.Select(p => p.Id).ToList());
            var dbfamilyList = sr.GetDBPrepareFamilyList(imfamilyCodeList);
            var dbfamilyDetailList = sr.GetPrepareProductDetailList(dbfamilyList.Select(p => p.Id).ToList());


            var importDataRows = importDataList.ToList();
            EntityList<PrepareProduct> saveParentList = new EntityList<PrepareProduct>();
            EntityList<PrepareProductDetail> saveChildList = new EntityList<PrepareProductDetail>();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                var rowData = importDataRows[i];
                var pass = true;
                var rowDataListRepeat = importDataRows.Count(p => p.Product == rowData.Product  && p.ProFamily == rowData.ProFamily && p.Process == rowData.Process && p.ProCode == rowData.ProCode) > 1;
                if (rowDataListRepeat)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "数据行重复！".L10N();
                    continue;
                }
                if (rowData.Product.Length == 0 && rowData.ProFamily.Length == 0)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "产品和产品族两者必须维护一个;".L10N();
                    pass = false;
                }
                if (rowData.Product.Length != 0 && rowData.ProFamily.Length != 0)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "产品和产品族两者只维护一个;".L10N();
                    pass = false;
                }
                var productExists = productList.FirstOrDefault(p => p.Code == rowData.Product);
                if (rowData.Product.Length != 0 && productExists == null)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "产品不存在;".L10N();
                    pass = false;
                }
                var familyExists = familyList.FirstOrDefault(p => p.Code == rowData.ProFamily);
                if (rowData.ProFamily.Length != 0 && familyExists == null)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "产品族不存在;".L10N();
                    pass = false;
                }
                var processExists = processList.FirstOrDefault(p => p.Code == rowData.Process);
                if (rowData.Process.Length != 0 && processExists == null)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "工序不存在;".L10N();
                    pass = false;
                }
                if (familyExists != null && processExists != null && processExists.ProductFamilyId != familyExists.Id)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "工序不为产品族下的工序;".L10N();
                    pass = false;
                }
                var projectExists = projectList.FirstOrDefault(p => p.ProCode == rowData.ProCode);
                if (!rowData.ProCode.IsNotEmpty())
                {
                    if (rowData.Process.IsNotEmpty())
                    {
                        importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "项目不能为空;".L10N();
                        pass = false;
                    }
                }
                else
                {
                    if (projectExists == null)
                    {
                        importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "项目不存在;".L10N();
                        pass = false;
                    }
                }
                var dbproduct = dbproductList.FirstOrDefault(p => p.ProductCode == rowData.Product);
                var dbfamily = dbfamilyList.FirstOrDefault(p => p.ProductFamilyCode == rowData.ProFamily);
                if (pass)
                {
                    if (rowData.Product.IsNotEmpty() && !rowData.ProFamily.IsNotEmpty())
                    {
                        if (dbproduct != null)
                        {
                            var detail = dbproductDetailList.FirstOrDefault(p => p.PrepareProductId == dbproduct.Id && p.ProcessId == processExists?.Id && p.PrepareProjectId == projectExists.Id);
                            if (detail != null)
                            {
                                importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "工序 + 项目唯一;".L10N();
                            }
                            else
                            {
                                if (projectExists != null)
                                {
                                    PrepareProductDetail prepareProductDetail = new PrepareProductDetail
                                    {
                                        PrepareProduct = dbproduct,
                                        ProcessId = processExists?.Id,
                                        PrepareProjectId = projectExists.Id,
                                    };
                                    saveChildList.Add(prepareProductDetail);
                                }
                            }
                        }
                        else
                        {
                            PrepareProduct prepareProduct = saveParentList.FirstOrDefault(p => p.ProductId == productExists.Id);
                            if (prepareProduct == null)
                            {
                                prepareProduct = new PrepareProduct
                                {
                                    ProductId = productExists.Id,
                                };
                                saveParentList.Add(prepareProduct);
                            }
                            if (projectExists != null)
                            {
                                PrepareProductDetail prepareProductDetail = new PrepareProductDetail
                                {
                                    PrepareProduct = prepareProduct,
                                    ProcessId = processExists?.Id,
                                    PrepareProjectId = projectExists.Id,
                                };
                                saveChildList.Add(prepareProductDetail);
                            }
                            
                        }
                    }
                    else
                    {
                        if(dbfamily != null)
                        {
                            var detail = dbfamilyDetailList.FirstOrDefault(p => p.PrepareProductId == dbfamily.Id && p.ProcessId == processExists?.Id && p.PrepareProjectId == projectExists.Id);
                            if (detail != null)
                            {
                                importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "工序 + 项目唯一;".L10N();
                            }
                            else
                            {
                                PrepareProductDetail prepareProductDetail = new PrepareProductDetail
                                {
                                    PrepareProduct = dbfamily,
                                    ProcessId = processExists?.Id,
                                    PrepareProjectId = projectExists.Id,
                                };
                                saveChildList.Add(prepareProductDetail);
                            }                        
                        }
                        else
                        {
                            PrepareProduct prepareProduct = saveParentList.FirstOrDefault(p => p.ProductFamilyId == familyExists.Id);
                            if (prepareProduct == null)
                            {
                                prepareProduct = new PrepareProduct
                                {
                                    ProductFamilyId = familyExists.Id,
                                };
                                saveParentList.Add(prepareProduct);
                            }
                            if (projectExists != null)
                            {
                                PrepareProductDetail prepareProductDetail = new PrepareProductDetail
                                {
                                    PrepareProduct = prepareProduct,
                                    ProcessId = processExists?.Id,
                                    PrepareProjectId = projectExists.Id,
                                };
                                saveChildList.Add(prepareProductDetail);
                            }
                        }
                    }
                }
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.BatchInsert(saveParentList);
                saveChildList.ForEach(child =>
                {
                    child.PrepareProductId = child.PrepareProduct.Id;
                });
                RF.BatchInsert(saveChildList);
                tran.Complete();
            }
        }
    }
}
