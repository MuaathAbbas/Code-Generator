using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGeneratorBusiness
{
    public class clsBusinessFunction
    {
        clsFunction Function;
        clsSource.enFunctionTemplateType enFunctionTemplateType;
        public clsBusinessFunction(clsDataAccessFunction Function ) 
        {
            this.Function = Function.function;
            this.enFunctionTemplateType = Function.TemplateType;
        }

        public string CodeAsText { get { return clsFunctionGeneratorBusinessLayer.GetFunctionCode(this.Function, this.enFunctionTemplateType); } }

    }
}
