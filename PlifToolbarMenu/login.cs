using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Json;
using System.Threading.Tasks;


//añadidas
using Android.Support.V7.App;
using SupportToolBar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics;


//Snackbar... creo
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
//using V4Fragment = Android.Support.V4.App.Fragment;
//using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

using Android.Views.InputMethods;

using Android.Util;

using Android.Views.Animations;
using Android.Webkit;

using Xamarin.Facebook;
using Xamarin.Facebook.Share.Widget;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.AppEvents;
using Xamarin.Facebook.Share.Model;
using Xamarin.Facebook.Share;
using Xamarin.Facebook.Login;

using System.Collections.Generic;
using System.Security.Cryptography;
using Java.IO;
using Java.Nio;
using Org.Json;

namespace PlifToolbarMenu
{
	[Activity (Label = "", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]			
	//, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation
	public class login : AppCompatActivity,IFacebookCallback,GraphRequest.IGraphJSONObjectCallback
	{

		Animation bounce;
		Animation sdown;
		Animation fadein;
		private ICallbackManager respuesta;
		private MyProfileTracker mProfileTracker;

		Dictionary<string,string> datosfb;


		LinearLayout infoface;
		ImageView imgface;
		TextView entrandoface;
		LinearLayout infologin; 



		//POR UNICA Y EXCLUSIVA VEZ LOS VOY A IMPLEMENTAR ANTES DEL ONCREATE. NOMAS PARA PODER VERLOS

		public void getDatos(){
			try{
			GraphRequest request = GraphRequest.NewMeRequest (AccessToken.CurrentAccessToken, this);
			Bundle parameters = new Bundle ();
			parameters.PutString ("fields", "id,name,age_range,email");
			request.Parameters = parameters;
			request.ExecuteAsync ();
			}catch(Exception ex){
				Log.Debug ("GetDatos","Algo salió mal! "+ex);
				Toast.MakeText (this, "Algo salió mal. Por favor inténtalo de nuevo!", ToastLength.Long).Show();
			}
		}



		public async void OnCompleted(Org.Json.JSONObject json, GraphResponse response){
			string tag = "OnCompleted";
			Log.Debug (tag, "Lel, datos :v");
			try{
			string email = json.GetString ("email");
			Log.Debug (tag, "el email debería ser: "+email);

			if (email == "" || email == null || email == "null") {
				//aqui vamos a cachar si no hay email para no hacer el registro.
			} else {

				datosfb.Add ("email", email);

				//ya tenemos todo, ahora podemos mandarlo!, tal vez aqui considere poner la foto de perfil y el nombre en la pantalla

				string resp = await plifserver.PostMultiPartForm ("http://plif.mx/pages/log_reg_face", null, "nada", "file[]", "image/jpeg", datosfb, true);
				Log.Debug (tag,"LA RESPUESTA!!!: "+resp);
        		JsonValue respuesta = JsonValue.Parse(resp);
				//Estructura de respuesta: [{"u":{"id":"225","username":"lukario@live.com.mx","nombre":"Rutiaga","apellidos":"Cervantes","email":"lukario@live.com.mx","rol":"cliente","facebook_id":"10204786604253973","puntos":"100"},"iu":{"ruta":"https:\/\/graph.facebook.com\/10204786604253973\/picture?height=800&width=800&migration_overrides=%7Boctober_2012%3Atrue%7D"}}]
				if (respuesta != null) {
					var prefs = this.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
					var editor = prefs.Edit ();

					string n=respuesta[0]["u"]["nombre"];
					string ap=respuesta[0]["u"]["apellidos"];

					editor.PutString ("id", respuesta[0]["u"]["id"]);
					editor.PutString ("nombre", n+ap);
					editor.PutString ("email", respuesta[0]["u"]["email"]);
					editor.PutString ("img_perfil", respuesta[0]["iu"]["ruta"]);
					editor.Commit();

					//JA

					//Toast.MakeText (this, "Inicio de sesión correcto", ToastLength.Long).Show();
					StartActivity(typeof(MainActivity));
					Finish();

				} else {
						Toast.MakeText (this, "Há ocurido un inconveniente. Por favor inténtalo de nuevo!", ToastLength.Long).Show();
				}

				

			}
				//CATCH
			}catch(Exception ex){
				Log.Debug ("OnCompleted", "Algo Salio mal en el OnCompleted! " + ex);
				//Toast.MakeText (this, "Algo salió mal. Por favor inténtalo de nuevo.", ToastLength.Long).Show();

				infoface = FindViewById<LinearLayout> (Resource.Id.infoface);
				infologin = FindViewById<LinearLayout> (Resource.Id.infologin);

				infologin.Visibility = ViewStates.Visible;
				infoface.Visibility = ViewStates.Gone;

			}

		}



		void mProfileTracker_mOnProfileChanged(object sender, OnProfileChangedEventsArgs e){
			string tag="ProfileTracker";

			try{
			if (e.mProfile != null) {
				datosfb = new Dictionary<string,string> ();

				datosfb.Add ("nombre", e.mProfile.FirstName);
				datosfb.Add ("apellidos", e.mProfile.LastName);
				//aqui agregariamos el email, pero lo vamos a añadir hasta el siguiente callback.

				Random rnd = new Random ();
				string str = rnd.Next (0, 100).ToString () + rnd.Next (0, 100).ToString () + rnd.Next (0, 100).ToString () + rnd.Next (0, 100).ToString ();

				MD5 md5 = MD5CryptoServiceProvider.Create();
				ASCIIEncoding encoding = new ASCIIEncoding();
				byte[] stream = null;
				StringBuilder sb = new StringBuilder();
				stream = md5.ComputeHash(encoding.GetBytes(str));
				for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
				string final = sb.ToString();

				datosfb.Add ("psw", final);

				string url = e.mProfile.GetProfilePictureUri (800, 800).ToString ();
				Log.Debug ("URL!!!", "la url sería" + url);


				imgface = FindViewById<ImageView> (Resource.Id.imgface);
				Koush.UrlImageViewHelper.SetUrlDrawable(imgface, url, Resource.Drawable.faceflat);
	
				entrandoface = FindViewById<TextView> (Resource.Id.entrandoface);
				if (entrandoface != null) {
					entrandoface.Text = "Entrando como " + e.mProfile.Name;
				} else {
				
					Log.Debug ("NOMBREPERFIL", "El pinche TV es nulo");
				}

				infoface = FindViewById<LinearLayout> (Resource.Id.infoface);
				infologin = FindViewById<LinearLayout> (Resource.Id.infologin);

				infologin.Visibility = ViewStates.Gone;
				infoface.Visibility = ViewStates.Visible;


				Log.Debug (tag, "El Primer Nombre es: " + e.mProfile.FirstName);
				Log.Debug (tag, "El Apellido es: " + e.mProfile.LastName);
				Log.Debug (tag, "El Username es: " + e.mProfile.Name);
				Log.Debug (tag, "El ID del perfil es: " + e.mProfile.Id);
				Log.Debug (tag, "La URI de la foto de perfil es: " + 	e.mProfile.GetProfilePictureUri(800,800));
				//Log.Debug(tag, "El correo electrónico es: ", "Lel, no lo tenemos" );

				datosfb.Add ("fbid",e.mProfile.Id);
				datosfb.Add ("profilepic", e.mProfile.GetProfilePictureUri (800, 800).ToString());
				getDatos();

			} else {
			//Es nulo, posiblemente el usuario salió 
				Log.Debug (tag, "El usuario há salido. No cachamos nada!");
			}
				//CATCH
			}catch(Exception ex){
				Log.Debug (tag, "Ocurrió una excepción más grande. Posiblemente no hay internet: "+ex);
				Toast.MakeText (this, "Ocurrió un error. Inténtalo nuevamente más tarde.", ToastLength.Long).Show();
			}
		}

		public void OnCancel(){
			Log.Debug("OnCanel", "Se Canceló :v");
		}

		public void OnSuccess(Java.Lang.Object respuesta){
			Log.Debug("OnSuccess", "Si pasó D:!!!");
			LoginResult loginresult = respuesta as LoginResult;
			Log.Debug("OnSuccess", "UserId de AccessToken:"+loginresult.AccessToken.UserId);
			//loginresult
		}

		public void OnError(FacebookException ex){
			Log.Debug("OnError", "Se erró :c "+ex.ToString());
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data){
			base.OnActivityResult (requestCode, resultCode, data);
			respuesta.OnActivityResult (requestCode, (int)resultCode, data);
		}

		protected override void OnDestroy(){
			mProfileTracker.StopTracking ();
			base.OnDestroy ();
		}

		//Se acaba todo el desmadre de métodos de facebook y empieza el oncreate
		protected override void OnCreate (Bundle bundle)
		{
						
			//LE QUITAMOS EL TÍTULO PRIMERO
			RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (bundle);
			FacebookSdk.SdkInitialize(Application.Context);
			mProfileTracker = new MyProfileTracker ();
			mProfileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
			mProfileTracker.StartTracking ();



			//Asignamos los contenedores de facebook
			infoface = FindViewById<LinearLayout> (Resource.Id.infoface);



			//LE INDICAMOS EL LAYOUT A CONTROLAR
			SetContentView (Resource.Layout.login);

			//CREAMOS LA ANIMACION
			bounce = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.bounce);
			sdown = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.slidedown);
			fadein = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.fadein);

			//CREAMOS LA REFERENCIA AL BOTÓN DE FACEBOOK
			LoginButton connectWithFbButton = FindViewById<LoginButton> (Resource.Id.connectWithFbButton);

			//LE DAMOS LOS PERMISOS DE FACEBOOK AL BOTÓN DE FACEBOOK PARA QUE RECIBA COSAS!

			List<String> lista  = new List<string> () {"public_profile", "email", "user_birthday", "user_friends"};
			connectWithFbButton.SetReadPermissions (lista);

			respuesta = CallbackManagerFactory.Create ();
			connectWithFbButton.RegisterCallback (respuesta, this);

			datosfb = new Dictionary<string,string> ();

			string logout=Intent.GetStringExtra ("logout");

			if (logout != null) {
				//Hay que cerrar la sesión de facebook
				Log.Debug ("CERRARSESIONFB", "CERAMOS LA SESION DE FACEBOOK!!!");

				try{
				LoginManager.Instance.LogOut ();
				}catch(Exception ex){
					Log.Debug ("CERRARSESIONFB", "Lel, no había sesión :v");
				}

			}else{
				Log.Debug ("CERRARSESIONFB", "No hay nada, no hacemos nada con la sesión");
			}


			//CREAMOS EL OBJETO PARA ACCEDER A LAS PREFERENCIAS
			var prefs = this.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);

			Button iniciar = FindViewById<Button> (Resource.Id.button1);
			Button registro = FindViewById<Button> (Resource.Id.registro);
			//Button facebooklog = FindViewById<Button> (Resource.Id.facebooklog);

			//Toast.MakeText (this, "Inicia actividad Login!", ToastLength.Long).Show();
			var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar (toolbar);
			SupportActionBar.SetDisplayHomeAsUpEnabled (false);

			var collapsingToolbar = FindViewById<CollapsingToolbarLayout> (Resource.Id.collapsing_toolbar);
			collapsingToolbar.SetTitle ("");

			//EJECUTAMOS LA ANIMACION EN LA IMAGEN
			ImageView logoplif = FindViewById<ImageView> (Resource.Id.logoplif); 
			logoplif.StartAnimation(bounce);


			LinearLayout slogan = FindViewById<LinearLayout> (Resource.Id.slogan);
			slogan.StartAnimation (sdown);

			ImageView backdrop = FindViewById<ImageView> (Resource.Id.backdrop);
			backdrop.StartAnimation (fadein);

			LinearLayout logincont = FindViewById<LinearLayout> (Resource.Id.logincont);
			logincont.StartAnimation (bounce);

			registro.Click += (object sender, EventArgs e) => {

				StartActivity(typeof(Registro));

			/*	FragmentTransaction transaction = FragmentManager.BeginTransaction();
				RegistroPlif registrarse = new RegistroPlif();
				registrarse.Show(transaction, "dialog fragment");*/
				/*
				var fabeee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
				Snackbar
					.Make (fabeee, "Regístrate en plif.mx o ingresa con Facebook!", Snackbar.LengthLong)
					.SetAction ("Ok", (view) => {  })
					.Show ()*/
			};

			logoplif.Click += (object sender, EventArgs e) => {
				logoplif.StartAnimation(bounce);
			};

			//AQUI EMPIEZO A COPIAR TODO LO DEL CODIGO ANTERIOR
			iniciar.Click += async (sender, e) => {

				//EditText correo=null;
				//EditText pass=null;

				var correo = FindViewById<EditText> (Resource.Id.editText1);
				var pass = FindViewById<EditText> (Resource.Id.editText2);

				//FOR SAKE OF SIMPLICITY!!!!
				//correo.Text="lukario@live.com.mx";
				//pass.Text="xveemon1";


				if(correo.Text.ToString()=="" || pass.Text.ToString()==""){
				//	Toast.MakeText (this, "Por favor introduzca su correo y su contraseña", ToastLength.Long).Show();
					var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
					Snackbar
						.Make (fab, "Por favor introduce tus datos!", Snackbar.LengthLong)
						.SetAction ("Ok", (view) => { /*Undo message sending here.*/ })
						.Show ();
				}else{

					ProgressBar loader = FindViewById<ProgressBar> (Resource.Id.progressBar1);
					loader.Visibility=ViewStates.Visible;

					try{

						string url="http://plif.mx/mobile/g_usr_dta?user="+correo.Text.ToString()+"&pass="+pass.Text.ToString();
						// Fetch the weather information asynchronously, 
						// parse the results, then update the screen:
						JsonValue json = await FetchWeatherAsync (url);
						// ParseAndDisplay (json);
						JsonValue usuarios = json["u"];
						JsonValue imagenes;

						//JsonValue usuarios = preusuarios["users"];
						int res=int.Parse(usuarios["id"]);

						if(res==0){
							//Toast.MakeText (this, "El nombre de usuario no existe o la contraseña es incorrecta.", ToastLength.Long).Show();	
							var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
							Snackbar
								.Make (fab, "Tus datos son incorrectos o no has verificado tu cuenta aún.", Snackbar.LengthLong)
								.SetAction ("Ok", (view) => { /*Undo message sending here.*/ })
								.Show ();

						}else{
							//GUARDAMOS LA INFORMACION DEL USUARIO
							imagenes = json["i"];


							var editor = prefs.Edit ();

							string n=usuarios["nombre"];
							string ap=usuarios["apellidos"];

							editor.PutString ("id", usuarios["id"]);
							editor.PutString ("nombre", n+ap);
							editor.PutString ("email", correo.Text.ToString());
							editor.PutString ("img_perfil", imagenes["ruta"]);
							editor.Commit();

							//JA

							Toast.MakeText (this, "Inicio de sesión correcto", ToastLength.Long).Show();
							StartActivity(typeof(MainActivity));
							Finish();



						}


					}catch(Exception ex){
						//Toast.MakeText (this, "Ocurrió un error, Inténtalo de nuevo", ToastLength.Long).Show();
						Log.Debug("login", "el error fué: "+ex);
						var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						Snackbar
							.Make (fab, "Ocurrió un error. Inténtalo de nuevo!", Snackbar.LengthLong)
							.SetAction ("Ok", (view) => { /*Undo message sending here.*/ })
							.Show ();
					}
					loader.Visibility=ViewStates.Gone;


					//JUSTO AQUI ACABA

				}
			};
			//AQUI TERMINO DE COPIAR TODO LO DEL CODIGO ANTERIOR

			// Create your application here
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
					System.Console.Out.WriteLine("Response: {0}", jsonDoc.ToString ());

					// Return the JSON document:
					return jsonDoc;
				}
			}
		}
		//FIN ASYNC METHOD

		//Manejar la tecla de regreso
		public override void OnBackPressed(){
			//StartActivity(typeof(HomeActivity));
			Finish ();
		}

		public void showSoftKeyboard(View view) {
			if (view.RequestFocus()) {
				
				InputMethodManager imm = (InputMethodManager)
					GetSystemService (Context.InputMethodService);
					//getSystemService(Context.INPUT_METHOD_SERVICE);
				//imm.showSoftInput(view, InputMethodManager.SHOW_IMPLICIT);
				imm.ShowSoftInput (view, InputMethodManager.ShowImplicit);
			}
		}

	}

	public class MyProfileTracker : ProfileTracker	{

		public EventHandler<OnProfileChangedEventsArgs> mOnProfileChanged;


		protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile){

			if (mOnProfileChanged!=null) {
				mOnProfileChanged.Invoke (this, new OnProfileChangedEventsArgs(newProfile));
			}
			
		}

	}

	public class OnProfileChangedEventsArgs : EventArgs {
		public Profile mProfile;

		public OnProfileChangedEventsArgs(Profile profile){
			mProfile = profile;
		}
	}
}

