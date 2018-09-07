﻿using System.Linq;

using Model = Discord.API.AuditLog;
using EntryModel = Discord.API.AuditLogEntry;

namespace Discord.Rest
{
    public class RoleCreateAuditLogData : IAuditLogData
    {
        private RoleCreateAuditLogData(ulong id, RoleEditInfo props)
        {
            RoleId = id;
            Properties = props;
        }

        internal static RoleCreateAuditLogData Create(BaseDiscordClient discord, Model log, EntryModel entry)
        {
            var changes = entry.Changes;

            var colorModel = changes.FirstOrDefault(x => x.ChangedProperty == "color");
            var mentionableModel = changes.FirstOrDefault(x => x.ChangedProperty == "mentionable");
            var hoistModel = changes.FirstOrDefault(x => x.ChangedProperty == "hoist");
            var nameModel = changes.FirstOrDefault(x => x.ChangedProperty == "name");
            var permissionsModel = changes.FirstOrDefault(x => x.ChangedProperty == "permissions");

            uint? colorRaw = colorModel?.NewValue?.ToObject<uint>(discord.ApiClient.Serializer);
            bool? mentionable = mentionableModel?.NewValue?.ToObject<bool>(discord.ApiClient.Serializer);
            bool? hoist = hoistModel?.NewValue?.ToObject<bool>(discord.ApiClient.Serializer);
            string name = nameModel?.NewValue?.ToObject<string>(discord.ApiClient.Serializer);
            ulong? permissionsRaw = permissionsModel?.NewValue?.ToObject<ulong>(discord.ApiClient.Serializer);

            Color? color = null;
            GuildPermissions? permissions = null;

            if (colorRaw.HasValue)
                color = new Color(colorRaw.Value);
            if (permissionsRaw.HasValue)
                permissions = new GuildPermissions(permissionsRaw.Value);

            return new RoleCreateAuditLogData(entry.TargetId.Value,
                new RoleEditInfo(color, mentionable, hoist, name, permissions));
        }

        public ulong RoleId { get; }
        public RoleEditInfo Properties { get; }
    }
}
