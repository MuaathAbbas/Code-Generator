using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGeneratorBusiness
{
    public class clsDataAccessFunction
    {
        
        public clsFunction function;
        public clsSource.enFunctionTemplateType TemplateType { get; set; }
        public string CodeAsText { get { return clsFunctionGeneratorDataAccessLayer.GetFunctionCode(this.function , this.TemplateType); } }
        public clsDataAccessFunction(string DataBaseName ,string TableName , clsSource.enFunctionTemplateType tempType) 
        {
            List<clsParameter> Parameters = clsParameter.GenrateParameters(clsSource.GetTableInfo(DataBaseName, TableName));
            this.function = new clsFunction(Parameters, TableName);
            this.TemplateType = tempType;
        }
        public clsDataAccessFunction(string TableName , List<clsParameter> Parameters, clsSource.enFunctionTemplateType tempType) 
        {
            this.function = new clsFunction(Parameters, TableName);
            this.TemplateType = tempType;
        }
        public clsDataAccessFunction(string DataBaseName ,string TableName )
        {
            //.. only on using defualt Mode
            List<clsParameter> Parameters = clsParameter.GenrateParameters(clsSource.GetTableInfo(DataBaseName, TableName));
            this.function = new clsFunction(Parameters, TableName);
        }
        public clsDataAccessFunction()
        {
        }

    }
}
