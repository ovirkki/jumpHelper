using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
using v7Widget = Android.Support.V7.Widget;


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
            this.filterList = new List<string>();
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
                default:
                    return true;
            }
        }

        private void initNoteList(View view)
        {
            this.filterList = FSNotesHandler.getFormationFilterList();
            ExpandableListView listOutput = view.FindViewById<ExpandableListView>(Resource.Id.notesListView);
            this.adapter = new FormationsWithNotesAdapter(this.Activity, FSNotesHandler.Notes, this.filterList);
            listOutput.SetAdapter(this.adapter);
            AppEventHandler.DataUpdated += this.onDataUpdate;
            AppEventHandler.CategoryUpdated += this.onCategoryUpdate;
        }

        private async Task updateDataToFile(string operation, string formation, string comment)
        {
            switch (operation)
            {
                case FSNotesHandler.ADD_OPERATION:
                    await FileHandler.addDataAsync(formation, comment);
                    break;
                case FSNotesHandler.REMOVE_OPERATION:
                    await FileHandler.removeDataAsync(formation, comment);
                    break;
                default:
                    break;
            }
        }

        private async void onDataUpdate(object sender, NoteDataUpdateEventArgs e)
        {
            this.adapter.NotifyDataSetChanged();
            await updateDataToFile(e.Operation, e.Formation, e.Comment);//add settings to turn automatic saving on/off  
        }

        private async Task updateFilterList()
        {
            List<string> filteredList = await FSNotesHandler.getFormationFilterListAsync();
            this.filterList.Clear();
            this.filterList.AddRange(filteredList);
        }

        private async void onCategoryUpdate(object sender, EventArgs e)
        {
            await updateFilterList();
            this.adapter.NotifyDataSetChanged();
        }

        private void requestNewComment()
        {
            AddCommentDialogFragment commentDialog = AddCommentDialogFragment.NewInstance();
            startDialogFragment(commentDialog);
        }

        public void startDialogFragment(DialogFragment dialogFragment)
        {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            
            //Add fragment
            dialogFragment.Show(ft, "dialog");
        }
    }
}