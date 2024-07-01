using UnityEngine;

namespace Alteruna.TextChatCommands
{
	public class CommandPlayers : ITextChatCommand
	{
		public string Command { get; } = "players";
		public string Description { get; } = "Get the amount of players in the room";
		public string Usage { get; } = "/me";
		public bool IsCheat { get; } = false;
		public bool IgnoreCase { get; } = true;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		public static void Init() => TextChatSynchronizable.Commands.Add(new CommandMe());

		public string Execute(TextChatSynchronizable textChat, string[] args)
		{

			if (textChat.Multiplayer == null)
			{
				textChat.LogError("No valid Multiplayer component.");
				return null;
			}
			if (textChat.Multiplayer. == null)
			{
				textChat.LogError("No user information available.");
				return null;
			}
			if (avatar != null)
			{
				return $"There are {textChat.Multiplayer.GetUsers().Count} player in the room";
			}
			else
			{
				return "An error occured";
			}
		}
	}
}