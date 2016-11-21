using System;
using System.Threading.Tasks;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using v4App = Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using v7Widget = Android.Support.V7.Widget;

namespace jumpHelper
{
    [Activity(Label = "jumpHelper", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Task init = initializeData();
            SetContentView(Resource.Layout.Main);
            AppEventHandler.NewInfoText += this.onNewInfoText;

            var toolbar = FindViewById<v7Widget.Toolbar>(Resource.Id.my_toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Jump helper";

            await init;
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            var adapter = new JumpHelperPagerAdapter(SupportFragmentManager);
            pager.Adapter = adapter;
            tabLayout.SetupWithViewPager(pager);
        }

        private async Task initializeData()
        {
            FileHandler.initialize(this.FilesDir.AbsolutePath);
            await FSNotesHandler.initialize();
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
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_toolbar, menu);
            //Reload category as 
            if (FSNotesHandler.isCategorySet())
            {
                IMenuItem item = menu.FindItem(Resource.Id.category);
                string catString = FSNotesHandler.CategoryName;
                item.SetTitle(catString);
            }
            AppEventHandler.CategoryUpdated += this.onCategoryUpdate;
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.category:
                    updateCategoryPopup();
                    return true;
                case Resource.Id.add_note:
                    requestNewComment();
                    return true;
                default:
                    return false;
            }
        }

        private void requestNewComment()
        {
            AddCommentDialogFragment commentDialog = AddCommentDialogFragment.NewInstance();
            startDialogFragment(commentDialog);
        }

        public void startDialogFragment(v4App.DialogFragment dialogFragment)
        {
            v4App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            v4App.Fragment prev = SupportFragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);

            //Add fragment
            dialogFragment.Show(ft, "dialog");
        }

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
                FSNotesHandler.updateCategory(categoryString);
            };
        }
    }
}

