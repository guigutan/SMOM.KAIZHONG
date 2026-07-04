using IronPython.Compiler.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    public class IotToken
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; } = "guid";
        /// <summary>
        /// 
        /// </summary>
        public string jsonrpc { get; set; } = "2.0";
        /// <summary>
        /// 
        /// </summary>
        public string method { get; set; } = "login";
        /// <summary>
        /// 
        /// </summary>
        public TokenParams @params { get; set; }
    }
    public class TokenParams
    {
        /// <summary>
        /// 
        /// </summary>
        public TokenArgs args { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TokenContext context { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tag { get; set; } = "master";
        /// <summary>
        /// 
        /// </summary>
        public string app { get; set; } = "base";
        /// <summary>
        /// 
        /// </summary>
        public string login { get; set; } = "admin_000001";
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; } = "QWRtaW5fMDAwMDAx";
        /// <summary>
        /// 
        /// </summary>
        public bool remember { get; set; } = false;
    }

    public class TokenContext
    {
        /// <summary>
        /// 
        /// </summary>
        public string lang { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uid { get; set; }
    }

    public class TokenArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public bool useDisplayForModel { get; set; } = true;
    }
}
