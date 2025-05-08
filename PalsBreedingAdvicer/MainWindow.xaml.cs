using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data;
using PalworldSaveDecoding;
using PalsBreedingAdvicer.Properties.PalsData;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

namespace PalsBreedingAdvicer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BreedingAdvicer? breedingAdvicer;
        SaveReadingProgress readingFileProgress = new();
        bool isEditWindowOpened = false;
        PassiveSkillsWeightSetsEditWindow? editWindow;
        string chosenPath = Config.Instance.DefaultSavePath;


        public Dictionary<SaveFileLocation, LevelMeta> SaveFilesData { get; set; } = new();
        public List<PalTribeData> PalTribeDataList { get; set; } = new();


        public MainWindow()
        {
            FillPalsDataDictionary();
            FillSaveFilesData(Config.Instance.DefaultSavePath);
            DataContext = this;


            InitializeComponent();
        }




        private void PathChooseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new();
            dialog.Multiselect = false;
            dialog.Title = Properties.Resources.SavePathChoosing_title;

            bool? result = dialog.ShowDialog();

            if (result == true) {
                chosenPath = dialog.FolderName;
                RefreshSaveData();
            }
        }



        private void PalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillPossibleParents();
        }


        private void WeightsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillPossibleParents();
        }


        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var settings = Config.Instance;
            if (settings.Language != settings.NewLanguage) {
                var newCulture = new System.Globalization.CultureInfo(settings.GetCultureCode(settings.NewLanguage));
                var message = Properties.Resources.ResourceManager.GetString("NeedRestart_msg", newCulture);
                MessageBox.Show(this, message);
            }
        }


        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveComboBox.SelectedIndex >= 0) {
                var selectedItem = SaveComboBox.SelectedItem as KeyValuePair<SaveFileLocation, LevelMeta>?;
                if (selectedItem != null)
                    LoadSaveFile(selectedItem.Value.Key.LevelFile);
            }
        }


        private void EditWeightsSet_Click(object sender, RoutedEventArgs e)
        {
            if (editWindow != null)
                return;

            Action<List<PassiveSkillsWeightSet>> onSaveAction = BreedingAdvicer.UpdatePassiveSkillsWeightSets;
            onSaveAction += (w) => PassiveSkillsWeightSetsManager.WriteSetsToJson(BreedingAdvicer.PassiveSkillsWeightSets);
            onSaveAction += (w) => WeightsComboBox.ItemsSource = BreedingAdvicer.PassiveSkillsWeightSets;
            onSaveAction += PassiveSkillsWeightSetsManager.WriteSetsToJson;

            Action onCloseAction = () => editWindow = null;

            editWindow = new PassiveSkillsWeightSetsEditWindow(BreedingAdvicer.PassiveSkillsWeightSets,
                onSaveAction, onCloseAction);
            editWindow.Show();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (editWindow != null) {
                var messageBoxResult = MessageBox.Show(Properties.Resources.WeightSetsEditorOpened_msg,
                    Properties.Resources.WeightSetsEditorOpened_title,
                    MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                    e.Cancel = true;
                else
                    editWindow.Close();
            }
        }


        private void RefreshSaveData_Click(object sender, RoutedEventArgs e)
        {
            RefreshSaveData();
        }








        private void FillPalsDataDictionary()
        {
            var resourceSet = PalNames.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            if (resourceSet == null)
                return;

            foreach (var resource in resourceSet) {
                if (resource is DictionaryEntry dictionaryEntry) {
                    if (Enum.TryParse<PalTribeId>(dictionaryEntry.Key as string, out var tribeId))
                        PalTribeDataList.Add(new PalTribeData(tribeId, dictionaryEntry.Value as string ?? ""));
                }
            }
            PalTribeDataList.Sort((x, y) => x.Name.CompareTo(y.Name));
        }



        private async void LoadSaveFile(string levelFilename)
        {
            readingFileProgress = new();
            LoadFileButton.IsEnabled = false;

            if (levelFilename == "" || levelFilename == null) {
                ShowError("Couldn't find Level.sav file.");
                return;
            }

            var progress = new Progress<SaveReadingProgressData>(SetProgressValue);
            string gvasFilename;
            try {
                gvasFilename = await SavDecompresser.DecompressAsync(levelFilename, progress);
            }
            catch (Exception ex) {
                ShowError($"Error with file decompressing:{Environment.NewLine}{ex.Message}");
                return;
            }

            var pathsList = new SavePathsList() {
                "WorldSaveData.CharacterSaveParameterMap",
            };
            Level level;
            try {
                level = await Level.ReadAsync(gvasFilename, pathsList, progress);
            }
            catch (Exception ex) {
                ShowError($"Error with file decoding:{Environment.NewLine}{ex.Message}");
                return;
            }

            breedingAdvicer = new BreedingAdvicer(level.WorldSaveData.CharacterSaveParameterMap.Values.Where(c => !c.IsPlayer && c.Gender != null).ToList());
            PalComboBox.IsEnabled = true;
            LoadFileButton.IsEnabled = true;
            ReadSaveFileProgressBar.Value = ReadSaveFileProgressBar.Minimum;
        }



        private async void FillPossibleParents()
        {
            if (PalComboBox.SelectedItem == null || WeightsComboBox.SelectedItem == null)
                return;

            var recommendedParents = await breedingAdvicer!.GetRecommendedParentsAsync(
                (PalTribeId)PalComboBox.SelectedValue, (PassiveSkillsWeightSet)WeightsComboBox.SelectedItem);

            ResultListView.ItemsSource = recommendedParents;
        }


        private void ShowError(string message)
        {
            MessageBox.Show(message);
        }


        private void SetProgressValue(SaveReadingProgressData progressData)
        {
            if (progressData.ProgressType == ProgressType.Decompress) {
                readingFileProgress.DecompressedPart = progressData.ProcessedPart;
            }
            if (progressData.ProgressType == ProgressType.Level) {
                readingFileProgress.WasReadPart = progressData.ProcessedPart;
            }

            ReadSaveFileProgressBar.Value = readingFileProgress.GetOveralProgress((int)ReadSaveFileProgressBar.Maximum);
        }




        private void FillSaveFilesData(string directoryPath)
        {
            var result = new Dictionary<SaveFileLocation, LevelMeta>();
            var saveFilesLocations = SaveFileSearcher.SearchSaveFiles(directoryPath);
            foreach (var saveFileLocation in saveFilesLocations) {
                string gvasFilename;
                try {
                    gvasFilename = SavDecompresser.Decompress(saveFileLocation.LevelMetaFile);
                }
                catch (Exception ex) {
                    ShowError($"Error with file decompressing:{Environment.NewLine}{ex.Message}");
                    return;
                }
                result.Add(saveFileLocation, LevelMeta.Read(gvasFilename));
            }

            SaveFilesData = result;
        }



        private void RefreshSaveData()
        {
            FillSaveFilesData(chosenPath);
            SaveComboBox.ItemsSource = null;
            SaveComboBox.ItemsSource = SaveFilesData;
            if (SaveFilesData.Count > 0)
                SaveComboBox.SelectedIndex = 0;
        }
    }
}
