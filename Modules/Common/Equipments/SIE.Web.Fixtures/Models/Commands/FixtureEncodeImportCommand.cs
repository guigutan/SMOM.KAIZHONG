using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.Fixtures.Models;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.Models.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Models.Commands.FixtureEncodeImportCommand")]
    public class FixtureEncodeImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 重写导入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cache"></param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var fixtureEncode = data.Entity as FixtureEncode;
            if (fixtureEncode.FixtureModel == null)
            {
                throw new ValidationException("第{0}行的工治具型号不能为空".L10nFormat(data.RowIndex));
            }
            foreach (var item in fixtureEncode.FixtureModel.MaintainProjectList)
            {
                fixtureEncode.FixtureEncodeMaintainProjectList.Add(new FixtureEncodeMaintainProject()
                {
                    MaintainProjectId = item.MaintainProjectId,
                    CheckTag = item.CheckTag,
                    MinValue = item.MinValue,
                    MaxValue = item.MaxValue,
                    OnlineMaintain = item.OnlineMaintain,
                    Acceptance = item.AcceptanceItems,
                    InStorageMaintain = item.InStorageMaintain,
                    CommonMaintain = item.CommonMaintain,
                    ToStorageMaintain = item.ToStorageMaintain,
                });
            }
        }
    }
}