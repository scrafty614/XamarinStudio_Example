using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

using System.Collections.Generic;

//mias
using Java.Net;
using Android.Graphics;
//using Java.IO;
using Android.Graphics.Drawables;
using Android.Util;
using System.Net;
using System.IO;
using System.Drawing;

//ASYNC LIBS
using System.Threading.Tasks;
using System.Json;
using System.Collections;
using System.Collections.Specialized;

//FRAGMENTOS
using SupportFragment = Android.Support.V4.App.Fragment;



using System.Linq;
using System.Text;

using Android.Locations;



using Android.Views.Animations;
using Android.Webkit;

using System.Threading;

using Android.Content.PM;
using Java.Security;



namespace PlifToolbarMenu
{
	[Activity (Label = "PLIF", MainLauncher = true, Icon = "@drawable/btn_plif", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]
	public class MainActivity : AppCompatActivity,ILocationListener
	{
		private Android.Support.V7.Widget.Toolbar mToolbar;
		DrawerLayout drawerLayout;
		NavigationView navigationView;
		public FrameLayout mFragmentContainer;
		public SupportFragment mCurrentFragment;
		public Fragment1 mFragment1;
		public Fragment2 mFragment2;
		public Fragment3 mFragment3;
		public Fragment4 mFragment4;
		public Fragment5 mFragment5;
		public Stack<SupportFragment> mStackFragment;

		public Animation floatbounce;

		//GEOLOC
		public string main_longitud="000000";
		public string main_latitud="000000";
		LocationManager locMgr;
		string tag = "MainActivity";
		Bundle gpsclass;

		//IMAGENES
		//public JsonValue imagenes_array=null;
		//public JsonValue objeto=null;

	    Bitmap[] regionesimg = new Bitmap[4];

			
		protected override async void OnCreate (Bundle savedInstanceState) 
		{
			Log.Debug (tag, "Iniciamos!");
			//SETEAMOS EL NOMBRE DEL USUARIO

			base.OnCreate (savedInstanceState);

			Log.Debug (tag, "pasamos el oncreate base");

			//esto es exclusivamente para el gps
			gpsclass=new Bundle();

			gpsclass.PutString ("latitud", "nada");
			gpsclass.PutString ("longitud", "nada");

			Log.Debug (tag, "pasamos las longitudes");

			//end gps

			//CREAMOS EL OBJETO QUE VA A LEER/ESCRIBIR LAS PREFERENCIAS DEL USUARIO
			var prefs = this.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);

			//CREAMOS EL OBJETO DEL OBJETO (lel) que va a editar las preferencias
			var editor = prefs.Edit ();

			Log.Debug (tag, "pasamos el editor");



			//AQUI ES DONDE HAY QUE VER SI HACEMOS LOGIN:

			//CHECAMOS SI YA ESTÁ EL ID DEL USUARIO GUARDADO
			if (!prefs.Contains ("id")) {
				Log.Debug (tag, "(no hay id en las prefs)");
				//Toast.MakeText (this, "Ya están guardados tus datos", ToastLength.Long).Show ();
				//StartActivity(typeof(login));
				StartActivity(typeof(login));
				Finish ();
				//Toast.MakeText (this, "No hay login", ToastLength.Long).Show();
			} else {
							
			}

			Log.Debug (tag, "pasamos la condicion home");

			SetContentView (Resource.Layout.Main);
			Log.Debug (tag, "pasamos el set content view");


			mFragment1 = new Fragment1 ();
			mFragment2 = new Fragment2 ();
			mFragment3 = new Fragment3 ();
			mFragment4 = new Fragment4 ();
			mFragment5 = new Fragment5 ();
			Log.Debug (tag, "pasamos los fragments");

			//seteamos el arg del gps
			mFragment2.Arguments = gpsclass;

			mStackFragment = new Stack<SupportFragment> ();
			Log.Debug (tag, "pasamos el stack");

			var trans = SupportFragmentManager.BeginTransaction ();
			Log.Debug (tag, "pasamos el begin transaction");
			//AQUI LE MOVI IGUAL en vez de fragment2 y mfragment2 es 1
			trans.Add (Resource.Id.fragmentContainer, mFragment1,"Fragment1");
			trans.Commit();
			Log.Debug (tag, "pasamos el commit");

			//MOVI ESTO en vez de mfragment2 es 1
			mCurrentFragment = mFragment1;

			try{
			mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
				Log.Debug (tag, "pasamos el mtoolbar igual a");
			}catch(Exception ex){
				Log.Debug (tag, "El error fue:" + ex + "ACABA ERROR");
			}

			try{
			SetSupportActionBar (mToolbar);
				Log.Debug (tag, "pasamos el set support mtoolbar");
			}catch(Exception ex){
				Log.Debug (tag, "El error del mToolbar fue:" + ex + "ACABA ERROR");

			}
			//ESTO TAMBIEN es Inicio

			try{
			SupportActionBar.Title = "Inicio";
				Log.Debug (tag, "pasamos el title igual a inicio");
			}catch(Exception ex){
				Log.Debug (tag, "El error del action bar title fue:" + ex + "ACABA ERROR");
			}

			try{
				SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_menu);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetHomeButtonEnabled (true);
				Log.Debug (tag, "pasamos TODOS los support action bar");
			}
			catch(Exception ex){
				Log.Debug (tag, "El error del alguno de los suportactionbar fue:" + ex + "ACABA ERROR");
			}

			try{
			drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
				Log.Debug (tag, "pasamos asignar drawerlayout");
			}catch(Exception ex){
				Log.Debug (tag, "El error deasignar el drawer layout fue" + ex + "ACABA ERROR");
			}
			navigationView = FindViewById<NavigationView> (Resource.Id.nav_view);

			
			//AQUI ES DONDE CAMBIA SEGUN EL ELEMENTO DEL MENU QUE ELIJAS
			try{
			navigationView.NavigationItemSelected += (sender, e) =>
			{
				e.MenuItem.SetChecked (true);
				SupportActionBar.Title = e.MenuItem.TitleFormatted.ToString();
				drawerLayout.CloseDrawers ();
				if(e.MenuItem.TitleFormatted.ToString() == "Inicio")
				{
					ReplaceFragment (mFragment1);
				}
				else if(e.MenuItem.TitleFormatted.ToString() == "Buscar")
				{
					gpsclass.PutString ("region", "nada");
					ReplaceFragment (mFragment2);
				}
				else if(e.MenuItem.TitleFormatted.ToString() == "Últimos Negocios")
				{
					ReplaceFragment (mFragment3);
				}
				else if(e.MenuItem.TitleFormatted.ToString() == "Añadir Negocios")
				{
					//ReplaceFragment (mFragment4);
						//Toast.MakeText (this, "Próximamente...", ToastLength.Long).Show ();
						Log.Debug("Añadir Negocio","Hizo click!");
						var nuevoneg = new Intent (this, typeof(AgregarNegocio));
						/*enviarmsj.PutExtra("negocioid",idres);
						enviarmsj.PutExtra("titulo", titulores);
						enviarmsj.PutExtra("propietario",propietario);*/
						StartActivity (nuevoneg);
				}
				else if(e.MenuItem.TitleFormatted.ToString() == "Cerrar Sesión")
				{
					var Dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
					Dialog.SetTitle("Cerrar Sesión");
					Dialog.SetMessage("¿Desea cerrar la sesión?");
					Dialog.SetPositiveButton("Cerrar Sesión",delegate 
						{
							editor.Clear ();
							editor.Commit();
								//aqui metemos el stringextra logout
								var salir = new Intent (this, typeof(login));
								salir.PutExtra("logout","yes");
								StartActivity (salir);
							Finish ();	
						});
					Dialog.SetNegativeButton("Cancelar",delegate {});
					Dialog.Show();
					//No hay necesidad de esto
					//SupportActionBar.Title = "PLIF";
				}
			};

				Log.Debug (tag, "pasamos navigation item selected");

			}catch(Exception ex){
				Log.Debug (tag, "El error de navigationitemselected" + ex + "ACABA ERROR");
			}


			//Todo lo que no sea del Drawer se pone abajo

			TextView UserName = FindViewById<TextView> (Resource.Id.username);

			//PONEMOS EL NOMBRE DEL USUARIO
			var nombre = prefs.GetString("nombre", null);
			UserName.Text = nombre;

			//PONEMOS LA IMAGEN DEL USUARIO

			//recuperamos la ruta de las preferencias
			string ruta = "http://plif.mx/"+prefs.GetString ("img_perfil", null);  //+prefs.GetString ("id", null)
			ImageView imagen = FindViewById<ImageView> (Resource.Id.image_circle);
			try{

				JsonValue rutapre = await plifserver.FetchWeatherAsync ("http://plif.mx/mobile/get_img_usr?id="+prefs.GetString ("id", null));
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
				//rutafoto=path;
				Koush.UrlImageViewHelper.SetUrlDrawable(imagen, path, Resource.Drawable.profile);
				//hayimagen="si";

				//////////////////////
			/*
				JsonValue rutapre = await plifserver.FetchWeatherAsync ("http://plif.mx/mobile/get_img_usr?id="+prefs.GetString ("id", null));
				Log.Debug ("json","SI existe la ruta");
				string path = "http://plif.mx/"+rutapre [0] ["imagen_usuarios"] ["ruta"];
				Koush.UrlImageViewHelper.SetUrlDrawable(imagen, path, Resource.Drawable.profile);*/
			}catch(Exception ex){
				Log.Debug ("json","no existe la ruta");
				imagen.SetImageResource (Resource.Drawable.profile);
			}
			
			//creamos el objeto de referencia al imageview


			//CREAMOS LA ANIMACION
			floatbounce = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.floatbounce);


			imagen.Click += (object sender, EventArgs e) => {
				drawerLayout.CloseDrawers ();
				StartActivity(typeof(profile));

			};

			UserName.Click += (sender, e) =>  
			{	
				drawerLayout.CloseDrawers ();
				StartActivity(typeof(profile));


			};

			//Todo lo que no sea del Drawer se pone arriba
		}



		public void BusquedaRegion(string id){
			Log.Debug (tag, "Abre Fragmento de Busqueda: "+id);

			gpsclass.PutString ("region", id);

			ReplaceFragment (mFragment2);
			}

		public void VerNegocio(string id){
			Log.Debug (tag, "Abre Fragmento de negocio: "+id);
			ReplaceFragment (mFragment5);
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					drawerLayout.OpenDrawer (Android.Support.V4.View.GravityCompat.Start);
					return true;
			}
			return base.OnOptionsItemSelected (item);
		}

		public void ReplaceFragment (SupportFragment fragment)
		{
			if (fragment.IsVisible) 
			{
				return;
			}

			var trans = SupportFragmentManager.BeginTransaction ();
			trans.Replace (Resource.Id.fragmentContainer, fragment);
			trans.AddToBackStack (null);
			trans.Commit ();
			mCurrentFragment = fragment;			
		}

		public override void OnBackPressed ()
		{
			base.OnBackPressed ();
		}

		//INICIAN METODOS GEO

		//ON START
		protected override void OnStart ()
		{
			base.OnStart ();
			Log.Debug (tag, "OnStart called");
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			Log.Debug (tag, "SALVANDO ESTADO DE LA BUSQUEDA!");
			base.OnSaveInstanceState (outState);
		}

		//ON RESUME
		protected override void OnResume ()
		{
			base.OnResume (); 
			Log.Debug (tag, "OnResume called");

			// initialize location manager
			locMgr = GetSystemService (Context.LocationService) as LocationManager;

				var locationCriteria = new Criteria();

				locationCriteria.Accuracy = Accuracy.Coarse;
				locationCriteria.PowerRequirement = Power.Medium;

				string locationProvider = locMgr.GetBestProvider(locationCriteria, true);
				string prov=locationProvider.ToString();

				if(prov=="passive"){
					Log.Debug(tag, "Están deshabilitados los proveedores de ubicación: " + locationProvider.ToString());
					Toast.MakeText (this, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show ();
				gpsclass.PutString ("latitud", "nogps");
				gpsclass.PutString ("longitud", "nogps");
				}else{
					Log.Debug(tag, "Starting location updates with " + locationProvider.ToString());
					locMgr.RequestLocationUpdates (locationProvider, 2000, 0, this);
				}

			//Last Known Location
			Log.Debug(tag, "Se obtiene la ultima localizacion conocida");

			//locMgr.GetLastKnownLocation(locationProvider)

			try{
			Log.Debug ("LastKnownLocation","Latitud: "+locMgr.GetLastKnownLocation(locationProvider).Latitude+" Longitud: "+locMgr.GetLastKnownLocation(locationProvider).Longitude);
			}catch(NullReferenceException ex){
			Log.Debug ("LastKnownLocation","No hay LastLocation disponible");
			}
		}

		//ON PAUSE
		protected override void OnPause ()
		{
			base.OnPause ();
			// stop sending location updates when the application goes into the background
			// to learn about updating location in the background, refer to the Backgrounding guide
			// http://docs.xamarin.com/guides/cross-platform/application_fundamentals/backgrounding/
			// RemoveUpdates takes a pending intent - here, we pass the current Activity
			locMgr.RemoveUpdates (this);
			Log.Debug (tag, "Location updates paused because application is entering the background");
		}

		//ON STOP

		protected override void OnStop ()
		{
			base.OnStop ();
			Log.Debug (tag, "OnStop called");
		}

		public void OnLocationChanged (Android.Locations.Location location)
		{
			Log.Debug (tag, "Location changed");

			main_latitud=location.Latitude.ToString();
			main_longitud=location.Longitude.ToString();

			gpsclass.PutString ("latitud", main_latitud);
			gpsclass.PutString ("longitud", main_longitud);

			Log.Debug (tag, "LATITUD: "+main_latitud+" LONGITUD: "+main_longitud);
			//Toast.MakeText (this, "LATITUD: "+main_latitud+" LONGITUD: "+main_longitud, ToastLength.Long).Show ();
		}

		public void OnProviderDisabled (string provider)
		{
			Log.Debug (tag, provider + " disabled by user");
			OnResume ();
		}
		public void OnProviderEnabled (string provider)
		{
			Log.Debug (tag, provider + " enabled by user");
			OnResume ();
		}
		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			Log.Debug (tag, provider + " availability has changed to " + status.ToString());
			OnResume ();
		}

		//TERMINAN METODOS GEO
	}

}


