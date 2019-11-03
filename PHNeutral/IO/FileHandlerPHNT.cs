using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHNeutral.PackageHandler;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Microsoft.Win32;

namespace PHNeutral.IO
{
    /** Primary handler for all operations related to .phnt files **/
    class FileHandlerPHNT
    {
        #region Class Variables
        private Properties.Settings settings = Properties.Settings.Default;
        
        // List of FileSystem Directory Paths that will get pushed to
        private List<string> targetEnvironments;

        // Full filepath of currently loaded file
        private string loadedFile = "";
        private string loadedFileName = "";
        #endregion

        #region Constructors
        public FileHandlerPHNT()
        {
            // If we need file handler settings that do not change, set them.
        }
        #endregion

        #region Methods

        /**
         * Parses a phnt file
         * */
        public ObservableCollection<PackageItem> readPHNTFile(string phntPath)
        {
            XElement phntXML = XElement.Load(phntPath);
            ObservableCollection<PackageItem> loadedCollection;
            // Read phnt version
            double dblPackageVersion = 0.0;

            // Get Package Details XElement
            XElement packageDetails = phntXML.Element("packagedetails");
            if (!Double.TryParse(packageDetails.Element("version").Value, out dblPackageVersion))
            {
                dblPackageVersion = 0.0;
            }
            if (dblPackageVersion < settings.MinCompatibleVersion)
            {
                // write error to console: "PHNT File is for an older version of PHNeutral and is incompatible"
                return null;
            }
            
            // Console - Write "Loading Package...."

            // Set PackageDetails
            setPackageDetails(packageDetails);

            // Build Tree

            Console.WriteLine(phntXML);

            
            loadedCollection = buildPackageTree(phntXML.Element("package"));
            if (loadedCollection != null)
            {
                loadedFile = phntPath;
                return loadedCollection;
            }
            else
            {
                loadedFile = "";
                return null;
            }
        }

        public bool savePHNTFile(ObservableCollection<PackageItem> package, bool saveAs)
        {
            bool isSuccessful = true;
            string filename = "";
            if (saveAs)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "New Package";
                dlg.DefaultExt = ".phnt";
                dlg.Filter = "PHNeutral Package Files (*.phnt)|*.phnt";

                // Open Save File Dialog

                Nullable<bool> dlgResult = dlg.ShowDialog();

                if (dlgResult.HasValue && (bool)dlgResult)
                {
                    filename = dlg.FileName;
                    loadedFile = dlg.FileName;
                    loadedFileName = dlg.SafeFileName;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                filename = loadedFile;
            }


            return isSuccessful;
            
        }

        /***
         * Sets the package details for the loaded phnt Package
         * */
        private void setPackageDetails(XElement phntXML)
        {
            // Set Package Name
            // Set Package ID
            // Set Package Description
            // Load Target Environments
        }

        private ObservableCollection<PackageItem> buildPackageTree(XElement phntCurrentDirectory)
        {
            var items = new ObservableCollection<PackageItem>();

            // For each directory node
            foreach (var directory in phntCurrentDirectory.Elements("directory"))
            {
                // Create a new PackageDirectory object and assign properties
                var item = new PackageDirectory
                {
                    ItemName = directory.Element("name").Value,
                    // Pass the current directory for recursion
                    PackageItems = buildPackageTree(directory)
                };

                // Add PackageItems to collection
                items.Add(item);
            }

            foreach (var file in phntCurrentDirectory.Elements("file"))
            {
                var item = new PackageFile
                {
                    ItemName = file.Element("name").Value,
                    WindowsPath = file.Element("path").Value,
                };
                // Pass the settings node for this file to the setPackageFileSettings function
                setPackageFileSettings(item, file.Element("settings"));

                items.Add(item);
            }

            return items;
        }

        public string LoadedFile
        {
            get { return loadedFile; }
            set { loadedFile = value; }
        }

        public string LoadedFileName
        {
            get { return loadedFileName; }
            set { loadedFileName = value; }
        }

        private void setPackageFileSettings(PackageFile item, XElement phntFileSettingsNode)
        {
            item.IsBackup = (phntFileSettingsNode.Element("fg_donotbackup").Value == "0");
            item.IsIgnoreValidation = (phntFileSettingsNode.Element("fg_ignorevalidation").Value == "1");
            item.IsIgnoreValidationForBackup = (phntFileSettingsNode.Element("fg_ignorevalidation").Value == "1");
            // TODO: Donotpush
            // TODO: Donotwatch
        }

        #endregion
    }
}
