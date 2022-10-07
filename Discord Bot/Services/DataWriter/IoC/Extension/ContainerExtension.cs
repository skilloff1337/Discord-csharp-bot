using System.Collections.Generic;
using Discord_Bot.Models;
using Discord_Bot.Services.DataWriter.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Services.DataWriter.IoC.Extension
{
    public static class ContainerExtension
    {
        public static IServiceCollection BindingWriters(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IJsonWriter<Config>, JsonConfigWriter>()
                .AddSingleton<IJsonWriter<List<string>>,JsonBadWordsWriter>()
                .AddSingleton<IJsonWriter<List<User>>,JsonUserWriter>();
        }
    }
}