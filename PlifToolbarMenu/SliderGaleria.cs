
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
using Android.Support.V4.View;
using Java.Lang;
using Android.Util;

namespace PlifToolbarMenu
{
	[Activity (Label = "SliderGaleria")]			
	public class SliderGaleria : PagerAdapter
	{
		private Activity activity;
		private List<string> rutas;
		private LayoutInflater inflater;
		string tag="PantallaCompleta";

		// constructor
		public SliderGaleria(Activity _activity, List<string> paths) {
			this.activity = _activity;
			this.rutas = paths;
		}

		public override int Count{
			get{ return rutas.Count; }
		}

		public override bool IsViewFromObject(Android.Views.View vista, Java.Lang.Object objeto){
			return vista ==  objeto;
		}

		public override Java.Lang.Object InstantiateItem(ViewGroup container, int position) {
			
			ImageView imagen;
			string ruta = rutas [position];

			inflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;

			View viewlayout = inflater.Inflate (Resource.Layout.pantalla_completa, container, false);

			imagen = viewlayout.FindViewById<ImageView> (Resource.Id.imageView1);

			Koush.UrlImageViewHelper.SetUrlDrawable (imagen, ParseRuta(ruta), Resource.Drawable.bolaplace);

			var viewPager = container.JavaCast<ViewPager>();


			Log.Debug (tag, "Agregamos la vista");
			viewPager.AddView(viewlayout);


			Log.Debug (tag,"JUSSSTO antes del return");
			return viewlayout;


		}

		public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objecto) {

			var viewPager = container.JavaCast<ViewPager>();
			//var Objecto = objecto.JavaCast<RelativeLayout> ();

      //			container.RemoveView(objecto);
			Log.Debug(tag,"Se destruye!");
			viewPager.RemoveView((View) objecto);

		}

		public string ParseRuta(string url){

			string extra="http://plif.mx/admin/";
			Log.Debug (tag,"se crea la ruta");
			//string ruta=mItems[position].Ruta;
			string ruta = url;
			Log.Debug (tag,"se crea el primer caracter");
			string first=ruta[0].ToString();

			Log.Debug (tag,"Inicia IF");
			if(first=="u" || first=="U"){
				//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
				ruta=extra+ruta;
				Log.Debug (tag,"Si era");
			}else{
				//no hagas nada, la imagen es de google
			}

			return ruta;

		}


	
	}

		


}

