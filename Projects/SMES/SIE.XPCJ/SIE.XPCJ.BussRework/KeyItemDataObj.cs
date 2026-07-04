using SIE.XPCJ.Models.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussRework
{
    public class KeyItemDataObj
    {
        public bool IsShowCheckBox { get; set; }

        public XPWipProductProcessKeyItem Item1 { get; set; }
        public XPWipProductProcessKeyItem Item2 { get; set; }
        public XPWipProductProcessKeyItem Item3 { get; set; }

        public static List<KeyItemDataObj> GenByXPWipProductProcessKeyItem(List<XPWipProductProcessKeyItem> list, bool isShowCheckBox)
        {
            int row = list.Count / 3;
            int left = list.Count % 3;

            List<KeyItemDataObj> result = new List<KeyItemDataObj>();

            for (int i= 0; i < row; i++)
            {
                KeyItemDataObj obj = new KeyItemDataObj();
                obj.IsShowCheckBox = isShowCheckBox;
                obj.Item1 = list[i * 3];
                obj.Item2 = list[i * 3 + 1];
                obj.Item3 = list[i * 3 + 2];
                result.Add(obj);
            }

            if (left == 1)
            {
                KeyItemDataObj obj = new KeyItemDataObj();
                obj.IsShowCheckBox = isShowCheckBox;
                obj.Item1 = list[row * 3];
                obj.Item2 = null;
                obj.Item3 = null;
                result.Add(obj);
            }
            else if (left == 2)
            {
                KeyItemDataObj obj = new KeyItemDataObj();
                obj.IsShowCheckBox = isShowCheckBox;
                obj.Item1 = list[row * 3];
                obj.Item2 = list[row * 3+1];
                obj.Item3 = null;
                result.Add(obj);
            }

            return result;
        }
    }
}
