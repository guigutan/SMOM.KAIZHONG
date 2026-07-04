using SIE.Common.Import;
using SIE.MES.Checker;
using SIE.Resources.Enterprises;
using SIE.Web.Common.Import.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.Checker.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class CheckerUpholdImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var factoryList = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(null, null);
            batch.ForEach(p =>
            {
                var entity = p.Entity as CheckerUphold;
                entity.Factory = factoryList.FirstOrDefault(x => x.Code == entity.FactoryCode);
                //entity.State = Domain.State.Enable;
            });
            base.OnSave(batch);
        }
    }

}
