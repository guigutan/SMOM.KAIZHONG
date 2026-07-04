using SIE.MetaModel.View;
using SIE.Resources.PersonnelSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.PersonnelSkills
{
    public class PersonnelSkillViewConfig : WebViewConfig<PersonnelSkill>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(PersonnelSkill));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeId).Show();
                View.Property(p => p.SkillId).Show();
                //View.Property(p => p.SkillCode).Show();
            }
        }
    }
}
