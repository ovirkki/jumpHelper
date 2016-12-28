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

namespace jumpHelper
{
    public class FormationsWithNotesAdapter : BaseExpandableListAdapter
    {
        SortedDictionary<string, List<string>> noteDictionary;
        List<string> filterList;
        FragmentActivity context;
        ExpandableListView parentView;
        ActivityCallBackListener listener;
        bool isDialog;
        public FormationsWithNotesAdapter(
            FragmentActivity context,
            ExpandableListView parentView,
            SortedDictionary<string, List<string>> noteDictionary,
            List<string> filterList,
            ActivityCallBackListener listener,
            bool isDialog)
        {
            this.noteDictionary = noteDictionary;
            this.context = context;
            this.parentView = parentView;
            this.filterList = filterList;
            this.listener = listener;
            this.isDialog = isDialog; //Shame on me using flag, maybe own group layout for dialogfragment and use layout id as constructor parameter
        }

        private Dictionary<string, List<string>> getFilteredData()
        {
            return this.noteDictionary
                .Where(kvp => ((this.filterList.Contains<string>(kvp.Key)  || kvp.Key == "Skills") && kvp.Value.Count > 0))
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
            CommentRowHolder holder = null;
            string formation = getKey(groupPosition);
            string comment = FilteredData[formation][childPosition];

            View view = convertView;
            if (view != null)
            {
                holder = view.Tag as CommentRowHolder;
            }

            if (holder == null)
            {
                holder = new CommentRowHolder();
                view = this.context.LayoutInflater.Inflate(Resource.Layout.NoteListRowComments, null);
                holder.commentView = view.FindViewById<TextView>(Resource.Id.commentField);
                holder.removeButton = view.FindViewById<ImageButton>(Resource.Id.removeComment);
                if (isDialog && this.listener == null)
                {
                    holder.removeButton.Visibility = ViewStates.Gone;
                }
                else
                {
                    holder.removeButton.Click += handleCommentRemoveClick;
                }
                view.Tag = holder;
            }
            holder.commentView.Text = comment;
            holder.removeButton.SetTag (Resource.Id.removeComment, makeTag(groupPosition, childPosition));
            return view;
        }
        
        private void handleCommentRemoveClick(object sender, EventArgs e)
        {
            int positionTag = (int)(((ImageButton)sender).GetTag(Resource.Id.removeComment));
            int groupPos = parseGroupFromTag(positionTag);
            int childPos = parseChildFromTag(positionTag);
            string formation = getKey(groupPos);
            string comment = FilteredData[formation][childPos];
            DeleteConfirmationDialog dialogFrag = DeleteConfirmationDialog.NewInstance(formation, comment);
            listener.startDialogFragment(dialogFrag);
        }

        private int makeTag(int group, int child)
        {
            return group * 100 + child;
        }

        private int parseGroupFromTag(int tag)
        {
            return (tag - (tag % 100)) / 100;
        }

        private int parseChildFromTag(int tag)
        {
            return tag % 100;
        }
        /*
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
        */

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

    public class CommentRowHolder : Java.Lang.Object
    {
        public TextView commentView { get; set; }
        public ImageButton removeButton { get; set; }
    }
}