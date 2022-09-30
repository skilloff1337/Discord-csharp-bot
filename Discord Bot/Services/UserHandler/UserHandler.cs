using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.DataReader;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.UserHandler.Interfaces;

namespace Discord_Bot.Services.UserHandler
{
    public class UserHandler : IUserHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IRepository<MessageUser> _repository;

        private readonly ulong _channelId;
        private readonly Color _color= new(153, 0, 0);
        public UserHandler(DiscordSocketClient client,
            IJsonReader<Config> configReader, 
            IRepository<MessageUser> repository)
        {
            _client = client;
            _repository = repository;

            _channelId = configReader.Load().ChannelIdForBotLog;
        }
        
        public async Task InstallEventsAsync()
        {
            _client.MessageUpdated += UpdateMessage;
            _client.MessageDeleted += DeletedMessage;
            _client.MessageReceived += WriteMessage;
            await Task.CompletedTask;
        }

        private Task WriteMessage(SocketMessage arg)
        {
            _ = Task.Run(async () =>
            {
                if (arg.Channel.Id == _channelId || arg.Author.IsBot)
                    return;

                var messageUser = new MessageUser()
                {
                    CreateMessage = arg.CreatedAt.DateTime,
                    EditMessage = arg.CreatedAt.DateTime,
                    MessageId = arg.Id,
                    CurrentMessage = arg.CleanContent,
                    HistoryMessage = new List<string> {$"{arg.CreatedAt.DateTime:G} | {arg.CleanContent}"},
                    NickName = $"{arg.Author.Username}#{arg.Author.Discriminator}",
                    ChannelId = arg.Channel.Id,
                    ChannelName = arg.Channel.Name
                };

                _repository.Create(messageUser);
            });
            return Task.CompletedTask;
        }

        private Task DeletedMessage(Cacheable<IMessage, ulong> msgId, Cacheable<IMessageChannel, ulong> arg2)
        {
            _ = Task.Run(async () =>
            {
                var message = _repository.GetById(msgId.Id);

                if (message is null)
                    return;

                message.IsDeleted = true;
                message.DeletedDate = DateTime.Now;

                _repository.Update(message);

                var channel = _client.GetChannel(_channelId) as IMessageChannel;

                var embed = new EmbedBuilder()
                    .WithColor(_color)
                    .WithDescription(
                        $"***Channel:*** {message.ChannelName} ***ID:*** {message.ChannelId}\n" +
                        $"***Message ID:*** {message.Id}\n" +
                        $"***Message:***\n{(string.IsNullOrEmpty(message.CurrentMessage) ? "Null" : message.CurrentMessage)}");
                await channel!.SendMessageAsync($"Message from ***{message.NickName}*** deleted", embed: embed.Build());
            });
            return Task.CompletedTask;
        }

        private Task UpdateMessage(Cacheable<IMessage, ulong> msgId, SocketMessage newMsg, ISocketMessageChannel arg3)
        {
            _ = Task.Run(async () =>
            {
                var message = _repository.GetById(msgId.Id);

                if (message is null)
                    return;

                message.HistoryMessage.Add($"{DateTime.Now:G} | {newMsg.CleanContent}");
                message.CurrentMessage = newMsg.CleanContent;
                message.EditMessage = DateTime.Now;
                message.EditCount++;

                _repository.Update(message);

                var channel = _client.GetChannel(_channelId) as IMessageChannel;

                var embed = new EmbedBuilder()
                    .WithColor(_color)
                    .WithDescription(
                        $"***Channel:*** {message.ChannelName} ***ID:*** {message.ChannelId}\n" +
                        $"***Message ID:*** {message.Id}\n" +
                        $"***Message before:***\n{message.HistoryMessage[^2]}\n" +
                        $"***Message after:***\n{DateTime.Now:G} | {message.CurrentMessage}");

                await channel!.SendMessageAsync($"Message from ***{message.NickName}*** updated", embed: embed.Build());
            });
            return Task.CompletedTask;
        }
        
    }
}