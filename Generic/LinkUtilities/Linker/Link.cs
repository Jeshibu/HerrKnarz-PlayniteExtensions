﻿using KNARZhelper;
using LinkUtilities.LinkActions;
using LinkUtilities.Models;
using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Linq;

namespace LinkUtilities.Linker
{
    /// <summary>
    /// Base class for a website link 
    /// </summary>
    public abstract class Link : ILink, ILinkAction
    {
        public abstract string LinkName { get; }
        public virtual string BaseUrl { get; } = string.Empty;
        public virtual string SearchUrl { get; } = string.Empty;
        public virtual string LinkUrl { get; set; } = string.Empty;
        public virtual LinkAddTypes AddType { get; } = LinkAddTypes.UrlMatch;
        public virtual bool CanBeSearched { get { return !string.IsNullOrWhiteSpace(SearchUrl); } }
        public LinkSourceSetting Settings { get; set; }
        public virtual bool AllowRedirects { get; set; } = true;
        public string ProgressMessage { get; } = "LOCLinkUtilitiesProgressLink";
        public string ResultMessage { get; } = "LOCLinkUtilitiesDialogAddedMessage";

        private readonly LinkUtilities plugin;
        public LinkUtilities Plugin { get { return plugin; } }
        public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();

        public virtual bool AddSearchedLink(Game game)
        {
            GenericItemOption result = API.Instance.Dialogs.ChooseItemWithSearch(
                new List<GenericItemOption>(),
                (a) => SearchLink(a),
                game.Name,
                $"{ResourceProvider.GetString("LOCLinkUtilitiesDialogSearchGame")} ({LinkName})");

            if (result != null)
            {
                return LinkHelper.AddLink(game, LinkName, ((SearchResult)result).Url, plugin, false);
            }
            else
            {
                return false;
            }
        }

        public virtual List<GenericItemOption> SearchLink(string searchTerm)
        {
            return SearchResults.ToList<GenericItemOption>();
        }

        public virtual bool AddLink(Game game)
        {
            LinkUrl = string.Empty;
            bool result = false;

            if (!LinkHelper.LinkExists(game, LinkName))
            {
                switch (AddType)
                {
                    case LinkAddTypes.SingleSearchResult:
                        LinkUrl = GetGamePath(game);
                        break;
                    case LinkAddTypes.UrlMatch:
                        string gameName = GetGamePath(game);

                        if (!string.IsNullOrEmpty(gameName))
                        {
                            if (CheckLink($"{BaseUrl}{gameName}"))
                            {
                                LinkUrl = $"{BaseUrl}{gameName}";
                            }
                            else
                            {
                                gameName = GetGamePath(game, game.Name.RemoveEditionSuffix());

                                if (CheckLink($"{BaseUrl}{gameName}"))
                                {
                                    LinkUrl = $"{BaseUrl}{gameName}";
                                }
                            }
                        }
                        break;
                }

                if (!string.IsNullOrEmpty(LinkUrl))
                {
                    result = LinkHelper.AddLink(game, LinkName, LinkUrl, plugin);
                }
            }

            return result;
        }

        public virtual bool CheckLink(string link)
        {
            return LinkHelper.CheckUrl(link, AllowRedirects);
        }

        public virtual string GetGamePath(Game game, string gameName = null)
        {
            string result = string.Empty;

            if (gameName == null)
            {
                gameName = game.Name;
            }

            if (!string.IsNullOrEmpty(gameName))
            {
                switch (AddType)
                {
                    case LinkAddTypes.UrlMatch:
                        result = gameName;
                        break;
                    case LinkAddTypes.SingleSearchResult:
                        if (CanBeSearched)
                        {
                            return TryToFindPerfectMatchingUrl(gameName) ??
                                TryToFindPerfectMatchingUrl(gameName.RemoveEditionSuffix()) ??
                                string.Empty;
                        }
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Searches for a game by name and looks for a matching search result.
        /// </summary>
        /// <param name="gameName">Name of the game</param>
        /// <returns>Url of the game. Returns null if no match was found.</returns>
        private string TryToFindPerfectMatchingUrl(string gameName)
        {
            _ = SearchLink(gameName);

            string searchName = gameName.RemoveSpecialChars().Replace(" ", "");

            SearchResult foundGame = SearchResults.Where(r => r.Name.RemoveSpecialChars().Replace(" ", "") == searchName).FirstOrDefault();

            if (foundGame != null)
            {
                return foundGame.Url;
            }
            else if (SearchResults.Count() == 1)
            {
                return SearchResults[0].Url;
            }

            else
            {
                return null;
            }
        }

        public virtual bool Execute(Game game, ActionModifierTypes actionModifier = ActionModifierTypes.None, bool isBulkAction = true)
        {
            switch (actionModifier)
            {
                case ActionModifierTypes.Add:
                    return AddLink(game);
                case ActionModifierTypes.Search:
                    return AddSearchedLink(game);
                default:
                    return false;
            }
        }

        public Link(LinkUtilities plugin)
        {
            this.plugin = plugin;
            Settings = new LinkSourceSetting()
            {
                LinkName = LinkName,
                IsAddable = AddType != LinkAddTypes.None ? true : (bool?)null,
                IsSearchable = CanBeSearched ? true : (bool?)null,
                ShowInMenus = true,
                ApiKey = null,
                NeedsApiKey = false
            };
        }
    }
}
