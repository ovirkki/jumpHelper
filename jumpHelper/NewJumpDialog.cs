using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace jumpHelper
{
    public class NewJumpDialogFragment : DialogFragment
    {
        private static List<string> formations;
        private static Context context;
        private const int INIT_FORMATION_AMOUNT = 2;
        private List<string> jumpList = new List<string>();
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
            Button jumpDoneButton = view.FindViewById<Button>(Resource.Id.jumpDoneButton);
            jumpDoneButton.Enabled = false;
            GridView gridView = view.FindViewById<GridView>(Resource.Id.formationsGridview);
            gridView.Adapter = new FormationsGridViewAdapter(formations, this.Activity);
            gridView.ChoiceMode = ChoiceMode.MultipleModal;
            gridView.SetMultiChoiceModeListener(new MultiChoiceModeListener());
            gridView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                string formation = formations[e.Position];
                if (jumpList.Contains(formation))
                {
                    jumpList.Remove(formation);
                    if (jumpList.Count == 0)
                        jumpDoneButton.Enabled = false;
                    styleUncheckedCell(e.View);
                }
                else
                {
                    jumpList.Add(formation);
                    jumpDoneButton.Enabled = true;
                    styleCheckedCell(e.View);
                }
                loadJumpSequenceText(jumpDoneButton);
            };
            jumpDoneButton.Click += delegate
            {
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

        private void styleCheckedCell(View view)
        {
            TextView textView = view.FindViewById<TextView>(Resource.Id.formationInGrid);
            textView.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.white)));
            textView.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.colorPrimaryDark)));
        }

        private void styleUncheckedCell(View view)
        {
            TextView textView = view.FindViewById<TextView>(Resource.Id.formationInGrid);
            textView.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.colorPrimaryDark)));
            textView.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.white)));
        }

        private void loadJumpSequenceText(Button doneButton)
        {
            if (this.jumpList.Count == 0)
            {
                doneButton.Text = "Waiting for selections";
            }
            else
            {
                doneButton.Text = "Get notes for jump: " + string.Join(this.Activity.GetString(Resource.String.JumpSeparator), jumpList);
            }
        }
    }

    public class MultiChoiceModeListener : Java.Lang.Object, GridView.IMultiChoiceModeListener
    {
        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            return true;
        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
        {
            AppEventHandler.emitInfoTextUpdate("State changed at pos: " + position + ", isChecked: " + isChecked);
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return true;
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
        }
    }
}