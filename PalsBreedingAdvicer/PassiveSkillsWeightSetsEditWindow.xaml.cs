using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace PalsBreedingAdvicer
{
    /// <summary>
    /// Interaction logic for PassiveSkillsWeightSetsEditWindow.xaml
    /// </summary>
    public partial class PassiveSkillsWeightSetsEditWindow : Window
    {
        PassiveSkillsWeightSetsEditor weightSetsEditor;
        Action<List<PassiveSkillsWeightSet>>? onSaveAction;
        Action? onCloseAction;

        bool isPressedSaveButton = false;




        public PassiveSkillsWeightSetsEditWindow(List<PassiveSkillsWeightSet> passiveSkillsWeightSets,
            Action<List<PassiveSkillsWeightSet>>? onSaveAction = null,
            Action? onCloseAction = null)
        {
            InitializeComponent();
            this.onSaveAction = onSaveAction;
            this.onCloseAction = onCloseAction;
            weightSetsEditor = new(passiveSkillsWeightSets);
            if (weightSetsEditor.IdsFixed)
                this.onSaveAction?.Invoke(weightSetsEditor.ToPassiveSkillsWeightSetsList());
            DataContext = weightSetsEditor;
        }



        private void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            if (!isPressedSaveButton && weightSetsEditor.HasChanges) {
                var messageBoxResult = MessageBox.Show(Properties.Resources.WeightSetsUnsavedChanges_msg,
                    Properties.Resources.WeightSetsUnsavedChanges_title,
                    MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                    e.Cancel = true;
            }
            
            if (!e.Cancel)
                onCloseAction?.Invoke();
        }




        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            weightSetsEditor.CreateNewSet();
            WeightSetsList_ListView.Items.Refresh();
        }




        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button) {
                var idStr = ((Button)sender).DataContext.ToString();
                if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int idToDelete)) {
                    weightSetsEditor.DeleteSet(idToDelete);
                    WeightSetsList_ListView.Items.Refresh();
                }
            }
        }




        private void CopyButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button) {
                var idStr = ((Button)sender).DataContext.ToString();
                if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int idToCopy)) {
                    weightSetsEditor.CreateNewSetAsCopy(idToCopy);
                    WeightSetsList_ListView.Items.Refresh();
                }
            }
        }




        private void SaveButton_Click(Object sender, RoutedEventArgs e)
        {
            isPressedSaveButton = true;
            onSaveAction?.Invoke(weightSetsEditor.ToPassiveSkillsWeightSetsList());
            this.Close();
        }




        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
