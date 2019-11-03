using System;
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
using System.Windows.Shapes;

namespace PHNeutral
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region Fields

        Properties.Settings settings = Properties.Settings.Default;

        #endregion

        #region Constructors

        public SettingsWindow()
        {
            InitializeComponent();
            ddlLogLevel.ItemsSource = Enum.GetValues(typeof(PHNeutral.ConsoleWriter.ConsoleWriter.ConsoleLogLevel)).Cast<PHNeutral.ConsoleWriter.ConsoleWriter.ConsoleLogLevel>();
            LoadSettings();
        }

        #endregion

        private void LoadSettings()
        {
            LoadDirectorySettings();
            LoadGeneralSettings();
        }

        private void LoadGeneralSettings()
        {
            // Load Title Checkbox
            cbShowTitleName.IsChecked = settings.DisplayPackageNameInTitle;
            // Load LogLevel
            ddlLogLevel.SelectedIndex = settings.ConsoleLogLevel;
        }

        private void LoadDirectorySettings()
        {
            tbPackagesDirectory.Text = settings.PackagesDirectory;
            tbBackupsDirectory.Text = settings.BackupsDirectory; 
        }

        private void LoadPushSettings()
        {

        }

        private void LoadBackupSettings()
        {

        }

        private void SaveGeneralSettings()
        {
            // Load Title Checkbox
            settings.DisplayPackageNameInTitle = cbShowTitleName.IsChecked ?? false;
            // Load LogLevel
            settings.ConsoleLogLevel = ddlLogLevel.SelectedIndex;
        }

        private void SaveDirectorySettings()
        {
            settings.PackagesDirectory = tbPackagesDirectory.Text.Trim();
            settings.BackupsDirectory = tbBackupsDirectory.Text.Trim();
        }

        #region EventHandlers
        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSaveAndApply_Click(object sender, RoutedEventArgs e)
        {
            SaveDirectorySettings();
            SaveGeneralSettings();

            settings.Save();
            Close();
        }

        // TODO: REdo this to just set all checkboxes and..boxes
        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            // Open messagebox asking if user is sure they want to do default !
            MessageBoxResult result = MessageBox.Show("Are you sure you want to default ALL settings?", "PHNeutral", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                settings.Reset();
                LoadSettings();
            }
            Close();
        }

        #endregion



    }
}
