using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGeneratorDataAccess;

namespace CGeneratorBusiness
{
    public static class clsSource
    {
        public enum enFunctionTemplateType { Find, IsExist,AddNew, Update, Delete, GetAll };

        public static DataTable GetAllDataBasesList()
        {
            return clsDataSource.GetAllDataBasesList();
        }
        public static DataTable GetAllDataBaseTablesAndViews(string DataBaseName)
        {
            return clsDataSource.GetAllDataBaseTablesAndViews(DataBaseName);
        }
        public static DataTable GetTableInfo(string DataBaseName, string TableName)
        {
            return clsDataSource.GetTableInfo(DataBaseName, TableName);
        }
        public static DataTable GetTableInfoWithCheckBox(string DataBaseName, string TableName)
        {
            return clsDataSource.GetTableInfoWhithCheckBox(DataBaseName, TableName);
        }
        public static string GetPrimaryKeyColumnNameInDataTable(string DataBaseName, string TableName)
        {
            return clsDataSource.GetPrimaryKeyColumnNameInDataTable(DataBaseName, TableName);
        }
        public static string ConvertDataTypeFromSqlToCS(string DataType)
        {
            switch (DataType)
            {
                //number
                case "tinyint":
                    return "byte";
                case "hsmallint":
                    return "short";
                case "int":
                    return "int";
                case "bigint":
                    return "decimal";
                case "float":
                    return "float";
                case "smallmoney":
                    return "float";
                //string
                case "nvarchar":
                    return "string";
                case "varchar":
                    return "string";
                //bool
                case "bit":
                    return "bool";
                //date
                case "date":
                    return "Datetime";
                case "datetime":
                    return "DateTime";
                case "smalldatetime":
                    return "DateTime";

                default:
                    return "_non_handled";
            }

        }
        public static string GetDefultValueForDataType(string DataType)
        {
            if (DataType == "string" || DataType == "char" ) { return "\"\"";}         
            else if (DataType == "bool"){ return "false"; }            
            else if(DataType == "int" || DataType == "Decimal" || DataType == "byte" || DataType == "short" || DataType == "long" ) { return "-1"; }
            else if(DataType == "float"  ) { return "-1f"; }
            else if(DataType == "DateTime"  ) { return "DateTime.Now"; }
            else {return "";}


        }
    }
}
