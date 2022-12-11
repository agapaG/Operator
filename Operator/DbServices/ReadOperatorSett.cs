using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using System.Configuration;
using System.Data.SqlClient;

using System.Collections.ObjectModel;
using Operator.Data;


namespace Operator.DbServices
{
    internal class ReadOperatorSett
    {
        static string cnn = ConfigurationManager.ConnectionStrings["cnnStr"].ConnectionString;

        public static ObservableCollection<glOperator> _get_Operators()
        {
            ObservableCollection<glOperator> operators = new ObservableCollection<glOperator>();

            using (SqlConnection SqlCnn = new SqlConnection(cnn))
            {
                SqlCommand cmd = null;

                try
                {
                    SqlCnn.Open();

                    string CommandText = "select * from Operators";
                    cmd = new SqlCommand(CommandText, SqlCnn);

                    cmd.Transaction = SqlCnn.BeginTransaction();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            glOperator oper = new glOperator();
                            oper.Id = reader.GetInt32(0);
                            oper.OperatorSurname = reader.GetString(1);
                            oper.OperatorName = reader.IsDBNull(2) ? null : reader.GetString(2);
                            oper.OperatorPatronymic = reader.IsDBNull(3) ? null : reader.GetString(3);
                            oper.Password = reader.IsDBNull(4) ? null : reader.GetString(4);
                            oper.Ipaddress = reader.GetString(5);
                            oper.Ipport = reader.GetInt32(6);
                            operators.Add(oper);
                        }
                    }

                    cmd.Transaction.Commit();
                }
                catch (SqlException ex)
                {
                    cmd.Transaction.Rollback();
                    cmd.Transaction.Dispose();
                    MessageBox.Show("Read Operators" + ex.Message);
                    if (SqlCnn.State == System.Data.ConnectionState.Open)
                        SqlCnn.Close();
                    return null;
                }
            }

            return operators;
        }

        public static void _updateOperator(string nameOper, byte OnOff )
        {
            using (SqlConnection SqlCnn = new SqlConnection(cnn))
            {
                SqlCommand cmd = null;

                try
                {
                    SqlCnn.Open();

                    string CommandText = $"update Operators set Online = @2 where Surname = @1";
                    cmd = new SqlCommand(CommandText, SqlCnn);
                    cmd.Parameters.AddWithValue("@1", nameOper);
                    cmd.Parameters.AddWithValue("@2", OnOff);
                    

                    cmd.Transaction = SqlCnn.BeginTransaction();
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();

                }
                catch (SqlException ex)
                {
                    cmd.Transaction.Rollback();
                    cmd.Transaction.Dispose();
                    MessageBox.Show("Delete record " + ex.Message);
                    if (SqlCnn.State == System.Data.ConnectionState.Open)
                        SqlCnn.Close();
                    return;
                }
            }

        }
    }
}
