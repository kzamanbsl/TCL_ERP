﻿using System.Data;
using System.Data.SqlClient;

namespace KGERP.Utility
{
    public class Storeproc
    {
        public DataSet GetDataSet(SqlConnection connection, string storedProcName, params SqlParameter[] parameters)
        {
            var command = new SqlCommand(storedProcName, connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(parameters);

            var result = new DataSet();
            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(result);
            return result;
        }
    }
}
