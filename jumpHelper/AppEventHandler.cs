using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace jumpHelper
{
    public static class AppEventHandler
    {
        public static void emitInfoTextUpdate(string infoText)
        {
            InfoTextEventArgs args = new InfoTextEventArgs();
            args.InfoText = infoText;
            OnNewInfoText(args);
        }
        private static void OnNewInfoText(InfoTextEventArgs e)
        {
            NewInfoText?.Invoke(null, e);
        }
        public static event EventHandler<InfoTextEventArgs> NewInfoText;

        public static void emitNoteUpdate(string operation, string formation, string comment)
        {
            NoteDataUpdateEventArgs args = new NoteDataUpdateEventArgs();
            args.Operation = operation;
            args.Formation = formation;
            args.Comment = comment;
            OnDataUpdated(args);
        }

        private static void OnDataUpdated(NoteDataUpdateEventArgs e)
        {
            DataUpdated?.Invoke(null, e);
        }
        public static event EventHandler<NoteDataUpdateEventArgs> DataUpdated;

        public static void emitCategoryUpdate(string newCategory)
        {
            CategoryUpdateEventArgs args = new CategoryUpdateEventArgs();
            args.Category = newCategory;
            OnCategoryUpdated(args);
        }

        private static void OnCategoryUpdated(CategoryUpdateEventArgs e)
        {
            CategoryUpdated?.Invoke(null, e);
        }
        public static event EventHandler<CategoryUpdateEventArgs> CategoryUpdated;
    }

    public class NoteDataUpdateEventArgs : EventArgs
    {
        public string Operation { get; set; }
        public string Formation { get; set; }
        public string Comment { get; set; }

    }
    public class CategoryUpdateEventArgs : EventArgs
    {
        public string Category { get; set; }

    }
    public class InfoTextEventArgs : EventArgs
    {
        public string InfoText { get; set; }

    }
}