#pragma warning disable IDE0051 // Remove unused private members
using Comfort.Common;
using UnityEngine;
using EFT;
using EFT.Communications;
using System;

namespace Gaylatea
{
    namespace Spawn
    {
        public class Controller : MonoBehaviour
        {
            IBotGame game { get => (IBotGame)Singleton<AbstractGame>.Instance; }

            WildSpawnType botType = WildSpawnType.assault; // Default to assault type
            WildSpawnType GetNextEnumValue(WildSpawnType currentValue, bool getPrevious)
            {
                WildSpawnType[] botValues = (WildSpawnType[])Enum.GetValues(typeof(WildSpawnType));
                int currentIndex = Array.IndexOf(botValues, currentValue);
                int nextIndex = (currentIndex + (getPrevious ? -1 : 1) + botValues.Length) % botValues.Length;
                return botValues[nextIndex];
            }

            private void Update()
            {
                if (Input.GetKeyDown(Plugin.Spawn.Value.MainKey))
                {
                    if (game == null)
                    {
                        NotificationManagerClass.DisplayMessageNotification("Not spawning because there's no game to spawn into.", ENotificationDurationType.Default, ENotificationIconType.Alert);
                        return;
                    }

                    //Note that the AKI server might rewrite this bot to be a PMC.
                    //Assign the correct faction to the bot
                    EPlayerSide botSide;
                    if (botType.ToString().Contains("Usec"))
                        botSide = EPlayerSide.Usec;
                    else if (botType.ToString().Contains("Bear"))
                        botSide = EPlayerSide.Bear;
                    else if (botType.ToString().Contains("pmcBot")) //50-50 chance of being usec or bear
                        botSide = (new System.Random().Next(2) == 0) ? EPlayerSide.Usec : EPlayerSide.Bear;
                    else
                        botSide = EPlayerSide.Savage;

                    game.BotsController.SpawnBotDebugServer(botSide, true, botType, Spawn.Plugin.AiDifficulty.Value, Plugin.SpawnType.Value);
                    NotificationManagerClass.DisplayMessageNotification("Spawned bot type: " + botType.ToString() + ", Difficulty: " + Plugin.AiDifficulty.Value.ToString(), ENotificationDurationType.Default, ENotificationIconType.Alert);
                }

                //Button to switch WildSpawnType
                if (Input.GetKeyDown(Plugin.PrevBot.Value.MainKey) || Input.GetKeyDown(Plugin.NextBot.Value.MainKey))
                {
                    bool isPrevKey = Input.GetKeyDown(Plugin.PrevBot.Value.MainKey);

                    botType = GetNextEnumValue(botType, isPrevKey);
                    NotificationManagerClass.DisplayMessageNotification("Bot type: " + botType.ToString(), ENotificationDurationType.Default, ENotificationIconType.Alert);
                }
            }
        }
    }
}