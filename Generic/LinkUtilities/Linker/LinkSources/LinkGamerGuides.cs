﻿using KNARZhelper;
using Playnite.SDK.Models;

namespace LinkUtilities.Linker
{
    /// <summary>
    /// Adds a link to Gamer Guides.
    /// </summary>
    class LinkGamerGuides : Link
    {
        public override string LinkName { get; } = "Gamer Guides";
        public override string BaseUrl { get; } = "https://www.gamerguides.com/";

        public override string GetGamePath(Game game, string gameName = null)
        {
            // Gamer Guides Links need the game name in lowercase without special characters and hyphens instead of white spaces.
            return (gameName ?? game.Name).RemoveSpecialChars().Replace("_", " ").CollapseWhitespaces().Replace(" ", "-").ToLower();
        }

        public LinkGamerGuides(LinkUtilities plugin) : base(plugin)
        {
        }
    }
}