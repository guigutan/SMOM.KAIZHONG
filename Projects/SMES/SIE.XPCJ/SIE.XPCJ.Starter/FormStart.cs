using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Starter
{
    public partial class FormStart : Form
    {
        public FormStart()
        {
            InitializeComponent();
        }

        private string exePath;
        private string zipPath;

        private void FormStart_Load(object sender, EventArgs e)
        {
            exePath = Path.Combine(Application.StartupPath, StarterSettings.Instance.ExePath);
            exePath = Path.Combine(exePath, StarterSettings.Instance.ExeName);
            zipPath = Path.Combine(Application.StartupPath, StarterSettings.Instance.ExePath + ".zip");

            if (File.Exists(zipPath))
                File.Delete(zipPath);

            //http://192.168.175.208:9002/prod-10.1/AppMenuAttachment/config.json
            UpdaterHelper.GetUrlContent(this, StarterSettings.Instance.VersionUrl, GetVersionCallBack);
        }

        private VersionInfo remoteVersionInfo;
        private void GetVersionCallBack<T>(UpdaterResult<T> result, string apiType, string method, string postData)
        {
            try
            {
                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                    Process.Start(exePath);
                    Application.Exit();
                    return;
                }

                string s = result.Result as string;
                remoteVersionInfo = JsonConvert.DeserializeObject<VersionInfo>(s);
                VersionInfo localVersionInfo = VersionInfo.GetLocalVersion();


                //if (CheckVersionNeedUpdate(fileVersionInfo, remoteVersionInfo)) //不再使用文件版本
                if (CheckVersionNeedUpdate(localVersionInfo, remoteVersionInfo)) //使用自定义文件来实现版本比较
                {
                    //开始下载更新包
                    this.DownLoadZipFile(remoteVersionInfo.Version);
                }
                else
                {
                    Process.Start(exePath);
                    Application.Exit();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Process.Start(exePath);
                Application.Exit();
            }
        }

        /// <summary>
        /// 验证版本是否需要更新
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        private bool CheckVersionNeedUpdate(FileVersionInfo local, VersionInfo remote)
        {
            Version remoteVersion = new Version(remote.Version);

            if (local.FileMajorPart < remoteVersion.Major)
                return true;

            if (local.FileMajorPart > remoteVersion.Major)
                return false;

            //local.FileMajorPart == remote.Major
            if (local.FileMinorPart < remoteVersion.Minor)
                return true;

            if (local.FileMinorPart > remoteVersion.Minor)
                return false;

            //local.FileMajorPart == remote.Major && local.FileMinorPart == remote.Minor
            if (local.FileBuildPart < remoteVersion.Build)
                return true;

            if (local.FileBuildPart > remoteVersion.Build)
                return false;

            //local.FileMajorPart == remote.Major && local.FileMinorPart == remote.Minor && local.FileBuildPart == remote.Build
            if (local.FilePrivatePart < remoteVersion.Revision)
                return true;

            if (local.FilePrivatePart > remoteVersion.Revision)
                return false;

            return false;
        }

        /// <summary>
        /// 验证版本是否需要更新
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        private bool CheckVersionNeedUpdate(Version local, Version remote)
        {
            if (local.Major < remote.Major)
                return true;

            if (local.Major > remote.Major)
                return false;

            //local.Minor == remote.Major
            if (local.Minor < remote.Minor)
                return true;

            if (local.Minor > remote.Minor)
                return false;

            //local.Major == remote.Major && local.Minor == remote.Minor
            if (local.Build < remote.Build)
                return true;

            if (local.Build > remote.Build)
                return false;

            //local.Major == remote.Major && local.Minor == remote.Minor && local.Build == remote.Build
            if (local.Revision < remote.Revision)
                return true;

            if (local.Revision > remote.Revision)
                return false;

            return false;
        }

        private bool CheckVersionNeedUpdate(VersionInfo local, VersionInfo remote)
        {
            Version localVersion = new Version(local.Version);
            Version remoteVersion = new Version(remote.Version);

            return CheckVersionNeedUpdate(localVersion, remoteVersion);
        }
        
        private void DownLoadZipFile(string remoteVersion)
        {
            this.labelMsg.Text = $"正在下载版本 V{remoteVersion}，请耐心等待......";
            UpdaterHelper.DowloadFile(this, StarterSettings.Instance.FileUrl, zipPath, DownloadFileCallBack);
        }

        public void ExtractZipFile(string zipFilePath, string extractPath)
        {
            using (ZipFile zipFile = new ZipFile(zipFilePath))
            {
                foreach (ZipEntry entry in zipFile)
                {
                    if (!entry.IsFile)
                    {
                        // 创建目录（包括子目录）
                        Directory.CreateDirectory(Path.Combine(extractPath, entry.Name));
                        continue;
                    }

                    string entryFileName = entry.Name;
                    string fullZipToPath = Path.Combine(extractPath, entryFileName);

                    // 创建目录
                    Directory.CreateDirectory(Path.GetDirectoryName(fullZipToPath));

                    // 解压文件
                    using (Stream zipStream = zipFile.GetInputStream(entry))
                    using (FileStream fileStream = File.Create(fullZipToPath))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }

        private void DownloadFileCallBack<T>(UpdaterResult<T> result, string apiType, string method, string postData)
        {
            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                Process.Start(exePath);
                Application.Exit();
                return;
            }

            //开始解压文件
            try
            {
                ExtractZipFile(zipPath, Application.StartupPath);
                Process.Start(exePath);
                VersionInfo.SaveLocalVersion(remoteVersionInfo); //更新本地的版本文件
                Application.Exit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Process.Start(exePath);
                Application.Exit();
                return;
            }
        }
    }
}
