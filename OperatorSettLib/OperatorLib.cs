using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OperatorSettLib
{
    public class OperatorSett : IEquatable<OperatorSett>
    {
        public int Id { get; set; }

        public string OperatorSurname { get; set; }

        public string Password { get; set; }

        public string Ipaddress { get; set; }

        public int Ipport { get; set; }

        #region IEquatable method
        public bool Equals(OperatorSett other)
        {
            if (this.Id == other.Id)
                return true;
            return false;
        }
        #endregion

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class CStatus
    {
        public string Tcentral { get; set; }
        public byte Status { get; set; }
    }

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

    public class RWStatusSett
    {
        public static ICollection<CStatus> _getStatus(string cnn)
        {
            List<CStatus> cStatuses = new List<CStatus>();

            using (SqlConnection mySql = new SqlConnection(cnn))
            {
                SqlCommand cmd = null;

                try
                {
                    mySql.Open();
                    cmd = mySql.CreateCommand();
                    cmd.Prepare();
                    cmd.CommandText = "select * from Status";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CStatus tbl = new CStatus();
                        tbl.Tcentral = reader.GetString(0);
                        tbl.Status = reader.IsDBNull(1) ? (byte)0b0 : reader.GetByte(1);

                        cStatuses.Add(tbl);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Read Status Lib" + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Read Status Lib" + ex.Message);
                    return null;
                }

            }

            return cStatuses;
        }

        public static void _setStatus(string Tcentral, byte value, string NameOp, string cnn)
        {
            using (SqlConnection mySql = new SqlConnection(cnn))
            {
                SqlCommand cmd = null;

                try
                {
                    mySql.Open();
                    cmd = mySql.CreateCommand();
                    cmd.Prepare();
                    cmd.CommandText = "update Status set Status=@p1, Operator=@p2 where Acount=@p3";
                    cmd.Parameters.AddWithValue("p1", value);
                    cmd.Parameters.AddWithValue("p2", NameOp);
                    cmd.Parameters.AddWithValue("p3", Tcentral);
                    cmd.ExecuteReader();

                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Write Status Lib" + ex.Message);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Write Status Lib" + ex.Message);
                    return;
                }

            }
        }

    }

}
