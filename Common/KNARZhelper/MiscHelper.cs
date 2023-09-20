﻿using Playnite.SDK;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KNARZhelper
{
    public static class MiscHelper
    {
        /// <summary>
        /// Converts a Unix Timestamp to DateTime.
        /// </summary>
        /// <param name="unixTimeStamp">The timestamp to convert</param>
        /// <returns>DateTime value of the timestamp</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
            => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
        public static DateTime StartOfMonth(this DateTime date)
            => new DateTime(date.Year, date.Month, 1);
        public static DateTime EndOfMonth(this DateTime date)
            => date.StartOfMonth().AddMonths(1).AddDays(-1);

        public static void AddTextIcoFontResource(string key, string text)
        {
            Application.Current.Resources.Add(key, new TextBlock
            {
                Text = text,
                FontSize = 16,
                FontFamily = ResourceProvider.GetResource("FontIcoFont") as FontFamily
            });
        }
    }
}