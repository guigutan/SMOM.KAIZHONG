using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;

namespace SIE.Wpf.ESop.Common
{
    /// <summary>
    /// 路径扩展类
    /// </summary>
    public static class UriExtensions
    { 
        /// <summary>  
        /// 通过NET获取网络图片  
        /// </summary>  
        /// <param name="url">要访问的图片所在网址</param>  
        /// <param name="requestAction">对于WebRequest需要进行的一些处理，比如代理、密码之类</param>  
        /// <param name="responseFunc">如何从WebResponse中获取到图片</param>  
        /// <returns>返回图片</returns>  
        public static Image GetImageFromNet(this string url, Action<WebRequest> requestAction = null, Func<WebResponse, Image> responseFunc = null)
        {
            return new Uri(url).GetImageFromNet(requestAction, responseFunc);
        }

        /// <summary>  
        /// 通过NET获取网络图片  
        /// </summary>  
        /// <param name="url">要访问的图片所在网址</param>  
        /// <param name="requestAction">对于WebRequest需要进行的一些处理，比如代理、密码之类</param>  
        /// <param name="responseFunc">如何从WebResponse中获取到图片</param>  
        /// <returns>返回图片</returns>  
        public static Image GetImageFromNet(this Uri url, Action<WebRequest> requestAction = null, Func<WebResponse, Image> responseFunc = null)
        {
            Image img = null;
            try
            {
                if (url.IsFile)
                {
                    WebRequest request = WebRequest.Create(url);
                    if (requestAction != null)
                    {
                        requestAction(request);
                    }

                    using (WebResponse response = request.GetResponse())
                    {
                        if (responseFunc != null)
                        {
                            img = responseFunc(response);
                        }
                        else
                        {
                            img = Image.FromStream(response.GetResponseStream());
                        }
                    }
                }
                else
                {
                    img = Image.FromFile(url.LocalPath);
                }
            }
            catch
            {
                img = null;
            }

            return img;
        }

        /// <summary>
        /// 从url读取内容到内存MemoryStream流中
        /// </summary>
        /// <param name="url">路径</param>
        /// <returns>返回MemoryStream</returns>
        public static MemoryStream UirFileToMemoryStream(this string url)
        {
            return new Uri(url).UirFileToMemoryStream();
        }

        /// <summary>
        /// 从url读取内容到内存MemoryStream流中
        /// </summary>
        /// <param name="url">路径</param>
        /// <returns>返回MemoryStream</returns>
        public static MemoryStream UirFileToMemoryStream(this Uri url)
        {
            MemoryStream ms = null;
            if (url.HostNameType == UriHostNameType.Dns)
            {
                var wreq = HttpWebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = wreq.GetResponse() as HttpWebResponse;
                using (var stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[response.ContentLength];
                    int offset = 0, actuallyRead = 0;
                    do
                    {
                        actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                        offset += actuallyRead;
                    }
                    while (actuallyRead > 0);
                    ms = new MemoryStream(buffer);
                }

                response.Close();
            }
            else if (url.HostNameType == UriHostNameType.Basic && url.Scheme == "file")
            {
                using (BinaryReader loader = new BinaryReader(File.OpenRead(url.LocalPath)))
                {
                    FileInfo fd = new FileInfo(url.LocalPath);
                    int length = (int)fd.Length;
                    byte[] buf = loader.ReadBytes(length);
                    loader.Close();
                    ms = new MemoryStream(buf);
                }
            }
            else
            {
                var rs = Application.GetResourceStream(url);
                byte[] bytes = new byte[rs.Stream.Length];
                int read;
                ms = new MemoryStream();
                while ((read = rs.Stream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    ms.Write(bytes, 0, read);
                }
                rs.Stream.Close();
            }

            return ms;
        }
    }
}
