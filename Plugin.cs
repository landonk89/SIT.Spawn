using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EFT;
using System;
using UnityEngine;

namespace Gaylatea
{
    namespace Spawn
    {
        [BepInPlugin("com.gaylatea.spawn", "SIT-Spawn", "1.0.0")]
        public class Plugin : BaseUnityPlugin
        {
            private GameObject Hook;
            private const string KeybindSectionName = "Keybinds";
            private const string SettingsSectionName = "Settings";
            internal static ManualLogSource logger;

            internal static ConfigEntry<KeyboardShortcut> Spawn;
            internal static ConfigEntry<KeyboardShortcut> PrevBot;
            internal static ConfigEntry<KeyboardShortcut> NextBot;
            internal static ConfigEntry<BotDifficulty> AiDifficulty;
            internal static ConfigEntry<bool> SpawnType;

            public Plugin()
            {
                logger = Logger;
                WildSpawnType[] botvals = (WildSpawnType[])Enum.GetValues(typeof(WildSpawnType));

                Spawn = Config.Bind(KeybindSectionName, "Spawn a Bot", new KeyboardShortcut(KeyCode.Equals), "Spawns a random bot somewhere on the map.");
                PrevBot = Config.Bind(KeybindSectionName, "Next Bot Type", new KeyboardShortcut(KeyCode.Minus), "Changes to next bot type.");
                NextBot = Config.Bind(KeybindSectionName, "Previous Bot Type", new KeyboardShortcut(KeyCode.Alpha0), "Changes to previous bot type.");
                AiDifficulty = Config.Bind(SettingsSectionName, "Bot Difficulty", BotDifficulty.normal, "Only applies to newly spawned bots.");
                SpawnType = Config.Bind(SettingsSectionName, "Force Spawn", true, "Force bot to spawn even if it doesn't want to.");

                // Load in the bot spawning optimization patches.
                new UseAKIHTTPForBotLoadingPatch().Enable();

                Hook = new GameObject("Gaylatea.Spawn");
                Hook.AddComponent<Controller>();
                DontDestroyOnLoad(Hook);
                Logger.LogInfo($"S.P.A.W.N Loaded");
            }
        }
    }
}
