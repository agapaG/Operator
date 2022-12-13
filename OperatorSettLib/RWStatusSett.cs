using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace OperatorSettLib
{
    public class RWStatusSett
    {
        private readonly string connectionString;

        public RWStatusSett(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<CStatus> ReadStatuses()
        {

            using (SqlConnection mySql = new SqlConnection(connectionString))
            {
                List<CStatus> cStatuses = new List<CStatus>();
                SqlCommand cmd = null;

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
                return cStatuses.Select(st => st);
            }
        }

        public void UpdateStatus(string Tcentral, byte value, string NameOp)
        {
            using (SqlConnection mySql = new SqlConnection(connectionString))
            {
                SqlCommand cmd = null;

                mySql.Open();
                cmd = mySql.CreateCommand();
                cmd.Prepare();
                cmd.CommandText = "update Status set Status=@p1, Operator=@p2 where Acount=@p3";
                cmd.Parameters.AddWithValue("p1", value);
                cmd.Parameters.AddWithValue("p2", NameOp);
                cmd.Parameters.AddWithValue("p3", Tcentral);
                cmd.ExecuteReader();

            }
        }

    }

}
