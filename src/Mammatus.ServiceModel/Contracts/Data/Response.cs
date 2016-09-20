using System.Runtime.Serialization;

namespace Mammatus.ServiceModel.Contracts.Data
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public string Field { get; set; }
    }
}