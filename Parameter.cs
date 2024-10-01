using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGeneratorBusiness
{
    public class clsParameter
    {   
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsRef { get; set; }
            public bool AllowNull { get; set; }
            public string DefultValue { get; set; }
            public clsParameter()
            {
                Name = "";
                Type = "";
                DefultValue = @"""""";
                IsRef = true;
                AllowNull = false;
            }
            public static List<clsParameter> GenrateParameters(DataTable dt)
            {
                List<clsParameter> p = new List<clsParameter>();

                foreach (DataRow dr in dt.Rows)
                {
                    p.Add(GenrateParameter(dr));
                }

                return p;
            }
            public static clsParameter GenrateParameter(DataRow dr)
            {
                clsParameter temp = new clsParameter();

                temp.Name = dr["Name"].ToString();
                temp.Type = clsSource.ConvertDataTypeFromSqlToCS(dr["Type"].ToString());
                if (Convert.ToBoolean(dr["IsPk"]))
                {
                    temp.IsRef = false;
                }
                temp.AllowNull = Convert.ToBoolean(dr["Allow Null"]);

                return temp;
            }
       
    }

}
