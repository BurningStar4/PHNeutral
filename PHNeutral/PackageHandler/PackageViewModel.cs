using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PHNeutral.ConsoleWriter;

namespace PHNeutral.PackageHandler
{
    /**
     *  Primary PHNeutral ViewModel Class
     *  This is the PackageHandler Main Logic Controller
     * 
     * */
    class PackageViewModel : INotifyPropertyChanged
    {
        Properties.Settings settings = Properties.Settings.Default;

        private PHNeutral.IO.FileHandlerPHNT fileHandlerPHNT;
        private ObservableCollection<PackageItem> testItems;
        private ConsoleWriter.ConsoleWriter console;
        private string consoleStream;
        private bool collectionIsDirty = false;

        public PackageViewModel()
        {
            fileHandlerPHNT = new PHNeutral.IO.FileHandlerPHNT();
            console = new ConsoleWriter.ConsoleWriter();
            consoleStream = "";
            WriteToConsole("PHNeutral 1.0", ConsoleWriter.ConsoleWriter.ConsoleLogLevel.Standard);
        }

        #region Methods

        private void WriteToConsole(string message, ConsoleWriter.ConsoleWriter.ConsoleLogLevel logLevel)
        {
            console.WriteLineAppendString(message, ref consoleStream, logLevel);
            NotifyPropertyChanged("ConsoleStream");
        }

        // Opens a package file
        public void OpenPackage(string filepath)
        {
            TestItems = fileHandlerPHNT.readPHNTFile(filepath);
        }
        

        public void OpenPackageSimple()
        {
            // TODO: Exception Handling
            
            // Open File Dialog
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Package File";
            dlg.Filter = "PHNeutral Package Files (*.phnt)|*.phnt";
            Nullable<bool> dlgResult = dlg.ShowDialog();
            // Pass filepath to IOHandler to parse
            if (dlgResult.HasValue && (bool)dlgResult)
            {
                WriteToConsole("Opening package from file: " + dlg.SafeFileName + "...", ConsoleWriter.ConsoleWriter.ConsoleLogLevel.Verbose);
                TestItems = fileHandlerPHNT.readPHNTFile(dlg.FileName);
                if (TestItems != null)
                {
                    WriteToConsole("Package opened successfully", ConsoleWriter.ConsoleWriter.ConsoleLogLevel.Standard);
                }
                else
                {
                    WriteToConsole("Could not open package!", ConsoleWriter.ConsoleWriter.ConsoleLogLevel.ErrorsOnly);
                }
            }
        }

        private void SavePackage(bool SaveAs)
        {
            
        }

        public void setTestItems(string path)
        {
            TestItems = new ObservableCollection<PackageItem>(BuildDirectoryTree(path));   
        }

        private ObservableCollection<PackageItem> BuildDirectoryTree(string path)
        {
            var items = new ObservableCollection<PackageItem>();

            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.GetDirectories())
            {
                var item = new PackageDirectory
                {
                    ItemName = directory.Name,
                    WindowsPath = directory.FullName,
                    PackageItems = BuildDirectoryTree(directory.FullName)
                };

                items.Add(item);
            }

            foreach (var file in dirInfo.GetFiles())
            {
                var item = new PackageFile
                {
                    ItemName = file.Name,
                    WindowsPath = file.FullName
                };

                items.Add(item);
            }

            return items;
        }

        private void OpenSettingsWindow()
        {
            SettingsWindow dlg = new SettingsWindow();
            dlg.Owner = Application.Current.MainWindow;
            dlg.ShowDialog();
        }


        #endregion

        #region Properties
        public string ConsoleStream
        {
            get { return consoleStream; }
            set
            {   
                consoleStream = value;
                NotifyPropertyChanged("ConsoleStream");
            }
        }
        #endregion

        #region Setting Grabbers

        /**
         * Gets the path of the local packages directory from settings.
         */
        private string GetPackageDirectoryPath()
        {
            string packageDirectoryPath = Properties.Settings.Default.PackagesDirectory;

            // If the settings path contains ~\ then we start looking for the directory in the root folder.
            if (packageDirectoryPath.StartsWith(@"~\"))
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\" + packageDirectoryPath.Substring(2);
            }

            return packageDirectoryPath;
        }

        private string GetBackupsDirectoryPath()
        {
            string backupsDirectoryPath = Properties.Settings.Default.BackupsDirectory;

            // If the settings path contains ~\ then we start looking for the directory in the root folder.
            if (backupsDirectoryPath.StartsWith(@"~\"))
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\" + backupsDirectoryPath.Substring(2);
            }

            return backupsDirectoryPath;
        }

        #endregion

        #region Commands
        private ICommand _openSettingsCommand;

        public ICommand OpenSettingsCommand
        {
            get
            {
                if (_openSettingsCommand == null)
                {
                    _openSettingsCommand = new DelegateCommand<object>(
                        param => OpenSettingsWindow(),
                        param => true
                    );
                }
                return _openSettingsCommand;
            }
        }

        private ICommand _buildDirectoryTreeCommand;

        public ICommand BuildDirectoryTreeCommand
        {
            get
            {
                if (_buildDirectoryTreeCommand == null)
                {
                    _buildDirectoryTreeCommand = new DelegateCommand<object>(
                        param => setTestItems(GetPackageDirectoryPath()),
                        param => true
                    );
                }
                return _buildDirectoryTreeCommand;
            }
        }

        private ICommand _openPackageSimpleCommand;
        public ICommand OpenPackageSimpleCommand
        {
            get
            {
                if (_openPackageSimpleCommand == null)
                {
                    _openPackageSimpleCommand = new DelegateCommand<object>(
                        param => OpenPackageSimple(),
                        param => true
                    );
                }
                return _openPackageSimpleCommand;
            }
        }

        private ICommand _saveOrSaveAsCommand;
        public ICommand SaveOrSaveAsCommand
        {
            get
            {
                if (_saveOrSaveAsCommand == null)
                {
                    _saveOrSaveAsCommand = new DelegateCommand<object>(
                    param => SavePackage(String.IsNullOrEmpty(fileHandlerPHNT.LoadedFile)), // If the Loaded File is empty, prompt SaveAs
                    param => true //Add error checking here you peasant
                    );
                }
                return _saveOrSaveAsCommand;
            }
        }

        private ICommand _saveAsCommand;
        public ICommand SaveAsCommand
        {
            get
            {
                if (_saveAsCommand == null)
                {
                    _saveAsCommand = new DelegateCommand<object>(
                    param => SavePackage(true),
                    param => true //Add error checking here you peasant
                    );
                }
                return _saveAsCommand;
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand<object>(
                    param => SavePackage(false),
                    param => true //Add error checking here you peasant
                    );
                }
                return _saveCommand;
            }
        }
        

        #endregion

        #region Properties

        public ObservableCollection<PackageItem> TestItems
        {
            get { return testItems; }
            set 
            { 
                testItems = value;
                NotifyPropertyChanged("TestItems");
            }
        }

        #endregion

        #region Events

        /** Generic Property Changed Event Handling. Needed to update UI when collections are reassigned - in order to not lose binding **/
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /** End Property Changed Event Handling **/

        #endregion
    }
}
