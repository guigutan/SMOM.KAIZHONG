using SIE.Common.Import;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MES.Checker;
using SIE.MES.ItemEquipAccount;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Checker.Commands
{
    /// <summary>
    /// 导入-仅修改
    /// </summary>
    public class CheckerUpholdImportEffectiveDateCommand : ImportExcelCommand
    {

        /// <summary>
        /// 保存（仅修改。批改有效日期、图号）
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as CheckerUphold).ToList();

            // 提取所有检具编码
            var checkerCodes = list.Select(p => p.CheckerCode)
                                   .Where(c => !string.IsNullOrWhiteSpace(c))
                                   .Distinct()
                                   .ToList();

            // 批量查询数据库已有记录（必须带出 FactoryId、ProcessId 等关键外键）
            var existingList = RT.Service.Resolve<CheckerUpholdController>()
                                 .GetCheckerUpholdsByCodes(checkerCodes);   // 请确保此方法存在

            var dels = new List<RowData>();
            var processedCheckerCodes = new HashSet<string>();

            foreach (var p in batch)
            {
                var entity = p.Entity as CheckerUphold;
                if (entity == null || string.IsNullOrWhiteSpace(entity.CheckerCode))
                {
                    dels.Add(p);
                    continue;
                }

                // 文件内重复的行跳过
                if (processedCheckerCodes.Contains(entity.CheckerCode))
                {
                    dels.Add(p);
                    continue;
                }

                // 数据库中不存在则跳过（仅修改模式）
                var dbEntity = existingList.FirstOrDefault(x => x.CheckerCode == entity.CheckerCode);
                if (dbEntity == null)
                {
                    dels.Add(p);
                    continue;
                }

                // ====================== 关键修复（强制保留不更新：工厂、检具名称、检具类型） ======================
                entity.Id = dbEntity.Id;
                entity.PersistenceStatus = Domain.PersistenceStatus.Modified;

                // 必须保留的外键（尤其是 FactoryId，否则会报“工厂为空”）
                entity.FactoryId = dbEntity.FactoryId;
                entity.ProcessId = dbEntity.ProcessId;

                // 其他重要字段也建议保留（防止被Excel中空值覆盖）
                if (string.IsNullOrWhiteSpace(entity.CheckerName))
                    entity.CheckerName = dbEntity.CheckerName;

                if (string.IsNullOrWhiteSpace(entity.CheckerType))
                    entity.CheckerType = dbEntity.CheckerType;

                //if (string.IsNullOrWhiteSpace(entity.DrawingNo))
                //    entity.DrawingNo = dbEntity.DrawingNo;



                // 如果导入时不想更新检具名称、类型、图号等，也可以全部强制保留数据库值
                //entity.CheckerName = dbEntity.CheckerName;
                //entity.CheckerType = dbEntity.CheckerType;
                //entity.DrawingNo = dbEntity.DrawingNo;
                // =======================================================

                processedCheckerCodes.Add(entity.CheckerCode);
            }

            // 过滤掉要忽略的行
            batch = batch.Where(p => !dels.Contains(p)).ToList();

            base.OnSave(batch);
        }

    }

}
