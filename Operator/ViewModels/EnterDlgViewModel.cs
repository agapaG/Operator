using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

using System.Configuration;
using Operator.ViewModels.Base;
using OperatorSettLib;

namespace Operator.ViewModels
{
    internal class EnterDlgViewModel : BaseViewModel
    {
        #region Свойства
        public ObservableCollection<OperatorSett> Operators { get; }
        private OperatorSett selectedRow;
        public OperatorSett SelectedRow { get => selectedRow; set => Set(ref selectedRow, value); }
        #endregion
        
        public EnterDlgViewModel()
        {
            string cnn = ConfigurationManager.ConnectionStrings["cnnStr"].ConnectionString;
            Operators = RWOperatorSett._get_Operators(cnn);
        }
    }
}
