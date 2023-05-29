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

namespace ContactApp
{
    [Activity(Label = "ContactListBaseAdapter")]
    public partial class ContactListBaseAdapter : BaseAdapter<AddressBook>
    {
        IList<AddressBook> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public ContactListBaseAdapter(Context context,
                                                IList<AddressBook> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }

        public override int Count
        {
            get { return contactListArrayList.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override AddressBook this[int position]
        {
            get { return contactListArrayList[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView btnDelete;
            ContactsViewHolder holder = null;
            if (convertView == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.list_row_contact_list, null);
                holder = new ContactsViewHolder();

                holder.txtFullName = convertView.FindViewById<TextView>(Resource.Id.lr_fullName);
                holder.txtMobile = convertView.FindViewById<TextView>(Resource.Id.lr_mobile);
                holder.txtEmail = convertView.FindViewById<TextView>(Resource.Id.lr_email);
                holder.txtDescription = convertView.FindViewById<TextView>(Resource.Id.lr_descriptin);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn);


                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].Id.ToString();
                        string fname = contactListArrayList[poldel].FullName;

                        contactListArrayList.RemoveAt(poldel);

                        DeleteSelectedContact(id);
                        NotifyDataSetChanged();
                    });

                    confirm.Show();
                };

                convertView.Tag = holder;
                btnDelete.Tag = position;
            }
            else
            {
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn);
                holder = convertView.Tag as ContactsViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtFullName.Text = contactListArrayList[position].FullName.ToString();
            holder.txtMobile.Text = contactListArrayList[position].Mobile;
            holder.txtEmail.Text = contactListArrayList[position].Email;
            holder.txtDescription.Text = contactListArrayList[position].Details;


                convertView.SetBackgroundResource(Resource.Drawable.list_selector_alternate);

            return convertView;
        }

        public IList<AddressBook> GetAllData()
        {
            return contactListArrayList;
        }

        public class ContactsViewHolder : Java.Lang.Object
        {
            public TextView txtFullName { get; set; }
            public TextView txtMobile { get; set; }
            public TextView txtEmail { get; set; }
            public TextView txtDescription { get; set; }
        }

        private void DeleteSelectedContact(string contactId)
        {
            AddressBookDbHelper _db = new AddressBookDbHelper(activity);
            _db.DeleteContact(contactId);
        }

    }
}