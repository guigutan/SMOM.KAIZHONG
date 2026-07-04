using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Common.Settings
{
    public class MenuPermission
    {
        public string ModuleKey { get; set; }
        public string OperationKey { get; set; }
        public int Platform { get; set; }
        public string ScopeKey { get; set; }
        public int PersistenceStatus { get; set; }
    }
}
