using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGeneratorBusiness
{
    public class clsBusinessGenerator
    {
        List<clsParameter> DefualtClassParameters = new List<clsParameter>();
        clsTable.GeneratorMode enGeneratorMode = clsTable.GeneratorMode.DefualtMode;
        string TableName;
        string SecondTableName;
        public List <clsBusinessFunction> BusinessFunctions = new List<clsBusinessFunction> () ;
        public clsBusinessGenerator(string DataBaseName , string TableName , List<clsDataAccessFunction> DataAccessFunctions , clsTable.GeneratorMode enGeneratorMode) 
        {
            this.TableName = TableName;
            SecondTableName = clsFunction.GenerateParentName(TableName);
            this.enGeneratorMode = enGeneratorMode;
            this.DefualtClassParameters = clsParameter.GenrateParameters( clsSource.GetTableInfo(DataBaseName , TableName));

            foreach (var datafunc in DataAccessFunctions)
            {
                this.BusinessFunctions.Add(new clsBusinessFunction(datafunc));
            }
        }
        public clsBusinessGenerator(string TableName ,List<clsParameter> parameters,List<clsDataAccessFunction> DataAccessFunctions , clsTable.GeneratorMode enGeneratorMode) 
        {
            this.TableName = TableName;
            SecondTableName = clsFunction.GenerateParentName(TableName);
            this.enGeneratorMode = enGeneratorMode;
            this.DefualtClassParameters = parameters;

            foreach (var datafunc in DataAccessFunctions)
            {
                this.BusinessFunctions.Add(new clsBusinessFunction(datafunc));
            }
        }
        string _Code;
        public string CodeAsText
        {
            get
            {
                if (this.enGeneratorMode == clsTable.GeneratorMode.DefualtMode)
                {

                    return GenerateCode();
                }
                return "code_Mode_NotHandled";
            }
        }
        private StringBuilder _GetPatametersDefinitionWithSetAndGet()
        {
            StringBuilder s = new StringBuilder();

            s.AppendLine($"private enum enMode {{ AddNew = 0, Update = 1 }};");
            s.AppendLine($"private enMode _Mode = enMode.AddNew;");
            s.AppendLine($"");
            foreach (var p in this.DefualtClassParameters)
            {            
                s.AppendLine($"public  {p.Type} {p.Name} {{ set; get; }}");
            }
            return s;
        }
        private StringBuilder _GetPublicConstructorAndInitialParameters()
        {
            StringBuilder s = new StringBuilder();

            s.AppendLine($"public cls{clsFunction.GenerateParentName(TableName)}()");
            s.AppendLine($"{{");
            s.AppendLine($"{_GetInitializeAllParametersTo()}");
            s.AppendLine("   _Mode = enMode.AddNew;");
            s.AppendLine($"}}");
            return s;
        }
        private StringBuilder _GetPrivateConstructorAndInitialParameters()
        {
            StringBuilder s = new StringBuilder();

            s.AppendLine($"private cls{clsFunction.GenerateParentName(TableName)}({_GetAllParametersAndDataTypeWithComaSeperator()})");
            s.AppendLine($"{{");
            s.Append($"   {_GetInitializeAllParametersForPrivateConstructor()}");
            s.AppendLine($"");
            s.AppendLine($"   _Mode = enMode.Update;");
            s.AppendLine($"}}");
            return s;
        }
        private StringBuilder _GetInitializeAllParametersTo()
        {
            StringBuilder s = new StringBuilder();

            foreach (var p in this.DefualtClassParameters)
            {
                s.AppendLine($"   this.{p.Name} = {clsSource.GetDefultValueForDataType(p.Type)} ;");
            }
            return s;
        }
        private StringBuilder _GetAllParametersAndDataTypeWithComaSeperator()
        {
            StringBuilder s = new StringBuilder();

            foreach (var p in this.DefualtClassParameters)
            {
                s.Append($@" , {p.Type} {p.Name}");                
            }
            
            s.Remove(0,3);
            return s;
        }
        private StringBuilder _GetInitializeAllParametersForPrivateConstructor()
        {
            StringBuilder s = new StringBuilder();

            foreach (var p in this.DefualtClassParameters)
            {
                s.AppendLine($@"    this.{p.Name} = { p.Name};");                
            }
            
            s.Remove(0,3);
            return s;
        }
        private string _GenerateFunctionsCode() 
        {
            StringBuilder s = new StringBuilder();
            foreach(var bf in BusinessFunctions)
            {
                s.AppendLine(bf.CodeAsText);
            }
            return s.ToString();
        }
        private string GenerateCode()
        {
            
            StringBuilder s = new StringBuilder();
            s.AppendLine($@"using System;");
            s.AppendLine($@"using System.Data;");
            s.AppendLine($@"using {TableName}DataAccessLayer;");
            s.AppendLine($@"");
            s.AppendLine($@"namespace {TableName}BusinessLayer");
            s.AppendLine($@"{{");
            s.AppendLine($@"    public class cls{SecondTableName}");           
            s.AppendLine($@"    {{");
            s.AppendLine($@"{_GetPatametersDefinitionWithSetAndGet()}");
            s.AppendLine($@"{_GetPublicConstructorAndInitialParameters()}");
            s.AppendLine($@"{_GetPrivateConstructorAndInitialParameters()}");
            s.AppendLine($@"{_GenerateFunctionsCode()}");
            s.AppendLine($@"    }}");
            s.AppendLine($@"}}");
            return s.ToString();
        }
    }
}
