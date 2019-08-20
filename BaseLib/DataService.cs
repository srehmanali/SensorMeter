using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

namespace BaseLib
{
    class DataService
    {

        //private readonly SqlConnection _con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SaadSuryaServer"].ConnectionString);
        private readonly SqlConnection _con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString);
        public DataService()
        {
            
        }
        public DataSet GetDataSet(Hashtable ht, string sp)
        {
            try
            {
                _con.Open();
                var cmd = new SqlCommand
                              {
                                  CommandText = sp,
                                  CommandType = CommandType.StoredProcedure,
                                  Connection = _con
                              };
                if (ht != null) { 
                    foreach (DictionaryEntry param in ht)
                    {
                        //SqlParameter p = new SqlParameter();
                        cmd.Parameters.AddWithValue("@" + param.Key, param.Value ?? DBNull.Value);
                    }
                }

                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _con.Close();
            }
        }

        public Hashtable ExecuteStoredProcedure(Hashtable ht, string sp, string[] outputParam = null)
        {
            try
            {
                _con.Open();
                var cmd = new SqlCommand
                {
                    CommandText = sp,
                    CommandType = CommandType.StoredProcedure,
                    Connection = _con
                };
                var keys = new List<string>();
                foreach (DictionaryEntry param in ht)
                {
                    cmd.Parameters.AddWithValue("@" + param.Key, param.Value ?? DBNull.Value);
                    keys.Add(param.Key.ToString());
                }
                if (outputParam != null)
                {
                    foreach (string param in outputParam)
                    {
                        cmd.Parameters["@" + param].Direction = ParameterDirection.Output;
                    }
                }
                cmd.ExecuteNonQuery();
                if (outputParam != null)
                {
                    foreach (string param in outputParam)
                    {
                        ht[param] = cmd.Parameters["@" + param].SqlValue;
                    }
                }
                return ht;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _con.Close();
            }
        }
    }
}
