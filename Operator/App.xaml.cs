using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;


using Operator.Data;

namespace Operator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool EndWork { get; set; }

        internal static currOperator currOperator = new currOperator();
        internal static EventWaitHandle eventWaitForStart = new EventWaitHandle(false, EventResetMode.AutoReset);

    }
}
