using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHNeutral.PackageHandler
{
    /**
     * Base class for PackageItems
     * 
     */ 
    class PackageItem
    {
       #region Constants

        #endregion

        #region Fields

        // Package Metadata
        private string packageID; // Internal ID of the package this file belongs to
        private string itemID; // Internal File ID
        private string itemName; // Internal File Name
        private string itemPath; // Internal File Path
        private string trueItemPath; // Actual file path of this item on disk
        private PackageItem parent; // Parent node of this file.

        // Individual settings
        private bool isBackup; // True if file is to be backed up, false if it is not to be.
        private bool isIgnoreValidation; // True if file is to be pushed even on validation write error.
        private bool isIgnoreValidationForBackup; // True if file is still to be backed up even on validation write error.
        
        #endregion Fields

        #region Constructors

        // Base File Constructor. Use only for root node as it will generate a new package ID.
        public PackageItem()
        {
            // Generate Metadata
            GenerateMetadata(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Delegates

        #endregion

        #region Events

        #endregion

        #region Properties

        public string PackageID
        {
            get { return packageID; }
            set { packageID = value; }
        }

        public string ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public string ItemPath
        {
            get { return itemPath; }
            set { itemPath = value; }
        }

        public string WindowsPath
        {
            get { return trueItemPath; }
            set { trueItemPath = value; }
        }

        public PackageItem ParentItem
        {
            get { return parent; }
            set { parent = value; }
        }

        public bool IsBackup
        {
            get { return isBackup; }
            set { isBackup = value; }
        }

        public bool IsIgnoreValidation
        {
            get { return isIgnoreValidation; }
            set { isIgnoreValidation = value; }
        }

        public bool IsIgnoreValidationForBackup
        {
            get { return IsIgnoreValidationForBackup; }
            set { isIgnoreValidationForBackup = value; }
        }

        #endregion Properties

        #region Methods

        private void GenerateMetadata(string _packageID)
        {
            packageID = _packageID;
            itemID = Guid.NewGuid().ToString("N");
            itemPath = ""; // This will be set by the PackageHandler;
        }
        #endregion
    }
}
