using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace ScpReplacer;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;

    [Description("Timer in secondsd until the SCPs are replaced.")]
    public int Timer { get; set; } = 120;

    [Description("Time until the SCPs are choosen.")]
    public int LotteryTimer { get; set; } = 15;

    [Description("Player's broadcast message when an SCP leave.")]
    public Exiled.API.Features.Broadcast PlayerBroadcast { get; set; } =
        new("%scp% left. Open the console and write \".volunteer %scp%\" to volunteer! If you don't specify a SCP name, you will be replaced to the last SCP!", 20);

    [Description("SCP's broadcast message at the round start")]
    public Exiled.API.Features.Broadcast ScpBroadcast { get; set; } =
        new("If you want to leave as an SCP open the console and write .spectator or .human!", 15);

    [Description("Is the .specator command enabled")]
    public bool IsSpecatorCmdEnabled { get; set; } = true;

    [Description("Is the .human command enabled")]
    public bool IsHumanCmdEnabled { get; set; } = true;

    [Description("Scientist spawn chance. If it's 10, then the D-Class spawn chance will be 90.")]
    public int ScientistChance { get; set; } = 10;

    [Description("If nobody volunteers, the SCP will be replaced with a random D-Class.")]
    public bool RandomDClass { get; set; } = true;

    [Description("Change the SCP even if there's no one to replace him.")]
    public bool ChangeScp { get; set; } = true;

    [Description("Blacklisted SCPs.")] public List<RoleTypeId> BlacklistedScps { get; set; } = [RoleTypeId.Scp0492];
}