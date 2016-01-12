
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

using Android.Graphics;
using Android.Content.Res;

//LEL


//añadidas
using Android.Support.V7.App;
using SupportToolBar = Android.Support.V7.Widget.Toolbar;

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






using System.Threading;


using Java.Util;
//ENDLEL

namespace PlifToolbarMenu
{
	
	[Activity (Label = "Mi Perfil", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]			

	public class RegistroPlif : DialogFragment
	{
		Typeface font; 
		Android.Support.V7.App.AlertDialog.Builder alert;



		//Varibables Foto
		int source;
		public const int PickImageId = 1000;
		public const int PickCameraId = 1500;
		string apath;
		Android.Net.Uri imgUri;
		System.IO.Stream stream;
	
		ImageView fotoperfil;
		List <byte[]> fossbytes;
		//End Variables Foto
		Button registrarse;
		LinearLayout fotocontainer;



		public override void OnCreate (Bundle savedInstanceState){
			base.OnCreate (savedInstanceState);
			font = Typeface.CreateFromAsset(Application.Context.Assets, "Fonts/fa.ttf");
			alert = new Android.Support.V7.App.AlertDialog.Builder (Application.Context);
		
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			var view = inflater.Inflate (Resource.Layout.registro_plif, container, false);

			TextView subefoto = view.FindViewById<TextView> (Resource.Id.subefoto);
			TextView subeimagen = view.FindViewById<TextView> (Resource.Id.subeimagen);

			subefoto.SetTypeface(font, TypefaceStyle.Normal);
			subeimagen.SetTypeface (font, TypefaceStyle.Normal);

			fotocontainer = view.FindViewById<LinearLayout> (Resource.Id.fotocontainer);

			subefoto.Click += (object sender, EventArgs e) => {

				source=1;
				fossbytes = new List<byte[]>();
				GetImage((async (b, p) =>  {

					string tag="ImagenGalería";
					Log.Debug(tag,"Inicia GETIMAGE");
					Java.IO.File f = new Java.IO.File(p[0]);
					stream = Application.Context.ContentResolver.OpenInputStream(Android.Net.Uri.FromFile(f));
					Bitmap temp =BitmapFactory.DecodeStream(stream);
					fotoperfil = view.FindViewById<ImageView> (Resource.Id.fotoperfil);
					fotoperfil.SetImageBitmap(temp);
					fotocontainer.Visibility=ViewStates.Visible;

					if(temp!=null){
						Log.Debug("FOSSBYTES","Inicia conversión a bytes!");
						byte[] tmp2 = pliffunctions.PathToByte2(p[0]);
						Log.Debug("FOSSBYTES","Termina conversión a bytes!");
						fossbytes.Add(tmp2);
					}

				}));
				
			};

			subeimagen.Click += (object sender, EventArgs e) => {
				source=2;

				fossbytes = new List<byte[]>();
				GetImage((async (b, p) =>  {
					string tag = "FOTOREGISTRO";
					Log.Debug(tag,"Inicia GETIMAGE");
					Java.IO.File f = new Java.IO.File(p[0]);
					stream = Application.Context.ContentResolver.OpenInputStream(Android.Net.Uri.FromFile(f));
					Bitmap temp =BitmapFactory.DecodeStream(stream);

					fotoperfil = view.FindViewById<ImageView> (Resource.Id.fotoperfil);
					fotoperfil.SetImageBitmap(temp);
					fotocontainer.Visibility=ViewStates.Visible;

					if(temp!=null){
						Log.Debug("FOSSBYTES","Inicia conversión a bytes!");
						byte[] tmp2 = pliffunctions.PathToByte2(p[0]);
						Log.Debug("FOSSBYTES","Termina conversión a bytes!");
						fossbytes.Add(tmp2);
					}

				}));
			};

			registrarse = view.FindViewById<Button> (Resource.Id.registrar);

			TextView nombre = view.FindViewById<TextView> (Resource.Id.nombre);
			TextView apellidos = view.FindViewById<TextView> (Resource.Id.apellidos);
			TextView email = view.FindViewById<TextView> (Resource.Id.email);
			TextView pass1 = view.FindViewById<TextView> (Resource.Id.pass1);
			TextView pass2 = view.FindViewById<TextView> (Resource.Id.pass2);


			registrarse.Click += async (object sender, EventArgs e) => {

				nombre.Text="Gabino";
				apellidos.Text="Rutiaga";
				email.Text="furdragonrt@gmail.com";
				pass1.Text="hola";
				pass2.Text="hola";
					
				if(nombre.Text==""){
					Toast.MakeText (Application.Context, "Por favor introduce tu nombre", ToastLength.Long).Show ();
				}else if(apellidos.Text==""){
					Toast.MakeText (Application.Context, "Por favor introduce tus apellidos", ToastLength.Long).Show ();
				}else if(email.Text==""){
					Toast.MakeText (Application.Context, "Por favor introduce tu correo electrónico", ToastLength.Long).Show ();
				}else if(pass1.Text==""){
					Toast.MakeText (Application.Context, "Por favor introduce una contraseña", ToastLength.Long).Show ();
				}else if(pass1.Text!=pass2.Text){
					Toast.MakeText (Application.Context, "Tus contraseñas no coinciden. por favor verifícalas", ToastLength.Long).Show ();
				}else if(fossbytes==null){
					Toast.MakeText (Application.Context, "Sube una foto de perfil!", ToastLength.Long).Show ();
				}else{
					ProgressBar esperaregistro = view.FindViewById<ProgressBar> (Resource.Id.esperaregistro);
					esperaregistro.Visibility=ViewStates.Visible;
					registrarse.Visibility=ViewStates.Gone;


					Dictionary<string, string> datos =  new Dictionary<string,string>();

					datos.Add("nombre",nombre.Text);
					datos.Add("apellidos",apellidos.Text);
					datos.Add("email",email.Text);
					datos.Add("psw",pass1.Text);

					/*try{*/

						Log.Debug("Try","Inicia envío de datos");
						string resp = PostMultiPartFormUI ("http://plif.mx/pages/registNewUser?droid", fossbytes, "nada", "file[]", "image/jpeg", datos, true);
						Log.Debug("RegistroPlif","Respuesta del servidor: "+resp);
						JsonValue respuesta = JsonValue.Parse(resp);
						string registroexitoso=respuesta["registro"];

						if(registroexitoso=="true"){
							Toast.MakeText (Application.Context, "Por favor activa tu cuenta con el email que te enviamos a "+email.Text, ToastLength.Long).Show ();
							this.Dismiss();
						}else{
							Toast.MakeText (Application.Context, "Parece que ya te encuentras registrado pero no has confirmado aún tu cuenta", ToastLength.Long).Show ();
							esperaregistro.Visibility=ViewStates.Gone;
							registrarse.Visibility=ViewStates.Visible;
						}

					/*}catch(Exception ex){
						Log.Debug("ErrorRegistro","ocurrió un errorzote: "+ex);
						Toast.MakeText (Application.Context, "Ooops! Parece que nuestros sistemas están saturados en este momento. ¿Por que no lo intentas de nuevo en un momento?", ToastLength.Long).Show ();
						esperaregistro.Visibility=ViewStates.Gone;
						registrarse.Visibility=ViewStates.Visible;
					}*/


				}


			};

			return view;
		}

		//Inician Métodos de imágenes
		public delegate void OnImageResultHandler(bool success, List<string> imagePath);

		protected OnImageResultHandler _imagePickerCallback;

		public async void GetImage(OnImageResultHandler callback)
		{
			if (callback == null) {
				throw new ArgumentException ("OnImageResultHandler callback cannot be null.");
			}

			_imagePickerCallback = callback;
			InitializeMediaPicker();
		}

		public void InitializeMediaPicker()
		{
			Log.Debug ("MediaPicker","Selected image source: "+source);
			switch (source){
			case 1:
				Intent intent = new Intent ();
				intent.SetType ("image/*");
				//Intent.PutExtra (Intent.ExtraAllowMultiple, true);
				intent.SetAction (Intent.ActionGetContent);
				StartActivityForResult (Intent.CreateChooser (intent, "Select Picture"), PickImageId);
				break;
			case 2:
				Log.Debug ("CameraIntent","Iniciamos la cámara!");
				//STARTTTTT
				DateTime rightnow = DateTime.Now;
				string fname = "/plifcap_" + rightnow.ToString ("MM-dd-yyyy_Hmmss") + ".jpg";
				Log.Debug ("CameraIntent","Se crea la fecha y el filename!");

				Java.IO.File folder = new Java.IO.File (Android.OS.Environment.ExternalStorageDirectory + "/PlifCam");
				bool success;
				bool cfile=false;

				Java.IO.File file=null;

				if (!folder.Exists ()) {
					Log.Debug ("CameraIntent","Crea el folder!");
					//aqui el folder no existe y lo creamos
					success=folder.Mkdir ();
					if (success) {
						Log.Debug ("CameraIntent","Sip, lo creó correctamente");
						//el folder se creo correctamente!
						file = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/PlifCam/", fname);
						cfile = true;


					}else{
						//el folder no se creó, error.
					}

				} else {
					Log.Debug ("CameraIntent","Ya existia el folder!");
					//aqui el folder si existe
					file = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/PlifCam/", fname);
					cfile = true;
					apath=file.AbsolutePath;
					Log.Debug ("CameraIntent","El absolut path sería: "+apath); 

				}

				if (cfile) {
					Log.Debug ("CameraIntent", "Si lo creó!!!");

					Log.Debug ("CameraIntent","Se crea el archivo");
					imgUri = Android.Net.Uri.FromFile (file);
					Log.Debug ("CameraIntent","se obtiene la URI del archivo");

					Log.Debug ("CameraIntent","El absolut path sería: "+apath); 

					if (imgUri == null) {
						Log.Debug ("CameraIntent", "ESTA NULO DX");
					} else {
						Log.Debug ("CameraIntent","Nope, no es nulo");
					}

				} else {
					Log.Debug ("CameraIntent", "Error, el archivo no se creó!!!!");
				}
				//ENDDDDD

				Log.Debug ("CameraIntent", "La URI con la que se iniciara el intent es: "+imgUri);

				Intent intento = new Intent (Android.Provider.MediaStore.ActionImageCapture);
				Log.Debug ("CameraIntent","");
				intento.PutExtra(Android.Provider.MediaStore.ExtraOutput, imgUri);
				StartActivityForResult(intento, PickCameraId);

				break;
			}
		}

		public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			Log.Debug ("OnActivityResult", "Entramos al OnActivityResult");

			if ((resultCode != Result.Ok)) {

				Log.Debug ("OnActivityResult", "Algo salió mal D:");
				return;
				//ALGO SALIO MAL y regresamos
			}

			switch (requestCode){
			case PickImageId:
				string imagePath = null;
				if (data.Data != null) {
					//AQUI TENEMOS SOLAMENTE UNA IMAGEN, PERO IGUAL HAY QUE DEVOLVER UNA LISTA
					List<string> solouna = new List<string> ();
					var uri = data.Data;
					try {
						imagePath = GetPathToImage (uri);
					} catch (Exception ex) {
						// Failed for some reason.
					}

					solouna.Add (imagePath);

					_imagePickerCallback (solouna != null, solouna);
				} else {
					if (data.ClipData != null) {
						ClipData mClipData = data.ClipData;
						List<Android.Net.Uri> ListaImagenes = new List<Android.Net.Uri> ();
						for (int i = 0; i < mClipData.ItemCount; i++) {
							ClipData.Item item = mClipData.GetItemAt (i);
							ListaImagenes.Add (item.Uri);
							Log.Debug ("ACTIVITYRESULT", "Uri Añadida: " + item.Uri.ToString ());
						}

						List<string> results = new List<string> ();
						try {
							results = GetPathToImage (ListaImagenes);
						} catch (Exception ex) {
							// Failed for some reason.
						}
						_imagePickerCallback (results != null, results);

					} else {
						//clip data esta vacio, probablemente un error
						Log.Debug ("OnActivityResult", "Retornó null de seleccionar imagen en la galería. Posiblemente un error");
					}

				}
				break;

			case PickCameraId:
				//AQUI VIENE LA CAMARA
				Log.Debug("PickCameraId", "La Imagen viene de la camara! La data es null, la recuperamos de la imguri");
				List<string> resultscam = new List<string> ();
				try {
					resultscam.Add(apath);
				} catch (Exception ex) {
					// Failed for some reason.
				}
				_imagePickerCallback (resultscam != null, resultscam);

				break;
			}//EndSwitch
		}

		private string GetPathToImage(Android.Net.Uri uri)
		{
			string doc_id = "";
			try{
				using (var c1 = Application.Context.ContentResolver.Query (uri, null, null, null, null)) {
					c1.MoveToFirst ();
					String document_id = c1.GetString (0);
					doc_id = document_id.Substring (document_id.LastIndexOf (":") + 1);
				}
			}catch{
				Log.Debug ("GetPathToImage","Excepcion! El content es File");
				Android.Content.Context context = Android.App.Application.Context;
				try{
					String[] proj = { MediaStore.MediaColumns.Data };
					Log.Debug ("GetPathToImage","Y la URI? "+uri.ToString());

					//	var c1 = context.ContentResolver.Query (uri, proj, null, null, null)
					var loader = new CursorLoader (context, uri, proj, null, null, null);
					var c1 = (ICursor)loader.LoadInBackground();

					if(c1==null){
						Log.Debug ("GetPathToImage","Sigue siendo nulo-");
					}

					c1.MoveToFirst ();
					Log.Debug ("GetPathToImage","Pasamos?");

					String document_id = c1.GetString (0);
					doc_id = document_id.Substring (document_id.LastIndexOf (":") + 1);
					Log.Debug ("GetPathToImage","Y bien? "+doc_id);

				}
				catch(Exception ex){
					Log.Debug ("GetPathToImage","Excepcion! "+ex);
				}
			}

			string path = null;
			// The projection contains the columns we want to return in our query.
			string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
			using (var cursor = Application.Context.ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] {doc_id}, null))
			{
				if (cursor == null) return path;
				var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
				cursor.MoveToFirst();
				path = cursor.GetString(columnIndex);
				//cursor.Close ();
			}
			return path;
		}

		//ES EL MISMO PERO PARA MULTIPLES IMAGENES, DEVUELVE UNA LISTA
		private List<string> GetPathToImage(List<Android.Net.Uri> urilist)
		{
			List<string> rutas = new List<string> ();
			int elementos = urilist.Count;
			for(int i=0; i<elementos; i++){
				string doc_id = "";
				using (var c1 = Application.Context.ContentResolver.Query (urilist[i], null, null, null, null)) {
					c1.MoveToFirst ();
					String document_id = c1.GetString (0);
					doc_id = document_id.Substring (document_id.LastIndexOf (":") + 1);
				}

				string path = null;

				// The projection contains the columns we want to return in our query.
				string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
				using (var cursor = Application.Context.ContentResolver.Query (Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null)) {
					if (cursor == null)
						return null;
					var columnIndex = cursor.GetColumnIndexOrThrow (Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
					cursor.MoveToFirst ();
					path = cursor.GetString (columnIndex);
					//cursor.Close ();
				}

				//aqui terminaria el ciclo y las meteriamos
				rutas.Add(path);
				Log.Debug ("GETPATHTOIMG", "Ruta añadida! "+path);

			}


			return rutas;
		}

		public static string PostMultiPartFormUI(string url, List<byte[]> file, string filename, string paramName, string contentType, Dictionary<String, string> nvc, Boolean respuestarequerida)
		{
			string tag = "PMFLOCAL";
			Log.Debug (tag, "Inicia POSTMULTIPARTFORM");
			// log.Debug(string.Format("Uploading {0} to {1}", file, url));
			string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
			Log.Debug ("PostMultiPartForm", "Boundary: "+boundary);

			HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

			wr.ContentType = "multipart/form-data; boundary=" + boundary;

			wr.Method = "POST";



			Stream rs =  wr.GetRequestStream();


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
				wresp = wr.GetResponse();
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


		//Terminan  métodos de imágenes
	}
}

