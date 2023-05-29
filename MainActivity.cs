using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content.PM;
using Android;
using Android.Nfc;
using Android.Util;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using static Android.Content.ClipData;
using AndroidX.Core;
using AndroidX.Core.App;
using Xamarin;
using AndroidX.Core.Content;

namespace ContactApp
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<ContactApp.AddressBook> listItsms = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            GetApplications();
            SetContentView(Resource.Layout.Main);
            btnAdd = FindViewById<Button>(Resource.Id.contactList_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.contactList_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.contactList_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.contactList_listView);


            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel("0000", "Contact Write success", NotificationImportance.Default)
                {
                    Description = "Contact Write permission",
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }

            btnAdd.Click += delegate
            {
                
                var activityAddEdit = new Intent(this, typeof(AddEditAddressBookActivity));
                StartActivity(activityAddEdit);

            };

            btnSearch.Click += delegate
            {
                LoadContactsInList();
            };
            LoadContactsInList();

        }


        private void LoadContactsInList()
        {
            AddressBookDbHelper dbVals = new AddressBookDbHelper(this);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllContacts();
            }
            else
            {

                listItsms = dbVals.GetContactsBySearchName(txtSearch.Text.Trim());
            }


            lv.Adapter = new ContactListBaseAdapter(this, listItsms);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AddressBook o = listItsms[e.Position];
            var activityAddEdit = new Intent(this, typeof(AddEditAddressBookActivity));
            activityAddEdit.PutExtra("ContactId", o.Id.ToString());
            activityAddEdit.PutExtra("ContactName", o.FullName);
            StartActivity(activityAddEdit);
        }
        private void GetApplications()
        {
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.GetAccounts) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Android.Manifest.Permission.GetAccounts }, 1);
            }

        }
    }
}

