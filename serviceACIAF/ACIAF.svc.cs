using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace serviceACIAF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ACIAF" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ACIAF.svc or ACIAF.svc.cs at the Solution Explorer and start debugging.
    public class ACIAF : IACIAF
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public RemoteACIAFAgentInstanceArchiveFileInfo
        SendAgentInstanceArchiveToRemoteACIAFServer(DownloadArchivedAgentRequest request)
        {
            RemoteACIAFAgentInstanceArchiveFileInfo result = new RemoteACIAFAgentInstanceArchiveFileInfo ();            
            try
            {
                string filePath = System.IO.Path.Combine(@"d:\toSEND\", request.FileName);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

                // check if exists
                if (!fileInfo.Exists)
                    throw new System.IO.FileNotFoundException("File not found",
                                                              request.FileName);

                // open stream
                System.IO.FileStream stream = new System.IO.FileStream(filePath,
                          System.IO.FileMode.Open, System.IO.FileAccess.Read);

                // return result 
                result.FileName = request.FileName;
                result.Length = fileInfo.Length;
                result.FileByteStream = stream;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        } // end SendAgentInstanceArchiveToRemoteACIAFServer....


        public void ReceiveAgentInstanceArchiveFROMRemoteACIAFServer(
            RemoteACIAFAgentInstanceArchiveFileInfo request)
        {
            FileStream targetStream = null;
            Stream sourceStream = request.FileByteStream;

            string uploadFolder = @"d:\toRECEIVE";

            string filePath = Path.Combine(uploadFolder, request.FileName);

            using (targetStream = new FileStream(filePath, FileMode.Create,
                                  FileAccess.Write, FileShare.None))
            {
                //read from the input stream in 65000 byte chunks

                const int bufferLen = 65000;
                byte[] buffer = new byte[bufferLen];
                int count = 0;
                while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                {
                    // save to output stream
                    targetStream.Write(buffer, 0, count);
                    count = 0;
                }
                targetStream.Close();
                sourceStream.Close();
            }
        } //end ReceiveAgentInstanceArchiveFROMRemoteACIAFServer...



    }
}
