using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Packages.Boxs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace SIE.TurnoverTools.TurnoverTools.ImportHandle
{
    /// <summary>
    /// 设备型号-导入逻辑处理
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportTurnoverToolHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportTurnoverToolHandle : IDisposable, IBusinessImport
    {
        private List<string> columnNameList = new List<string>{
            "编码*", "名称*", "周转工具类型*", "周转工具型号*"
        };

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get { return columnNameList; }
            set { columnNameList = value; }
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
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入对象
        /// </summary>
        /// <returns></returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("编码*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("名称*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("周转工具类型*", new ValidColumn(ImportDataType._String, true, ValidType));
            this.ColumnValidList.Add("周转工具型号*", new ValidColumn(ImportDataType._String, true, ValidTurnoverToolModel));
            return this;
        }

        /// <summary>
        /// 数据回收
        /// </summary>
        public void Dispose()
        {

            if (ColumnValidList != null)
            {
                ColumnValidList = null;
            }
        }

        /// <summary>
        /// 导入数据处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            for (int i = 0; i < drs.Count(); i++)
            {
                var data = drs[i];
                try
                {
                    var model = data.Field<string>(ColIndex("周转工具型号*"));
                    var tool = new TurnoverTool()
                    {
                        Code = data.Field<string>(ColIndex("编码*")),
                        Name = data.Field<string>(ColIndex("名称*")),
                        ToolType = data.Field<string>(ColIndex("周转工具类型*")),
                        ModelId = dicTurnoverToolModel.FirstOrDefault(p => p.Value == model).Key,
                        State = TurnoverToolState.Unused
                    };
                    RF.Save(tool);
                }
                catch (Exception exc)
                {
                    data[ImportDataHandle.MessageColumnName] = exc.Message;
                    Debug.WriteLine(exc);
                }
            }
        }

        #region 数据验证

        /// <summary>
        /// 类型
        /// </summary>
        private Dictionary<double, string> dicType;

        /// <summary>
        /// 类型
        /// </summary>
        private Dictionary<double, string> dicTurnoverToolModel;

        #region 验证周转工具型号   ValidTurnoverToolModel
        /// <summary>
        /// 验证周转工具型号
        /// </summary>
        /// <param name="obj">周转工具型号</param>
        /// <param name="messageTip">提示信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        public bool ValidTurnoverToolModel(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            var context = obj.ToString();
            if (dicTurnoverToolModel == null)
            {
                dicTurnoverToolModel = RT.Service.Resolve<KitTurnoverToolController>().GetTurnoverToolModelCodes();
            }
            if (!dicTurnoverToolModel.ContainsValue(context))
            {
                messageTip = "不存在于系统".L10N();
                isValid = false;
            }

            return isValid;
        }
        #endregion

        #region 验证类型  ValidType 
        /// <summary>
        /// 验证周转箱类型
        /// </summary>
        /// <param name="obj">周转箱类型</param>
        /// <param name="messageTip">提示信息</param>
        /// <param name="dr">数据行</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        public bool ValidType(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            var context = obj.ToString();
            if (dicType == null)
            {
                dicType = RT.Service.Resolve<CatalogController>().GetCatalogList(TurnoverBox.BoxTypeCatalog).ToDictionary(c => c.Id, c => c.Code);
            }

            if (!dicType.ContainsValue(context))
            {
                messageTip = "不存在于系统".L10N();
                isValid = false;
            }

            return isValid;
        }
        #endregion

        #endregion

    }
}
