using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//using Android.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
//using Android.Support.V7.Widget;


namespace jumpHelper
{
    public class FSNotesFragment : jumpHelperFragment
    {
        string title;
        FormationsWithNotesAdapter adapter;
        List<string> filterList;

        public FSNotesFragment(string title) : base(title)
        {
            this.title = title;
            this.filterList = FSNotesHandler.getFormationFilterList();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            this.HasOptionsMenu = true;
            
            View view = inflater.Inflate(Resource.Layout.FSNotes, container, false);

            initNoteList(view);
            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.add_note:
                    requestNewComment();
                    return true;
                case Resource.Id.category:
                    updateCategory();
                    return true;
                default:
                    return true;
            }
        }

        private void initNoteList(View view)
        {
            ExpandableListView listOutput = view.FindViewById<ExpandableListView>(Resource.Id.notesListView);
            this.adapter = new FormationsWithNotesAdapter(this.Activity, FSNotesHandler.Notes, this.filterList);
            listOutput.SetAdapter(this.adapter);
            FSNotesHandler.DataUpdated += this.onDataUpdate;
        }

        private void onDataUpdate(object sender, EventArgs e)
        {
            this.adapter.NotifyDataSetChanged();
        }

        private void requestNewComment()
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            CommentDialogFragment commentDialog = CommentDialogFragment.NewInstance();
            //Add fragment
            commentDialog.Show(ft, "dialog");
        }

        private void addComment()
        {
            FSNotesHandler.addComment("D", "d kommentti");
            this.adapter.NotifyDataSetChanged();
        }

        private void updateCategory()
        {
            FSNotesHandler.updateCategory("A");
            this.filterList.Clear();
            this.filterList.AddRange(FSNotesHandler.getFormationFilterList());
            this.adapter.NotifyDataSetChanged();
        }
    }

    public class CommentDialogFragment : DialogFragment
    {

        private EditText editText;
        public static CommentDialogFragment NewInstance()
        {
            CommentDialogFragment fragment = new CommentDialogFragment();
            fragment.Arguments = new Bundle();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.NewCommentLayout, container, false);

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.formationSelectSpinner);
            List<string> formations = FSNotesHandler.getFormationFilterList();
            var adapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, formations);
            spinner.Adapter = adapter;

            EditText edittext = view.FindViewById<EditText>(Resource.Id.newCommentText);
            Button saveButton = view.FindViewById<Button>(Resource.Id.SaveButton);
            Button cancelButton = view.FindViewById<Button>(Resource.Id.CancelButton);
            saveButton.Click += delegate
            {
                string formation = spinner.SelectedItem.ToString();
                FSNotesHandler.addComment(formation, edittext.Text);
                Dismiss();
                Toast.MakeText(Activity, "Saved comment for " + formation, ToastLength.Short).Show();
            };
            cancelButton.Click += delegate {
                Dismiss();
                Toast.MakeText(Activity, "Canceled...", ToastLength.Short).Show();
            };
            return view;
        }
    }
}