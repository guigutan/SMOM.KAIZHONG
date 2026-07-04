using SIE.Andon.Andons;
using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons.Commands
{
    public class AndonGroupImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            //var entitys = batch.Select(p => p.Entity as AndonGroup).ToList();
            //var codes = entitys.GroupBy(p => p.Code).ToDictionary(p => p.Key, p => p.ToList()).Where(p => p.Value.Count > 1).Select(p => p.Key).Distinct().ToList();
            //var names = entitys.GroupBy(p => p.Name).ToDictionary(p => p.Key, p => p.ToList()).Where(p => p.Value.Count > 1).Select(p => p.Key).Distinct().ToList();
            //string err = "";
            //if (codes.Count > 0)
            //{
            //    err += "已存在相同编码" + string.Join('、', codes) + "不可重复导入;";
            //}
            //if (names.Count > 0)
            //{
            //    err += "已存在相同名称" + string.Join('、', codes) + "不可重复导入;";
            //}
            //if (err != "")
            //{
            //    throw new ValidationException(err);
            //}
            base.OnSave(batch);
        }
    }
}
