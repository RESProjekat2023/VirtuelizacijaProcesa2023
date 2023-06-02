using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza
{
    public interface IXmlDataBase
    {
        void IzbrisiXML();
        void UpisXml(Load l);
        void UpisXmlAudit(Audit a);
        void UpisXmlImported(ImportedFile If);
    }
}
