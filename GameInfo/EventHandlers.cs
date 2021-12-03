// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GameInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AdvancedHints;
    using AdvancedHints.Enums;
    using Exiled.API.Features;
    using MEC;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers"/>.
    /// </summary>
    public class EventHandlers
    {
        private readonly Plugin plugin;
        private CoroutineHandle infoCoroutine;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlers"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted()"/>
        public void OnRoundStarted()
        {
            infoCoroutine = Timing.RunCoroutine(RunInfoLoop());
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers()"/>
        public void OnWaitingForPlayers()
        {
            if (infoCoroutine.IsRunning)
                Timing.KillCoroutines(infoCoroutine);
        }

        private IEnumerator<float> RunInfoLoop()
        {
            while (true)
            {
                string info = FormatInfo();
                foreach (Player player in Player.List)
                    player?.ShowManagedHint(info, plugin.Config.RefreshRate, false, DisplayLocation.Top);

                yield return Timing.WaitForSeconds(plugin.Config.RefreshRate);
            }
        }

        private string FormatInfo()
        {
            int timeUntilRespawn = Respawn.IsSpawning ? Respawn.TimeUntilRespawn + 15 : Respawn.TimeUntilRespawn;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeUntilRespawn);

            return plugin.Config.Format
                .Replace("{spectators}", Player.Get(Team.RIP).Count().ToString())
                .Replace("{spawntime}", timeSpan.ToString(@"mm\:ss"))
                .Replace("{aliveplayers}", Player.List.Where(player => player.IsAlive).Count().ToString())
                .Replace("{playercount}", Server.PlayerCount.ToString())
                .Replace("{maxplayercount}", Server.MaxPlayerCount.ToString())
                .Replace("{escapedclassd}", RoundSummary.EscapedClassD.ToString())
                .Replace("{escapedscientists}", RoundSummary.EscapedScientists.ToString())
                .Replace("{totalescapes}", (RoundSummary.EscapedClassD + RoundSummary.EscapedScientists).ToString());
        }
    }
}