using System.Collections.Generic;
using System.Linq;
using Discord.API;
using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class MessageDeleteAuditLogData : IAuditLogData
    {
        private MessageDeleteAuditLogData(ulong channelId, int count, AuditLogChange[] changes)
        {
            MessageIds = changes.Where(c => c.ChangedProperty == "id").SelectMany(c => c.OldValue.Values<ulong>());
            ChannelId = channelId;
            MessageCount = count;
        }

        internal static MessageDeleteAuditLogData Create(BaseDiscordClient discord, Model log, EntryModel entry)
        {
            return new MessageDeleteAuditLogData(entry.Options.MessageDeleteChannelId.Value, entry.Options.MessageDeleteCount.Value, entry.Changes);
        }

        public int MessageCount { get; }
        public ulong ChannelId { get; }
        public IEnumerable<ulong> MessageIds { get; }
    }
}
