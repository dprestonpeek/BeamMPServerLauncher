using System;
using System.Diagnostics;
using System.Globalization;
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

        string mapDir = Path.Combine(Directory.GetCurrentDirectory(), "maps");
        string modDir = Path.Combine(Directory.GetCurrentDirectory(), "mods");
        string launcherDataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
        string serverConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "ServerConfig.toml");
        string resourceDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Client");
        string logFile = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");

        List<BeamMap> maps = new List<BeamMap>();

        public Main()
        {
            try
            {
                InitializeComponent();
                MakeDirs();
                ReadInMapOptions();
                ReadInServerInfo(selectedMap);
            }
            catch(Exception ex)
            {
                ErrorWindow error = new ErrorWindow(ErrorCode.PassMessage, ex.Message);
                WriteToLog(ex);
                return;
            }
        }

        private void WriteToLog(string message)
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

        private void WriteToLog(string[] messages)
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

        private void WriteToLog(Exception exception) 
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

            string[] dirs = { "mapDir = " + mapDir, "modDir = " + modDir, "dataDir = " + launcherDataDir, "serverConfig = " + serverConfigFile,
                    "resourceDir = " + resourceDir, "logFile = " + logFile };
            //WriteToLog(dirs);
        }

        private string[] ReadInPreviewFilenames(string infoPath)
        {
            string[] previews;
            string infoJson = File.ReadAllText(infoPath);
            string previewsString = infoJson.Split("\"previews\":[")[1];
            previewsString = previewsString.Split("]")[0];
            previews = previewsString.Split(',');
            for (int i = 0; i < previews.Length; i++)
            {
                string cleanedPreview = previews[i].Trim();
                cleanedPreview = cleanedPreview.Replace("\"", "");
                previews[i] = cleanedPreview;
                WriteToLog(cleanedPreview);
            }

            return previews;
        }

        private string[] GetStoredPreviews(BeamMap beamMap)
        {
            List<string> files = Directory.GetFiles(beamMap.unzippedInfo).ToList();
            files.Remove(Path.Combine(beamMap.unzippedInfo, "info.json"));
            WriteToLog(files.ToArray());
            return files.ToArray();
        }

        private void SetNewPreviewFile(BeamMap beamMap)
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
                catch(IndexOutOfRangeException ex)
                {
                    ErrorWindow errorWindow = new ErrorWindow(ErrorCode.PreviewsNotFound, ex.Message);
                    WriteToLog("Had a problem loading image previews.\n\tMapPreview.ImageLocation = " + MapPreview.ImageLocation + "\n\t" + ex.Message);
                }
            }
        }

        private void SelectMap(BeamMap beamMap)
        {
            selectedMap = beamMap;
            MapSelector.Text = beamMap.name;
            SetNewPreviewFile(selectedMap);
        }

        private void SelectMap(string mapName)
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

        private void ReadInMapOptions()
        {
            bool firstTime = true;
            foreach (string map in Directory.GetFiles(mapDir))
            {
                //create the new map object and save path
                BeamMap beamMap = new BeamMap();
                beamMap.path = map;
                //get the human readable name and save it
                string readableMap = GetReadableName(map);
                MapSelector.Items.Add(readableMap);
                beamMap.name = readableMap;
                string[] unzippedInfo = SaveUnzippedInfo(beamMap);
                beamMap.unzippedInfo = unzippedInfo[0];
                beamMap.jsonInfo = unzippedInfo[1];
                beamMap.savedInfo = Path.Combine(beamMap.unzippedInfo, "info.json");
                beamMap.previews = GetStoredPreviews(beamMap);

                //select first map in list
                if (firstTime)
                {
                    SelectMap(beamMap);
                    firstTime = false;
                }
            }
        }

        private string[] SaveUnzippedInfo(BeamMap beamMap)
        {
            string unzippedInfo = Path.Combine(launcherDataDir, beamMap.name);
            string infoJsonPath = "";
            if (!Directory.Exists(unzippedInfo))
            {
                string unzippedFile = Path.Combine(unzippedInfo, "zipFile");
                Directory.CreateDirectory(unzippedFile);
                ZipFile.ExtractToDirectory(beamMap.path, unzippedFile);

                string nameNoExt = Path.GetFileNameWithoutExtension(beamMap.path);
                string levelsParent = Path.Combine(unzippedFile, "levels");
                string[] dirs = Directory.GetDirectories(levelsParent);
                string levelsPath = Path.Combine(levelsParent, dirs[0]);
                string infoPath = Path.Combine(levelsPath, "info.json");
                string newInfoPath = Path.Combine(unzippedInfo, "info.json");
                File.Move(infoPath, newInfoPath);
                string[] previews = ReadInPreviewFilenames(newInfoPath);
                foreach (string previewFile in previews)
                {
                    string previewPath = Path.Combine(levelsPath, previewFile);
                    File.Move(previewPath, Path.Combine(unzippedInfo, previewFile));
                }
                var dir = new DirectoryInfo(@unzippedFile);
                dir.Delete(true);

                infoJsonPath = Path.Combine("levels", Path.GetFileName(dirs[0]), "info.json");
            }
            string[] unzipAndJson = { unzippedInfo, infoJsonPath };
            return unzipAndJson;
        }

        private string GetReadableName(string path)
        {
            string readableName = Path.GetFileNameWithoutExtension(path);
            readableName = readableName.Replace("_", " ");
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            readableName = myTI.ToTitleCase(readableName);
            return readableName;
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
            catch(Exception ex)
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
            serverConfig = serverConfig.Replace("Private = " + privateStr, "Private = " + PrivateCheckbox.Checked.ToString().Trim());

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

            //Map info file
            string infoFile = serverConfig.Split("Map = \"")[1];
            infoFile = infoFile.Split("\"")[0];
            infoFile = infoFile.Trim();
            try
            {
                serverConfig = serverConfig.Replace("Map = \"" + infoFile + "\"", "Map = \"" + selectedMap.jsonInfo.Trim() + "\"");
            }
            catch(System.NullReferenceException ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ErrorCode.MapNotFound, ex.Message);
                return;
            }

            File.WriteAllText(serverConfigFile, serverConfig);
        }

        private void ConfigureMods()
        {
            foreach(string file in Directory.GetFiles(modDir))
            {
                string resourceFile = Path.Combine(resourceDir, Path.GetFileName(file));
                if (!File.Exists(resourceFile))
                {
                    File.Copy(file, resourceFile, false);
                }
            }
            string mapResourceFile = Path.Combine(resourceDir, Path.GetFileName(selectedMap.path));
            if (!File.Exists(mapResourceFile))
            {
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
            Process.Start("BeamMP-Server.exe");
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            ConfigureServer();
            ConfigureMods();
        }
    }
}
