
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

using System.Threading;


using Java.Util;


namespace PlifToolbarMenu
{
	[Activity (Label = "Mi Perfil", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]			
	public class profile : AppCompatActivity, ViewTreeObserver.IOnScrollChangedListener
	{
		private const int FULLY_VISIBLE_AT = 2;
		private SupportToolBar mToolbar;
		private ScrollView mScrollView;
		private int mScreenHeight;
		ImageView fotoperfil;
		string idusuario="";
		string correo="";
		string tag="PerfilUsuario";
		TextView nombreu;
		TextView correou;
		TextView puntos;
		TextView completado;
		TextView countup;

		ProgressBar completadobarra;
		List <BlogEntry> entradasblog; 
		JsonValue datos=null;
		Android.Widget.Button updatefoto;
		int source;
		public const int PickImageId = 1000;
		public const int PickCameraId = 1500;
		string apath;
		Android.Net.Uri imgUri;
		System.IO.Stream stream; 
		string hayimagen="";
		ProgressBar esperafoto;
		string rutafoto="";

		//PlifToolbarMenu.CircleImageView foto;
		ImageView foto;

		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.profile);
			Log.Debug (tag, "INICIANDO PERFIL");

			Log.Debug (tag, "Seteando toolbar y scrollview");
			mToolbar = FindViewById<SupportToolBar> (Resource.Id.toolbar);
			mScrollView = FindViewById<ScrollView> (Resource.Id.scrollView);

			SetSupportActionBar (mToolbar);
			SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_arrow_back);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetHomeButtonEnabled (true);

			Point size = new Point ();
			Display display = WindowManager.DefaultDisplay;
			display.GetSize (size);
			mScreenHeight = size.Y;

			mScrollView.ViewTreeObserver.AddOnScrollChangedListener (this);

			nombreu = FindViewById<TextView> (Resource.Id.nombre);
			correou = FindViewById<TextView> (Resource.Id.correo);
			puntos = FindViewById<TextView> (Resource.Id.puntos);
			completado = FindViewById<TextView> (Resource.Id.completado);
			countup = FindViewById<TextView> (Resource.Id.countup);
			completadobarra = FindViewById<ProgressBar> (Resource.Id.completadobarra);
			entradasblog = new List<BlogEntry> ();
			updatefoto = FindViewById<Button> (Resource.Id.updatefoto);
			Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder (this);
			//progressBar.getProgressDrawable().setColorFilter(Color.RED, Mode.SRC_IN);
			completadobarra.ProgressDrawable.SetColorFilter(Color.ParseColor("#68FF68"), Android.Graphics.PorterDuff.Mode.SrcIn);
			source = 1;
			apath = "";
			imgUri = null;

			esperafoto = FindViewById<ProgressBar> (Resource.Id.esperafoto);

			//foto = FindViewById<PlifToolbarMenu.CircleImageView> (Resource.Id.fotoperfil);
			foto = FindViewById<ImageView> (Resource.Id.fotoperfil);
			Typeface font = Typeface.CreateFromAsset(Assets, "Fonts/fa.ttf");

			//Preferencias de la App
			var prefs = this.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);

			idusuario = prefs.GetString ("id", null);
			correo = prefs.GetString ("email",null);

			var editor = prefs.Edit ();
			var nombre = prefs.GetString("nombre", null);
			SupportActionBar.Title = nombre;

			Log.Debug (tag, "Inicia foto de perfil");
			fotoperfil = FindViewById<ImageView> (Resource.Id.fotoperfil);

			try{
				JsonValue rutapre = await plifserver.FetchWeatherAsync ("http://plif.mx/mobile/get_img_usr?id="+idusuario);
				Log.Debug ("json","SI existe la ruta");
				string extra="http://plif.mx/";
				string path = rutapre [0] ["imagen_usuarios"] ["ruta"];


				string first=path[0].ToString();

				if(first=="u" || first=="U"){
					//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
					path=extra+path;
				}else{
					//no hagas nada, la imagen es de google o de algun otro lado.
				}


				Log.Debug (tag, "La ruta es: "+path);
				rutafoto=path;
				Koush.UrlImageViewHelper.SetUrlDrawable(fotoperfil, path, Resource.Drawable.profile);
				hayimagen="si";
			}catch(Exception ex){
				Log.Debug ("json","no existe la ruta");
				fotoperfil.SetImageResource (Resource.Drawable.profile);
				hayimagen="no";
			}

			string url = "http://plif.mx/perfil/" + idusuario + ".json?droid";
			Log.Debug (tag, "la URL es: " + url);

			string completadonum = "";



			try{
			datos = await plifserver.FetchWeatherAsync(url);

			nombreu.Text = nombre;
			correou.Text = correo;
			puntos.Text = datos ["0"] ["Users"] ["puntos"]+" puntos";
			//Log.Debug(tag, "El completado: "+datos ["completadoperfil"]);

				int comptemp=Int32.Parse(datos ["completadoperfil"]);

				if(comptemp==20){
					//no hagas nada
				}else{
					comptemp=comptemp+20;
				}

				completadonum = comptemp.ToString();
			completado.Text="Tu perfil está completo al "+completadonum+"% ¡Complétalo desde nuestra web para ganar más puntos!";

		    //aqui ponemos lo que hay completado del perfil;

			}catch(Exception ex){
				Log.Debug (tag, "FUCK!! "+ex.ToString() );
			}



			System.Threading.Tasks.Task.Factory.StartNew(() => {

				TextView cp = FindViewById<TextView> (Resource.Id.countup);
				int j = 0;
				Log.Debug(tag,"thread started");
				//while (j<100){
				for(j=0; j<=Int32.Parse(completadonum); j++){
					Log.Debug(tag,"Ciclo For");
					ThreadPool.QueueUserWorkItem (o => setprogreso (countup, j));
					Log.Debug(tag,"Se llama al ThreadPool");
					Thread.Sleep (25);
					Log.Debug(tag,"Retardo de 100ms");
					j++;
					Log.Debug(tag,"El nuevo valor de J: "+j);
				}
				//cp.Text = j.ToString()+"%";

			});

			//Creamos el inflater para las vistas de los negocios
			LayoutInflater inflater = LayoutInflater.From(this);

			//creamos la ll donde se van a meter las entradas
			LinearLayout blogcontainer = FindViewById<LinearLayout> (Resource.Id.blogcontainer);


			try{
			//Aquí vamos a llenar el blog
				JsonValue datosblog = datos["entradas"];
				foreach(JsonObject data in datosblog){
					Log.Debug(tag, "Entrada al blog: "+data["b"]["titulo"]);
					//JsonValue b = data["b"];
					//JsonValue cero = data ["0"];
					entradasblog.Add(new BlogEntry(){
						Id=data["b"]["id"],
						Titulo=data["b"]["titulo"],
						User=data["b"]["user"],
						FechaPublicacion=data["b"]["fecha_publicacion"],
						Likes = data["b"]["likes"],
						ImagenCabezado=data["b"]["imgencabezado"],
						Promedio="lel"
					});
				}

				Log.Debug(tag, "Justo antes del foreach del blog");
				for(int j=0; j<entradasblog.Count; j++){
					Log.Debug(tag, "Entramos al foreach del blog e inflamos");
					View row = inflater.Inflate(Resource.Layout.blog_row, blogcontainer, false);

					//ESTE SETEA EL NOMBRE
					Log.Debug(tag, "creamos textview");
					TextView tituloblog = row.FindViewById<TextView> (Resource.Id.entryname);
					Log.Debug(tag, "seteamos textview");
					tituloblog.Text = entradasblog [j].Titulo;

					//inicia estrellas
					//ESTE SETEA LAS ESTRELLAS DE LA CALIFICACION
					ImageView cali = row.FindViewById<ImageView> (Resource.Id.calificacion);

					string cal = entradasblog[j].Promedio;

					switch (cal) {

					case "0":
						cali.SetImageResource (Resource.Drawable.e0);
						break;

					case "":
						cali.SetImageResource (Resource.Drawable.e0);
						break;

					case "null":
						cali.SetImageResource (Resource.Drawable.e0);
						break;

					case null:
						cali.SetImageResource (Resource.Drawable.e0);
						break;

					case "1":
						cali.SetImageResource (Resource.Drawable.e1);
						break;

					case "2":
						cali.SetImageResource (Resource.Drawable.e2);
						break;

					case "3":
						cali.SetImageResource (Resource.Drawable.e3);
						break;

					case "4":
						cali.SetImageResource (Resource.Drawable.e4);
						break;

					case "5":
						cali.SetImageResource (Resource.Drawable.e5);
						break;



					default:
						cali.SetImageResource (Resource.Drawable.e0);
						break;
					}
					//termina estrellas

					//inicia imagen
					//ESTE SETEA LA IMAGEN
					ImageView imagen = row.FindViewById<ImageView> (Resource.Id.NegocioFoto);

					if (entradasblog[j].ImagenCabezado == null || entradasblog[j].ImagenCabezado == "" || entradasblog[j].ImagenCabezado == "null") {
						//pon la imagen por defecto
						imagen.SetImageResource (Resource.Drawable.marca);
					} else {
						//TENEMOS QUE VERIFICAR SI LA IMAGEN ES DE GOOGLE O DE NOSOTROS!!!
						string extra="http://plif.mx/admin/";
						string ruta=entradasblog[j].ImagenCabezado;
						string first=ruta[0].ToString();

						if(first=="u" || first=="U"){
							//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
							ruta=extra+ruta;
						}else{
							//no hagas nada, la imagen es de google
						}

						Koush.UrlImageViewHelper.SetUrlDrawable (imagen, ruta, Resource.Drawable.bolaplace);
					//acaba imagen
					}

					//SETEA EL NUMERO DE CORAZONES
					TextView corazones = row.FindViewById<TextView> (Resource.Id.corazones);
					corazones.Text=entradasblog[j].Likes;


					Log.Debug(tag, "retornamos vista");
					blogcontainer.AddView(row);

				

				}//For


			}catch(Exception ex){
				Log.Debug (tag, "Algo en el blog falló D: "+ex.ToString());
			}


			updatefoto.SetTypeface(font, TypefaceStyle.Normal);

			updatefoto.Click += async (object sender, EventArgs e) => {
				//actualizamos la foto

				alert.SetTitle("Actualizar foto");

				alert.SetPositiveButton ("Desde cámara", (senderAlert, args) => {
					//abrimos el intent de la cámara
					updatefoto.Visibility=ViewStates.Gone;
					esperafoto.Visibility=ViewStates.Visible;
						

					source=2;

					Dictionary<string,string> diccionario = new Dictionary<string,string>();

					List <byte[]> fossbytes = new List<byte[]>();
					GetImage((async (b, p) =>  {
						Log.Debug(tag,"Inicia GETIMAGE");

						AlphaAnimation alpha = new AlphaAnimation(1.0F, 0.4F); // change values as you want
						alpha.Duration=500; // Make animation instant
						alpha.FillAfter=true; // Tell it to persist after the animation ends
						// And then on your imageview
						fotoperfil.StartAnimation(alpha);

						Java.IO.File f = new Java.IO.File(p[0]);
						stream = this.ContentResolver.OpenInputStream(Android.Net.Uri.FromFile(f));
						Bitmap temp =BitmapFactory.DecodeStream(stream);

						if(temp!=null){
							Log.Debug("FOSSBYTES","Inicia conversión a bytes!");
							byte[] tmp2 = pliffunctions.PathToByte2(p[0]);
							Log.Debug("FOSSBYTES","Termina conversión a bytes!");

							fossbytes.Add(tmp2);
							diccionario.Add("imagen_usuario_id",hayimagen);
							diccionario.Add("usuario_id",idusuario);


							try{
							//AQUI vamos a actualizar la foto del perfil con el codigo del multipart
							string resp = await plifserver.PostMultiPartForm ("http://plif.mx/pages/UpdateImgPerfil", fossbytes, "nada", "file[]", "image/jpeg", diccionario, true);
							Log.Debug(tag,"Respuesta del servidor: "+resp);

							//PONEMOS LA IMAGEN, MOSTRAMOS EL BOTON Y OCULTAMOS EL PROGRESSBAR
							JsonValue rutapre = await plifserver.FetchWeatherAsync ("http://plif.mx/mobile/get_img_usr?id="+idusuario);
							Log.Debug ("json","SI existe la ruta");
							string path = "http://plif.mx/"+rutapre [0] ["imagen_usuarios"] ["ruta"];
							Log.Debug (tag, "La ruta es: "+path);
								rutafoto=path;
							Koush.UrlImageViewHelper.SetUrlDrawable(fotoperfil, path, Resource.Drawable.profile);
							hayimagen="si";

							

							AlphaAnimation alpha2 = new AlphaAnimation(0.4F, 1.0F); // change values as you want
							alpha2.Duration=500; // Make animation instant
							alpha2.FillAfter=true; // Tell it to persist after the animation ends
							// And then on your imageview
							updatefoto.Visibility=ViewStates.Visible;
							esperafoto.Visibility=ViewStates.Gone;
							fotoperfil.StartAnimation(alpha2);
							}catch(Exception ex){
								updatefoto.Visibility=ViewStates.Visible;
								esperafoto.Visibility=ViewStates.Gone;

								AlphaAnimation alpha2 = new AlphaAnimation(0.4F, 1.0F); // change values as you want
								alpha2.Duration=500; // Make animation instant
								alpha2.FillAfter=true; // Tell it to persist after the animation ends
								// And then on your imageview
								fotoperfil.StartAnimation(alpha2);
								Toast.MakeText (Application.Context, "Ocurrió un error al actualizar la foto de perfil. Inténtalo de nuevo", ToastLength.Long).Show ();

							}

						}

					}));

				} );

				fotoperfil.Click += (object sender2, EventArgs e2) => {
					//abrimos el intent de pantalla completa
					Log.Debug(tag,"clickeo la imagen!");
					var pantallacompleta = new Intent (this, typeof(PantallaCompleta));
					pantallacompleta.PutExtra("ruta",rutafoto);

					StartActivity (pantallacompleta);

				};

				alert.SetNeutralButton ("Desde galería", (senderAlert, args) => {

					//abrimos el intent de la galería
					updatefoto.Visibility=ViewStates.Gone;
					esperafoto.Visibility=ViewStates.Visible;


					source=1;

					Dictionary<string,string> diccionario = new Dictionary<string,string>();

					List <byte[]> fossbytes = new List<byte[]>();
					GetImage((async (b, p) =>  {
						Log.Debug(tag,"Inicia GETIMAGE");

						AlphaAnimation alpha = new AlphaAnimation(1.0F, 0.4F); // change values as you want
						alpha.Duration=500; // Make animation instant
						alpha.FillAfter=true; // Tell it to persist after the animation ends
						// And then on your imageview
						fotoperfil.StartAnimation(alpha);

						Java.IO.File f = new Java.IO.File(p[0]);
						stream = this.ContentResolver.OpenInputStream(Android.Net.Uri.FromFile(f));
						Bitmap temp =BitmapFactory.DecodeStream(stream);

						if(temp!=null){
							Log.Debug("FOSSBYTES","Inicia conversión a bytes!");
							byte[] tmp2 = pliffunctions.PathToByte2(p[0]);
							Log.Debug("FOSSBYTES","Termina conversión a bytes!");

							fossbytes.Add(tmp2);
							diccionario.Add("imagen_usuario_id",hayimagen);
							diccionario.Add("usuario_id",idusuario);


							try{
								//AQUI vamos a actualizar la foto del perfil con el codigo del multipart
								string resp = await plifserver.PostMultiPartForm ("http://plif.mx/pages/UpdateImgPerfil", fossbytes, "nada", "file[]", "image/jpeg", diccionario, true);
								Log.Debug(tag,"Respuesta del servidor: "+resp);

								//PONEMOS LA IMAGEN, MOSTRAMOS EL BOTON Y OCULTAMOS EL PROGRESSBAR
								JsonValue rutapre = await plifserver.FetchWeatherAsync ("http://plif.mx/mobile/get_img_usr?id="+idusuario);
								Log.Debug ("json","SI existe la ruta");
								string path = "http://plif.mx/"+rutapre [0] ["imagen_usuarios"] ["ruta"];
								Log.Debug (tag, "La ruta es: "+path);
								rutafoto=path;
								Koush.UrlImageViewHelper.SetUrlDrawable(fotoperfil, path, Resource.Drawable.profile);
								hayimagen="si";



								AlphaAnimation alpha2 = new AlphaAnimation(0.4F, 1.0F); // change values as you want
								alpha2.Duration=500; // Make animation instant
								alpha2.FillAfter=true; // Tell it to persist after the animation ends
								// And then on your imageview
								updatefoto.Visibility=ViewStates.Visible;
								esperafoto.Visibility=ViewStates.Gone;
								fotoperfil.StartAnimation(alpha2);
							}catch(Exception ex){
								updatefoto.Visibility=ViewStates.Visible;
								esperafoto.Visibility=ViewStates.Gone;

								AlphaAnimation alpha2 = new AlphaAnimation(0.4F, 1.0F); // change values as you want
								alpha2.Duration=500; // Make animation instant
								alpha2.FillAfter=true; // Tell it to persist after the animation ends
								// And then on your imageview
								fotoperfil.StartAnimation(alpha2);
								Toast.MakeText (Application.Context, "Ocurrió un error al actualizar la foto de perfil. Inténtalo de nuevo", ToastLength.Long).Show ();

							}

						}

					}));

				} );

				alert.Show();
		

			};


		}

		public void setprogreso(TextView progreso, int valor){
			RunOnUiThread (() => progreso.Text = valor.ToString()+"%");
			RunOnUiThread (() => completadobarra.Progress = valor);
		//	completadobarra.Progress = valor;

		}

		//INICIAN COSAS FOTO PERFIL
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
				Intent = new Intent ();
				Intent.SetType ("image/*");
				//Intent.PutExtra (Intent.ExtraAllowMultiple, true);
				Intent.SetAction (Intent.ActionGetContent);
				StartActivityForResult (Intent.CreateChooser (Intent, "Select Picture"), PickImageId);
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

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
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
				using (var c1 = ContentResolver.Query (uri, null, null, null, null)) {
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
			using (var cursor = this.ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] {doc_id}, null))
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
				using (var c1 = ContentResolver.Query (urilist[i], null, null, null, null)) {
					c1.MoveToFirst ();
					String document_id = c1.GetString (0);
					doc_id = document_id.Substring (document_id.LastIndexOf (":") + 1);
				}

				string path = null;

				// The projection contains the columns we want to return in our query.
				string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
				using (var cursor = this.ContentResolver.Query (Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null)) {
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


		//TERMINAN COSAS FOTO PERFIL




		public float GetOpacity()
		{
			float fullVisibleAtPx = mScreenHeight /FULLY_VISIBLE_AT;

			float alpha = mScrollView.ScrollY / fullVisibleAtPx;

			if (alpha > 1)
			{
				return 1;
			}

			else if (alpha < 0)
			{
				return 0;
			}

			return alpha;
		}

		public void OnScrollChanged()
		{
			mToolbar.SetBackgroundColor(Color.Argb((int)(GetOpacity() * 255), 51, 150, 209));
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.action_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
						StartActivity(typeof(MainActivity));
						Finish();
					return true;
			}
			return base.OnOptionsItemSelected (item);
		}
		public override void OnBackPressed ()
		{
			base.OnBackPressed ();
		}
	}
}