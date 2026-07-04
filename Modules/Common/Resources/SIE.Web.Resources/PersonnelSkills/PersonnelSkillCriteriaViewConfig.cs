using SIE.Resources.PersonnelSkills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.PersonnelSkills
{
    public class PersonnelSkillCriteriaViewConfig : WebViewConfig<PersonnelSkillCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeId).Show();
                View.Property(p => p.SkillId).Show();
            }
        }
    }
}
