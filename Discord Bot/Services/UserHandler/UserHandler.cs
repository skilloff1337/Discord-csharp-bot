using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.Models;
using Discord_Bot.Services.DataBase.Interfaces;
using Discord_Bot.Services.DataReader.Interfaces;
using Discord_Bot.Services.UserHandler.Interfaces;

namespace Discord_Bot.Services.UserHandler
{
    public class UserHandler : IUserHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IRepository<MessageUser> _repository;
        private readonly Config _config;
        private readonly Color _color = new(153, 0, 0);

        private readonly List<MessageUser> _newMessages = new();

        public UserHandler(DiscordSocketClient client,
            Config config,
            IRepository<MessageUser> repository)
        {
            _client = client;
            _config = config;
            _repository = repository;
        }

        public async Task InstallEventsAsync()
        {
            _client.MessageUpdated += UpdateMessage;
            _client.MessageDeleted += DeletedMessage;
            _client.MessageReceived += WriteMessage;
            await Task.CompletedTask;
        }

        private async Task WriteMessage(SocketMessage arg)
        {
            if (arg.Channel.Id == _config.ChannelIdForBotLog 
                || arg.Author.IsBot 
                || arg.Channel is IDMChannel)
                return;

            var messageUser = new MessageUser()
            {
                CreateMessage = arg.CreatedAt.DateTime,
                EditMessage = arg.CreatedAt.DateTime,
                MessageId = arg.Id,
                CurrentMessage = arg.CleanContent,
                HistoryMessage = new List<string> 
                    {$"{arg.CreatedAt.DateTime:G} | {arg.CleanContent}"},
                NickName = $"{arg.Author.Username}#{arg.Author.Discriminator}",
                ChannelId = arg.Channel.Id,
                ChannelName = arg.Channel.Name
            };

            _newMessages.Add(messageUser);

            if (_newMessages.Count < 2) //todo need change to 50
                return;

            _repository.CreateMany(_newMessages);
            _newMessages.Clear();
        }

        private async Task DeletedMessage(Cacheable<IMessage, ulong> msgId, Cacheable<IMessageChannel, ulong> arg2)
        {
            var channel = _client.GetChannel(_config.ChannelIdForBotLog) as IMessageChannel;

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithCurrentTimestamp()
                .WithDescription(
                    $"***Channel Name:*** {_client.GetChannel(arg2.Id)} \n" +
                    $"***Channel ID:*** {arg2.Id}\n" +
                    $"***Message ID:*** {msgId.Id}\n");
            await channel!.SendMessageAsync($"Message deleted", embed: embed.Build());
        }

        private async Task UpdateMessage(Cacheable<IMessage, ulong> msgId, SocketMessage newMsg,
            ISocketMessageChannel arg3)
        {
            if (_client.GetChannel(_config.ChannelIdForBotLog) is not IMessageChannel logChannel)
            {
                Console.WriteLine($"Invalid channel ID for logs");
                return;
            }

            var itsDataBase = false;
            var message = _newMessages.FirstOrDefault(x => x.MessageId == msgId.Id);
            
            if (message is null)
            {
                message = _repository.GetById(msgId.Id);
                itsDataBase = true;
            }

            if (message is null)
                return;

            message.HistoryMessage.Add($"{DateTime.Now:G} | {newMsg.CleanContent}");
            message.CurrentMessage = newMsg.CleanContent;
            message.EditMessage = DateTime.Now;
            message.EditCount++;

            
            if (itsDataBase)
                _repository.Update(message);

            var embed = new EmbedBuilder()
                .WithColor(_color)
                .WithDescription(
                    $"***Channel:*** {message.ChannelName} ***ID:*** {message.ChannelId}\n" +
                    $"***Message ID:*** {message.Id}\n" +
                    $"***Message before:***\n{message.HistoryMessage[^2]}\n" +
                    $"***Message after:***\n{DateTime.Now:G} | {message.CurrentMessage}");

            await logChannel!.SendMessageAsync($"Message from ***{message.NickName}*** updated", embed: embed.Build());
        }
    }
}