using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;

namespace ScpReplacer.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class Volunteer : ICommand
{
    public string Command => "volunteer";
    public string[] Aliases => null;
    public string Description => "Volunteer to be replaced as SCP";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);
        var roleTypeId = RoleTypeId.None;
        if (Round.IsLobby)
        {
            response = "You can't use this command in the lobby!";
            return false;
        }

        if (player.IsScp)
        {
            response = "As an SCP you can't use this command!";
            return false;
        }

        switch (arguments.Count)
        {
            case 0:
                if (Plugin.Instance.RoleTypeIds.LastOrDefault() == null || Plugin.Instance.RoleTypeIds.Count == 0)
                {
                    response = "No available SCPs!";
                    return false;
                }

                roleTypeId = Plugin.Instance.RoleTypeIds.LastOrDefault()!.Item1;
                break;
            case 1 when !Enum.TryParse(arguments.First(), true, out roleTypeId):
                response =
                    $"Usage: .volunteer [scp] Available SCPs: {string.Join(", ", string.Join(", ", Plugin.Instance.RoleTypeIds.Select(r => r.Item1.ToString()).DefaultIfEmpty("No available SCPs!")))}";
                return false;
        }

        if (Plugin.Instance.RoleTypeIds.All(r => r.Item1 != roleTypeId))
        {
            response =
                $"Unknown SCP! Available SCPs: {string.Join(", ", string.Join(", ", Plugin.Instance.RoleTypeIds.Select(r => r.Item1.ToString()).DefaultIfEmpty("No available SCPs!")))}";
            return false;
        }

        var data = Plugin.Instance.RoleTypeIds.FirstOrDefault(p => p.Item1 == roleTypeId);
        if (data == null)
        {
            response = "Data is null: " +
                       string.Join(", ", Plugin.Instance.RoleTypeIds.Select(r => r.Item1.ToString()));
            return false;
        }

        if (data.Item7.Contains(player))
        {
            response = "You have already sent a request!";
            return false;
        }

        data.Item7.Add(player);
        response = "You have successfully sent the request!";
        return true;
    }
}