using System;
using System.Collections.Generic;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace TerrariaMusicPlayer
{
    [ApiVersion(2, 1)]
    public class TerrariaMusicPlayer : TerrariaPlugin
    {

        public override string Name => "TerrariaMusicPlayer";
        public override Version Version => new Version(1, 1);
        public override string Author => "ExitiumTheCat";
        public override string Description => "";

        public string MusicName;
        public TerrariaMusicPlayer(Main game) : base(game) { }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(PlayMusic, "play"));
            Commands.ChatCommands.Add(new Command(PlayMusicOW, "playow"));
            Commands.ChatCommands.Add(new Command(MusicList, "musiclist"));
            Commands.ChatCommands.Add(new Command(MusicListOW, "musiclistow"));
            ServerApi.Hooks.GameUpdate.Register(this, OnUpdate);
        }
        private void OnUpdate(EventArgs args)
        {
            //No money farm
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player plr = Main.player[i];
                if (Main.player[i].active)
                {
                    for (int i2 = 0; i2 < 59; i2++)
                    {
                        if (plr.inventory[i2].Name == MusicName)
                        {
                            plr.inventory[i2].netDefaults(0);
                            NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, NetworkText.Empty, TShock.Players[i].Index, i2);
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                ServerApi.Hooks.ServerChat.Deregister(this, OnUpdate);
            }
        }

        private void PlayMusic(CommandArgs args)
        {
            MusicName = "Music Box (";
            if (args.Parameters.Count > 0)
            {
                foreach (string input in args.Parameters)
                {
                    MusicName = MusicName + input + " ";
                }
                MusicName = MusicName.Remove(MusicName.Length - 1) + ")";
                if (TShock.Utils.GetItemByName(MusicName).Count != 0)
                {
                    if (Main.player[args.Player.Index].armor[13].IsAir || Main.player[args.Player.Index].armor[13].Name.StartsWith("Music Box") || Main.player[args.Player.Index].armor[13].Name.StartsWith("Otherworldly Music Box"))
                    {
                        Main.player[args.Player.Index].armor[13] = TShock.Utils.GetItemByName(MusicName)[0];
                        NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, NetworkText.Empty, TShock.Players[args.Player.Index].Index, 13 + 59);
                    }
                    else
                    {
                        args.Player.SendErrorMessage("Please remove the item in your first social accessory slot");
                    }
                }
                else
                {
                    args.Player.SendErrorMessage("No music found.");
                }
            }
            else
            {
                args.Player.SendErrorMessage("Please input a music name.");
            }
        }
        private void PlayMusicOW(CommandArgs args)
        {
            MusicName = "Otherworldly Music Box (";
            if (args.Parameters.Count > 0)
            {
                foreach (string input in args.Parameters)
                {
                    MusicName = MusicName + input + " ";
                }
                MusicName = MusicName.Remove(MusicName.Length - 1) + ")";
                if (TShock.Utils.GetItemByName(MusicName).Count != 0)
                {
                    if (Main.player[args.Player.Index].armor[13].IsAir || Main.player[args.Player.Index].armor[13].Name.StartsWith("Otherworldly Music Box") || Main.player[args.Player.Index].armor[13].Name.StartsWith("Music Box"))
                    {
                        Main.player[args.Player.Index].armor[13] = TShock.Utils.GetItemByName(MusicName)[0];
                        NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, NetworkText.Empty, TShock.Players[args.Player.Index].Index, 13 + 59);
                    }
                    else
                    {
                        args.Player.SendErrorMessage("Please remove the item in your first social accessory slot");
                    }
                }
                else
                {
                    args.Player.SendErrorMessage("No music found.");
                }
            }
            else
            {
                args.Player.SendErrorMessage("Please input a music name.");
            }
        }
        private void MusicList(CommandArgs args)
        {
            args.Player.SendInfoMessage("Overworld Day, Alt Overworld Day, Night, Rain, Snow, Ice, Desert, Ocaen Day, Ocaen Night, Space Day, Night, Underground, Alt Underground, Mushrooms, Jungle, Corruption, Underground Corruption, Crimson, Underground Crimson, The Hallow, Underground Hallow, Hell, Dungeon, Temple, Boss 1, Boss 2, Boss 3, Boss 4, Boss 5, Queen Slime, Plantera, Empress of Light, Duke Fishron, Eerie, Eclipse, Goblin Invasion, Pirate Invasion, Martian Madness, Pumpkin Moon, Frost Moon, The Towers, Lunar Boss, Sandstorm, Old One's Army, Slime Rain, Town Day, Town Night, Windy Day, Day Remix, Journey's Beginning, Storm, Graveyard, Underground Jungle, Jungle Night, Morning Rain, Alt Title, Underground Desert, Journey's End, Title.");
        }
        private void MusicListOW(CommandArgs args)
        {
            args.Player.SendInfoMessage("Rain, Overworld Day, Night, Underground, Desert, Ocean, Mushrooms, Dungeon, Space, Underworld, Snow, Corruption, Underground Corruption, Crimson, Underground Crimson, Ice, Underground Hallow, Eerie, Boss 2, Boss 1, Invasion, The Towers, Lunar Boss, Plantera, Jungle, Wall of Flesh, Hallow.");
        }
    }
}