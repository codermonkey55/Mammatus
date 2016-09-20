using System.Runtime.Serialization;

namespace Mammatus.ServiceModel.Contracts.Data
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public string Field { get; set; }
    }
}