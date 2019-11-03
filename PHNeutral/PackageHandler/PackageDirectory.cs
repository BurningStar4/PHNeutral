using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHNeutral.PackageHandler
{
    class PackageDirectory : PackageItem
    {
        #region Fields
        private ObservableCollection<PackageItem> packageItemList;

        #endregion

        #region Constructors

        public PackageDirectory()
        {
            packageItemList = new ObservableCollection<PackageItem>();
        }

        #endregion

        #region Properties

        public ObservableCollection<PackageItem> PackageItems
        {
            get { return packageItemList; }
            set { packageItemList = value; }
        }

        #endregion

        #region Methods

        #endregion
    }
}
