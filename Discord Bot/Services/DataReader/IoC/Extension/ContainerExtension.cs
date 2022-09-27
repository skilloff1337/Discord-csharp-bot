﻿using System.Collections.Generic;
using Discord_Bot.Models;
using Discord_Bot.Services.DataReader.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Services.DataReader.IoC.Extension
{
    public static class ContainerExtension
    {
        public static IServiceCollection BindingReaders(this IServiceCollection collection)
        {
            return collection
                .AddSingleton<IJsonReader<Config>,JsonConfigReader>()
                .AddSingleton<IJsonReader<Dictionary<string,TranslationWord>>,JsonLanguageReader>();
        }
    }
}