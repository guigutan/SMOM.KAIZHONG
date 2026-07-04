using SIE.Common.ImportHelper;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.EMS.Faults;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Devices.Abnormals
{
    /// <summary>
    /// 设备异常维护导入处理
    /// </summary>
    [Services.Service(FallbackType = typeof(DeviceAbnormalImpHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class DeviceAbnormalImpHandle : IBusinessImport
    {
        /// <summary>
        /// 导入模板列头
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "故障名称*",
            "类型*",
            "故障描述",
            "设备类型编码",
        };

        /// <summary>
        /// 验证列
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }


        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("故障名称*", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("类型*", new ValidColumn(ImportDataType._String, true, true));
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
        /// 转化类型
        /// </summary>
        /// <returns></returns>
        private AbnormalType? GetAbnormalType(string type)
        {
            switch (type)
            {
                case "故障现象":
                    return AbnormalType.Unusual;
                case "故障描述":
                    return AbnormalType.Fault;
                default:
                    return null;
            }
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
                                     Name = g.Field<string>(ColIndex("故障名称*")).Trim(),
                                     Type = g.Field<string>(ColIndex("类型*")).Trim(),
                                     Desc = g.Field<string>(ColIndex("故障描述")).Trim(),
                                     EquipTypeCode = g.Field<string>(ColIndex("设备类型编码")).Trim(),
                                     DetailInfo = g,
                                 };

            // 故障名称
            List<string> names = new List<string>();
            // 设备类型编码
            List<string> equipTypeCodes = new List<string>();
            importDataList.ForEach(p =>
            {
                names.Add(p.Name);
                equipTypeCodes.Add(p.EquipTypeCode);
            });

            // 数据库故障名称
            List<string> dbNames = RT.Service.Resolve<DeviceAbnormalController>().GetAbnormalNames(names);

            // 数据库设备类型
            List<BaseDataInfo> dbType = RT.Service.Resolve<EquipTypeController>().GetDBEquipTypeBaseInfos(equipTypeCodes);

            var importDataRows = importDataList.ToList();
            var saveList = new EntityList<DeviceAbnormal>();

            for (int i = 0; i < importDataRows.Count; i++)
            {
                StringBuilder errMsg = new StringBuilder();

                var data = importDataRows[i];

                if (names.Count(p => p == data.Name) > 1 || dbNames.Count(p => p == data.Name) > 0)
                {
                    errMsg.Append("故障名称{0}重复".L10nFormat(data.Name));
                }

                var type = dbType.FirstOrDefault(p => p.Code == data.EquipTypeCode);
                if (data.EquipTypeCode.IsNotEmpty() && type == null)
                {
                    errMsg.Append("设备类型编码{0}不存在".L10nFormat(data.EquipTypeCode));
                }

                if (GetAbnormalType(data.Type) == null)
                {
                    errMsg.Append("类型{0}不存在".L10nFormat(data.Type));
                }

                if (errMsg.ToString().Length <= 0)
                {
                    DeviceAbnormal deviceAbnormal = new DeviceAbnormal
                    {
                        Code = data.Name,
                        AbnormalType = GetAbnormalType(data.Type),
                        Description = data.Desc,
                        EquipTypeId = data.EquipTypeCode.IsNotEmpty() ? type.Id : null,
                    };
                    saveList.Add(deviceAbnormal);
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
