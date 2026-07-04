using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIE.Domain.Validation;
using SIE.Web.Command;
using SIE.Web.Common.Export;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SIE.Web.EMS.Report.EquipmentMixReport.Commands
{
    /// <summary>
    /// 导出
    /// </summary>
    public class ExportMixCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行导出，保存至Excel
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //获取数据。因为涉及到多态转换，所以使用JObject的方式，逐个转换
            var json = JsonConvert.DeserializeObject<JObject>(args.Data);

            List<Bitmap> img = Base64StrToImage(json["Images"]);

            JObject dataView = json["GridData"] as JObject;

            EmsExcelHelper excel = new EmsExcelHelper();
            using (MemoryStream ms = excel.ExportData(dataView, img))
            {
                byte[] byteArr = ms.ToArray();
                ms.Close();

                return new
                {
                    FileName = "设备综合统计报表.xls",
                    FileContent = FileStreamHelper.ExcelToBase64(byteArr, FileType.xls)
                };
            }
        }

        /// <summary>
        /// 将Base64字符串转换为Image对象
        /// </summary>
        /// <param name="Images">base64字符串</param>
        /// <returns></returns>
        public static List<Bitmap> Base64StrToImage(JToken Images)
        {
            var images = Images.ToObject<List<JToken>>();
            List<Bitmap> result = new List<Bitmap>();
            images.ForEach(p =>
            {
                Bitmap bitmap = null;
                try
                {
                    var base64Str = p["data"].ToString();
                    //截掉无效部分
                    const string base64Discriminator = "base64,";
                    var base64Index = base64Str.IndexOf(base64Discriminator) + base64Discriminator.Length;
                    if (base64Index > -1)
                        base64Str = base64Str.Substring(base64Index);
                    byte[] arr = Convert.FromBase64String(base64Str);
                    MemoryStream ms = new MemoryStream(arr);
                    Bitmap bmp = new Bitmap(ms);
                    ms.Close();
                    bitmap = bmp;
                    result.Add(bitmap);
                }
                catch (Exception ex)
                {
                    throw new ValidationException("获取截图失败。原因：{0}".L10nFormat(ex.Message));
                }
            });
            return result;
        }
    }
}
