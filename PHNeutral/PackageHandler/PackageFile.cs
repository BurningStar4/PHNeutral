using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHNeutral.PackageHandler
{
    /**
     * Virtual representation of a file in the package. This is the base class for all items in the Package Structure
     **/
    class PackageFile : PackageItem
    {
        #region Constants

        #endregion

        #region Fields
        
        // Windows File System Metadata Collection - this will be all properties windows knows about the file. TODO: Create function to search for property.
        private FileInfo windowsFileInfo;

        #endregion Fields

        #region Constructors

        // Base File Constructor. Use only for root node as it will generate a new package ID.
        public PackageFile()
        {

        }

        // For creating a file in the Package.
        public PackageFile(string packageID, PackageDirectory parentPackageFile, FileInfo _windowsFileInfo)
        {
            windowsFileInfo = _windowsFileInfo;
            ParentItem = parentPackageFile;
            // GenerateMetadata(packageID);
        }

        #endregion

        #region Delegates

        #endregion

        #region Events

        #endregion

        #region Properties

        public FileInfo WindowsFileInfo
        {
            get { return windowsFileInfo; }
        }

        #endregion Properties

        #region Methods

        private void GenerateMetadata(string _packageID)
        {
            PackageID = _packageID;
            ItemID = Guid.NewGuid().ToString("N");
            ItemName = windowsFileInfo.Name + windowsFileInfo.Extension;
            ItemPath = ""; // This will be set by the PackageHandler;
            WindowsPath = windowsFileInfo.FullName;
        }

        // To be used with watch functionality - to be called on filechange event
        private void UpdateFile()
        {

        }

        #endregion
    }
}
