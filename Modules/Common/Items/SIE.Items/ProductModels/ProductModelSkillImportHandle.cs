using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Resources.Skills;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SIE.Items.ProductModels
{
    /// <summary>
    /// 导入机型技能
    /// </summary>
    [Services.Service(FallbackType = typeof(ProductModelSkillImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ProductModelSkillImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        private List<string> columnNameList = new List<string>()
        {
            "编码", "名称", "需求人数"
        };

        /// <summary>
        /// 列名
        /// </summary>
        public List<string> ColumnNameList
        {
            get { return columnNameList; }
            set { columnNameList = value; }
        }

        #region 私有属性
        /// <summary>
        /// 技能编号-技能
        /// </summary>
        private Dictionary<string, Skill> dicSkill ;

        /// <summary>
        /// 技能名称
        /// </summary>
        private Dictionary<string, string> dicSkillName;


        #endregion

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列的验证
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { columnNameList[0], new ValidColumn(ImportDataType._Custom, true, VaildSkillCode) },    // 技能编码
                { columnNameList[1], new ValidColumn(ImportDataType._String, true, true) },      // 技能名称
                { columnNameList[2], new ValidColumn(ImportDataType._Int, true, VaildDemandQty) }           // 需求人数
            };
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicSkill != null)
            {
                dicSkill.Clear();
                dicSkill = null;
            }

            if (dicSkillName != null)
            {
                dicSkillName.Clear();
                dicSkillName = null;
            }
        }

        #region 验证数据
        /// <summary>
        /// 验证技能编码
        /// </summary>
        /// <param name="obj">技能编码</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns></returns>
        private bool VaildSkillCode(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;

            dicSkill ??= new Dictionary<string, Skill>();
            string skillCode = obj.ToString();
            if (!dicSkill.ContainsKey(skillCode))
            {
                var skill = RT.Service.Resolve<SkillController>().GetSkill(skillCode);
                if (skill != null)
                {
                    dicSkill.Add(skillCode, skill);
                }
                else
                {
                    isValid = false;
                    messageTip = ":[{0}]不存在;".L10nFormat(skillCode);
                }
            }
            else
            {
                isValid = false;
                messageTip = ":[{0}]重复添加;".L10nFormat(skillCode);
            }
            return isValid;
        }

        /// <summary>
        /// 验证需求数量
        /// </summary>
        /// <param name="obj">需求数量</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前数据行</param>
        /// <returns></returns>
        private bool VaildDemandQty(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;

            if (obj != null)
            {
                if (int.TryParse(obj.ToString(), out int demandQty))
                {
                    if (demandQty < 0)
                    {
                        messageTip = "需求数量必须大于0!".L10N();
                        isValid = false;
                    }
                }
                else
                {
                    messageTip = "需求数量请填入数字!".L10N();
                    isValid = false;
                }
            }
            else
            {
                messageTip = "需求数量不能为空!".L10N();
                isValid = false;
            }

            return isValid;
        }


        #endregion


        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="drs">数据行</param>
        /// <returns></returns>
        private DataRow[] VaildData(DataRow[] drs)
        {
            if (drs == null || !drs.Any())
            {
                return new DataRow[0];
            }
            var repeatDataList = drs.GroupBy(p => p[0].ToString()).Select(f => new { CODE = f.Key, Value = f.ToList() }).Where(q => q.Value.Count > 1).ToList();
            repeatDataList.ForEach(p => p.Value.ForEach(q =>
                AppendErrorMsg(q, ImportDataHandle.MessageColumnName, "当前技能编码存在重复;".L10N())));

            //// 查看机型技能是否存在于系统中
            EntityList<ProductModelSkill> productModelSkillList = RT.Service.Resolve<ProductModelController>().GetProductModel(double.Parse(drs[0][ImportDataHandle.ParentId].ToString().Trim())).SkillList;
            List<string> skillList = productModelSkillList.Select(p => p.Skill.Code).ToList();
            drs.Where(p => skillList.Contains(p[0].ToString())).ForEach(q =>
                   AppendErrorMsg(q, ImportDataHandle.MessageColumnName, "当前技能已经存在于系统;".L10N()));

            return drs.Where(p => string.IsNullOrEmpty(p[ImportDataHandle.MessageColumnName].ToString())).ToArray();
        }

        /// <summary>
        /// 业务数据处理
        /// </summary>
        /// <param name="drs">数据集合</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            drs = VaildData(drs);
            if (drs == null || !drs.Any())
            {
                return;
            }
            if (drs.Any(p => !string.IsNullOrEmpty(p[ImportDataHandle.MessageColumnName].ToString())))
            {
                return;
            }
            if (drs.Length == 0)
            {
                return;
            }
            var mainDataList = from g in drs
                               select new
                               {
                                   SkillCode = g.Field<string>(ColIndex("编码")),
                                   SkillName = g.Field<string>(ColIndex("名称")),
                                   DemandQty = g.Field<string>(ColIndex("需求人数")),
                                   DetailInfo = g
                               };

            foreach (var mainDataItem in mainDataList)
            {
                ProductModelSkill pms = new ProductModelSkill();
                var skill = RT.Service.Resolve<SkillController>().GetSkill(mainDataItem.SkillCode);
                pms.SkillId = skill.Id;
                pms.Skill = skill;
                string tempId = drs[0][ImportDataHandle.ParentId].ToString().Trim();
                double.TryParse(tempId, out double pmId);
                pms.ModelId = pmId;
                pms.DemandQty = Convert.ToInt32(mainDataItem.DemandQty);

                // 如果不能新增记录错误信息
                try
                {
                    RF.Save(pms);
                }
                catch (Exception ex)
                {
                    string strMsg = AppRuntime.Location.ConnectDataDirectly ? ex.Message : ex.InnerException.Message;
                    AppendErrorMsg(mainDataItem.DetailInfo, ImportDataHandle.MessageColumnName, strMsg);
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
        /// 给保存错误的数据行记录错误数据信息
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <param name="errorMsg">错误信息</param>
        private void AppendErrorMsg(DataRow row, string columnName, string errorMsg)
        {
            row[columnName] += errorMsg;
        }
    }
}