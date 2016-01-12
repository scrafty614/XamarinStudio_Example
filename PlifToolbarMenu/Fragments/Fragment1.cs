
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

//slider

//using Macaw.UIComponents;


//ASYNC LIBS
using Java.Net;
using System.Threading.Tasks;
using System.Json;
using System.Collections;
using System.Collections.Specialized;


using Android.Graphics;
//using Java.IO;
using Android.Graphics.Drawables;

using System.Net;
using System.IO;
using System.Drawing;


namespace PlifToolbarMenu
{
	public class Fragment1 : Android.Support.V4.App.Fragment
	{

		Button getimg;
		JsonValue imagenes_array;
		JsonValue objeto;
		string tag="Fragment1->Inicio";

		ImageView[] regs = new ImageView[4];
		ImageView mazatlan;
		ImageView torreon;
		ImageView zacatecas;
		ImageView durango;
	

		MainActivity este;

		string extra="http://plif.mx/admin/";


		//tasks = new List<Task>();

		int counter=0;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			Log.Debug (tag, "Iniciamos");
			este = new MainActivity ();
			View view = inflater.Inflate (Resource.Layout.Fragment1, container, false);
			getimg = view.FindViewById<Button> (Resource.Id.getimgsciudades);

			mazatlan = view.FindViewById<ImageView> (Resource.Id.mazatlan_img);
			torreon = view.FindViewById<ImageView> (Resource.Id.torreon_img);
			zacatecas = view.FindViewById<ImageView> (Resource.Id.zacatecas_img);
			durango = view.FindViewById<ImageView> (Resource.Id.durango_img);

			//var webClient = new WebClient ();

			regs [0] = mazatlan;
			regs [1] = torreon;
			regs [2] = zacatecas;
			regs [3] = durango;
			Log.Debug (tag, "Asignamos");

			getimg.Click += async (sender, e) => {
				Log.Debug(tag,"GET IMG CLICK!!!!");
				try{
					Log.Debug(tag,"GET IMG CLICK EN EL TRY!!!");
					imagenes_array = await FetchWeatherAsync("http://plif.mx/mobile/get_img_index");
					objeto =imagenes_array["respuesta"];

					foreach(JsonObject data in objeto){
						Log.Debug (tag, "LA RUTA: "+data["data"]["ruta"]);

						Koush.UrlImageViewHelper.SetUrlDrawable (regs[counter], extra+data["data"]["ruta"], Resource.Drawable.bolaplace);
						counter++;
					}

				}catch(Exception ex){
					Log.Debug (tag, "ERROR FETCHING IMAGE: "+ex);
					Toast.MakeText (Application.Context, "Ocurrió un error al recuperar las imágenes del inicio", ToastLength.Long).Show ();
					counter++;

				}

				counter=0;

			};

			getimg.PerformClick ();
			Log.Debug (tag, "Perform Click");

			durango.Click += delegate {
				
				var myActivity = (MainActivity) this.Activity;
				myActivity.BusquedaRegion("9");

			};

			torreon.Click += delegate {
				Toast.MakeText (Application.Context, "Próximamente...", ToastLength.Long).Show ();

			};

			zacatecas.Click += delegate {
				Toast.MakeText (Application.Context, "Próximamente...", ToastLength.Long).Show ();

			};

			torreon.Click += delegate {
				Toast.MakeText (Application.Context, "Próximamente...", ToastLength.Long).Show ();

			};

			Log.Debug (tag, "Terminamos y RETURN VIEW");

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




	

	}



}

