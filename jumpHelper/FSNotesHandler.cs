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
    public static class FSNotesHandler
    {
        private static SortedDictionary<string, List<string>> commentDictionary =
            new SortedDictionary<string, List<string>>(FileHandler.getCompleteDataAsDictionary());
        private static Category category = new Rookie(); //init category

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

        private static void OnDataUpdated(EventArgs e)
        {
            DataUpdated?.Invoke(null, e);
        }

        public static event EventHandler DataUpdated;

        public static List<string> getFormationFilterList()
        {
            return category.getFormations();
        }

        public static void updateCategory(string categoryString)
        {
            switch (categoryString.ToUpper())
            {
                case "R":
                    category = new Rookie();
                    break;
                case "A":
                    category = new Intermediate();
                    break;
                case "AA":
                    category = new DoubleA();
                    break;
                case "AAA":
                    category = new Open();
                    break;
                default:
                    return;
            }
        }

        public static void addComment(string formation, string comment)
        {
            if(commentDictionary.ContainsKey(formation))
            {
                commentDictionary[formation].Add(comment);
            } else
            {
                List<string> commentList = new List<string>();
                commentList.Add(comment);
                commentDictionary.Add(formation, commentList);
            }
            FileHandler.addData(formation, comment);
            OnDataUpdated(EventArgs.Empty);
        }

        public static void removeComment(string formation, string comment)
        {
            commentDictionary[formation].Remove(comment);
            FileHandler.removeData(formation, comment);
        }
    }
}
