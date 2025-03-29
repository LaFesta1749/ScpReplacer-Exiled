using System;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace ScpReplacer;

public class Plugin : Plugin<Config>
{
    public override string Author => "LaFesta1749";
    public override string Name => "ScpReplacer";
    public override string Prefix => "ScpReplacer";
    public override Version Version { get; } = new(1, 0, 0);
    public override Version RequiredExiledVersion { get; } = new(9, 5, 1);
    public static Plugin Instance;
    public readonly List<Tuple<RoleTypeId, Player, Vector3, float, string, bool, List<Player>>> RoleTypeIds = [];

    public override void OnEnabled()
    {
        Instance = this;
        Exiled.Events.Handlers.Player.Left += EventHandlers.OnLeft;
        Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStarted;
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Instance = null;
        Exiled.Events.Handlers.Player.Left -= EventHandlers.OnLeft;
        Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStarted;
        base.OnDisabled();
    }
}