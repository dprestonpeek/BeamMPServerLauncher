using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BeamMPServerLauncher
{
    public struct BeamMap
    {
        public string path;
        public string name;
        public string info;
        public string[] previews;
        public string unzippedInfo;
    }

    public partial class Main : Form
    {
        public static BeamMap selectedMap;

        string mapDir = Path.Combine(Directory.GetCurrentDirectory(), "maps");
        string modDir = Path.Combine(Directory.GetCurrentDirectory(), "mods");
        string launcherDataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");

        List<BeamMap> maps = new List<BeamMap>();

        public Main()
        {
            InitializeComponent();
            MakeDirs();
            FillMapOptionsFromInfo();
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

        private string[] GetPreviewFilenamesFromInfo(string infoPath)
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
            }

            return previews;
        }

        private string[] GetStoredPreviews(BeamMap beamMap)
        {
            List<string> files = Directory.GetFiles(beamMap.unzippedInfo).ToList();
            files.Remove(Path.Combine(beamMap.unzippedInfo, "info.json"));
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
                MapPreview.ImageLocation = beamMap.previews[0];
            }
        }

        private void SelectMap(BeamMap beamMap)
        {
            selectedMap = beamMap;
            MapSelector.Text = beamMap.name;
            //MapPreview.ImageLocation = beamMap.previews[0];
            SetNewPreviewFile(selectedMap);
        }

        private void SelectMap(string mapName)
        {
            foreach(BeamMap map in maps)
            {
                if (map.name == mapName)
                {
                    SelectMap(map);
                    return;
                }
            }
        }

        private void FillMapOptionsFromInfo()
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
                beamMap.unzippedInfo = GetUnzippedInfo(beamMap);
                beamMap.info = Path.Combine(beamMap.unzippedInfo, "info.json");
                beamMap.previews = GetStoredPreviews(beamMap);

                if (firstTime)
                {
                    SelectMap(beamMap);
                    firstTime = false;
                }
            }
        }

        private string GetUnzippedInfo(BeamMap beamMap)
        {
            string unzippedInfo = Path.Combine(launcherDataDir, beamMap.name);
            if (!Directory.Exists(unzippedInfo))
            {
                string unzippedFile = Path.Combine(unzippedInfo, "zipFile");
                Directory.CreateDirectory(unzippedFile);
                ZipFile.ExtractToDirectory(beamMap.path, unzippedFile);

                string nameNoExt = Path.GetFileNameWithoutExtension(beamMap.path);
                string levelsParent = Path.Combine(unzippedFile, nameNoExt, "levels");
                string[] dirs = Directory.GetDirectories(levelsParent);
                string levelsPath = Path.Combine(levelsParent, dirs[0]);
                string infoPath = Path.Combine(levelsPath, "info.json");
                string newInfoPath = Path.Combine(unzippedInfo, "info.json");
                File.Move(infoPath, newInfoPath);
                string[] previews = GetPreviewFilenamesFromInfo(newInfoPath);
                foreach (string previewFile in previews)
                {
                    string previewPath = Path.Combine(levelsPath, previewFile);
                    File.Move(previewPath, Path.Combine(unzippedInfo, previewFile));
                }
                var dir = new DirectoryInfo(@unzippedFile);
                dir.Delete(true);
            }
            return unzippedInfo;
        }

        private string GetReadableName(string path)
        {
            string readableName = Path.GetFileNameWithoutExtension(path);
            readableName = readableName.Replace("_", " ");
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            readableName = myTI.ToTitleCase(readableName);
            return readableName;
        }

        private void FillInfo()
        {
            
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
    }
}
