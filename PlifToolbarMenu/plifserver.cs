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

//añadidas
using Android.Support.V7.App;
using SupportToolBar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics;
using System.Net;


//async task
using Java.Net;
using System.Threading.Tasks;
using System.Json;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

//LOG
using Android.Util;

//WebView
using Android.Webkit;

//Snackbar... creo
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
//using V4Fragment = Android.Support.V4.App.Fragment;
//using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Views.Animations;


//POST
using Org.Apache.Http.Message;
using Org.Apache;

using Android.Database;
using Android.Provider;

using Java.IO;
using Java.Nio;
namespace PlifToolbarMenu
{
	public class plifserver
	{
		static string tag="plifserverFunction";

		//ASYNC METHOD
		// Gets weather data from the passed URL.
		public static async Task<JsonValue> FetchWeatherAsync (string url)
		{
			try{
			Log.Debug (tag, "Inicia FETCHWEATHER");
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
					System.Console.Out.WriteLine("Response: {0}", jsonDoc.ToString ());



					// Return the JSON document:
					return jsonDoc;
				}
			}

			}catch(Exception ex){
				Log.Debug (tag, "Error Cachado: "+ex);
				return null;
			}//le catch
		}


		//FIN ASYNC METHOD

		public static async Task<string> PostMultiPartForm(string url, List<byte[]> file, string filename, string paramName, string contentType, Dictionary<String, string> nvc, Boolean respuestarequerida)
		{
			Log.Debug (tag, "Inicia POSTMULTIPARTFORM");
			// log.Debug(string.Format("Uploading {0} to {1}", file, url));
			string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
			Log.Debug ("PostMultiPartForm", "Boundary: "+boundary);

			HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

			wr.ContentType = "multipart/form-data; boundary=" + boundary;

			wr.Method = "POST";



			Stream rs =  await wr.GetRequestStreamAsync();


			string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
			Log.Debug ("PostMultiPartForm", "formdataTemplate: "+formdataTemplate);

			foreach (string key in nvc.Keys)
			{
				rs.Write(boundarybytes, 0, boundarybytes.Length);
				string formitem = string.Format(formdataTemplate, key, nvc[key]);
				Log.Debug ("PostMultiPartForm", key+" ===> "+nvc[key]);
				Log.Debug ("PostMultiPartForm", "El FORMITEM FINAL: "+formitem);
				byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
				rs.Write(formitembytes, 0, formitembytes.Length);

			}

			Log.Debug ("PostMultiPartForm", "Llegamos hasta aquí?");

			rs.Write(boundarybytes, 0, boundarybytes.Length);

			Log.Debug ("PostMultiPartForm", "Y que tal hasta aqui?");



			Log.Debug ("PostMultiPartForm", "No sé si hasta aquí tambien");

			if (file == null) {
				Log.Debug ("PostMultiPartForm", "No hay imagen en el comentario. Skipping this!");
			} 
			else {

				try {
					string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

					file.ForEach(delegate(byte[] obj) {

						string header = string.Format(headerTemplate, paramName, file[0], contentType);
						byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
						rs.Write(headerbytes, 0, headerbytes.Length);
						rs.Write (obj, 0, obj.Length);

						rs.Write(boundarybytes, 0, boundarybytes.Length);

						Log.Debug ("PostMultiPartForm", "IMAGEN DE ARRAY BYTE EN EL CICLO");


					});

					byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
					rs.Write(trailer, 0, trailer.Length);

				} catch (Exception ex) {
					Log.Debug ("PostMultiPartForm", "Exepción controlada al cargar imagen: "+ex);
				}
				Log.Debug ("PostMultiPartForm", "Bien o mal, Pasamos la subida de la imagen");
			}


			//rs.Close();
			string responseString = String.Empty;
			WebResponse wresp = null;
			Log.Debug ("PostMultiPartForm", "Justo antes del Try");
			try
			{
				Log.Debug ("PostMultiPartForm", "En el try");
				wresp = await wr.GetResponseAsync().ConfigureAwait(false);			
				Log.Debug ("PostMultiPartForm", "despues del async");
				Stream respStream = wresp.GetResponseStream();

				if(respuestarequerida){
					Log.Debug ("PostMultiPartForm", "despues del response stream");
					StreamReader respReader = new StreamReader(respStream);
					Log.Debug ("PostMultiPartForm", "despues del respreader");
					responseString = respReader.ReadToEnd();
					Log.Debug ("PostMultiPartForm", "despues del readtoend");
				}else{
					responseString = "algo";
				}

				//log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
				//
				respStream.Close();
			}
			catch (Exception ex)
			{
				//log.Error("Error uploading file", ex);
				if (wresp != null)
				{
					//wresp.Close();
					wresp = null;
				}
			}
			finally
			{
				wr = null;
			}


			return responseString;

			//return "hello";

		}

		public static Bitmap getImagen(string url){
			Log.Debug (tag, "Inicia GETIMAGEN");
			Bitmap imageBitmap = null;
			try{


				var webClient = new WebClient ();
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0) {
					//SI TODO ESTA BIEN, LA CONVERTIMOS Y LA SETEAMOS
					imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				} else {
					//es nula
				}



			}catch(Exception ex){
				Toast.MakeText (Application.Context, "No se pudo recuperar la imagen de perfil ", ToastLength.Long).Show ();
				Log.Debug ("ImagenPerfil", ""+ex);
			}

			return imageBitmap;

		}

	}
}

