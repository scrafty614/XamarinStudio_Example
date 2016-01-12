
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
using System.Timers;

using SupportFragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace PlifToolbarMenu
{
	[Activity (Label = "Plif!")]			
	public class HomeActivity : Activity
	{
		//Se declaran los fragments que contienen las imagenes
		HomeFragment1 mHomeFragment1 = new HomeFragment1 ();
		HomeFragment2 mHomeFragment2 = new HomeFragment2 ();
		HomeFragment3 mHomeFragment3 = new HomeFragment3 ();

		//Es el el detector y listener de las gesturas (movimiento de dedos) en android
		public GestureDetector Gdetector;
		public GestureListener Glistener;

		//Se reserva un nombre para indicar el fragment que se esta mostrando
		private Fragment currentFragment;

		//Variables auxiliares
		int count = 1;
		bool rtl = true;
		bool recorrido = false;

		protected override void OnCreate (Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (bundle); 
			this.SetContentView(Resource.Layout.Home);

			String package = PackageName;


			Glistener = new GestureListener();
			Gdetector = new GestureDetector (this, Glistener);
			var trans = FragmentManager.BeginTransaction ();
			trans.Add (Resource.Id.frame_home,mHomeFragment3,"HomeFragment3");
			trans.Hide (mHomeFragment3);
			trans.Add (Resource.Id.frame_home,mHomeFragment2,"HomeFragment2");
			trans.Hide (mHomeFragment2);
			trans.Add (Resource.Id.frame_home,mHomeFragment1,"HomeFragment1");
			trans.Commit();
			currentFragment = mHomeFragment1;

			Button haz_plif = FindViewById <Button> (Resource.Id.haz_plif);
			FrameLayout fcontainer = FindViewById<FrameLayout> (Resource.Id.frame_home);

			haz_plif.Click += (object sender, EventArgs e) => {
					StartActivity(typeof(login));
					Finish ();
			};
			fcontainer.Touch += Fcontainer_Touch;

			RemoteViews rmv = new RemoteViews(package, Resource.Layout.Home);
			rmv.SetTextViewText(Resource.Id.haz_plif,"Elias was here");
		}
		//t.Interval = 4300;
		void Fcontainer_Touch (object sender, View.TouchEventArgs e)
		{
			if(Gdetector.OnTouchEvent(e.Event)){
				rtl = Glistener.gtype();
				NextFragment ();
			}
		}

		public void NextFragment(){
			if (rtl) {
				count += 1;
				if (count == 3) {
					recorrido = true;
				}
				if(count>3){
					count = 1;
				}
			} else {
				count -= 1;
				if(count<=0){
					count = 3;
				}
			}
			switch (count) {
			case 1:
				ChangeFragment (mHomeFragment1);
				break;
			case 2:
				ChangeFragment (mHomeFragment2);
				break;
			case 3:
				ChangeFragment (mHomeFragment3);
				break;
			}
		}
		public void ChangeFragment(Fragment fragment){
			var trans = FragmentManager.BeginTransaction ();
			if (rtl) {
				//trans.SetCustomAnimations (Resource.Animation.Flip_right_in, Resource.Animation.Flip_right_out, Resource.Animation.Flip_left_in, Resource.Animation.Flip_left_out);
				trans.SetCustomAnimations(Resource.Animation.Slide_left_in, Resource.Animation.Slide_left_out);

			} else {
				//trans.SetCustomAnimations (Resource.Animation.Flip_left_in, Resource.Animation.Flip_left_out, Resource.Animation.Flip_right_in, Resource.Animation.Flip_right_out);
				trans.SetCustomAnimations(Resource.Animation.Slide_right_in, Resource.Animation.Slide_right_out);
			}
			trans.Hide (currentFragment);
			trans.Show (fragment);
			trans.AddToBackStack (null);
			trans.Commit();
			currentFragment = fragment;
		}


		public class GestureListener : GestureDetector.SimpleOnGestureListener{
			public bool tipo = true;
			public override bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
			{	
				var delta_y = e1.GetY() - e2.GetY();
				if (delta_y == 0 || delta_y <= 200) {
					isMoveToLeft (e1.GetX(),e2.GetX());
					return true;
				} else {
					return false;
				}
			}
			public void isMoveToLeft(float x1,float x2){
				var Dx = x1- x2;
				if (Dx > 0) {
					// Movimiento de derecha a izquierda
					tipo =  true;	
				} else {
					// Movimiento de izquierda a derecha
					tipo = false;
				}
			}
			public bool gtype(){
				return tipo;
			}

		}
	}
}


