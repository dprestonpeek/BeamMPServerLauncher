using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeamMPServerLauncher
{
    public partial class ProgressWindow : Form
    {
        public static string outputLine = "";
        int maps = 0;
        string line = "";
        static string lastLine = "";
        int lastProgress = 0;

        public ProgressWindow()
        {
            InitializeComponent();
        }

        public void ImportMaps()
        {
            Show();
            backgroundWorker1.RunWorkerAsync();
        }

        public void ReadInMapOptions()
        {
            bool firstTime = true;
            string[] files = Directory.GetFiles(Main.mapDir);
            UpdateProgressWindow(1, "Need to unzip " + files.Length + " maps...");
            for (int i = 0; i < files.Length; i++)
            //foreach (string map in Directory.GetFiles(mapDir))
            {
                //update progress bar steps
                backgroundWorker1.ReportProgress(1);
                maps = files.Length;

                //create the new map object and save path
                BeamMap beamMap = new BeamMap();
                beamMap.path = files[i];
                //get the human readable name and save it
                string readableMap = Main.GetReadableName(files[i]);
                Main.AddToMapSelectorLater(readableMap);
                beamMap.name = readableMap;
                string[] unzippedInfo = SaveUnzippedInfo(beamMap);
                beamMap.unzippedInfo = unzippedInfo[0];
                beamMap.jsonInfo = unzippedInfo[1];
                beamMap.savedInfo = Path.Combine(beamMap.unzippedInfo, "info.json");
                beamMap.previews = GetStoredPreviews(beamMap);
                Main.AddToMaps(beamMap);

                //select first map in list
                if (firstTime)
                {
                    Main.SelectMapLater(beamMap);
                    firstTime = false;
                }
            }
            Main.loading = false;
        }

        private string[] GetStoredPreviews(BeamMap beamMap)
        {
            List<string> files = Directory.GetFiles(beamMap.unzippedInfo).ToList();
            files.Remove(Path.Combine(beamMap.unzippedInfo, "info.json"));
            Main.WriteToLog(files.ToArray());
            return files.ToArray();
        }

        private string[] ReadInPreviewFilenames(string infoPath)
        {
            string[] previews;
            string infoJson = File.ReadAllText(infoPath);
            //Regex reg = new Regex("[\"][p][r][e][v][i][e][w][s][\"][:][ ]*[[][\"]");
            string previewsString = "";
            if (infoJson.Contains("\"previews\":["))
            {
                previewsString = infoJson.Split("\"previews\":[")[1];
            }
            else if (infoJson.Contains("\"previews\": ["))
            {
                previewsString = infoJson.Split("\"previews\": [")[1];
            }
            previewsString = previewsString.Split("]")[0];
            previews = previewsString.Split(',');
            for (int i = 0; i < previews.Length; i++)
            {
                string cleanedPreview = previews[i].Trim();
                cleanedPreview = cleanedPreview.Replace("\"", "");
                previews[i] = cleanedPreview;
                Main.WriteToLog(cleanedPreview);
            }

            return previews;
        }

        private string[] SaveUnzippedInfo(BeamMap beamMap)
        {
            string unzippedInfo = Path.Combine(Main.launcherDataDir, beamMap.name);
            string infoJsonPath = "";
            if (!Directory.Exists(unzippedInfo))
            {
                string nameNoExt = Path.GetFileNameWithoutExtension(beamMap.path);
                string unzippedFile = Path.Combine(unzippedInfo, "zipFile");
                UpdateProgressWindow(lastProgress += 100 / maps, "Unzipping " + nameNoExt + "...");
                Directory.CreateDirectory(unzippedFile);
                ZipFile.ExtractToDirectory(beamMap.path, unzippedFile);

                string levelsParent = Path.Combine(unzippedFile, "levels");
                string[] dirs = Directory.GetDirectories(levelsParent);
                string levelsPath = Path.Combine(levelsParent, dirs[0]);
                string infoPath = Path.Combine(levelsPath, "info.json");
                string newInfoPath = Path.Combine(unzippedInfo, "info.json");
                File.Move(infoPath, newInfoPath);
                string[] previews = ReadInPreviewFilenames(newInfoPath);
                UpdateProgressWindow("Done unzipping " + nameNoExt + ".");
                foreach (string previewFile in previews)
                {
                    string previewPath = Path.Combine(levelsPath, previewFile);
                    File.Move(previewPath, Path.Combine(unzippedInfo, previewFile));
                }
                var dir = new DirectoryInfo(@unzippedFile);
                dir.Delete(true);

                infoJsonPath = Path.Combine("levels", Path.GetFileName(dirs[0]), "info.json");
                Main.UpdateJsonInfo(beamMap.name, infoJsonPath);
            }
            string[] unzipAndJson = { unzippedInfo, infoJsonPath };
            return unzipAndJson;
        }

        private void UpdateProgressWindow(int progressPercentage, string line)
        {
            this.line = line;
            backgroundWorker1.ReportProgress(progressPercentage);
        }

        private void UpdateProgressWindow(string line)
        {
            this.line = line;
            backgroundWorker1.ReportProgress(lastProgress);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadingProgressBar.Value = e.ProgressPercentage;
            if (line != lastLine)
            {
                LoadingDetails.Text += "\n" + line;
                lastLine = line;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadInMapOptions();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Main.instance.WindowState = FormWindowState.Normal;
            Main.instance.ContinueLoading();
            Close();
        }
    }
}
