namespace Mammatus.ServiceModel.Model
{
    public interface ICommunicator
    {
        object InvokeOperation(object[] parameters);
    }
}
