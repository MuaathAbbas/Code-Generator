using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static CGeneratorBusiness.clsSource;

namespace CGeneratorBusiness
{
    public static class clsFunctionGeneratorDataAccessLayer
    {
        private static StringBuilder _GetParameterWhitComaSepratorIncludeReferanceWithDataType(clsFunction func)
        {
            StringBuilder s = new StringBuilder() ;
            foreach (var item in func.Parameters)
            {
                s.Append((item.IsRef) ? " , ref " : " , " );
                s.Append(item.Type + " ");
                s.Append(item.Name);
            }

            if (s.Length > 2) return s.Remove(0,2) ;

            return s;
        }
        private static StringBuilder _GetParameterWhitComaSepratorIncludeReferancedWithOutDataType(clsFunction func)
        {
            StringBuilder s = new StringBuilder() ;
            foreach (var item in func.Parameters)
            {
                s.Append((item.IsRef) ? " , ref " : " , ");
                s.Append(item.Name);
            }

            if (s.Length > 2) return s.Remove(0,2) ;

            return s;
        }
        private static StringBuilder _GetParameterWhitComaSepratorIncludeReferanceBetweenBrackitse(clsFunction func)
        {
            return new StringBuilder ("("+ _GetParameterWhitComaSepratorIncludeReferanceWithDataType(func) + ")" );
        }
        private static StringBuilder _GetReferacialParameterNamesWithComaSeprator(clsFunction func)
        {
            StringBuilder s = new StringBuilder() ;
            foreach (var item in func.Parameters)
            {
                s.Append(","+item.Name  );
            }

            if (s.Length > 2) return s.Remove(0,1) ;

            return s;
        }
        private static StringBuilder _GetReferacialParameterNamesWithComaSepratorAndthisKeyWordToBusiness(clsFunction func)
        {
            StringBuilder s = new StringBuilder() ;
            foreach (var item in func.Parameters)
            {
                s.Append(", this."+item.Name  );
            }

            if (s.Length > 2) return s.Remove(0,2) ;

            return s;
        }
        private static StringBuilder _GetReferacialParameterNamesWithComaSepratorAndAtSymbol(clsFunction func)
        {
            StringBuilder s = new StringBuilder() ;
            foreach (var item in func.Parameters)
            {
                s.Append(",@"+item.Name  );
            }

            if (s.Length > 2) return s.Remove(0,1) ;

            return s;
        }
        private static StringBuilder _GetRefrancialParameters(clsFunction func)
        {
            StringBuilder s = new StringBuilder() ;
            foreach (var item in func.Parameters)
            {
                if (item.IsRef)
                {
                    s.Append(" , " + item.Type + " ");
                    s.Append(item.Name);
                }

            }

            if (s.Length > 2) return s.Remove(0, 2);

            return s;
        }
        private static StringBuilder _GetRefrancialParametersWithOutDataType(clsFunction func)
        {
            StringBuilder s = new StringBuilder() ;
            foreach (var item in func.Parameters)
            {
                if (item.IsRef)
                {
                    s.Append(" , this.");
                    s.Append(item.Name);
                }

            }

            if (s.Length > 2) return s.Remove(0, 2);

            return s;
        }
        private static StringBuilder _GetRefrancialParametersBetweenBrackitse(clsFunction func)
        {
            return new StringBuilder ("(" + _GetRefrancialParameters( func) + ")") ;
        }
        private static StringBuilder _GetRefrancialParametersEqualWithAtSign(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in func.Parameters)
            {
                if(item.IsRef)
                {
                    s.AppendLine("                  , " + item.Name + " =@" + item.Name);
                }

            }
            return s.Remove(0, 20);
        }
        private static StringBuilder _GetReferancialParameterForCommandToAddWhitValue(clsFunction func, string CommandName = "command")
        {
            StringBuilder s = new StringBuilder(), TempForAllowNull = new StringBuilder();

            foreach (var p in func.Parameters)
            {
                if (p.IsRef)
                {
                    if (p.AllowNull)
                    {
                        TempForAllowNull.AppendLine("");
                        TempForAllowNull.AppendLine($@"     //{p.Name}: is allow null we should handle to save database null ");
                        TempForAllowNull.AppendLine($@"     if({p.Name} == {clsSource.GetDefultValueForDataType(p.Type)} || {p.Name} == null)");
                        TempForAllowNull.AppendLine($@"     {{");
                        TempForAllowNull.AppendLine($@"         " + CommandName + $@".Parameters.AddWithValue(""@{p.Name}"" , System.DBNull.Value ); ");
                        TempForAllowNull.AppendLine($@"     }}");
                        TempForAllowNull.AppendLine($@"         else");
                        TempForAllowNull.AppendLine($@"     {{");
                        TempForAllowNull.AppendLine("           " + CommandName + $@".Parameters.AddWithValue(""@{p.Name}"" , {p.Name} );");
                        TempForAllowNull.AppendLine($@"     }}");
                    }
                    else
                    {
                        s.Append("    " + CommandName);
                        s.AppendLine($@".Parameters.AddWithValue(""@{p.Name}"" , {p.Name} );");
                    }
                }

            }
            s.Append(TempForAllowNull);
            return s;
        }
        private static StringBuilder _GetNonRefrancialParametersEqualWithAtSign(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in func.Parameters)
            {
                if(!item.IsRef)
                {
                    s.AppendLine("                  , " + item.Name + " = @" + item.Name);
                }
            }
            return s.Remove(0, 20);
        }
        public static StringBuilder GetNoneRefrancialParameters(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in func.Parameters)
            {
                if (!item.IsRef)
                {
                    s.Append(" , " + item.Type + " ");
                    s.Append(item.Name);
                }

            }

            if (s.Length > 2) return s.Remove(0, 2);

            return s;
        }
        public static StringBuilder GetNoneRefrancialParametersWithOutDataType(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in func.Parameters)
            {
                if (!item.IsRef)
                {
                    s.Append(" , ");
                    s.Append(item.Name);
                }

            }

            return s.Remove(0, 3);
        }  
        private static StringBuilder _GetNoneRefrancialParametersBetweenBrackitse(clsFunction func)
        {
            return new StringBuilder( "(" + GetNoneRefrancialParameters(func) + ")");
        }
        private static StringBuilder _GetNonReferancialParameterForWhereCondationInQuery(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            bool d = false;

            foreach (var p in func.Parameters)
            {
                if (! p.IsRef)
                {  
                    if(d)
                    {
                        s.Append(" AND ");
                    }

                    s.Append(p.Name);
                    s.Append(" = ");
                    s.Append("@" + p.Name);
  
                    d = true;
                }
              
            }    
            return s ;
        }
        private static StringBuilder _GetNonReferancialParameterForCommandToAddWhitValue(clsFunction func , string CommandName = "command")
        {
            StringBuilder s = new StringBuilder();
           
            foreach (var p in func.Parameters)
            {  
                if(! p.IsRef)
                {
                    s.Append("    " + CommandName);
                    s.AppendLine($@".Parameters.AddWithValue(""@{p.Name}"" , {p.Name} );");
                }
                
            }    
            return s ;
        }
        private static StringBuilder _GetNoneRefrancialParameterForReaderWhithReaderCasting(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            StringBuilder TempForAllowNull = new StringBuilder();
            foreach (var p in func.Parameters)
            {
                if(p.IsRef)
                {
                    if(p.AllowNull)
                    {
                        TempForAllowNull.AppendLine("");
                        TempForAllowNull.AppendLine($@"            //{p.Name}: allows null in database so we should handle null");
                        TempForAllowNull.AppendLine($@"            if(reader[""{p.Name}""] != System.DBNull.Value)");
                        TempForAllowNull.AppendLine($@"            {{");
                        TempForAllowNull.AppendLine($@"                 {p.Name} = ({p.Type})reader[""{p.Name}""];");
                        TempForAllowNull.AppendLine($@"            }}");
                        TempForAllowNull.AppendLine($@"            else");
                        TempForAllowNull.AppendLine($@"            {{");
                        TempForAllowNull.AppendLine($@"                 {p.Name} = {p.DefultValue};");
                        TempForAllowNull.AppendLine($@"            }}");
                    }
                    else
                    {
                        s.AppendLine($@"            {p.Name} = ({p.Type})reader[""{p.Name}""];");
                    }
                }              
            }  
            
            s.Append(TempForAllowNull);  
            return s ;
        }
        private static StringBuilder _GetAllParameterForCommandToAddWhitValue(clsFunction func , string CommandName = "command")
        {
            StringBuilder s = new StringBuilder();
            

            foreach (var p in func.Parameters)
            {
                s.Append("    " + CommandName);
                s.AppendLine($@".Parameters.AddWhithValue(""@{p.Name}"" , {p.Name} );");
              
            }    
            return s ;
        }
        private static StringBuilder _GetAllParameterWhitComaSeprator(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in func.Parameters)
            {
                s.Append(" , "+item.Type + " ");
                s.Append(item.Name);
            }

            return s.Remove(0, 2);

        }
        private static StringBuilder _GetAllParameterWhitComaSepratorWithOutDataType(clsFunction func)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in func.Parameters)
            {
                s.Append(" , this.");
                s.Append(item.Name);
            }

             return s.Remove(0, 2);
        }
        
        private static string _GenerateAsFindByTemplate(clsFunction func)
        {
           func.TextToCalling = $@"Get{func.ParentName}Info({_GetParameterWhitComaSepratorIncludeReferancedWithOutDataType(func)})";
           StringBuilder s = new StringBuilder( $@"public static bool  Get{func.ParentName}Info {_GetParameterWhitComaSepratorIncludeReferanceBetweenBrackitse(func)}
{{
    bool isFound = false;
    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
    string query = ""SELECT * FROM {func.ParentGeneralName} WHERE  {_GetNonReferancialParameterForWhereCondationInQuery(func)}"";

    SqlCommand command = new SqlCommand(query, connection);
{_GetNonReferancialParameterForCommandToAddWhitValue(func)}
    try
    {{
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {{
            isFound = true;

{_GetNoneRefrancialParameterForReaderWhithReaderCasting(func)}

        }}
        else
        {{
            // The record was not found
            isFound = false;
        }}

    }}
    catch /*(Exception ex)*/
    {{
        isFound = false;
    }}
    finally
    {{ 
        connection.Close(); 
    }}

    return isFound;
}}
");
            return s.ToString();
        }
        private static string _GenerateAsIsExistTemplate(clsFunction func)
        {
            func.TextToCalling = $@"Is{func.ParentName}Exist({GetNoneRefrancialParametersWithOutDataType(func)})";
            StringBuilder s = new StringBuilder($@"public static bool  Is{func.ParentName}Exist ({GetNoneRefrancialParameters(func)})
{{
    bool isFound = false;
    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
    string query = ""SELECT Found=1 FROM {func.ParentGeneralName} WHERE  {_GetNonReferancialParameterForWhereCondationInQuery(func)}"";

    SqlCommand command = new SqlCommand(query, connection);
{_GetNonReferancialParameterForCommandToAddWhitValue(func)}
    try
    {{
        connection.Open();

        SqlDataReader reader = command.ExecuteReader();
        isFound = reader.HasRows;				
        reader.Close(); 
    }}
    catch /*(Exception ex)*/
    {{
        isFound = false;
    }}
    finally
    {{ 
        connection.Close(); 
    }}

    return isFound;
}}
");
            return s.ToString();

        }
        private static string _GenerateAsAddNewTemplate(clsFunction func)
        {
            func.TextToCalling = $@"AddNew{func.ParentName}({_GetRefrancialParametersWithOutDataType(func)})";
            StringBuilder s = new StringBuilder( $@"public static int  AddNew{func.ParentName} {_GetRefrancialParametersBetweenBrackitse(func)} ");
            s.Append($@"
{{
    //this function will return the new contact id if succeeded and -1 if not.
    int {func.ParentName}ID = -1;

    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

    string query = @""INSERT INTO {func.ParentGeneralName} ({_GetReferacialParameterNamesWithComaSeprator(func)})
                    VALUES ({_GetReferacialParameterNamesWithComaSepratorAndAtSymbol(func)});
                    SELECT SCOPE_IDENTITY();"";
    
    SqlCommand command = new SqlCommand(query, connection);

{_GetReferancialParameterForCommandToAddWhitValue(func)}
    try
    {{
        connection.Open();

        object result = command.ExecuteScalar();
			

        if (result != null && int.TryParse(result.ToString(), out int insertedID))
        {{
            {func.ParentName}ID = insertedID;
        }}
    }}

    catch /*(Exception ex)*/
    {{			   
    }}

    finally 
    {{ 
        connection.Close(); 
    }}

    return {func.ParentName}ID;

}}
                ");
            return s.ToString();
        }
        private static string _GenerateAsUpdateTemplate(clsFunction func)
        {
            func.TextToCalling = $@"Update{func.ParentName}({_GetAllParameterWhitComaSepratorWithOutDataType(func)})";

            StringBuilder s = new StringBuilder( $@"public static bool  Update{func.ParentName} ({_GetAllParameterWhitComaSeprator(func)}) ");
            s.Append($@"
{{    
    int rowsAffected=0;

    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

    string query = @""Update {func.ParentGeneralName} 
                    set 
                    {_GetRefrancialParametersEqualWithAtSign(func)}                     Where {_GetNonRefrancialParametersEqualWithAtSign(func)};"";
    
    SqlCommand command = new SqlCommand(query, connection);

{_GetNonReferancialParameterForCommandToAddWhitValue(func)}{_GetReferancialParameterForCommandToAddWhitValue(func)}
    try
    {{
        connection.Open();
        rowsAffected = command.ExecuteNonQuery();
    }}

    catch /*(Exception ex)*/
    {{			   
    }}

    finally 
    {{ 
        connection.Close(); 
    }}

    return (rowsAffected > 0);

}}");
            return s.ToString();
        }
        private static string _GenerateAsDeleteTemplate(clsFunction func)
        {
            func.TextToCalling = $@"Delete{func.ParentName}({GetNoneRefrancialParametersWithOutDataType(func)})";

            StringBuilder s = new StringBuilder( $@"public static bool  Delete{func.ParentName} ({GetNoneRefrancialParameters(func)}) ");
            s.Append($@"
{{    
    int rowsAffected=0;

    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

    string query = @""Delete Contacts where Where {_GetNonRefrancialParametersEqualWithAtSign(func)};"";
    
    SqlCommand command = new SqlCommand(query, connection);

{_GetNonReferancialParameterForCommandToAddWhitValue(func)}
    try
    {{
        connection.Open();
        rowsAffected = command.ExecuteNonQuery();
    }}

    catch /*(Exception ex)*/
    {{			   
    }}

    finally 
    {{ 
        connection.Close(); 
    }}

    return (rowsAffected > 0);

}}");
            return s.ToString();
        }
        private static string _GenerateAsGetAllTemplate(clsFunction func)
        {
            func.TextToCalling = $@"GetAll{func.ParentGeneralName}()";

            StringBuilder s = new StringBuilder($@"public static DataTable GetAll{func.ParentGeneralName}()");
            s.Append($@"
{{    
    DataTable dt = new DataTable();

    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

    string query = @""SELECT * FROM {func.ParentGeneralName} ;"";
    
    SqlCommand command = new SqlCommand(query, connection);

    try
    {{
        connection.Open();

        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {{
            dt.Load(reader);
        }}

        reader.Close();
    }}

    catch /*(Exception ex)*/
    {{			   
    }}

    finally 
    {{ 
        connection.Close(); 
    }}

    return dt;

}}");
            return s.ToString();
        }

        public static string GetFunctionCode(clsFunction func , enFunctionTemplateType TemplateType)
        {
            switch (TemplateType)
            {
                case clsSource.enFunctionTemplateType.Find :
                {
                    return _GenerateAsFindByTemplate(func);
                }
                case clsSource.enFunctionTemplateType.IsExist :
                {
                    return _GenerateAsIsExistTemplate(func);
                }
                case clsSource.enFunctionTemplateType.AddNew :
                {
                    return _GenerateAsAddNewTemplate(func);
                }
                case clsSource.enFunctionTemplateType.Update :
                {
                    return _GenerateAsUpdateTemplate(func);
                }
                case clsSource.enFunctionTemplateType.Delete :
                {
                    return _GenerateAsDeleteTemplate(func);
                }
                case clsSource.enFunctionTemplateType.GetAll :
                {
                    return _GenerateAsGetAllTemplate(func);
                }
            }
            return "///NotDefineTempletType///";
        }
        public static string GenerateDefualtFunctionsCode(clsFunction func)
        {
            return
                (
                    _GenerateAsFindByTemplate(func) +
                    _GenerateAsAddNewTemplate(func) +
                    _GenerateAsUpdateTemplate(func) +
                    _GenerateAsDeleteTemplate(func) +
                    _GenerateAsGetAllTemplate(func) +
                    _GenerateAsIsExistTemplate(func)

                );
        }
    }
}
