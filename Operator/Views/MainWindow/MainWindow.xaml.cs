using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;

using Operator.DbServices;

using Operator.Views;


namespace Operator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Semaphore singleApp;
        
        public MainWindow()
        {
            InitializeComponent();
                        
        }
               
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnterDialog();

            //Сигнализирую - оператор подключился
            ReadOperatorSett._updateOperator(App.currOperator.OperatorSurname, 0b1);

            App.eventWaitForStart.Set();    
                       
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            ReadOperatorSett._updateOperator(App.currOperator.OperatorSurname, 0);
        }
               
        private void EnterDialog()
        {
            EnterDlg enterInDlg = new EnterDlg();
            if (true == enterInDlg.ShowDialog())
            {
                if (enterInDlg.GetEnterSett != null)
                {
                    App.currOperator = enterInDlg.GetEnterSett;

                    if (App.currOperator.OperatorSurname == "Оператор 1" && App.currOperator.Password == "1")
                    {
                        bool createNewApp;

                        singleApp = new Semaphore(0, 1, "04B9137F-9D90-4CD4-8998-ADFB3B75B722", out createNewApp);
                        if (!createNewApp)
                        {
                            MessageBox.Show("Приложение уже запущено");
                            App.Current.Shutdown();
                        }
                    }
                    else if (App.currOperator.OperatorSurname == "Оператор 2" && App.currOperator.Password == "2")
                    {
                        bool createNewApp;

                        singleApp = new Semaphore(0, 1, "4698D436-F6D5-4D47-912C-5B1006BF4EAA", out createNewApp);
                        if (!createNewApp)
                        {
                            MessageBox.Show("Приложение уже запущено");
                            App.Current.Shutdown();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не реализовано!");
                        App.Current.Shutdown();
                    }
                }
            }
            else
                App.Current.Shutdown();
        }
                
    }
}
