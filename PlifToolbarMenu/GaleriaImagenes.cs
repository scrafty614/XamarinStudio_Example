
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

//async task
using Java.Net;
using System.Threading.Tasks;
using System.Json;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

//añadidas
using Android.Support.V7.App;
using SupportToolBar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics;
using System.Net;


using Android.Util;

namespace PlifToolbarMenu
{
	[Activity (Label = "", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]			
	public class GaleriaImagenes : Activity
	{

		private GridView photoGrid;
		private int mPhotoSize, mPhotoSpacing;
		private PlifGallery plifgallery;
		public List<ListaImagenes> imagelist;
		public List<string> solorutas;
		public JsonValue imagenes_servidor;
		public int pagina=0;


		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.galeria_imagenes);

			mPhotoSize = Application.Context.Resources.GetDimensionPixelSize(Resource.Dimension.photo_size);
			mPhotoSpacing = Application.Context.Resources.GetDimensionPixelSize(Resource.Dimension.photo_spacing);

			Log.Debug ("GALERIAIMAGENESPLIF","PhotoSize: "+mPhotoSize);
			Log.Debug ("GALERIAIMAGENESPLIF","PhotoSpacing: "+mPhotoSpacing);
			JsonValue imagenes_servidor2=null;

			try{

			//Log.Debug ("GALERIAIMAGENESPLIF","Inicia Async");
			string urel = "http://plif.mx/mobile/get_imagenes_negocio?id=" + Intent.GetStringExtra ("negocioid")+"&page="+pagina;
			Log.Debug ("GALERIAIMAGENESPLIF","La URL: "+urel);
			imagenes_servidor= await plifserver.FetchWeatherAsync(urel);
			Log.Debug ("GALERIAIMAGENESPLIF","Se asigna respuesta");
			imagenes_servidor2=imagenes_servidor["respuesta"];
			Log.Debug ("GALERIAIMAGENESPLIF","Se crea imagelist");
			imagelist = new List<ListaImagenes>();
			solorutas = new List<string> ();
			Log.Debug ("GALERIAIMAGENESPLIF","Inicia foreach");
			
			}catch(Exception ex){
				Toast.MakeText (Application.Context, "Por favor revisa tu conexión a internet e inténtalo de nuevo.", ToastLength.Long).Show ();
				Finish ();
			}

			int cnt = 1;

			if (imagenes_servidor2 != null) {
				foreach (JsonObject data in imagenes_servidor2) {
					Log.Debug ("GALERIAIMAGENESPLIF", "En el FOREACH");
					Log.Debug ("GALERIAIMAGENESPLIF", "Ruta: " + data ["imagenes"] ["ruta"]);
					solorutas.Add (data ["imagenes"] ["ruta"]);
					imagelist.Add (new ListaImagenes {
						Id = data ["imagenes"] ["id"],
						Titulo = data ["imagenes"] ["comentario"],
						//Titulo="Imagen "+cnt,
						Ruta = data ["imagenes"] ["ruta"]
					});
					cnt++;

				}
			

			Log.Debug ("GALERIAIMAGENESPLIF","Creando Adapter");
			plifgallery = new PlifGallery (Android.App.Application.Context, imagelist);

			Log.Debug ("GALERIAIMAGENESPLIF","Creando GV");
			photoGrid = FindViewById<GridView> (Resource.Id.albumGrid);

			Log.Debug ("GALERIAIMAGENESPLIF","Asignando Adapter");
			photoGrid.Adapter = plifgallery;

			Log.Debug ("GALERIAIMAGENESPLIF","Se acabó!");

			photoGrid.ViewTreeObserver.GlobalLayout += (object sender, EventArgs e) => {
				Log.Debug ("VTO-GL","Recalcula!");
					int numColumns = (int) Math.Floor(photoGrid.Width / ((mPhotoSize+0.0) + (mPhotoSpacing+0.0))); 
					Log.Debug ("VTO-GL","NumCols recien creado: "+numColumns);
				    int columnWidth = (photoGrid.Width / numColumns) - mPhotoSpacing;
					plifgallery.setNumColumns(numColumns);
					plifgallery.setItemHeight(columnWidth);
};

				photoGrid.ItemClick += photoGrid_ItemClick;

			}//if !=null

							
		}//OnCreate

		void photoGrid_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			Log.Debug ("PantallaCompleta","CLICKEVENT");



			try{
				var galeriaimg = new Intent (this, typeof(SliderActivity));
		//galeriaimg.AddFlags (ActivityFlags.NewTask);
		galeriaimg.PutExtra("ruta",imagelist [e.Position].Ruta);
				galeriaimg.PutExtra("posicion", e.Position.ToString());
		galeriaimg.PutStringArrayListExtra("rutas",solorutas);
		
		Log.Debug("ClickObjeto", "Posicion: "+e.Position);
		this.StartActivity (galeriaimg);
			}catch(Exception ex){
				Log.Debug ("PantallaCompleta","FAK!!!!");
			}
		
		
		}



		/*
		public void SigueBajando(object sender, AbsListView.ScrollEventArgs args){
			Log.Debug ("SigueBajando","*Baja :v*");
			Log.Debug ("SigueBajando","First Item: "+args.FirstVisibleItem);
			Log.Debug ("SigueBajando","Visible Items: "+args.VisibleItemCount);
			Log.Debug ("SigueBajando","Total Items: "+args.TotalItemCount); 

			var mustLoadMore = args.FirstVisibleItem + args.VisibleItemCount >= args.TotalItemCount - 10;

			if (mustLoadMore) {
				Log.Debug ("SigueBajando","Final! D:");
			}

		}*/

		private void elemento_seleccionado (object sender, AdapterView.ItemSelectedEventArgs e){
			Log.Debug ("ElementoSeleccionado","Wuuuh!");

		}


	}
}

