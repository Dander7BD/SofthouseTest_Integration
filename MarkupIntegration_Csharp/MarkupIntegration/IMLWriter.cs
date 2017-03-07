using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupIntegration
{
    public enum ElementType { Base, Element, List, ListElement };

    public interface IMLWriter : IDisposable
    {
        string CurrentName { get; }
        ElementType CurrentType { get; }
        
        void WriteHeader();
        void WriteFooter();
        void PushList(string name);
        void PushListElement();
        void PushElement(string name);
        void AttachProperty(string name, string value);
        void AttachProperty(string name, int value);
        void PopLast();
        void PopUntilMatch(ElementType type);
        void PopUntilMatch(ElementType type, string name);
        bool WithinElement(ElementType type);
        bool WithinElement(ElementType type, string name);
    }
}
