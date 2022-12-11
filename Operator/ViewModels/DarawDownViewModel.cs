using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Operator.ViewModels.Base;
using Operator.Models;
using Operator.Data;
using System.Windows.Data;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using System.Diagnostics;
using Operator.DbServices;

namespace Operator.ViewModels
{
    public class DarawDownViewModel : BaseViewModel
    {

        #region Свойства
        /// <summary>  /// Динамические данные для отображения  /// </summary>
        public ObservableCollection<DrawDown> DataDraw { get; }

        ///// <summary>  /// Выбранная категория   /// </summary>
        private DrawDown _SelectedCategory;
        public  DrawDown SelectedCategory { get => _SelectedCategory; set => Set(ref _SelectedCategory, value); }
        #endregion

        #region Команды
        #endregion

        #region Поля
        private readonly ListCollectionView view;
        Thread workerThread;
        ObservableCollection<DataPackage> dataPackages = null;
        List<CStatus> cStatuses = null;
        #endregion


        public DarawDownViewModel()
        {
            #region Свойства
            DataDraw = new ObservableCollection<DrawDown>();
            #endregion

            #region Команды
            #endregion

            #region Поля
            view = (ListCollectionView)CollectionViewSource.GetDefaultView(DataDraw);
            view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Time", System.ComponentModel.ListSortDirection.Descending));
            #endregion

            #region Закулисье
            workerThread = new Thread(WorkerOperator);
            workerThread.Start();   
            #endregion

        }
        ~DarawDownViewModel()
        {
            App.EndWork = true;
            workerThread.Join();
        }

        private void WorkerOperator()
        {            
            byte[] buffer = null;

            App.eventWaitForStart.WaitOne();

            //Инициализируем точку соединения (прослушка сервера)
            IPEndPoint ipServer = new IPEndPoint(IPAddress.Parse(App.currOperator.Ipaddress), App.currOperator.Ipport);

            //Следующим шагом будит инициализация сокета
            Socket Server = new Socket(ipServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket client = null;
            //Связываем сокет с конечной точкой
            Server.Bind(ipServer);

            //Состояние прослушивания
            Server.Listen(10);

            while (!App.EndWork)
            {                
                client = Server.Accept();

                client.ReceiveTimeout = 1000;
                client.SendTimeout = 1000;

                try
                {
                    int sizeBuff = client.Available;
                    if (sizeBuff != 0)
                    {
                        buffer = new byte[sizeBuff];
                        client.Receive(buffer);
                    }

                    if (buffer != null)
                    {
                        dataPackages = new ObservableCollection<DataPackage>();

                        string xmlData = ConvertByteArrayToString(buffer);
                        DataPackage dataPackage = null;
                        dataPackage = DeserializeFromXML(xmlData);
                                                

                        dataPackages.Add(dataPackage);

                        cStatuses = (List<CStatus>)ReadStatus_Tbl._getStatus();

                        for (int i = 0; i < dataPackages.Count; ++i)
                        {
                            App.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                (System.Threading.ThreadStart)delegate
                                {
                                    DrawDown drawDown = new DrawDown();
                                    drawDown.Tcentral = dataPackages[i].Tcentral;

                                    CStatus stat = cStatuses.Find(s => Equals(s.Tcentral, drawDown.Tcentral));
                                    if (stat != null)
                                        drawDown.Status = (short)stat.Status;

                                    drawDown.Description = dataPackages[i].Description;
                                    drawDown.Time = dataPackages[i].Time;
                                    drawDown.Color = dataPackages[i].Color;
                                    int k = DataDraw.IndexOf(drawDown);
                                    if (k < 0)
                                    {
                                        drawDown.dataPackages.Add(dataPackages[i]);
                                        DataDraw.Add(drawDown);
                                    }
                                    else
                                    {
                                        DataDraw[k].dataPackages.Add(dataPackages[i]);
                                    }
                                });
                        }
                    }
                }
                catch (SocketException ex)
                {
                    MessageBox.Show($"Связь разорвана!!! {ex.Message} (код ошибки {ex.ErrorCode})");

                    client.Close();

                    continue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Связь разорвана!!! {ex.Message}");

                    client.Close();

                    continue;

                }
                             
            }
                        
            Server.Close();
            client.Close();
        }

        private DataPackage DeserializeFromXML(string xmlData)
        {
            DataPackage data = null;

            StringReader stringReader = null;

            XmlSerializer deserializer = new XmlSerializer(typeof(DataPackage));
            stringReader = new StringReader(xmlData);
            data = (DataPackage)deserializer.Deserialize(stringReader);
            stringReader.Close();

            return data;
        }

        private string ConvertByteArrayToString(byte[] values)
        {
            System.Text.UTF8Encoding encoding =
                new System.Text.UTF8Encoding();
            return encoding.GetString(values);
        }
    }
}
