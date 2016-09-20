
namespace Mammatus.ServiceModel.Model
{
    public interface ITranslator
    {
        TOut Translate<TIn, TOut>(TIn obj);
    }
}
