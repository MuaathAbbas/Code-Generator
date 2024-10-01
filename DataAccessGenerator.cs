using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static CGeneratorBusiness.clsSource;

namespace CGeneratorBusiness
{
    public class clsDataAccessGenerator
    {
        string ParentTableName;
        string CurrntDataBaseName;

        public clsTable.GeneratorMode enGeneratorMode = clsTable.GeneratorMode.DefualtMode;

        public List<clsDataAccessFunction> DataAccessFunctions = new List<clsDataAccessFunction> ();
        string _Code;

        public string CodeAsText
        {
            get 
            {
                if(enGeneratorMode == clsTable.GeneratorMode.DefualtMode)      
                {
                    _InitialisDefaltCode();
                    return _Code; 
                    
                }
                return "code_Mode_NotHandled"; 
            }
        }
        private void  _InitialisDefaltCode()
        {
             this._AddDefualtFunctions();
             this.  _Code = GenerateCode();
        }
        public clsDataAccessGenerator(string CurrntDataBaseName , string ParentTableName , clsTable.GeneratorMode enGeneratorMode)
        {
            this.enGeneratorMode = enGeneratorMode;
            this.ParentTableName = ParentTableName;
            this.CurrntDataBaseName = CurrntDataBaseName;

            _InitialisDefaltCode();
        }
        private void _AddDefualtFunctions()
        {
            List<clsParameter> DefualtParameters = clsParameter.GenrateParameters(clsSource.GetTableInfo(CurrntDataBaseName, ParentTableName));

            List<clsSource.enFunctionTemplateType> FunctionsDefultTypes = new List<clsSource.enFunctionTemplateType>();

            FunctionsDefultTypes.Add(enFunctionTemplateType.Find);
            FunctionsDefultTypes.Add(enFunctionTemplateType.AddNew);
            FunctionsDefultTypes.Add(enFunctionTemplateType.Update);
            FunctionsDefultTypes.Add(enFunctionTemplateType.Delete);
            FunctionsDefultTypes.Add(enFunctionTemplateType.GetAll);
            FunctionsDefultTypes.Add(enFunctionTemplateType.IsExist);

            foreach (var typs in FunctionsDefultTypes)
            {
                this.DataAccessFunctions.Add(new clsDataAccessFunction(ParentTableName , DefualtParameters , typs));
            }    

        }
        private string _GetFunctionsCode()
        {
            StringBuilder s = new StringBuilder();
            foreach (var f in this.DataAccessFunctions)
            {
                s.Append(clsFunctionGeneratorDataAccessLayer.GetFunctionCode(f.function , f.TemplateType));
            }
            return s.ToString();
        }
        private string GenerateCode()
        {
          
            StringBuilder code = new StringBuilder($@"using System;
using System.Data;
using System.Data.SqlClient;

namespace {this.ParentTableName}DataAccessLayer
{{
    public class cls{clsFunction.GenerateParentName(ParentTableName)}DataAccess
    {{
        {this._GetFunctionsCode()}
    }}
}}
");
            return code.ToString();

        }

    }
}
