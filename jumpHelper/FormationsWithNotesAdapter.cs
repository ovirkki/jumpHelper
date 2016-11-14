using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.Support.V4.App;
//using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace jumpHelper
{
    public class FormationsWithNotesAdapter : BaseExpandableListAdapter
    {
        SortedDictionary<string, List<string>> noteDictionary;
        List<string> filterList;
        FragmentActivity context;
        public FormationsWithNotesAdapter(FragmentActivity context, SortedDictionary<string, List<string>> noteDictionary, List<string> filterList)
        {
            this.noteDictionary = noteDictionary;
            this.context = context;
            this.filterList = filterList;
        }

        private Dictionary<string, List<string>> getFilteredData()
        {
            return this.noteDictionary
                .Where(kvp => this.filterList.Contains<string>(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private Dictionary<string, List<string>> FilteredData
        {
            get {
                return getFilteredData();
            }
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return FilteredData[getKey(groupPosition)].ToArray();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override int GroupCount
        {
            get { return FilteredData.Count; }
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            string key = getKey(groupPosition);
            View view = convertView;
            if (view == null)
            {
                //LayoutInflater infalInflater = (LayoutInflater)this.context.GetSystemService(Context.LayoutInflaterService);
                view = this.context.LayoutInflater.Inflate(Resource.Layout.NoteListRowMain, null);
            }
            view.FindViewById<TextView>(Resource.Id.noteListHeader).Text = key;
            return view;
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return FilteredData[getKey(groupPosition)][childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return FilteredData[getKey(groupPosition)].Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            string formation = getKey(groupPosition);
            string comment = FilteredData[formation][childPosition];

            View view = convertView;
            if (view == null)
            {
                //LayoutInflater infalInflater = (LayoutInflater)this.context.GetSystemService(Context.LayoutInflaterService);
                view = this.context.LayoutInflater.Inflate(Resource.Layout.NoteListRowComments, null);
            }
            view.FindViewById<TextView>(Resource.Id.commentField).Text = comment;
            ImageButton removeButton = view.FindViewById<ImageButton>(Resource.Id.removeComment);
            removeButton.Click += ((sender, eventArgs) =>
            {
                DeleteConfirmationDialog dialogFrag = DeleteConfirmationDialog.NewInstance(formation, comment);
                startDialogFragment(dialogFrag);
            });
            return view;
        }
        
        private void startDialogFragment(DialogFragment dialogFragment)
        {
            FragmentTransaction ft = context.SupportFragmentManager.BeginTransaction();
            Fragment prev = context.SupportFragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);

            dialogFragment.Show(ft, "dialog");
        }
        

        public override bool HasStableIds
        {
            get { return false; }
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
        private string getKey(int groupPosition)
        {
            return FilteredData.Keys.ToArray()[groupPosition];
        }
        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
        }
    }
}