using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace serviceACIAF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IACIAF" in both code and config file together.
    [ServiceContract]
    public interface IACIAF
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here

        [OperationContract]
        RemoteACIAFAgentInstanceArchiveFileInfo 
        SendAgentInstanceArchiveToRemoteACIAFServer(DownloadArchivedAgentRequest request);

        [OperationContract]
        void ReceiveAgentInstanceArchiveFROMRemoteACIAFServer(
            RemoteACIAFAgentInstanceArchiveFileInfo request);
    }
    [MessageContract]
    public class DownloadArchivedAgentRequest
    {
        [MessageBodyMember]
        public string FileName;
    }
    [MessageContract]
    public class RemoteACIAFAgentInstanceArchiveFileInfo  //: IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName; 

        [MessageHeader(MustUnderstand = true)]
        public long Length; // { get; set; }

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream; 

        /****
        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        } 
        ****/
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }






}
