using System;
using System.Collections.Generic;
using System.Windows;

using System.Configuration;
using System.Data.SqlClient;

using Operator.Data;

namespace Operator.DbServices
{
    public class ReadStatus_Tbl
    {
        static string cnn = ConfigurationManager.ConnectionStrings["cnnStr"].ConnectionString;

        public static ICollection<CStatus> _getStatus()
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
                    MessageBox.Show(ex.Message + "чтение 'Status'");
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "чтение 'Status'");
                    return null;
                }

            }

            return cStatuses;
        }

        public static void _setStatus(string Tcentral, byte value, string NameOp)
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
                    MessageBox.Show(ex.Message + "запись 'Status'");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "запись 'Status'");
                    return;
                }

            }
        }

    }
}
