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

using Operator.Data;


namespace Operator.Views
{
    /// <summary>
    /// Логика взаимодействия для EnterDlg.xaml
    /// </summary>
    public partial class EnterDlg : Window
    {
        public EnterDlg()
        {
            InitializeComponent();
        }

        public currOperator GetEnterSett
        {
            get
            {
                glOperator op = (glOperator)User.SelectedItem;
                currOperator currOperator = new currOperator();
                currOperator.Id = (int)op.Id;
                currOperator.OperatorSurname = op.OperatorSurname;
                currOperator.Ipaddress = op.Ipaddress;
                currOperator.Ipport = (int)op.Ipport;
                currOperator.Password = pass.Password;
                return currOperator;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
