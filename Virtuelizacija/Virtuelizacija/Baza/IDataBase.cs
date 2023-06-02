using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Baza
{
   
      public interface IDataBase
    {
        //Write operacije
       
      
        void WriteToInMemoryLoad(Load load);
       
        void WriteToInMemoryAudit(Audit audit);
        
        void WriteToInMemoryImportedFile(ImportedFile importedFiles);

        //Read operacije
        
        List<Load> ReadFromInMemoryLoad();
       
        List<ImportedFile> ReadFromInMemoryImportedFile();
       
        List<Audit> ReadFromInMemoryAudit();
    }
}
