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

namespace jumpHelper
{
    public class AddCommentDialogFragment : DialogFragment
    {
        private static List<string> formations;
        public static AddCommentDialogFragment NewInstance(List<string> formationsList)
        {
            AddCommentDialogFragment fragment = new AddCommentDialogFragment();
            formations = formationsList;
            fragment.Arguments = new Bundle();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.NewCommentLayout, container, false);

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.formationSelectSpinner);
            //List<string> formations = FSNotesHandler.getFormationFilterListAsync();
            var adapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, formations);
            spinner.Adapter = adapter;

            EditText edittext = view.FindViewById<EditText>(Resource.Id.newCommentText);
            Button saveButton = view.FindViewById<Button>(Resource.Id.SaveButton);
            Button cancelButton = view.FindViewById<Button>(Resource.Id.CancelButton);
            saveButton.Click += async delegate
            {
                string formation = spinner.SelectedItem.ToString();
                await FSNotesHandler.addComment(formation, edittext.Text);
                Dismiss();
            };
            cancelButton.Click += delegate {
                Dismiss();
            };
            return view;
        }
    }
}