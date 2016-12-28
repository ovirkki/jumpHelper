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
    public class NewJumpDialogFragment : DialogFragment
    {
        private static List<string> formations;
        private static Context context;
        private const int INIT_FORMATION_AMOUNT = 2;
        public static NewJumpDialogFragment NewInstance(List<string> formationsList)
        {
            NewJumpDialogFragment fragment = new NewJumpDialogFragment();
            formations = formationsList;
            //context = con;
            fragment.Arguments = new Bundle();
            return fragment;
        }

        public override void OnStart()
        {
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            base.OnStart();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.NewJumpRequestDialog, container, false);
            List<Spinner> spinnerList = new List<Spinner>();
            for (var i = 0; i < INIT_FORMATION_AMOUNT; i++)
            {
                addSpinner(spinnerList, view);
            }
            ImageButton addSpinnerButton = view.FindViewById<ImageButton>(Resource.Id.addSpinner);
            addSpinnerButton.Click += delegate
            {
                addSpinner(spinnerList, view);
            };
            Button jumpDoneButton = view.FindViewById<Button>(Resource.Id.jumpDoneButton);
            jumpDoneButton.Enabled = false;
            jumpDoneButton.Click += delegate
            {
                List<string> jumpList = getJumpList(spinnerList);
                JumpCommentsFragment fragment = new JumpCommentsFragment(jumpList, this.Activity);
                var ft = this.Activity.SupportFragmentManager.BeginTransaction();
                ft.SetTransition(FragmentTransaction.TransitFragmentFade);
                Fragment prev = this.Activity.SupportFragmentManager.FindFragmentByTag("comments");
                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);
                Dismiss();
                fragment.Show(ft, "comments");
            };
            return view;
        }

        private void addSpinner(List<Spinner> spinnerList, View view)
        {
            LinearLayout spinnerLayout = view.FindViewById<LinearLayout>(Resource.Id.formationSpinners);
            Spinner spinner = new Spinner(this.Activity);
            spinner.ItemSelected += (sender, args) =>
            {
                Button doneButton = view.FindViewById<Button>(Resource.Id.jumpDoneButton);
                doneButton.Enabled = true;
                loadJumpSequenceText(doneButton, spinnerList);
            };
            var adapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, formations);
            spinner.Adapter = adapter;
            spinnerList.Add(spinner);
            addSpinnerToEnd(spinner, spinnerLayout);
        }

        private void loadJumpSequenceText(Button doneButton, List<Spinner> spinnerList)
        {
            List<string> jumpList = getJumpList(spinnerList);
            doneButton.Text = "Get notes for jump: " + string.Join(this.Activity.GetString(Resource.String.JumpSeparator), jumpList);
        }

        private List<string> getJumpList(List<Spinner> spinnerList)
        {
            List<string> jumpList = new List<string>();
            spinnerList.ForEach(delegate (Spinner spinner)
            {
                jumpList.Add(spinner.SelectedItem.ToString());
            });
            return jumpList;
        }

        private void addSpinnerToEnd(Spinner spinner, LinearLayout spinnerLayout)
        {
            int childCount = spinnerLayout.ChildCount;
            Console.WriteLine("childcount: " + childCount);
            spinnerLayout.AddView(spinner, childCount - 1);
        }
    }
}