using DocumentFormat.OpenXml.Drawing.Charts;
using SIE.Andon.Andons;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class AndonGroupDetailViewConfig : WebViewConfig<AndonGroupDetail>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(AndonGroupDetailSelectUserCommand).FullName, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(AndonGroupDtlImportCommand).FullName, typeof(AndonGroupDtlDLTemplateCommand).FullName);
                View.Property(p => p.AndonGroupCode).Show().Readonly();
                View.Property(p => p.UserId).Show().UsePagingLookUpEditor((e, m) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(m.UserName), nameof(m.User.EmployeeName));
                    dic.Add(nameof(m.UserState), nameof(m.User.State));
                    e.DicLinkField = dic;
                });
                View.Property(p => p.UserName).Show().Readonly();
                View.Property(p => p.UserState).Show().Readonly();
                View.Property(p => p.IsResponser).Show();
                View.Property(p => p.IsAcceptancer).Show();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.AndonGroup.Code).Show().HasLabel("安灯责任组编码");
                View.PropertyRef(p => p.User.Code).Show().HasLabel("用户编码");
                View.Property(p => p.IsResponser).Show();
                View.Property(p => p.IsAcceptancer).Show();
            }
        }
    }
}
