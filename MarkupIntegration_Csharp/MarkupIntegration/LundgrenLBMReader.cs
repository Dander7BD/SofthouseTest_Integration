using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupIntegration
{
    /*
     * Lundgren's Line Based Markup language Reader
     *  P|firstname|lastname        -> person
     *  T|mobile|landline           -> phone
     *  A|street|city|zipcode       -> adress
     *  F|name|born                 -> street
     *  P can be followed by T, A and F
     *  F can be followed by T and A
     */
    public class LundgrenLBMReader : IMLReader
    {
    }
}
