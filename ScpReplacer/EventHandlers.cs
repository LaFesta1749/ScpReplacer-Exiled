using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using ScpReplacer.Commands;
using UnityEngine;

namespace ScpReplacer;

public class EventHandlers
{
    public static void OnRoundStarted()
    {
        Plugin.Instance.RoleTypeIds.Clear();
        foreach (var player in Player.List.Where(p => p.IsScp))
            player.Broadcast(Plugin.Instance.Config.ScpBroadcast);
    }

    public static void OnLeft(LeftEventArgs ev)
    {
        if (Round.ElapsedTime.TotalSeconds > Plugin.Instance.Config.Timer) return;
        if (!ev.Player.IsScp) return;
        if (Plugin.Instance.RoleTypeIds.Any(r => r.Item5 == ev.Player.UserId)) return;
        if (Plugin.Instance.Config.BlacklistedScps.Contains(ev.Player.Role)) return;
        Timing.CallDelayed(Plugin.Instance.Config.LotteryTimer, () => SwapRoles(ev.Player.UserId));
        Plugin.Instance.RoleTypeIds.Add(new Tuple<RoleTypeId, Player, Vector3, float, string, bool, List<Player>>(
            ev.Player.Role.Type, ev.Player, ev.Player.Position, ev.Player.Health, ev.Player.UserId, false, []));
        foreach (var player in Player.List.Where(p => !p.IsScp))
        {
            player.Broadcast(Plugin.Instance.Config.PlayerBroadcast.Duration, Plugin.Instance.Config.PlayerBroadcast.Content.Replace("%scp%", ev.Player.Role.Type.ToString()));
        }
    }

    public static void SwapRoles(string userId)
    {
        if (userId is null) return;
        var data = Plugin.Instance.RoleTypeIds.FirstOrDefault(r => r.Item5 == userId);
        if (data is null) return;
        var players = data.Item7;
        if (players.Count == 0 && Plugin.Instance.Config.RandomDClass)
        {
            players = Player.List.Where(p => p.Role == RoleTypeId.ClassD).ToList();
        }

        if (players.Count == 0)
        {
            if (!Plugin.Instance.Config.ChangeScp) return;
            if (data.Item6)
            {
                var scientistChance = Plugin.Instance.Config.ScientistChance;
                data.Item2.Role.Set(UnityEngine.Random.Range(0, 100) < scientistChance
                    ? RoleTypeId.Scientist
                    : RoleTypeId.ClassD);
            }
            else
            {
                data.Item2.Role.Set(RoleTypeId.Spectator);
            }

            return;
        }

        var selectedPlayer = players[UnityEngine.Random.Range(0, players.Count)];
        var scpPlayer = data.Item2;
        var scpPosition = data.Item3;
        var scpHealth = data.Item4;
        var scpId = data.Item5;
        if (Player.Get(scpId) != null && Player.Get(scpId).Role.Type == data.Item1)
        {
            scpPlayer = Player.Get(scpPlayer.Id);
            scpPosition = scpPlayer.Position;
            scpHealth = scpPlayer.Health;
        }

        selectedPlayer.Role.Set(data.Item1);
        selectedPlayer.Teleport(scpPosition);
        selectedPlayer.Health = scpHealth;
        if (Player.Get(scpId) != null && Player.Get(scpId).IsScp && selectedPlayer.UserId != scpPlayer.UserId)
        {
            if (data.Item6)
            {
                var scientistChance = Plugin.Instance.Config.ScientistChance;
                scpPlayer.Role.Set(UnityEngine.Random.Range(0, 100) < scientistChance
                    ? RoleTypeId.Scientist
                    : RoleTypeId.ClassD);
            }
            else
            {
                scpPlayer.Role.Set(RoleTypeId.Spectator);
            }
        }

        Plugin.Instance.RoleTypeIds.Remove(Plugin.Instance.RoleTypeIds.First(r => r.Item5 == scpId));
        foreach (var roleTypeId in Plugin.Instance.RoleTypeIds)
        {
            roleTypeId.Item7.RemoveAll(p => p.UserId == selectedPlayer.UserId);
        }
    }
}