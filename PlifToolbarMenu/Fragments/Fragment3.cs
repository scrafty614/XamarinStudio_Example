
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
using Java.Net;	


using Android.Graphics;
//using Java.IO;
using Android.Graphics.Drawables;

using System.Net;
using System.IO;
using System.Drawing;

//async
using System.Threading.Tasks;
using System.Json;
using System.Collections;
using System.Collections.Specialized;

namespace PlifToolbarMenu
{
	public class Fragment3 : Android.Support.V4.App.Fragment
	{
		private List<Negocio> mItems;
		private ListView mListView;
		public int counter = 0;

		public JsonValue negocios;
		JsonValue objeto;
		JsonValue objeto2;

		public int page = -1;


		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			page = -1;

			View view = inflater.Inflate (Resource.Layout.Fragment3, container, false);

			Button descubrir = view.FindViewById<Button> (Resource.Id.button1);
			Button regresar = view.FindViewById<Button> (Resource.Id.button2);
			//Button lel = view.FindViewById<Button> (Resource.Id.button3);

			ProgressBar cargando = view.FindViewById<ProgressBar> (Resource.Id.progressBar1);
			ProgressBar masneg = view.FindViewById<ProgressBar> (Resource.Id.progressBar2);



			descubrir.Click += async (sender, e) => {
				counter=0;
				page++;
				descubrir.Visibility=ViewStates.Gone;
				regresar.Visibility=ViewStates.Gone;


				if(page>-1 && page!=0){
				masneg.Visibility=ViewStates.Visible;
				}

				mListView = view.FindViewById<ListView> (Resource.Id.listView1);

				try{

				//obtenemos los del servidor según la página en la que estamos.
		        negocios = await FetchWeatherAsync("http://plif.mx/mobile/get_last_neg_add?pag="+page);

				objeto = negocios["respuesta"];
									
				
					mItems = new List<Negocio> ();

				foreach(JsonObject data in objeto){
						//AQUI LOS AGREGAMOS
						//mItems.Add(WebUtility.HtmlDecode(data["Data"]["titulo"]));

						mItems.Add(new Negocio(){
							NegocioId = WebUtility.HtmlDecode(data["Data"]["id"]),
							NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
							NegocioDir = WebUtility.HtmlDecode(data["Data"]["geo_calle"]+" "+data["Data"]["geo_numero"]+" "+data["Data"]["geo_colonia"]),
							NegocioCat = WebUtility.HtmlDecode(data["Data"]["nombre"]),
							NegocioFoto = WebUtility.HtmlDecode(data["Data"]["ruta"]),
							NegocioCal = WebUtility.HtmlDecode(data["Data"]["calificacion"]),
							NegocioNum = counter

						});

						counter++;
				}

					//Toast.MakeText (Application.Context, "llegamos?", ToastLength.Long).Show ();


//AQUI CARGA LOS NEGOCIOS
				MyListAdapter adapter = new MyListAdapter (Application.Context, mItems);
								    
				mListView.Adapter = adapter;
				
					cargando.Visibility=ViewStates.Gone;
					masneg.Visibility=ViewStates.Gone;

					descubrir.Visibility=ViewStates.Visible;

					if(page==0){
						regresar.Visibility=ViewStates.Gone;
					}else{
						regresar.Visibility=ViewStates.Visible;
					}



				}catch(Exception ex){
					Toast.MakeText (Application.Context, "Ocurrió un error al recuperar los negocios", ToastLength.Long).Show ();
					masneg.Visibility=ViewStates.Gone;
				}

			};

			//AQUI INICIA EL BOTON DE REGRESAR!!!!
				regresar.Click += async (sender, e) => {
				counter=0;
				page--;
				descubrir.Visibility=ViewStates.Gone;
				regresar.Visibility=ViewStates.Gone;

				masneg.Visibility=ViewStates.Visible;


				try{

					//obtenemos los del servidor según la página en la que estamos.
					negocios = await FetchWeatherAsync("http://plif.mx/mobile/get_last_neg_add?pag="+page);

					objeto = negocios["respuesta"];

					mListView = view.FindViewById<ListView> (Resource.Id.listView1);
					mItems = new List<Negocio> ();

					foreach(JsonObject data in objeto){
						//mItems.Add(WebUtility.HtmlDecode(data["Data"]["titulo"]));
					

						mItems.Add(new Negocio(){
							NegocioId = WebUtility.HtmlDecode( data["Data"]["id"]),
							NegocioName = WebUtility.HtmlDecode( data["Data"]["titulo"]),
							NegocioDir = WebUtility.HtmlDecode(data["Data"]["geo_calle"]+" "+data["Data"]["geo_numero"]+" "+data["Data"]["geo_colonia"]),
							NegocioCat = WebUtility.HtmlDecode(data["Data"]["nombre"]),
						    NegocioFoto = WebUtility.HtmlDecode(data["Data"]["ruta"]),
							NegocioCal = WebUtility.HtmlDecode(data["Data"]["calificacion"]),
							NegocioNum = counter
						});
					
						counter++;
					}

					MyListAdapter adapter = new MyListAdapter (Application.Context, mItems);
					mListView.Adapter = adapter;

					cargando.Visibility=ViewStates.Gone;
					masneg.Visibility=ViewStates.Gone;

					descubrir.Visibility=ViewStates.Visible;

					if(page==0){
						regresar.Visibility=ViewStates.Gone;
					}else{
						regresar.Visibility=ViewStates.Visible;
					}


				}catch(Exception ex){
					Toast.MakeText (Application.Context, "Ocurrió un error al recuperar los negocios", ToastLength.Long).Show ();
					masneg.Visibility=ViewStates.Gone;
				}

			};

			descubrir.PerformClick ();

			mListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				//Toast.MakeText (Application.Context, "El ID del negocio es: "+negItems [e.Position].NegocioId, ToastLength.Long).Show ();

				var negocio = new Intent (this.Activity, typeof(PerfilNegocio));
				negocio.PutExtra("id",mItems [e.Position].NegocioId);
				negocio.PutExtra("nombre",mItems [e.Position].NegocioName);
				negocio.PutExtra("direccion",mItems [e.Position].NegocioDir);
				negocio.PutExtra("categoria",mItems [e.Position].NegocioCat);
				negocio.PutExtra("calificacion",mItems [e.Position].NegocioCal);

				StartActivity (negocio);

				//StartActivity(typeof(PerfilNegocio));
				//AQUI INCICIAMOS LA ACTIVIDAD DE PERFIL Y LE PASAMOS DATOS



				/*
				var myActivity = (MainActivity) this.Activity;
				myActivity.VerNegocio(negItems [e.Position].NegocioId);
				*/

			};

			return view;
		}

		//ASYNC METHOD
		// Gets weather data from the passed URL.
		private async Task<JsonValue> FetchWeatherAsync (string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";

			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync ())
			{
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ())
				{
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					Console.Out.WriteLine("Response: {0}", jsonDoc.ToString ());

					// Return the JSON document:
					return jsonDoc;
				}
			}
		}
		//FIN ASYNC METHOD

		//ONRESUME METHOD

		public override void OnResume()
		{
			base.OnResume();

		}

		//FIN ONRESUME METHOD
	}
}



