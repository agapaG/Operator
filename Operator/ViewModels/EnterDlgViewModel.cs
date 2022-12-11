using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

using System.Configuration;
using Operator.ViewModels.Base;
using Operator.DbServices;
using Operator.Data;

namespace Operator.ViewModels
{
    internal class EnterDlgViewModel : BaseViewModel
    {
        #region Свойства
        public ObservableCollection<glOperator> Operators { get; }
        private glOperator selectedRow;
        public glOperator SelectedRow { get => selectedRow; set => Set(ref selectedRow, value); }
        #endregion
        
        public EnterDlgViewModel()
        {
            Operators = ReadOperatorSett._get_Operators();

        }
    }
}
