using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect
{
    public static class MessageBox
    {
        /// <summary>
        /// 弹窗提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>

        public static async Task<ButtonResult> ShowMessage(string message, string title = "提示")
        {
            var box = MessageBoxManager
                 .GetMessageBoxStandard(title, message);

            return await box.ShowAsync();

        }

    }
}
