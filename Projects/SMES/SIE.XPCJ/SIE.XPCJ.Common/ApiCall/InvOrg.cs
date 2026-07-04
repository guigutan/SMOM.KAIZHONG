using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Common.ApiCall
{
    public class InvOrg
    {
        public double Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
