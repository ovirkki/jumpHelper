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
            //this.HasOptionsMenu = true;
            
            View view = inflater.Inflate(Resource.Layout.FSNotes, container, false);
            string[] tempArray = { "A", "B" };
            this.filterList = new List<string>();
            ExpandableListView listOutput = view.FindViewById<ExpandableListView>(Resource.Id.notesListView);
            this.adapter = new FormationsWithNotesAdapter(this.Activity, listOutput, FSNotesHandler.Notes, this.filterList, false);
            listOutput.SetAdapter(this.adapter);
            AppEventHandler.DataUpdated += this.onDataUpdate;
            AppEventHandler.CategoryUpdated += this.onCategoryUpdate;
            //initNoteList(view);
            return view;
        }

        public async override void OnResume()
        {
            base.OnResume();
            await updateFilterList();
            this.adapter.NotifyDataSetChanged();
        }
        /*
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
        }*/
        
        private async Task initNoteList(View view)
        {
            this.filterList = await FSNotesHandler.getFormationFilterListAsync();
            ExpandableListView listOutput = view.FindViewById<ExpandableListView>(Resource.Id.notesListView);
            this.adapter = new FormationsWithNotesAdapter(this.Activity, listOutput, FSNotesHandler.Notes, this.filterList, false);
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
            filteredList.Add("Skills");
            this.filterList.Clear();
            this.filterList.AddRange(filteredList);
        }

        private async void onCategoryUpdate(object sender, EventArgs e)
        {
            await updateFilterList();
            this.adapter.NotifyDataSetChanged();
        }
    }
}