using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OperatorSettLib
{
    public class RWOperatorSett
    {
        public static ObservableCollection<OperatorSett> _get_Operators(string cnn)
        {
            ObservableCollection<OperatorSett> operators = new ObservableCollection<OperatorSett>();

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
                            OperatorSett oper = new OperatorSett();
                            oper.Id = reader.GetInt32(0);
                            oper.OperatorSurname = reader.GetString(1);
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
                    MessageBox.Show("Read Operators Lib" + ex.Message);
                    if (SqlCnn.State == System.Data.ConnectionState.Open)
                        SqlCnn.Close();
                    return null;
                }
            }

            return operators;
        }

        public static void _updateOperator(string nameOper, byte OnOff, string cnn)
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
                    MessageBox.Show("Update Operators Lib" + ex.Message);
                    if (SqlCnn.State == System.Data.ConnectionState.Open)
                        SqlCnn.Close();
                    return;
                }
            }

        }
    }

}
