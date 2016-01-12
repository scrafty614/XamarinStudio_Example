using System;
using Android.Support.V4.App;
using Android.App;
using Android.Support.V4.Widget;


using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


using Android.Support.Design.Widget;

using System.Collections.Generic;

namespace PlifToolbarMenu
{
	public class MyActionBarDrawerToggle : ActionBarDrawerToggle
	{
		
		Activity mActivity;

		public MyActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, int imageResource, int OpenDrawerDesc, int CloseDrawerDesc)
			: base(activity,drawerLayout, imageResource, OpenDrawerDesc, CloseDrawerDesc)
		{ 

		}

		public override void OnDrawerOpened (Android.Views.View drawerView)
		{
			base.OnDrawerOpened (drawerView);
		}

		public override void OnDrawerClosed (Android.Views.View drawerView)
		{
			base.OnDrawerClosed (drawerView);
		}

		public override void OnDrawerSlide (Android.Views.View drawerView, float slideOffset)
		{
			base.OnDrawerSlide (drawerView, slideOffset);
		}
	}
}

