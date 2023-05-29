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
using Android.Database.Sqlite;
using Android.Database;
using System.Collections;

namespace ContactApp
{
    public class AddressBookDbHelper : SQLiteOpenHelper
    {
        private const string APP_DATABASENAME = "Contacts.db3";
        private const int APP_DATABASE_VERSION = 1;

        public AddressBookDbHelper(Context ctx) :
            base(ctx, APP_DATABASENAME, null, APP_DATABASE_VERSION)
        {

        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS AddressBook(
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            FullName TEXT NOT NULL,
                            Mobile  TEXT NOT NULL,
                            Email   TEXT NULL,
                            Details TEXT)");

        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS AddressBook");
            OnCreate(db);
        }

        //Retrive All Contact Details
        public IList<AddressBook> GetAllContacts()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("AddressBook", new string[] { "Id", "FullName", "Mobile", "Email", "Details" }, null, null, null, null, null);

            var contacts = new List<AddressBook>();

            while (c.MoveToNext())
            {
                contacts.Add(new AddressBook
                {
                    Id = c.GetInt(0),
                    FullName = c.GetString(1),
                    Mobile = c.GetString(2),
                    Email = c.GetString(3),
                    Details = c.GetString(4)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }


        //Retrive All Contact Details
        public IList<AddressBook> GetContactsBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("AddressBook", new string[] { "Id", "FullName", "Mobile", "Email", "Details" }, "upper(FullName) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<AddressBook>();

            while (c.MoveToNext())
            {
                contacts.Add(new AddressBook
                {
                    Id = c.GetInt(0),
                    FullName = c.GetString(1),
                    Mobile = c.GetString(2),
                    Email = c.GetString(3),
                    Details = c.GetString(4)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Add New Contact
        public void AddNewContact(AddressBook contactinfo)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("FullName", contactinfo.FullName);
            vals.Put("Mobile", contactinfo.Mobile);
            vals.Put("Email", contactinfo.Email);
            vals.Put("Details", contactinfo.Details);
            db.Insert("AddressBook", null, vals);
        }

        //Get contact details by contact Id
        public ICursor getContactById(int id)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from AddressBook where Id=" + id + "", null);
            return res;
        }

        //Update Existing contact
        public void UpdateContact(AddressBook contitem)
        {
            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("FullName", contitem.FullName);
            vals.Put("Mobile", contitem.Mobile);
            vals.Put("Email", contitem.Email);
            vals.Put("Details", contitem.Details);


            ICursor cursor = db.Query("AddressBook",
                    new String[] { "Id", "FullName", "Mobile", "Email", "Details" }, "Id=?", new string[] { contitem.Id.ToString() }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("AddressBook", vals, "Id=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }


        //Delete Existing contact
        public void DeleteContact(string contactId)
        {
            if (contactId == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("AddressBook",
                    new String[] { "Id", "FullName", "Mobile", "Email", "Details" }, "Id=?", new string[] { contactId }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("AddressBook", "Id=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }



    }
}