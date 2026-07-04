using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.ApiModels
{
    [Serializable]
    public class DictionaryData
    {
        public string DicKey { get; set; }

        public List<string> DicValue { get; set; }
    }

    [Serializable]
    public class DictionaryObjData
    {
        public string DicKey { get; set; }

        public List<object> DicValue { get; set; }
    }
}
