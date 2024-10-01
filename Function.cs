using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CGeneratorBusiness
{
    public class clsFunction 
    {
        public clsFunction(){}
        public clsFunction(List<clsParameter> Parameters , string ParentTableName ) 
        {
            this.ParentName = ParentTableName;
            this.Parameters = Parameters; 
        }
        public void UpdateParameters(List<clsParameter> Parameters) { this.Parameters = Parameters; }
        public List<clsParameter> Parameters = new List<clsParameter>();
        
        public string Name = "";
        public string Prefix = "";
        public string ParentGeneralName = "";

        private string _parentName;
        public string ParentName
        {
            set 
            {
                this.ParentGeneralName = value;
                _parentName = GenerateParentName(value); 
            }
            get { return _parentName; }
        }
        
        private string _TextToCalling;
        public string TextToCalling
        {
            set { _TextToCalling = value; }
            get { return _TextToCalling; }
        }

        public static string  GenerateParentName(string PName)
        {
            //ChangeToSingle

            switch(PName.ToLower())
            {
                case "people":
                    return "Person";
                case "countries":
                    return "Country";

                default:
                    PName = _Delete_ViewWordFromLast(PName);
                    PName = _DeleteSFromLast(PName);     
                    return PName;
            }
        }
        private static string _DeleteSFromLast(string PName)
        {
            
            if (PName.ToLower().EndsWith("s")) {return PName.Remove(PName.Count() -1 ); }            
            else { return PName; }
        }
        private static string _Delete_ViewWordFromLast(string PName)
        {
            
            if (PName.ToLower().EndsWith("_view")) {return PName.Remove(PName.Count() - 5 ); }            
            else { return PName; }
        }
    }
}
