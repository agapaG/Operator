using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OperatorSettLib
{
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
