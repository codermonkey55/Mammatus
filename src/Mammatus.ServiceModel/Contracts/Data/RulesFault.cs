using System.Runtime.Serialization;

namespace Mammatus.ServiceModel.Contracts.Data
{
    [DataContract]
    public class RulesFault
    {
        [DataMember]
        public string[] Operation { get; set; }

        [DataMember]
        public string[] ProblemType { get; set; }
    }
}