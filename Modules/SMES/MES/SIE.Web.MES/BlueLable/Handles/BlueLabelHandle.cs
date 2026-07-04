using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.MES.BlueLable;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using SIE.Packages.ItemLabels;
using SIE.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.MES.BlueLabel.Handles
{
    /// <summary>
    /// 产前准备项目维护
    /// </summary>
    [SIE.Services.Service(FallbackType = typeof(BlueLabelHandle), ServiceLifeStyle = SIE.Services.ServiceLifeStyle.Transient)]
    public class BlueLabelHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "旧蓝标*", "新蓝标*",
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
                                     OldBlueLabel = g.Field<string>(ColIndex("旧蓝标*")).Trim(),
                                     NewBlueLabel = g.Field<string>(ColIndex("新蓝标*")).Trim(),
                                     DetailInfo = g,
                                 };
            List<string> codeList = new List<string>();
            List<string> nameList = new List<string>();
            List<string> typeList = new List<string>();
            importDataList.ForEach(data =>
            {
                codeList.Add(data.OldBlueLabel);
                nameList.Add(data.NewBlueLabel);
            });


            //var sr = RT.Service.Resolve<PrepareProjectService>();
            //var dataBaseCodes = sr.GetDataBasePreProByCode(codeList);
            //var dataBaseNames = sr.GetDataBasePreProByName(nameList);
            //codeList.AddRange(dataBaseCodes);
            //nameList.AddRange(dataBaseNames);
            var importDataRows = importDataList.ToList();
            EntityList<BlueLabelReplace> saveList = new EntityList<BlueLabelReplace>();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                var pass = true;
                if (importDataRows[i].OldBlueLabel.IsNullOrEmpty())
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "旧蓝标为空;".L10N();
                    pass = false;
                }
                if (importDataRows[i].NewBlueLabel.IsNullOrEmpty())
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "新蓝标为空;".L10N();
                    pass = false;
                }
                var codeRepeat = codeList.Count(p => p == importDataRows[i].OldBlueLabel) > 1;
                var nameRepeat = nameList.Count(p => p == importDataRows[i].NewBlueLabel) > 1;
                PrepareProjectType? type = null;
                if (codeRepeat)
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] += "编码重复;".L10N();
                    pass = false;
                }
               
                if (pass)
                {
                    BlueLabelReplace prepareProject = new BlueLabelReplace
                    {
                        OldBlueLabel = importDataRows[i].OldBlueLabel,
                        NewBlueLabel = importDataRows[i].NewBlueLabel,
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
