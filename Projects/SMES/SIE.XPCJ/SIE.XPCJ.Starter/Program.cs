using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace SIE.XPCJ.Starter
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormStart());

            ////获取服务器版本
            //UpdateVersion();

            ////验证执行文件是否存在, 再更新一次
            //if (!File.Exists("SIE.XPCJ.exe"))
            //{
            //    UpdateVersion();
            //}

            //解压文件
            //UnZip();

            ////启动程序
            //Process.Start("SIE.XPCJ.exe");
        }

        public static int serverVersionCode = 0;

        /// <summary>
        /// 解压文件
        /// </summary>
        static void UnZip()
        { 
        }

        public static void UpdateVersion()
        {
            int localVersionCode = 0;
            if (File.Exists("res//local-version-exe.txt"))
            {
                string localVersionStr = File.ReadAllText("res//local-version-exe.txt");
                int.TryParse(localVersionStr, out localVersionCode);
            }

            if (!File.Exists("res//UpdateServerConfig.txt"))
                return;

            string[] arr = File.ReadAllLines("res//UpdateServerConfig.txt");

            if (arr.Length <= 0 || string.IsNullOrEmpty(arr[0]))
                return;

            string url = arr[0] + "/published/version-exe.txt";
            try
            {
                string serverVersion = GetResponse(url);
                if (string.IsNullOrEmpty(serverVersion))
                    return;

                int.TryParse(serverVersion, out serverVersionCode);
                if (localVersionCode >= serverVersionCode)
                    return;

                url = arr[0] + $"/published/exe/{serverVersionCode}.exe";
                DownloadFile(url, "_.exe", serverVersion);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static string Get(string url)
        {
            if (!url.StartsWith("http::") && !url.StartsWith("https::"))
                url = "http://" + url;

            var wbRequest = (HttpWebRequest)WebRequest.Create(url);
            wbRequest.Method = "GET";
            HttpWebResponse responseResult = (HttpWebResponse)wbRequest.GetResponse();

            using (Stream responseStream = responseResult.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string res = reader.ReadToEnd();
                return res;
            }
        }

        public static string GetResponse(string url)
        {
            //using (HttpClient client = new HttpClient())
            //{
            //    try
            //    {
            //        HttpResponseMessage response = client.GetAsync(url).Result;

            //        if (response.IsSuccessStatusCode)
            //        {
            //            string responseBody = response.Content.ReadAsStringAsync().Result;
            //            return responseBody;
            //        }
            //        else
            //        {
            //            //throw new Exception($"HTTP request failed with status code: {response.StatusCode}");
            //            return string.Empty;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return string.Empty;
            //    }
            //}
            return "";
        }

        public static void DownloadFile(string url, string filePath, string serverVersion)
        {
            //using (HttpClient client = new HttpClient())
            //{
            //    try
            //    {
            //        HttpResponseMessage response = client.GetAsync(url).Result;

            //        if (response.IsSuccessStatusCode)
            //        {
            //            byte[] contentBytes = response.Content.ReadAsByteArrayAsync().Result;
            //            File.WriteAllBytes(filePath, contentBytes);
            //            File.WriteAllText("res//local-version-exe.txt", serverVersion);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return;
            //    }
            //}
        }
    }
}
