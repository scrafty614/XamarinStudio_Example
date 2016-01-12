
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
using Android.Util;

namespace PlifToolbarMenu
{
	[Activity (Label = "", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]			
			
	public class SliderActivity : Activity
	{
		IList<string> prerutas;
		List<string> rutas;
		private SliderGaleria slidergaleria;
		Android.Support.V4.View.ViewPager paginador;
		string posicion;

		protected override void OnCreate (Bundle bundle)
		{
			
			base.OnCreate (bundle);
			//prerutas = new IList<string> ();
			SetContentView (Resource.Layout.SliderGaleria);
			this.Window.AddFlags(WindowManagerFlags.Fullscreen);

			paginador = FindViewById<Android.Support.V4.View.ViewPager> (Resource.Id.pager);

			rutas = new List<string> ();
			prerutas = Intent.GetStringArrayListExtra ("rutas");
			posicion = Intent.GetStringExtra ("posicion");

			Log.Debug ("SliderActivity", "La posición del intent es: "+posicion);

			for(int i=0; i<prerutas.Count; i++){
				Log.Debug ("SliderActivity", "Añadiendo: "+prerutas[i]);
				rutas.Add (prerutas [i]);
			}

			Log.Debug ("SliderActivity", "Se crea la galería");
			slidergaleria = new SliderGaleria (this, rutas);

			Log.Debug ("SliderActivity", "se setea el adaptador");
			paginador.Adapter = slidergaleria;

			paginador.SetCurrentItem (Int32.Parse(posicion), false);

			Log.Debug ("SliderActivity", "Acabamos!");
			// Create your application here
		}
	}
}

