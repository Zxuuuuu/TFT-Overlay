using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using TFT_Overlay.Properties;

namespace TFT_Overlay
{
    public partial class App : Application
    {
        private void AutoUpdater(object sender, StartupEventArgs e)
        {
            string currentVersion = Version.version;

            using (WebClient client = new WebClient())
            {
                try
                {
                    string htmlCode = client.DownloadString("https://raw.githubusercontent.com/izoyo/TFT-Overlay/zh-CN/Version.cs");
                    int versionFind = htmlCode.IndexOf("public static string version = ");
                    string version = htmlCode.Substring(versionFind + 32, 5);
                    if (currentVersion != version && Settings.Default.AutoUpdate)
                    {
                        var result = MessageBox.Show($"现在已经发布新的版本啦~\n你需要下载 V{version} 吗？", "TFT Overlay Update Available", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            string link = "https://github.com/izoyo/TFT-Overlay/releases/download/V" + version + ".CN/V" + version + ".zip";
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            client.DownloadFile(new Uri(link), "TFTOverlay.zip");

                            var res = MessageBox.Show("zip文件已下载到您的本地目录，请解压并使用更新的版本。\n要打开本地目录吗？",
                                "Success", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (res == MessageBoxResult.Yes)
                            {
                                Process.Start(Directory.GetCurrentDirectory());
                            }
                        }
                        else if (result == MessageBoxResult.No)
                        {
                            Settings.FindAndUpdate("AutoUpdate", false);
                        }
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show(ex.ToString(), "报错啦!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}