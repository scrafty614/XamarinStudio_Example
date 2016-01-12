
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

namespace PlifToolbarMenu
{
	//[Activity (Label = "drawer_header")]			
	public class drawer_header : Activity
	{
		//public static TextView nombre = FindViewById<TextView> (Resource.Id.username);

		public static drawer_header Instance;


		public static void cambiarNombre(string val){
		    TextView nombre = Instance.FindViewById<TextView> (Resource.Id.useLogo);
			nombre.Text = val;


		
		}
			//nombre.Text = vnombre;
		


		protected override void OnCreate (Bundle bundle)
		{
			
			// Create your application here
		}



	}
}

