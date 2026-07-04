using SIE.Domain;
using SIE.Items.ViewModels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 人员模板
    /// </summary>
    [RootEntity,Serializable]
    public class AndonEmpViewModel:ViewModel
    {
        #region 人员 AndonEmpNo
        /// <summary>
        /// 人员
        /// </summary>
        [Label("人员")]
        public static readonly Property<string> AndonEmpNoProperty = P<AndonEmpViewModel>.Register(e => e.AndonEmpNo);

        /// <summary>
        /// 人员
        /// </summary>
        public string AndonEmpNo
        {
            get { return this.GetProperty(AndonEmpNoProperty); }
            set { this.SetProperty(AndonEmpNoProperty, value); }
        }
        #endregion
    }
}
