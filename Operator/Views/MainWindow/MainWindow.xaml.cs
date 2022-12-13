using System.Windows;
using System.Threading;
using System.Configuration;


using Operator.Views;
using OperatorSettLib;

namespace Operator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Semaphore singleApp;
        string cnn;
        
        public MainWindow()
        {
            InitializeComponent();

            cnn = ConfigurationManager.ConnectionStrings["cnnStr"].ConnectionString;
        }
               
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnterDialog();

            //Сигнализирую - оператор подключился
            RWOperatorSett.UpdateOperator(App.currOperator.OperatorSurname, 0b1,cnn);

            App.eventWaitForStart.Set();    
                       
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            RWOperatorSett.UpdateOperator(App.currOperator.OperatorSurname, 0,cnn);
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
