﻿using Playnite.SDK;
using System;
using KNARZhelper.Enum;
using Playnite.SDK.Models;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace KNARZhelper.DatabaseObjectTypes
{
    internal class TypeReleaseDate : BaseType
    {
        public override bool CanBeAdded => false;
        public override bool CanBeDeleted => false;
        public override bool CanBeEmptyInGame => true;
        public override bool CanBeModified => false;
        public override bool CanBeSetInGame => true;
        public override bool IsList => false;
        public override string LabelSingular => ResourceProvider.GetString("LOCGameReleaseDateTitle");

        public override FieldType Type => FieldType.ReleaseDate;
        public override ItemValueType ValueType => ItemValueType.Date;

        public override bool DbObjectExists(string name) => false;

        public override bool DbObjectInGame(Game game, Guid id) => false;

        public override bool DbObjectInUse(Guid id) => false;

        public override void EmptyFieldInGame(Game game) =>
            API.Instance.MainView.UIDispatcher.Invoke(() => game.ReleaseDate = default);

        public override bool FieldInGameIsEmpty(Game game) => !game.ReleaseDate.HasValue;

        public override Guid GetDbObjectId(string name) => default;

        public override int GetGameCount(Guid id, bool ignoreHidden = false) => 0;

        public override bool IsBiggerThan<T>(Game game, T value) =>
            (value != null || value is DateTime) && game.ReleaseDate?.Date > (value as DateTime?);

        public override bool IsSmallerThan<T>(Game game, T value) =>
            (value != null || value is DateTime) && game.ReleaseDate?.Date < (value as DateTime?);

        public override bool NameExists(string name, Guid id) => false;
    }
}