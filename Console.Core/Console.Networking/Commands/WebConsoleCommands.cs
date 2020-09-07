using System.Diagnostics;
using System.IO;
using System.Net;

using Console.Core;
using Console.Core.CommandSystem.Attributes;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

namespace Console.Networking.Commands
{
    /// <summary>
    /// Commands used for Downloading Files or Running a Script from URL
    /// </summary>
    public static class WebConsoleCommands
    {

        private static readonly WebClient Client = new WebClient();
        private static readonly ALogger WebLogger = TypedLogger.CreateTypedWithPrefix("web");

        private static string DownloadString;

        [Property("networking.download.displayprogress")]
        private static readonly bool DisplayProgress = true;

        private static readonly Stopwatch DownloadDisplayTimer = new Stopwatch();

        [Property("networking.download.displayinterval")]
        private static readonly int DownloadDisplayTime = 500;


        static WebConsoleCommands()
        {
            AConsoleManager.OnConsoleTick += AConsoleManager_OnConsoleTick;
            Client.DownloadFileCompleted += Client_DownloadFileCompleted;
            Client.DownloadProgressChanged += Client_DownloadProgressChanged;
        }

        [Property("networking.download.isbusy")]
        private static bool IsDownloading => Client.IsBusy;

        private static void SetClientDownloadString(DownloadProgressChangedEventArgs e)
        {
            DownloadString =
                $"Downloading {e.ProgressPercentage}% {e.BytesReceived / 1024}KB/{e.TotalBytesToReceive / 1024}KB";
        }

        private static void AConsoleManager_OnConsoleTick()
        {
            if (IsDownloading)
            {
                if (DownloadDisplayTimer.ElapsedMilliseconds > DownloadDisplayTime)
                {
                    DownloadDisplayTimer.Restart();
                    if (DownloadString != null)
                    {
                        WebLogger.Log(DownloadString);
                    }
                }
            }
        }

        private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (DisplayProgress)
            {
                SetClientDownloadString(e);
            }
            else
            {
                DownloadString = null;
            }
        }

        private static void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                WebLogger.LogWarning("Web Client encountered an Error Downloading Resources: " + e.Error);
                return;
            }

            if (e.Cancelled)
            {
                WebLogger.LogWarning("Web Client Download was Aborted by the User");
                return;
            }

            WebLogger.LogWarning("Download Completed");
        }

        [Command(
            "wget",
            Namespace = NetworkingSettings.NETWORKING_HOST_NAMESPACE,
            HelpMessage = "Downloads the Specified Web Resource"
        )]
        private static void WGet(
            string url, string destination, [CommandFlag]
            bool overwrite)
        {
            if (!NetworkingSettings.WGetAllow)
            {
                WebLogger.LogWarning("Can not use wget command when \"networking.wget.enabled\" is set to false.");
                return;
            }

            if (IsDownloading)
            {
                WebLogger.LogWarning(
                                     "Can not use wget command when the Underlying Client is Busy, is there a WGET Operation Running?"
                                    );
                return;
            }

            if (File.Exists(destination))
            {
                if (!overwrite)
                {
                    WebLogger.LogWarning("Can not download File as it already exists");
                    return;
                }

                WebLogger.Log("Overwriting File: " + destination);
                File.Delete(destination);
            }

            DownloadDisplayTimer.Restart();
            Client.DownloadFileTaskAsync(url, destination);
        }

        [Command(
            "run-url",
            Namespace = NetworkingSettings.NETWORKING_HOST_NAMESPACE,
            HelpMessage = "Runs a Script file from a Url"
        )]
        private static void RunUrl(string url, string parameter)
        {
            if (!NetworkingSettings.RunUrlAllow)
            {
                WebLogger.LogWarning("Can not use run-url command when \"networking.runurl.enabled\" is set to false.");
                return;
            }

            DownloadDisplayTimer.Restart();
            string file = Path.GetRandomFileName();
            WebLogger.Log("Downloading Url: " + url);
            Client.DownloadFile(url, file);
            WebLogger.Log("Running File: " + file);
            AConsoleManager.Instance.EnterCommand($"run \"{file}\" \"{parameter}\"");
            File.Delete(file);
        }

    }
}