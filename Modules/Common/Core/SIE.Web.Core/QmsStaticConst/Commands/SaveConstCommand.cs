using SIE.Domain;
using SIE.Core.QmsStaticConst;
using SIE.Web.Command;
using System;

namespace SIE.Web.Core.QmsStaticConst.Commands
{
    class SaveConstCommand : SaveCommand
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            EntityList deserializeData = GetDeserializeData(args, scope);   
            RT.Service.Resolve<StaticConstService>().Save((EntityList<StaticConst>)deserializeData);
            return default;
        }
    }
}
