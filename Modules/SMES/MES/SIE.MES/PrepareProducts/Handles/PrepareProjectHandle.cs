using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using SIE.Packages.ItemLabels;
using SIE.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.MES.PrepareProducts.Handles
{
    /// <summary>
    /// 产前准备项目维护
    /// </summary>
    [SIE.Services.Service(FallbackType = typeof(PrepareProjectHandle), ServiceLifeStyle = SIE.Services.ServiceLifeStyle.Transient)]
    public class PrepareProjectHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "项目编码*", "项目名称*","项目类型*","项目描述",
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
        /// <exception cref="NotImplementedException"></exception>
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
                                     ProCode = g.Field<string>(ColIndex("项目编码*")).Trim(),
                                     ProName = g.Field<string>(ColIndex("项目名称*")).Trim(),
                                     ProType = g.Field<string>(ColIndex("项目类型*")).Trim(),
                                     ProDesc = g.Field<string>(ColIndex("项目描述")).Trim(),
                                     DetailInfo = g,
                                 };
            List<string> codeList = new List<string>();
            List<string> nameList = new List<string>();
            List<string> typeList = new List<string>();
            importDataList.ForEach(data =>
            {
                codeList.Add(data.ProCode);
                nameList.Add(data.ProName);
                typeList.Add(data.ProType);
            });
            var sr = RT.Service.Resolve<PrepareProjectService>();
            var dataBaseCodes = sr.GetDataBasePreProByCode(codeList);
            var dataBaseNames = sr.GetDataBasePreProByName(nameList);
            codeList.AddRange(dataBaseCodes);
            nameList.AddRange(dataBaseNames);
            var importDataRows = importDataList.ToList();
            EntityList<PrepareProject> saveList = new EntityList<PrepareProject>();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                var pass = true;
                if (importDataRows[i].ProCode.IsNullOrEmpty())
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "编码为空;".L10N();
                    pass = false;
                }
                if (importDataRows[i].ProName.IsNullOrEmpty())
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "名称为空;".L10N();
                    pass = false;
                }
                if (importDataRows[i].ProType.IsNullOrEmpty())
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "类型为空;".L10N();
                    pass = false;
                }
                var codeRepeat = codeList.Count(p => p == importDataRows[i].ProCode) > 1;
                var nameRepeat = nameList.Count(p => p == importDataRows[i].ProName) > 1;
                PrepareProjectType? type = null;
                if (codeRepeat)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "编码重复;".L10N();
                    pass = false;
                }
                //if (nameRepeat)
                //{
                //    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "名称重复;";
                //    pass = false;
                //}
                switch (importDataRows[i].ProType)
                {
                    case "人":
                        type = PrepareProjectType.Man;
                        break;
                    case "机":
                        type = PrepareProjectType.Machine;
                        break;
                    case "料":
                        type = PrepareProjectType.Material;
                        break;
                    case "法":
                        type = PrepareProjectType.Method;
                        break;
                    case "环":
                        type = PrepareProjectType.Environments;
                        break;
                    case "测":
                        type = PrepareProjectType.Measure;
                        break;
                    default: 
                        type = null; 
                        break;
                }
                if (type == null && importDataRows[i].ProType.IsNotEmpty())
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "类型无效;".L10N();
                    pass = false;
                }
                if (pass)
                {
                    PrepareProject prepareProject = new PrepareProject
                    {
                        ProCode = importDataRows[i].ProCode,
                        ProName = importDataRows[i].ProName,
                        ProType = type.Value,
                        ProDesc = importDataRows[i].ProDesc,
                    };
                    saveList.Add(prepareProject);
                }
            }
            if (saveList.Any())
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    RF.BatchInsert(saveList);
                    tran.Complete();
                }
            }
            
        }
    }
}
