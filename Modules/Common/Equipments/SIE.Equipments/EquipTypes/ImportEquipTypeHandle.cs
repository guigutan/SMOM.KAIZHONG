using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Equipments.EquipModels.ImportEquipModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipTypes
{
    /// <summary>
    /// 设备类型-导入逻辑处理
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipTypeHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipTypeHandle : IBusinessImport
    {
        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "类型编码*".L10N(),
            "类型名称*".L10N()
        };

        /// <summary>
        /// 验证列
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入对象
        /// </summary>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("类型编码*".L10N(), new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("类型名称*".L10N(), new ValidColumn(ImportDataType._String, true, 80));
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
        /// 数据导入处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs
                                 select new
                                 {
                                     Code = g.Field<string>(ColIndex("类型编码*".L10N())).Trim(),
                                     Name = g.Field<string>(ColIndex("类型名称*".L10N())).Trim(),
                                     DetailInfo = g,
                                 };

            // 编码
            List<string> codes = new List<string>();
            importDataList.ForEach(data =>
            {
                codes.Add(data.Code);
            });

            // 数据库编码
            var dbCodes = RT.Service.Resolve<EquipTypeController>().GetDBEquipTypeCodes();
            var importDataRows = importDataList.ToList();

            // 保存列表
            EntityList<EquipType> equipTypes = new EntityList<EquipType>();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                // 是否通过
                bool pass = true;

                var code = importDataRows[i].Code;
                var name = importDataRows[i].Name;
                if (codes.Count(p => p == code) > 1 || dbCodes.Count(p => p == code) > 0) // 校验编码唯一
                {
                    pass = false;
                }

                if (pass)
                {
                    EquipType equipType = new EquipType
                    {
                        TypeCode = code,
                        TypeName = name,
                    };
                    equipTypes.Add(equipType);
                }
                else // 数据验证不通过
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = "类型编码不可重复".L10N();
                }
            }

            RF.BatchInsert(equipTypes);
        }
    }
}
