using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.Fixtures.Models;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;

namespace SIE.Web.Fixtures.Models.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Models.Commands.FixtureModelMaintainImportCommand")]
    public class FixtureModelMaintainImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 重写导入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cache"></param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var fixtureModelMaintainProject = data.Entity as FixtureModelMaintainProject;
            if (fixtureModelMaintainProject.FixtureModel == null)
            {
                throw new ValidationException("第{0}行的工治具型号不能为空".L10nFormat(data.RowIndex + 1));
            }
            if (fixtureModelMaintainProject.MaintainProject == null)
            {
                throw new ValidationException("第{0}行的保养项目不能为空".L10nFormat(data.RowIndex + 1));
            }
            if (fixtureModelMaintainProject.MaxValue.HasValue && fixtureModelMaintainProject.MinValue.HasValue
                && fixtureModelMaintainProject.MaxValue.Value < fixtureModelMaintainProject.MinValue.Value)
            {
                throw new ValidationException("第{0}行的最小值必须小于等于检测合格最大值".L10nFormat(data.RowIndex + 1));
            }
            fixtureModelMaintainProject.CheckTag = fixtureModelMaintainProject.MaintainProject.CheckTag;
        }
    }

}
