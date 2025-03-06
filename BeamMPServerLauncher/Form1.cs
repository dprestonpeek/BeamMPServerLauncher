using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BeamMPServerLauncher
{
    public struct BeamMap
    {
        public string path;
        public string name;
        public string savedInfo;
        public string jsonInfo;
        public string[] previews;
        public string unzippedInfo;
    }

    public partial class Main : Form
    {
        public static BeamMap selectedMap;

        public static string mapDir = Path.Combine(Directory.GetCurrentDirectory(), "maps");
        public static string modDir = Path.Combine(Directory.GetCurrentDirectory(), "mods");
        public static string launcherDataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
        public static string serverConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "ServerConfig.toml");
        public static string resourceDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Client");
        public static string logFile = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
        public static string jsonInfo = Path.Combine(launcherDataDir, "json.txt");

        public static Main instance;
        public static bool loading = true;

        public static List<BeamMap> maps = new List<BeamMap>();
        ProgressWindow progress;
        Task progressBarTask;
        Process serverProcess;

        public static BeamMap mapToSelectLater = new BeamMap();
        public static List<string> mapNamesToAddLater = new List<string>();

        public Main()
        {
            instance = this;
            progress = new ProgressWindow();
            try
            {
                InitializeComponent();
                MakeDirs();
                progress.ImportMaps();
            }
            catch (Exception ex)
            {
                ErrorWindow error = new ErrorWindow(ErrorCode.PassMessage, ex.Message);
                WriteToLog(ex);
                return;
            }
        }

        public void ContinueLoading()
        {
            try
            {
                if (mapToSelectLater.name != "")
                {
                    SelectMap(mapToSelectLater);
                }
                if (mapNamesToAddLater.Count > 1)
                {
                    foreach (string name in mapNamesToAddLater)
                    {
                        AddToMapSelector(name);
                    }
                }
                ReadInJsonFileLocations();

                ReadInServerInfo(selectedMap);
                if (progress != null)
                {
                    progress.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorWindow error = new ErrorWindow(ErrorCode.PassMessage, ex.Message);
                WriteToLog(ex);
                return;
            }
            Enabled = true;
            SaveConfigButton.Enabled = true;
            StartButton.Enabled = true;
        }

        public static void UpdateJsonInfo(string mapName, string jsonPath)
        {
            if (!File.Exists(jsonInfo))
            {
                File.WriteAllText(jsonInfo, "");
            }
            List<string> lines = File.ReadAllLines(jsonInfo).ToList();
            foreach (string line in lines)
            {
                if (line.Contains(mapName + " = "))
                {
                    return;
                }
            }
            lines.Add(mapName + " = " + jsonPath);
            File.WriteAllLines(jsonInfo, lines);
        }

        public void RemoveSelectedMap()
        {
            RemoveMap(selectedMap);
        }

        private void RemoveMap(BeamMap map)
        {
            var dir = new DirectoryInfo(@map.unzippedInfo);
            dir.Delete(true);
            File.Delete(map.path);
            int index = MapSelector.SelectedIndex;
            MapSelector.Items.Remove(map.name);
            maps.Remove(map);

            if (index == 0)
            {
                MapSelector.SelectedIndex = index + 1;
            }
            else if (index > 0)
            {
                MapSelector.SelectedIndex = index - 1;
            }
        }

        private void ReadInJsonFileLocations()
        {
            if (!File.Exists(jsonInfo))
            {
                File.WriteAllText(jsonInfo, "");
            }
            List<string> lines = File.ReadAllLines(jsonInfo).ToList();
            foreach (string line in lines)
            {
                string[] split = line.Split(" = ");
                string name = split[0];
                string json = split[1];
                for (int i = 0; i < maps.Count; i++)
                {
                    if (maps[i].name == name)
                    {
                        bool selected = selectedMap.Equals(maps[i]);
                        BeamMap newMap = new BeamMap();
                        newMap.name = maps[i].name;
                        newMap.savedInfo = maps[i].savedInfo;
                        newMap.path = maps[i].path;
                        newMap.previews = maps[i].previews;
                        newMap.unzippedInfo = maps[i].unzippedInfo;
                        newMap.jsonInfo = json;
                        maps[i] = newMap;
                        if (selected)
                        {
                            SelectMap(maps[i]);
                        }
                    }
                }
            }
        }

        public static string GetReadableName(string path)
        {
            string readableName = Path.GetFileNameWithoutExtension(path);
            readableName = readableName.Replace("_", " ");
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            readableName = myTI.ToTitleCase(readableName);
            return readableName;
        }

        void AddToMapSelector(string readableMap)
        {
            MapSelector.Items.Add(readableMap);
        }

        public static void AddToMapSelectorLater(string readableMap)
        {
            mapNamesToAddLater.Add(readableMap);
        }

        public static void AddToMaps(BeamMap beamMap)
        {
            maps.Add(beamMap);
        }

        public static void WriteToLog(string message)
        {
            string file = "";
            if (File.Exists(logFile))
            {
                file = File.ReadAllText(logFile);
                file += "\n\n---\n\n";
            }
            file += DateTime.Now + " " + message;
            File.WriteAllText(logFile, file);
        }

        public static void WriteToLog(string[] messages)
        {
            string file = "";
            if (File.Exists(logFile))
            {
                file = File.ReadAllText(logFile);
                file += "\n\n---\n\n";
            }
            file += DateTime.Now;
            foreach (string message in messages)
            {
                file += "\t" + message + "\n";
            }
            File.WriteAllText(logFile, file);
        }

        public static void WriteToLog(Exception exception)
        {
            string file = "";
            if (File.Exists(logFile))
            {
                file = File.ReadAllText(logFile);
                file += "\n\n---\n\n";
            }
            file += DateTime.Now + " " + exception.Message + " : " + exception.Data;
            File.WriteAllText(logFile, file);
        }

        private void MakeDirs()
        {
            if (!Directory.Exists(mapDir))
            {
                Directory.CreateDirectory(mapDir);
            }
            if (!Directory.Exists(modDir))
            {
                Directory.CreateDirectory(modDir);
            }
            if (!Directory.Exists(launcherDataDir))
            {
                Directory.CreateDirectory(launcherDataDir);
            }
        }

        void SetNewPreviewFile(BeamMap beamMap)
        {
            //is the preview image already set to one of these images?
            if (MapPreview.ImageLocation != null && beamMap.previews.Contains(MapPreview.ImageLocation))
            {
                //find out which one it's set to
                for (int i = 0; i < beamMap.previews.Length; i++)
                {
                    if (beamMap.previews[i] == MapPreview.ImageLocation)
                    {
                        //if a "next" image exists, set it
                        if (beamMap.previews.Length > i + 1)
                        {
                            MapPreview.ImageLocation = beamMap.previews[i + 1];
                            return;
                        }
                        //otherwise reset to 0
                        else
                        {
                            MapPreview.ImageLocation = beamMap.previews[0];
                            return;
                        }
                    }
                }
            }
            else if (MapPreview.ImageLocation == null)
            {
                try
                {
                    MapPreview.ImageLocation = beamMap.previews[0];
                }
                catch (IndexOutOfRangeException ex)
                {
                    ErrorWindow errorWindow = new ErrorWindow(ErrorCode.PreviewsNotFound, ex.Message);
                    WriteToLog("Had a problem loading image previews.\n\tMapPreview.ImageLocation = " + MapPreview.ImageLocation + "\n\t" + ex.Message);
                }
            }
            else
            {
                MapPreview.ImageLocation = beamMap.previews[0];
            }
        }

        void SelectMap(BeamMap beamMap)
        {
            selectedMap = beamMap;
            instance.MapSelector.Text = beamMap.name;
            SetNewPreviewFile(selectedMap);
        }

        void SelectMap(string mapName)
        {
            foreach (BeamMap map in maps)
            {
                if (map.name == mapName)
                {
                    SelectMap(map);
                    return;
                }
            }
        }

        public static void SelectMapLater(BeamMap beamMap)
        {
            mapToSelectLater = beamMap;
        }

        private void ReadInServerInfo(BeamMap beamMap)
        {
            //get server.toml
            string serverConfig = "";
            try
            {
                serverConfig = File.ReadAllText(serverConfigFile);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                ErrorWindow error = new ErrorWindow(ErrorCode.ServerNotFound, ex.Message);
                return;
            }
            catch (Exception ex)
            {
                ErrorWindow error = new ErrorWindow(ErrorCode.Unknown, ex.Message);
                return;
            }

            string nameStr = serverConfig.Split("Name = \"")[1];
            nameStr = nameStr.Split("\"")[0];
            ServerNameBox.Text = nameStr.Trim();

            string descStr = serverConfig.Split("Description = \"")[1];
            descStr = descStr.Split("\"")[0];
            ServerDescBox.Text = descStr.Trim();

            string privateStr = serverConfig.Split("Private = ")[1];
            privateStr = privateStr.Split("\n")[0];
            PrivateCheckbox.Checked = bool.Parse(privateStr);
            string fullpvtString = "Private = " + privateStr;

            string offlineStr = "";
            try
            {
                offlineStr = serverConfig.Split("Offline = ")[1];
            }
            catch (IndexOutOfRangeException ex)
            {
                serverConfig = serverConfig.Replace(fullpvtString, fullpvtString + "\n" + "Offline = false");
                File.WriteAllText(serverConfigFile, serverConfig);
                offlineStr = serverConfig.Split("Offline = ")[1];
            }
            offlineStr = offlineStr.Split("\n")[0];
            OfflineCheckbox.Checked = bool.Parse(offlineStr);

            string maxCarsStr = serverConfig.Split("MaxCars = ")[1];
            maxCarsStr = maxCarsStr.Split("\n")[0];
            MaxCarsBox.Text = maxCarsStr.Trim();

            string maxPlayersStr = serverConfig.Split("MaxPlayers = ")[1];
            maxPlayersStr = maxPlayersStr.Split("\n")[0];
            MaxPlayersBox.Text = maxPlayersStr.Trim();
        }

        private void ConfigureServer()
        {
            string serverConfig = File.ReadAllText(serverConfigFile);

            //Server name
            string nameStr = serverConfig.Split("Name = \"")[1];
            nameStr = nameStr.Split("\"")[0];
            nameStr = nameStr.Trim();
            serverConfig = serverConfig.Replace("Name = \"" + nameStr + "\"", "Name = \"" + ServerNameBox.Text.Trim() + "\"");

            //server desc
            string descStr = serverConfig.Split("Description = \"")[1];
            descStr = descStr.Split("\"")[0];
            descStr = descStr.Trim();
            serverConfig = serverConfig.Replace("Description = \"" + descStr + "\"", "Description = \"" + ServerDescBox.Text.Trim() + "\"");

            //private or public server
            string privateStr = serverConfig.Split("Private = ")[1];
            privateStr = privateStr.Split("\n")[0];
            privateStr = privateStr.Trim();
            serverConfig = serverConfig.Replace("Private = " + privateStr, "Private = " + PrivateCheckbox.Checked.ToString().Trim().ToLower());
            string fullpvtString = "Private = " + privateStr;

            //offline server option
            string offlineStr = "";
            try
            {
                offlineStr = serverConfig.Split("Offline = ")[1];
            }
            catch (IndexOutOfRangeException ex)
            {
                serverConfig = serverConfig.Replace(fullpvtString, fullpvtString + "\n" + "Offline = false");
                File.WriteAllText(serverConfigFile, serverConfig);
                offlineStr = serverConfig.Split("Offline = ")[1];
            }
            offlineStr = offlineStr.Split("\n")[0];
            offlineStr = offlineStr.Trim();
            serverConfig = serverConfig.Replace("Offline = " + offlineStr, "Offline = " + OfflineCheckbox.Checked.ToString().Trim().ToLower());

            //max cars per player
            string maxCarsStr = serverConfig.Split("MaxCars = ")[1];
            maxCarsStr = maxCarsStr.Split("\n")[0];
            maxCarsStr = maxCarsStr.Trim();
            serverConfig = serverConfig.Replace("MaxCars = " + maxCarsStr, "MaxCars = " + MaxCarsBox.Text.Trim());

            //max players
            string maxPlayersStr = serverConfig.Split("MaxPlayers = ")[1];
            maxPlayersStr = maxPlayersStr.Split("\n")[0];
            maxPlayersStr = maxPlayersStr.Trim();
            serverConfig = serverConfig.Replace("MaxPlayers = " + maxPlayersStr, "MaxPlayers = " + MaxPlayersBox.Text.Trim());

            File.WriteAllText(serverConfigFile, serverConfig);

            //Map info file
            string[] serverConfigLines = File.ReadAllLines(serverConfigFile);
            for (int i = 0; i < serverConfigLines.Length; i++)
            {
                if (serverConfigLines[i].Contains("Map = "))
                {
                    selectedMap.jsonInfo = selectedMap.jsonInfo.Replace("\\", "/");
                    serverConfigLines[i] = "Map = \"/" + selectedMap.jsonInfo.Trim() + "\"";
                }
            }
            File.WriteAllLines(serverConfigFile, serverConfigLines);
        }

        private void ConfigureMods()
        {
            foreach (string file in Directory.GetFiles(modDir))
            {
                string resourceFile = Path.Combine(resourceDir, Path.GetFileName(file));
                if (!File.Exists(resourceFile))
                {
                    File.Copy(file, resourceFile, false);
                }
            }
            string mapResourceFile = Path.Combine(resourceDir, Path.GetFileName(selectedMap.path));
            //ensure resource dir exists
            if (!Directory.Exists(resourceDir))
            {
                Directory.CreateDirectory(resourceDir);
            }
            if (!File.Exists(mapResourceFile))
            {
                //delete any pre-existing map files
                string[] resourceFiles = Directory.GetFiles(resourceDir);
                foreach (string file in resourceFiles)
                {
                    for (int i = 0; i < maps.Count; i++)
                    {
                        if (Path.GetFileName(maps[i].path) == Path.GetFileName(file))
                        {
                            File.Delete(file);
                        }
                    }
                }
                File.Copy(selectedMap.path, mapResourceFile);
            }
        }

        private void SourceButton_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }

        private void MapSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectMap((string)MapSelector.SelectedItem);
        }

        private void NextPreviewButton_Click(object sender, EventArgs e)
        {
            SetNewPreviewFile(selectedMap);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ConfigureServer();
            ConfigureMods();
            try
            {
                serverProcess = Process.Start("BeamMP-Server.exe");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ErrorCode.BeamMPServerNotFound, ex.Message);
            }
            StopServerButton.Enabled = true;
            RestartButton.Enabled = true;
            StartButton.Enabled = false;
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            ConfigureServer();
            ConfigureMods();
        }

        private void Main_Shown(object sender, EventArgs e)
        {
        }

        private void Main_Enter(object sender, EventArgs e)
        {
        }

        private void StopServerButton_Click(object sender, EventArgs e)
        {
            serverProcess.Kill();
            serverProcess.Dispose();
            StopServerButton.Enabled = false;
            RestartButton.Enabled = false;
            StartButton.Enabled = true;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            serverProcess.Kill();
            serverProcess.Dispose();
            serverProcess = Process.Start("BeamMP-Server.exe");
        }

        private void RemoveMapButton_Click(object sender, EventArgs e)
        {
            ErrorWindow error = new ErrorWindow(ErrorCode.AreYouSureDelete, "");
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
