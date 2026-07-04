using SIE.Andon.Andons.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    internal class AndonManageImageViewModelViewConfig : WebViewConfig<AndonManageImageViewModel>
    {
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.FileName)
                .UseImageComponentEditor(p => { p.XType = "andonmanageimagebtn"; })
                .HasLabel("")
                .Readonly();
        }
    }
}
