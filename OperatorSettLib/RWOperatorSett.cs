using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace OperatorSettLib
{
    public class RWOperatorSett
    {
        private readonly string connectionString;

        public RWOperatorSett(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<OperatorSett> ReadOperators()
        {
            using (SqlConnection SqlCnn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = null;

                try
                {
                    List<OperatorSett> operators = new List<OperatorSett>();
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
                    return operators.Select(op => op);
                }
                catch (SqlException ex)
                {
                    cmd.Transaction.Rollback();
                    cmd.Transaction.Dispose();
                    if (SqlCnn.State == System.Data.ConnectionState.Open)
                        SqlCnn.Close();
                    throw ex;
                }
            }

        }

        public void UpdateOperator(string nameOper, byte OnOff)
        {
            using (SqlConnection SqlCnn = new SqlConnection(connectionString))
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
                    if (SqlCnn.State == System.Data.ConnectionState.Open)
                        SqlCnn.Close();
                    throw ex;
                }
            }

        }
    }

}
