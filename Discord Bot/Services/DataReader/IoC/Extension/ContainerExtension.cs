using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Services.DataReader.IoC.Extension
{
    public static class ContainerExtension
    {
        public static IServiceCollection BindingReaders(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<JsonConfigReader>()
                .AddSingleton<JsonCommandTextReader>()
                .AddSingleton<JsonLanguageReader>();
        }
    }
}