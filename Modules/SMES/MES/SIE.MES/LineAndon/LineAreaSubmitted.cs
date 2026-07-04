using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.LineAndon
{
    public class LineAreaSubmitted : OnSubmitted<LineArea>
    {
        protected override void Invoke(LineArea entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                //当创建新的时候，就给它更新到产线与安灯区域中去
                RT.Service.Resolve<AndonLineController>().UpdateAndonLine(entity);
            }
        }
    }
}
