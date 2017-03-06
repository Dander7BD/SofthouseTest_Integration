using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupIntegration
{
    public interface IMLWriter
    {
        string CurrentObject { get; }
        
        void CreateHeader();
        void CreateFooter();
        void CreateObject(string name);
        void AddProperty(string name, string value);
        void CloseObject();        
        void CloseAll();
    }
}
