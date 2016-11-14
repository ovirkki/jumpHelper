using System;
using System.Threading.Tasks;

using Android.App;
using Android.Widget;
//using mainApp = Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
//using Android.Support.V4.App;
//using v4App = Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using v7Widget = Android.Support.V7.Widget;
//using Android.Support.V7.Widget;

namespace jumpHelper
{
    [Activity(Label = "jumpHelper", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        //private FSNotesHandler fsDataHandler;

        /*public FSNotesHandler FSNotesHandler
        {
            get { return fsDataHandler; }
        }*/
        private bool menuIsInflated;
        private PopupMenu catChangePopup;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            AppEventHandler.CategoryUpdated += this.onCategoryUpdate;
            AppEventHandler.NewInfoText += this.onNewInfoText;
            FSNotesHandler.initialize();
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            var adapter = new JumpHelperPagerAdapter(SupportFragmentManager);
            var toolbar = FindViewById<v7Widget.Toolbar>(Resource.Id.my_toolbar);

            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Jump helper";
            pager.Adapter = adapter;
            tabLayout.SetupWithViewPager(pager);
        }

        private async Task updateCategoryLetterAsync(IMenu menu)
        {
            IMenuItem item = menu.FindItem(Resource.Id.category);
            string catString = await FSNotesHandler.getCategoryName();
            item.SetTitle(catString);
        }

        private void onNewInfoText(object sender, InfoTextEventArgs e)
        {
            Toast.MakeText(this, e.InfoText, ToastLength.Short).Show();
        }

        private void onCategoryUpdate(object sender, CategoryUpdateEventArgs e)
        {
            v7Widget.Toolbar toolbar = FindViewById<v7Widget.Toolbar>(Resource.Id.my_toolbar);
            IMenuItem item = toolbar.Menu.FindItem(Resource.Id.category);
            item.SetTitle(e.Category);
            //await updateCategoryLetterAsync(toolbar.Menu);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            MenuInflater.Inflate(Resource.Menu.top_toolbar, menu);
            //For some reason toolbar is refreshed with tab change so refresh also category data
            if (FSNotesHandler.isCategorySet())
            {
                IMenuItem item = menu.FindItem(Resource.Id.category);
                string catString = FSNotesHandler.CategoryName;
                item.SetTitle(catString);
            }
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.category:
                    //requestCategoryUpdate();
                    updateCategoryPopup();
                    return true;
                default:
                    return false;
            }
        }
        /*
        private void requestCategoryUpdate()
        {
            CategoryDialogFragment categoryDialog = CategoryDialogFragment.NewInstance();
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);

            //Add fragment
            categoryDialog.Show(ft, "dialog");
        }*/

        private void updateCategoryPopup()
        {
            PopupMenu catChangePopup = new PopupMenu(this, FindViewById(Resource.Id.category));
            catChangePopup.Inflate(Resource.Menu.category_change_popup);
            catChangePopup.Show();
            catChangePopup.MenuItemClick += (s1, arg1) =>
            {
                string categoryString;
                switch (arg1.Item.ItemId)
                {
                    case Resource.Id.rookie:
                        categoryString = FSNotesHandler.ROOKIE_ID;
                        break;
                    case Resource.Id.intermediate:
                        categoryString = FSNotesHandler.INTERMEDIATE_ID;
                        break;
                    case Resource.Id.advanced:
                        categoryString = FSNotesHandler.ADVANCED_ID;
                        break;
                    case Resource.Id.open:
                        categoryString = FSNotesHandler.OPEN_ID;
                        break;
                    default:
                        return;
                }
                Console.WriteLine("{0} selected", arg1.Item.TitleFormatted);
                FSNotesHandler.updateCategory(categoryString);
            };
        }
    }
/*
    public class CategoryDialogFragment : DialogFragment
    {
        public static CategoryDialogFragment NewInstance()
        {
            CategoryDialogFragment fragment = new CategoryDialogFragment();
            fragment.Arguments = new Bundle();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CategoryRequestDialog, container, false);
            RadioGroup radioGroup = view.FindViewById<RadioGroup>(Resource.Id.categories);
            radioGroup.CheckedChange += (sender, eventArgs) => {
                switch (eventArgs.CheckedId)
                {
                    case Resource.Id.rRadioBtn:
                        FSNotesHandler.updateCategory(FSNotesHandler.ROOKIE_ID);
                        break;
                    case Resource.Id.aRadioBtn:
                        FSNotesHandler.updateCategory(FSNotesHandler.INTERMEDIATE_ID);
                        break;
                    case Resource.Id.aaRadioBtn:
                        FSNotesHandler.updateCategory(FSNotesHandler.ADVANCED_ID);
                        break;
                    case Resource.Id.aaaRadioBtn:
                        FSNotesHandler.updateCategory(FSNotesHandler.OPEN_ID);
                        break;
                }
                Dismiss();
            };

            return view;
        }
    }*/

}

