
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
using Android.Views.Animations;


using Android.Util;

namespace PlifToolbarMenu
{
	[Activity (Label = "", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]			
	public class PantallaCompleta : Activity
	{
		Animation floatbounce;
		Animation bounce;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Log.Debug ("PantallaCompleta","Inicia PantallaCompleta");
			Log.Debug ("PantallaCompleta","SetContentView");
			floatbounce = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.floatbounce);
			bounce = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.floatbounce);

			this.Window.AddFlags(WindowManagerFlags.Fullscreen);

			SetContentView (Resource.Layout.pantalla_completa);

			Log.Debug ("PantallaCompleta","Se recupera la ruta");
			string ruta = Intent.GetStringExtra ("ruta");



			ImageView imagen = FindViewById<ImageView> (Resource.Id.imageView1);
			//imagen.StartAnimation (bounce);
			Koush.UrlImageViewHelper.SetUrlDrawable (imagen, ParseRuta(ruta), Resource.Drawable.bolaplace);

			// Create your application here
		}

		public string ParseRuta(string url){
			
			string extra="http://plif.mx/admin/";
			Log.Debug ("PantallaCompleta","se crea la ruta");
			//string ruta=mItems[position].Ruta;
			string ruta = url;
			Log.Debug ("PantallaCompleta","se crea el primer caracter");
			string first=ruta[0].ToString();

			Log.Debug ("PantallaCompleta","Inicia IF");
			if(first=="u" || first=="U"){
				//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
				ruta=extra+ruta;
				Log.Debug ("PantallaCompleta","Si era");
			}else{
				//no hagas nada, la imagen es de google
			}

			return ruta;

		}
	}
}

