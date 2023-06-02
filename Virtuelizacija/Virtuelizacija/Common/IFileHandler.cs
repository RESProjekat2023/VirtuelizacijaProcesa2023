using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{ 
    [ServiceContract]
    public interface IFileHandler
    {

        [OperationContract]
       void UploadFiles(List<FileCSV> fileStream1, List<FileCSV> fileStream2);
        [OperationContract]
        List<Audit> VratiKlijentuAudit();
        [OperationContract]
        List<Load> VratiKlijentuLoad();
        [OperationContract]
        List<ImportedFile> VratiKlijentuImported();

    }
}
