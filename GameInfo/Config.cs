// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace GameInfo
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the string to format and display on player's screens.
        /// </summary>
        [Description("The string to format and display on player's screens.")]
        public string Format { get; set; } = "Alive players: {aliveplayers}/{playercount}\nRespawn Time: {spawntime}";

        /// <summary>
        /// Gets or sets the amount of time, in seconds, between updating the info.
        /// </summary>
        [Description("The amount of time, in seconds, between updating the info.")]
        public float RefreshRate { get; set; } = 0.1f;
    }
}