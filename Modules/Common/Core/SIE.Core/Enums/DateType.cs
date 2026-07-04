using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Enums
{

    public enum DateType
    {
        [Label("全部")]
        All = 0,
        [Label("本月")]
        ThisMonth = 1,
        [Label("本年")]
        ThisYear = 2,
        [Label("上个月")]
        UpMonth = 3,
    }
}
