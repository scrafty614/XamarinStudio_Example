
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace PlifToolbarMenu
{
	public class Fragment4 : Android.Support.V4.App.Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate (Resource.Layout.Fragment4, container, false);
			Button btn4 = view.FindViewById<Button> (Resource.Id.btn_fra_4);
			TextView text4 = view.FindViewById<TextView>(Resource.Id.textFragment4);

			Toast.MakeText (Application.Context, "Estas en el fragmento 4", ToastLength.Long).Show ();


			btn4.Click += (sender, e) => 
			{
				text4.Text = "Estas en el Fragmento 4";
			};
			return view;
		}
	}
}

