using System.Collections.Generic;
using Discord_Bot.Models;
using Discord_Bot.Services.DataWriter;
using Discord_Bot.Services.DataWriter.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Services.RankHandler.IoC.Extension
{
    public static class ContainerExtension
    {
        public static IServiceCollection BindingRankSystem(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IExperienceService, ExperienceService>()
                .AddSingleton<IRankHandler,RankHandler>();
        }
    }
}