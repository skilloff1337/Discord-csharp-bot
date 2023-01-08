# Discord-csharp-bot

## The bot can:

- Check messages for forbidden words.
- Level leveling system.
- Admin Team
- Custom commands
- Writing news, messages on behalf of the bot.
- History messages
- Choosing a language for your bot
- Automatic role assignment
- Full logging of the bot and users


## **Admin commands:**
* !addAdmin [user] - Set a new text channel for admin commands.
* !setAdminChannel [channel] - Set a new text channel for admin commands.
* !setCommandChannel [channel] - Set a new text channel for common commands
* !setGameName [name] - Help on commands
* !setLanguage [language] - Set the language of the bot. (The language will change after restart)
* !setLogChannel [channel] - Set a new text channel for logging.
* !setBotName [newName] - Change bot's nickname
* !setProtectionBadWords [value] - Enable/disable protection from censored words (true/false)
* !setWelcomeChannel [channel] - Set a new text channel for general commands
* !addRank [role] [needExp] [level] [name] - Add a new rank in the ranking system
* !deleteRank [level] - Delete a rank
* !infoRanks - Information on user ranks.
* !listRanks - View the entire list of ranks
* !updateRank [levelRank] [role] [needExp] [name] - Update rank
* !ban [user] [days] [reason] - Ban the user
* !kick [user] [reason] - Kick a user
* !mute [user] [hours] [reason] - Mute the user
* !unban [user] - Unban a user
* !unmute [user] - Unmute user
* !addBotLanguageReaction [idChannel] [idMessage] - Add emoji from bot to message, language roles
* !addBotServerReaction [idChannel] [idMessage] - Add emoji from bot to message, server roles
* !deleteMessageChannel [channel] [countMessages] - Delete messages in a text channel.
* !deleteMessageUserInChannel [user] [channel] [checkLastMessages] - Delete user's messages in recent messages, [checkLastMessages] specifies how many latest messages in channels to view
* !deleteMessageUser [user] [checkLastMessages] - Delete user's messages in recent messages, [checkLastMessages] specifies how many latest messages in channels to view
* !messageHistory [idMessage] - Get full message information
* !send [channel] [role] [text] - Send a message to chat with role notification.
* !sendreply [channel] [idMessage] [text] - Send a message to the chat with a quote
* !sendPrivate [user] [text] - Send message to private messages
* !sendRoleLanguageMessage - Send a message about getting a language role.
* !sendRoleServerMessage - Send a message about getting a server role.
* !help - Help on commands
* !admins - List of administrators
* !inforole [role] - Show information about the role
* !infoServer - Show information about the server
* !infoUser [user] - Show information about the user
* !addBadWord [word] - Add a new bad word to the checklist
* !delBadWord [word] - Remove a word from the check list
* !listBadWords - Show the entire list of bad words

## **User commands:**
* /help - Show commands
* /myrank - Show information for your rank
* /8ball question - Ask your question and get an answer
* /flip user - Flip a coin with a user
* /roll min max - Roll number
