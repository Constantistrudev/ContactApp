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
using Java.Lang;

namespace ContactApp
{
    public class AddressBook
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }

        public static explicit operator AddressBook(Java.Lang.Object v)
        {
            throw new NotImplementedException();
        }
    }
}