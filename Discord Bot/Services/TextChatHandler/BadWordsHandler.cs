using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.BadWords;

namespace Discord_Bot.Services.TextChatHandler
{
    public class BadWordsHandler : IBadWordsHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IBadWords _badWords;

        private readonly Config _config;

        public BadWordsHandler(DiscordSocketClient client, IBadWords badWords, Config config)
        {
            _client = client;
            _badWords = badWords;
            _config = config;
        }

        public async Task Install()
        {
            _client.MessageReceived += MessageReceived;
            await Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage messageParam)
        {
            if (messageParam.Author.IsBot || messageParam is not SocketUserMessage message
                                          || message.Channel is IDMChannel)
                return;
            
            var channel = await _client.GetChannelAsync(_config.ChannelIdForBotLog) as IMessageChannel;
 
            if(!_badWords.CheckForBadWords(message.CleanContent))
                return;
            Console.WriteLine("BAD WORDS FOUND!");
            
            await message.DeleteAsync();
            await message.Author.SendMessageAsync("Do not break the rules of the server, do not use profanity. Your message has been deleted. Be kinder next time :)");
            await channel.SendMessageAsync($"Found a bad word from {message.Author.Username}#{message.Author.Discriminator}" +
                                           $" in a sentence:\n ```{message.CleanContent}```");

        }
    }
}