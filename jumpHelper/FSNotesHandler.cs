using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace jumpHelper
{
    public static class FSNotesHandler
    {
        public const string ROOKIE_ID = "R";
        public const string INTERMEDIATE_ID = "A";
        public const string ADVANCED_ID = "AA";
        public const string OPEN_ID = "AAA";

        public const string ADD_OPERATION = "ADD";
        public const string REMOVE_OPERATION = "REMOVE";

        private static SortedDictionary<string, List<string>> commentDictionary;
            //= new SortedDictionary<string, List<string>>(FileHandler.getCompleteDataAsDictionary());
        private static Category category;

        public static SortedDictionary<string, List<string>> Notes
        {
            get
            {
                return commentDictionary;
            }
        }

        public static Dictionary<string, List<string>> getNotesWithFilterList(List<string> formations)
        {
            return commentDictionary
                .Where(kvp => formations.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static async Task initialize()
        {
            category = new Intermediate();
            //updateCategory(INTERMEDIATE_ID);
            commentDictionary = new SortedDictionary<string, List<string>>(await FileHandler.getCompleteDataAsDictionary(), new FormationSorter());
        }

        public async static Task<string> getCategoryName()
        {
            return await category.getShortName();
        }

        public static string CategoryName
        {
            get { return category.ShortName; }
        }

        public static bool isCategorySet()
        {
            return category != null;
        }

        public async static Task<List<string>> getFormationFilterListAsync()
        {
            return await Task.FromResult(category.FormationList);
        }

        public static List<string> getFormationFilterList()
        {
            return category.FormationList;
        }

        public static void updateCategory(string categoryString)
        {
            switch (categoryString)
            {
                case ROOKIE_ID:
                    category = new Rookie();
                    break;
                case INTERMEDIATE_ID:
                    category = new Intermediate();
                    break;
                case ADVANCED_ID:
                    category = new DoubleA();
                    break;
                case OPEN_ID:
                    category = new Open();
                    break;
                default:
                    return;
            }
            AppEventHandler.emitCategoryUpdate(categoryString);
        }

        public async static Task addComment(string formation, string comment)
        {
            Task addCommentTask;
            if (commentDictionary.ContainsKey(formation))
            {
                addCommentTask = Task.Run(() =>
               {
                   commentDictionary[formation].Add(comment);
               });
            } else
            {
                addCommentTask = Task.Run(() =>
                {
                    List<string> commentList = new List<string>();
                    commentList.Add(comment);
                    commentDictionary.Add(formation, commentList);
                });
            }
            await addCommentTask;
            AppEventHandler.emitNoteUpdate(ADD_OPERATION, formation, comment);
            AppEventHandler.emitInfoTextUpdate("Comment for formation " + formation + " added");
        }

        public async static Task removeComment(string formation, string comment)
        {
            await Task.Run(() =>
            {
                commentDictionary[formation].Remove(comment);
            });
            AppEventHandler.emitNoteUpdate(REMOVE_OPERATION, formation, comment);
            AppEventHandler.emitInfoTextUpdate("Comment for formation " + formation + " removed");
        }

        public async static Task<List<List<string>>> getRandomizedJumps()
        {
            return await Task.FromResult(category.getDraw());
        }
    }
}
