using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace ScpReplacer.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class Specatator : ICommand
{
    public string Command => "spectator";
    public string[] Aliases => null;
    public string Description => "To change SCP to spectator";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Plugin.Instance.Config.IsSpecatorCmdEnabled)
        {
            response = "This command is disabled!";
            return false;
        }

        var player = Player.Get(sender);
        if (Round.IsLobby)
        {
            response = "In the lobby you can't use this command!";
            return false;
        }

        if (!player.IsScp)
        {
            response = "As a human you can't use this command!";
            return false;
        }

        if (Plugin.Instance.Config.BlacklistedScps.Contains(player.Role))
        {
            response = "With this role you can't use this command!";
            return false;
        }

        if (Round.ElapsedTime.TotalSeconds > Plugin.Instance.Config.Timer)
        {
            response = "The time has expired!";
            return false;
        }

        if (Plugin.Instance.RoleTypeIds.Any(r => r.Item5 == player.UserId))
        {
            response = "You have already sent a request!";
            return false;
        }

        Timing.CallDelayed(Plugin.Instance.Config.LotteryTimer, () => EventHandlers.SwapRoles(player.UserId));
        Plugin.Instance.RoleTypeIds.Add(new Tuple<RoleTypeId, Player, Vector3, float, string, bool, List<Player>>(
            player.Role.Type, player,
            player.Position, player.Health, player.UserId, false, []));
        foreach (var players in Player.List.Where(p => !p.IsScp))
            players.Broadcast(Plugin.Instance.Config.PlayerBroadcast.Duration, Plugin.Instance.Config.PlayerBroadcast.Content.Replace("%scp%", player.Role.Type.ToString()));
        response = "You have successfully sent the request!";
        return true;
    }
}