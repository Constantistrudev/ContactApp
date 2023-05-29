using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Android.Database;
using System.Text.RegularExpressions;
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
    [Activity(Label = "@string/AddEditContacts")]
    public class AddEditAddressBookActivity : Activity
    {
        EditText txtId, txtFullName, txtMobile, txtEmail, txtDescription;
        Button btnSave;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddEditContacts);

            txtId = FindViewById<EditText>(Resource.Id.addEdit_ContactId);
            txtFullName = FindViewById<EditText>(Resource.Id.addEdit_FullName);
            txtMobile = FindViewById<EditText>(Resource.Id.addEdit_Mobile);
            txtEmail = FindViewById<EditText>(Resource.Id.addEdit_Email);
            txtDescription = FindViewById<EditText>(Resource.Id.addEdit_Description);
            btnSave = FindViewById<Button>(Resource.Id.addEdit_btnSave);

            btnSave.Click += buttonSave_Click;
            
            string editId = Intent.GetStringExtra("ContactId") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtId.Text = editId;
                LoadDataForEdit(editId);
            }
        }

        private void LoadDataForEdit(string contactId)
        {
            AddressBookDbHelper db = new AddressBookDbHelper(this);
            ICursor cData = db.getContactById(int.Parse(contactId));
            if (cData.MoveToFirst())
            {
                txtFullName.Text = cData.GetString(cData.GetColumnIndex("FullName"));
                txtMobile.Text = cData.GetString(cData.GetColumnIndex("Mobile"));
                txtEmail.Text = cData.GetString(cData.GetColumnIndex("Email"));
                txtDescription.Text = cData.GetString(cData.GetColumnIndex("Details"));
            }
        }

        void buttonSave_Click(object sender, EventArgs e)
        {
            AddressBookDbHelper db = new AddressBookDbHelper(this);
            if (txtFullName.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, Resources.GetText(Resource.String.AddEditActivity_Name), ToastLength.Short).Show();
                return;
            }

            if (txtMobile.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, Resources.GetText(Resource.String.AddEditActivity_Mobile), ToastLength.Short).Show();
                return;
            }

            if (txtEmail.Text.Trim().Length > 0)
            {
                string EmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                if (!Regex.IsMatch(txtEmail.Text, EmailPattern, RegexOptions.IgnoreCase))
                {
                    Toast.MakeText(this, Resources.GetText(Resource.String.AddEditActivity_Email), ToastLength.Short).Show();
                    return;
                }
            }

            AddressBook ab = new AddressBook();

            if (txtId.Text.Trim().Length > 0)
            {
                ab.Id = int.Parse(txtId.Text);
            }
            ab.FullName = txtFullName.Text;
            ab.Mobile = txtMobile.Text;
            ab.Email = txtEmail.Text;
            ab.Details = txtDescription.Text;

            try
            {

                if (txtId.Text.Trim().Length > 0)
                {
                    db.UpdateContact(ab);
                    Toast.MakeText(this, Resources.GetText(Resource.String.AddEditActivity_Update), ToastLength.Short).Show();
                }
                else
                {
                    db.AddNewContact(ab);
                    Toast.MakeText(this, Resources.GetText(Resource.String.AddEditActivity_Create), ToastLength.Short).Show();
                }

                Finish();

                //Go to main activity after save/edit
                var mainActivity = new Intent(this, typeof(MainActivity));
                StartActivity(mainActivity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            AddContactNotification();
        }
        private void AddContactNotification()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel("AddContact", "Sucсessful notification", NotificationImportance.Default)
                {
                    Description = "Успешное добавление контакта",
                };

                var notificationManagerO = (NotificationManager)GetSystemService(NotificationService);
                notificationManagerO.CreateNotificationChannel(channel);
            }
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "AddContact")
                .SetContentTitle("Уведомление")
                .SetContentText("Контакт Добавлен")
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetDefaults(NotificationCompat.DefaultAll).SetSmallIcon(Resource.Drawable.icon);
            NotificationManagerCompat notificationManagerCompat = NotificationManagerCompat.From(this);
            notificationManagerCompat.Notify(1, builder.Build());
        }
    }
}