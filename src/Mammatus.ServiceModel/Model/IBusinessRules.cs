using System.Collections.Generic;

namespace Mammatus.ServiceModel.Model
{
    public interface IBusinessRules
    {
        IEnumerable<string> Execute(object obj);
    }
}
