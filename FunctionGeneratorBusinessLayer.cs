using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGeneratorBusiness.clsSource;

namespace CGeneratorBusiness
{
    public static class clsFunctionGeneratorBusinessLayer
    {
        static bool IsSaveIncluded = false;
        private static StringBuilder _GetReferencialParametersWithDefaultValueComaSeperator(clsFunction f)
        {
            StringBuilder s = new StringBuilder();

            foreach (var p in f.Parameters)
            {
                if (p.IsRef)
                {
                    s.AppendLine($@"    {p.Type} {p.Name} = {clsSource.GetDefultValueForDataType(p.Type)} ;");
                }               
            }

            return s;
        }
        private static StringBuilder _GeAllParameterWithComaSeprator(clsFunction f)
        { 
            StringBuilder s = new StringBuilder();

            foreach (var p in f.Parameters)
            {
                 s.Append($@" , {p.Name}");                               
            }
            s.Remove(0,3);
            return s;
        }

        private static string _GenerateAsFindByTemplate(clsFunction func)
        {
            StringBuilder s= new StringBuilder();
            s.AppendLine($@"public static cls{func.ParentName} Find({clsFunctionGeneratorDataAccessLayer.GetNoneRefrancialParameters(func)})");
            s.AppendLine($@"{{");
            s.AppendLine($@"{_GetReferencialParametersWithDefaultValueComaSeperator(func)}");
            s.AppendLine($@"    if(cls{func.ParentName}DataAccess.{func.TextToCalling})");
            s.AppendLine($@"    {{");
            s.AppendLine($@"        return new cls{func.ParentName}({_GeAllParameterWithComaSeprator(func)});");
            s.AppendLine($@"    }}");
            s.AppendLine("      else return null;");
            s.AppendLine($@"}}");
            s.AppendLine($@"");
           
            return s.ToString();
        }        
        private static string _GenerateAsIsExistTemplate(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($"public static bool IsExist(int {clsTable.PrimaryKey}) \n{{");
            s.AppendLine($@"        return cls{func.ParentName}DataAccess.{func.TextToCalling} ;");
            s.AppendLine($"}}");
            return s.ToString();
        } 
        private static string _GenerateAsAddNewTemplate(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($@"private bool _AddNew{clsFunction.GenerateParentName(func.ParentName)}()");
            s.AppendLine($@"{{");
            s.AppendLine($@"    this.{clsTable.PrimaryKey} = cls{func.ParentName}DataAccess.{func.TextToCalling};");
            s.AppendLine($@"    return (this.{clsTable.PrimaryKey} != -1);");
            s.AppendLine($@"}}");
            s.AppendLine($@"");

            return s.ToString();
        } 
        private static string _GenerateAsUpdateTemplate(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($@"private bool _Update{clsFunction.GenerateParentName(func.ParentName)}()");
            s.AppendLine($@"{{");
            s.AppendLine($@"    return cls{func.ParentName}DataAccess.{func.TextToCalling};");
            s.AppendLine($@"}}");
            s.AppendLine($@"");
            return s.ToString();

        } 
        private static string _GenerateAsDeleteTemplate(clsFunction func)
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine($"public static bool Delete{func.ParentName}(int {clsTable.PrimaryKey}) \n{{");
            s.AppendLine($@"        return cls{func.ParentName}DataAccess.{func.TextToCalling};");
            s.AppendLine($"}}");
            return s.ToString();
        } 
        private static string _GenerateAsGetAllTemplate(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($"public static DataTable {func.TextToCalling} \n{{");
            s.AppendLine($@"        return cls{func.ParentName}DataAccess.{func.TextToCalling};");
            s.AppendLine($"}}");
            return s.ToString();
        } 
        private static string _GenerateAsSaveTemplate(clsFunction func)
        {
            if (IsSaveIncluded) return "";

            StringBuilder s = new StringBuilder();
            s.AppendLine($@"public bool Save()");
            s.AppendLine($@"{{");
          //  s.AppendLine($@"    return cls{func.ParentGeneralName}DataAccess.{func.TextToCalling}");
            s.AppendLine($"\tswitch  (_Mode)");
            s.AppendLine($"\t{{");            
            s.AppendLine($"\t\tcase enMode.AddNew:");
            s.AppendLine($"\t\t\tif (_AddNew{clsFunction.GenerateParentName(func.ParentName)}())");
            s.AppendLine($"\t\t\t{{");
            s.AppendLine($"\t\t\t\t_Mode = enMode.Update;");
            s.AppendLine($"\t\t\t\treturn true;");
            s.AppendLine($"\t\t\t}}");
            s.AppendLine($"\t\t\telse");
            s.AppendLine($"\t\t\t{{");
            s.AppendLine($"\t\t\t\treturn false;");
            s.AppendLine($"\t\t\t}}");
            s.AppendLine($"\t\tcase enMode.Update:");
            s.AppendLine($"\t\treturn _Update{clsFunction.GenerateParentName(func.ParentName)}();");
            s.AppendLine($"\t\t}}\n");
            s.AppendLine($"\t\treturn false;");
            s.AppendLine($"\t}}");
            s.AppendLine($@"");

            IsSaveIncluded = true;
            return s.ToString();
        } 

        public static string GetFunctionCode(clsFunction func, enFunctionTemplateType TemplateType)
        {
            switch (TemplateType)
            {
                case clsSource.enFunctionTemplateType.Find:
                {
                    return _GenerateAsFindByTemplate(func); 
                }
                case clsSource.enFunctionTemplateType.IsExist:
                {
                    return _GenerateAsIsExistTemplate(func);
                }
                case clsSource.enFunctionTemplateType.AddNew:
                {                   
                    return _GenerateAsAddNewTemplate(func) 
                            +
                            _GenerateAsSaveTemplate(func);                       
                }
                case clsSource.enFunctionTemplateType.Update:
                {
                    return _GenerateAsUpdateTemplate(func) 
                            +
                            _GenerateAsSaveTemplate(func);
                }
                case clsSource.enFunctionTemplateType.Delete:
                {
                     return _GenerateAsDeleteTemplate(func);
                }
                case clsSource.enFunctionTemplateType.GetAll:
                {
                    return _GenerateAsGetAllTemplate(func);
                }
            }

            return "///NotDefineTempletType///";
        }


    }
}
