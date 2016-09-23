namespace Mammatus.AspNet.Mvc.Initialization
{
    public interface IPostInitializationContext
    {

    }

    public interface IPostWebApplicationInitializationContext : IPostInitializationContext
    {

    }

    public sealed class PostWebApplicationInitializationContext : IPostWebApplicationInitializationContext
    {

    }
}
