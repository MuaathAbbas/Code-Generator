using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGeneratorBusiness
{
    public class clsTable
    {
        public static string TableName { get; set; } = "";
        public static string DataBaseName { get; set; } = "";
        public static string PrimaryKey { get; set; } = "";

        public enum GeneratorMode { DefualtMode, HandleMode }
        public GeneratorMode enGeneratorMode = GeneratorMode.DefualtMode;
        public clsDataAccessGenerator DataAccess { get; }
        public clsBusinessGenerator Business { get; }
        public clsTable(string dataBaseName , string tableName) 
        {
            TableName = tableName;
            DataBaseName = dataBaseName;
            PrimaryKey = clsSource.GetPrimaryKeyColumnNameInDataTable(dataBaseName , tableName);

            this.DataAccess = new clsDataAccessGenerator(DataBaseName, TableName, enGeneratorMode);           
            this.Business = new clsBusinessGenerator(DataBaseName, TableName, this.DataAccess.DataAccessFunctions , enGeneratorMode);
        }
    }
}
