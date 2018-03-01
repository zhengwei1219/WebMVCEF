using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ZhengWei.ExtractOil.WebApp
{
    public class MysqlHelper
    {
        //private static readonly string conStr = ConfigurationManager.ConnectionStrings["mysqlConstr"].ConnectionString;
        private static string conStr = "server=192.168.0.105;user id=root;password=hysmwh;database=db_wsbg";
        public static int ExecuteNonQuery(string sql,CommandType commType,params MySqlParameter[] pms)
        { 
                using(MySqlConnection con = new MySqlConnection(conStr))
                {
                    using(MySqlCommand cmd = new MySqlCommand(sql,con))
                    {
                        cmd.CommandType = commType;
                        if (pms != null)
                        {
                            cmd.Parameters.AddRange(pms);
                        }
                        con.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
        }
    }
}