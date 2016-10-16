namespace Mammatus.ComponentModel.Models
{
    interface IModelConveter
    {
        object Convert(object from);
    }

    interface IModelConveter<out TOut> : IModelConveter where TOut : class
    {
        TOut Convert(object from);
    }

    interface IModelConveter<TIn, TOut> : IModelConveter<TOut>
        where TIn : class
        where TOut : class
    {
        TOut Convert(TIn from);
    }
}
