using SIE.Common.ImportHelper;
using SIE.Core.ApiModels;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 故障类别导入处理
    /// </summary>
    [Services.Service(FallbackType = typeof(EquipFaultImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class EquipFaultImportHandle : IBusinessImport
    {
        /// <summary>
        /// 导入模板列头
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "故障类别编码*",
            "故障类别名称*",
        };

        /// <summary>
        /// 验证列
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get ; set ; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("故障类别编码*", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("故障类别名称*", new ValidColumn(ImportDataType._String, true, true));
            return this;
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
        /// 数据处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs
                                 select new
                                 {
                                     Code = g.Field<string>(ColIndex("故障类别编码*")).Trim(),
                                     Name = g.Field<string>(ColIndex("故障类别名称*")).Trim(),
                                     DetailInfo = g,
                                 };

            // 编码
            List<string> codes = new List<string>();
            // 名称
            List<string> names = new List<string>();

            importDataList.ForEach(p =>
            {
                codes.Add(p.Code);
                names.Add(p.Name);
            });

            // 数据库
            List<BaseDataInfo> baseDataInfos = RT.Service.Resolve<EquipFaultController>().GetFaultsByCAN(codes, names);

            var importDataRows = importDataList.ToList();
            // 保存列表
            EntityList<EquipLargeFault> saveList = new EntityList<EquipLargeFault>();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                StringBuilder errMsg = new StringBuilder();

                var data = importDataRows[i];

                if (codes.Count(p => p == data.Code) > 1 || baseDataInfos.Count(p => p.Code == data.Code) > 0)
                {
                    errMsg.Append("故障编码{0}重复".L10nFormat(data.Code));
                }

                if (names.Count(p => p == data.Name) > 1 || baseDataInfos.Count(p => p.Name == data.Name) > 0)
                {
                    errMsg.Append("故障名称{0}重复".L10nFormat(data.Name));
                }

                if (errMsg.ToString().Length <= 0)
                {
                    EquipLargeFault fault = new EquipLargeFault
                    {
                        Code = data.Code,
                        Name = data.Name,
                    };
                    saveList.Add(fault);
                }
                else
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errMsg.ToString();
                }
            }
            RF.BatchInsert(saveList);
        }
    }
}
