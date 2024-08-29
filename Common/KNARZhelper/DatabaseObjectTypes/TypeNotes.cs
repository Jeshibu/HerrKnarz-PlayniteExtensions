﻿using Playnite.SDK;
using System;
using System.Linq;
using KNARZhelper.Enum;
using Playnite.SDK.Models;

namespace KNARZhelper.DatabaseObjectTypes
{
    internal class TypeNotes : BaseType
    {
        public override bool CanBeAdded => false;
        public override bool CanBeDeleted => false;
        public override bool CanBeEmptyInGame => true;
        public override bool CanBeModified => false;
        public override bool CanBeSetInGame => true;
        public override bool IsList => false;
        public override string LabelSingular => ResourceProvider.GetString("LOCNotesLabel");
        public override FieldType Type => FieldType.Notes;
        public override ItemValueType ValueType => ItemValueType.String;

        public override bool DbObjectExists(string name) => false;

        public override bool DbObjectInGame(Game game, Guid id) => false;

        public override bool DbObjectInUse(Guid id) => false;

        public override void EmptyFieldInGame(Game game) => API.Instance.MainView.UIDispatcher.Invoke(() => game.Notes = default);

        public override bool FieldInGameIsEmpty(Game game) => !game.Notes.Trim().Any();

        public override Guid GetDbObjectId(string name) => default;

        public override int GetGameCount(Guid id, bool ignoreHidden = false) => 0;

        public override bool NameExists(string name, Guid id) => false;
    }
}