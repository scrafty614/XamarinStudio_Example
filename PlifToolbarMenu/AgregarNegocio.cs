
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
using Java.Util;

using Android.Locations;


using System.Threading;
using System.Text.RegularExpressions;

namespace PlifToolbarMenu
{
	[Activity (Label = "", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]						
	public class AgregarNegocio : AppCompatActivity, ViewTreeObserver.IOnScrollChangedListener, ILocationListener
	{

		private const int FULLY_VISIBLE_AT = 2;
		private SupportToolBar mToolbar;
		private ScrollView mScrollView;
		private int mScreenHeight;
		public JsonValue categorias_array;
		private List<Categoria> mItems;
		JsonValue objeto;
		string cat_id="1";
		string geo_long="0.000000";
		string geo_lat="0.000000";
		string lat="";
		string lng="";
		TextView currenthour;
		Marker currentm;
		//STREAM PARA DECODIFICAR EL BITMAP
		System.IO.Stream stream;
		bool enviando=false;

		public MapFragment mapFrag;
		public GoogleMap map;
		LinearLayout FragmentMap;

		const int TIME_DIALOG_ID = 0;
		LocationManager locMgr;

		ProgressBar esperadatos;


		private int hour;
		private int minute;

		bool showinghorarios=false;

		//VARIABLE PARA SABER SI SE SELECCIONA DE LA CAMARA O DE LA GALERIA
		int source;

		//VARIABLE QUE CONTENDRÁ LA URI DE LA IMAGEN DESDE LA CAMARA
		Android.Net.Uri imgUri;
		string apath;

		public const int PickImageId = 1000;
		public const int PickCameraId = 1500;
		int imgcount=0;
		public Bitmap imagencomentario;

		List <byte[]> fossbytes; 



		//INICIAN VARIABLES DE VISTA
		EditText nombre,descripcion,sitioweb,email,telefono,pais,ciudad,colonia,calle,numero,codigopostal,referencias,facebook,twitter,whatsapp,tagsnegocio;
		TextView lunes_a,lunes_c,martes_a,martes_c,miercoles_a,miercoles_c,jueves_a,jueves_c,viernes_a,viernes_c,sabado_a,sabado_c, domingo_a,domingo_c;
		RadioGroup region, reservaciones, entregaadomicilio, parallevar,aceptatarjeta, ambientefamiliar, estacionamiento, nivelruido, alcohol, tienetv, tienemeseros;
		Button enviarnegocio,imhere;
		//TERMINAN VARIABLES DE VISTA


		Button deleteimgnegocio, imagennegocio, imagencamaranegocio, enviarimgneg;




		protected async override void OnCreate (Bundle savedInstanceState){
			base.OnCreate (savedInstanceState);
			var prefs = this.GetSharedPreferences ("RunningAssistant.preferences", FileCreationMode.Private);
			SetContentView (Resource.Layout.agregar_negocio);


			mToolbar = FindViewById<SupportToolBar> (Resource.Id.toolbar);
			mScrollView = FindViewById<ScrollView> (Resource.Id.scrollView);

			SetSupportActionBar (mToolbar);
			//ESTA ES LA FLECHA PARA ATRÁS
			SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_arrow_back);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			//SupportActionBar.SetHomeButtonEnabled (true);
			SupportActionBar.Title = "Añadir Negocio";

			mItems = new List<Categoria> ();
			fossbytes = new List<byte[]>();
			deleteimgnegocio = FindViewById<Button> (Resource.Id.deleteimgnegocio);

			esperadatos = FindViewById<ProgressBar> (Resource.Id.esperadatos);

			FragmentMap = FindViewById<LinearLayout> (Resource.Id.fragmentmap);
			mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
			map = mapFrag.Map;

			Point size = new Point ();
			Display display = WindowManager.DefaultDisplay;
			display.GetSize (size);
			mScreenHeight = size.Y;

			mToolbar.SetBackgroundColor (Color.Argb (255, 51, 150, 209));
			string tag="ASIGNACION";
			//Asignacion de variables
			try{
			Log.Debug(tag, "asigna");
			nombre = FindViewById<EditText> (Resource.Id.nombre);
				Log.Debug(tag, "asigna");
			descripcion = FindViewById<EditText> (Resource.Id.descripcion);
				Log.Debug(tag, "asigna");
			sitioweb = FindViewById<EditText> (Resource.Id.sitioweb);
				Log.Debug(tag, "asigna");
			email = FindViewById<EditText> (Resource.Id.email);
				Log.Debug(tag, "asigna");
			telefono = FindViewById<EditText> (Resource.Id.telefonoedit);
				Log.Debug(tag, "asigna");
			pais = FindViewById<EditText> (Resource.Id.pais);
				Log.Debug(tag, "asigna");
			ciudad = FindViewById<EditText> (Resource.Id.ciudad);
				Log.Debug(tag, "asigna");
			colonia = FindViewById<EditText> (Resource.Id.colonia);
				Log.Debug(tag, "asigna");
			calle = FindViewById<EditText> (Resource.Id.calle);
				Log.Debug(tag, "asigna");
			numero = FindViewById<EditText> (Resource.Id.numero);
				Log.Debug(tag, "asigna");
			codigopostal = FindViewById<EditText> (Resource.Id.codigopostal);
				Log.Debug(tag, "asigna");
			referencias = FindViewById<EditText> (Resource.Id.referencias);
				Log.Debug(tag, "asigna");
			facebook = FindViewById<EditText> (Resource.Id.facebook);
				Log.Debug(tag, "asignaeste");
			twitter = FindViewById<EditText> (Resource.Id.twitter);
				Log.Debug(tag, "asignaultimo");
			whatsapp = FindViewById<EditText> (Resource.Id.whatsapp);
				Log.Debug(tag, "asignaultimoultimo");
				tagsnegocio = FindViewById<EditText> (Resource.Id.tagsnegocio);
				Log.Debug(tag, "pasó");

				//<!-- Dias de la semana -->
				Log.Debug(tag, "asigna");
				lunes_a = FindViewById<TextView> (Resource.Id.lunes_a);
				Log.Debug(tag, "asigna");
				lunes_c = FindViewById<TextView> (Resource.Id.lunes_c);
				Log.Debug(tag, "asigna");
				martes_a = FindViewById<TextView> (Resource.Id.martes_a);
				Log.Debug(tag, "asigna");
				martes_c = FindViewById<TextView> (Resource.Id.martes_c);
				Log.Debug(tag, "asigna");
				miercoles_a = FindViewById<TextView> (Resource.Id.miercoles_a);
				Log.Debug(tag, "asigna");
				miercoles_c = FindViewById<TextView> (Resource.Id.miercoles_c);
				Log.Debug(tag, "asigna");
				jueves_a = FindViewById<TextView> (Resource.Id.jueves_a);
				Log.Debug(tag, "asigna");
				jueves_c = FindViewById<TextView> (Resource.Id.jueves_c);
				Log.Debug(tag, "asigna");
				viernes_a = FindViewById<TextView> (Resource.Id.viernes_a);
				Log.Debug(tag, "asigna");
				viernes_c = FindViewById<TextView> (Resource.Id.viernes_c);
				Log.Debug(tag, "asigna");
				sabado_a = FindViewById<TextView> (Resource.Id.sabado_a);
				Log.Debug(tag, "asigna");
				sabado_c = FindViewById<TextView> (Resource.Id.sabado_c);
				Log.Debug(tag, "asigna");
				domingo_a = FindViewById<TextView> (Resource.Id.domingo_a);
				Log.Debug(tag, "asigna");
				domingo_c = FindViewById<TextView> (Resource.Id.domingo_c);
				Log.Debug(tag, "asigna");

			//<!-- Radio Groups -->
				Log.Debug(tag, "asigna");
			region = FindViewById<RadioGroup> (Resource.Id.region);
				Log.Debug(tag, "asigna");
			reservaciones = FindViewById<RadioGroup> (Resource.Id.reservaciones);
				Log.Debug(tag, "asigna");
			entregaadomicilio = FindViewById<RadioGroup> (Resource.Id.entregaadomicilio);
				Log.Debug(tag, "asigna");
			parallevar = FindViewById<RadioGroup> (Resource.Id.parallevar);
				Log.Debug(tag, "asigna");
			aceptatarjeta = FindViewById<RadioGroup> (Resource.Id.aceptatarjeta);
				Log.Debug(tag, "asigna");
			ambientefamiliar = FindViewById<RadioGroup> (Resource.Id.ambientefamiliar);
				Log.Debug(tag, "asigna");
			estacionamiento = FindViewById<RadioGroup> (Resource.Id.estacionamiento);
				Log.Debug(tag, "asigna");
			nivelruido = FindViewById<RadioGroup> (Resource.Id.nivelruido);
				Log.Debug(tag, "asigna");
			alcohol = FindViewById<RadioGroup> (Resource.Id.alcohol);
				Log.Debug(tag, "asigna");
			tienetv = FindViewById<RadioGroup> (Resource.Id.tienetv);
				Log.Debug(tag, "asigna");
			tienemeseros = FindViewById<RadioGroup> (Resource.Id.tienemeserios);
				Log.Debug(tag, "asigna");
			enviarnegocio = FindViewById<Button> (Resource.Id.enviarnegociop);
				imhere=FindViewById<Button> (Resource.Id.imhere);


		}catch(Exception ex){
				Log.Debug (tag, "fuck: "+ex);
			}


			//Se llena el Spinner de las categorías		
			try{
				categorias_array = await plifserver.FetchWeatherAsync("http://plif.mx/mobile/get_cats_neg");
				objeto = categorias_array["respuesta"];
				Log.Debug("AgregarNegocio", "Antes del Foreach");
				foreach(JsonObject data in objeto){
					
					Log.Debug("AgregarNegocio", "Entra Foreach: "+data["categorias"]["id"]+" "+data["categorias"]["nombre"]);
					mItems.Add(new Categoria(){
						Id = data["categorias"]["id"],
						Nombre = data["categorias"]["nombre"]
					});

					Log.Debug("AgregarNegocio", "Sale de Foreach");

				}


			}catch(Exception ex){
				Toast.MakeText (Application.Context, "Ocurrió un error al recuperar las categorías", ToastLength.Long).Show ();
				Log.Debug ("Foreach","ERRORRRR!!! "+ex.ToString());
			}

			//Se asignan las categorias al spinnes
			Spinner categ = FindViewById<Spinner> (Resource.Id.categorias);
			MyNegociosAdapter adapter_c = new MyNegociosAdapter (Application.Context, mItems);
			categ.Adapter = adapter_c;

			//Se crea el event handler para las categorías
			try{
			categ.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (c_spinner_ItemSelected);
			}catch(Exception ex){
				Log.Debug ("Handler","Lel, error: "+ex.ToString());
			}

			//ya aqui hacemos todo el desmadre

			enviarnegocio = FindViewById<Button> (Resource.Id.enviarnegociop);

			if (enviarnegocio == null) {
				Log.Debug (tag, "es nulo D:");
			}

			hour = DateTime.Now.Hour;
			minute = DateTime.Now.Minute;
			//AQUI PARA PONER LAS HORAS!!!!!

			//INICIA LUNES
			lunes_a.InputType = Android.Text.InputTypes.Null;
			lunes_c.InputType = Android.Text.InputTypes.Null;

			Android.Support.V7.Widget.SwitchCompat lunes_ac = FindViewById<Android.Support.V7.Widget.SwitchCompat> (Resource.Id.lunes_ac);

			lunes_ac.Click += (object sender, EventArgs e) => {
				if(lunes_ac.Checked==true){
					lunes_a.Text="";
					lunes_a.Enabled=true;

					lunes_c.Text="";
					lunes_c.Enabled=true;
				}else{
					lunes_a.Text="Cerrado";
					lunes_a.Enabled=false;

					lunes_c.Text="Cerrado";
					lunes_c.Enabled=false;
				}
			};

			lunes_ac.PerformClick ();


			lunes_a.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó lunes a");
				currenthour=lunes_a;
				ShowDialog(TIME_DIALOG_ID);
			};

			lunes_c.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó lunes c");
				currenthour=lunes_c;
				ShowDialog(TIME_DIALOG_ID);
			};
			//TERMINA LUNES

			//INICIA martes
			martes_a.InputType = Android.Text.InputTypes.Null;
			martes_c.InputType = Android.Text.InputTypes.Null;

			Android.Support.V7.Widget.SwitchCompat martes_ac = FindViewById<Android.Support.V7.Widget.SwitchCompat> (Resource.Id.martes_ac);

			martes_ac.Click += (object sender, EventArgs e) => {
				if(martes_ac.Checked==true){
					martes_a.Text="";
					martes_a.Enabled=true;

					martes_c.Text="";
					martes_c.Enabled=true;
				}else{
					martes_a.Text="Cerrado";
					martes_a.Enabled=false;

					martes_c.Text="Cerrado";
					martes_c.Enabled=false;
				}
			};

			martes_ac.PerformClick ();


			martes_a.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó martes a");
				currenthour=martes_a;
				ShowDialog(TIME_DIALOG_ID);
			};

			martes_c.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó martes c");
				currenthour=martes_c;
				ShowDialog(TIME_DIALOG_ID);
			};
			//TERMINA martes

			//INICIA miercoles
			miercoles_a.InputType = Android.Text.InputTypes.Null;
			miercoles_c.InputType = Android.Text.InputTypes.Null;

			Android.Support.V7.Widget.SwitchCompat miercoles_ac = FindViewById<Android.Support.V7.Widget.SwitchCompat> (Resource.Id.miercoles_ac);

			miercoles_ac.Click += (object sender, EventArgs e) => {
				if(miercoles_ac.Checked==true){
					miercoles_a.Text="";
					miercoles_a.Enabled=true;

					miercoles_c.Text="";
					miercoles_c.Enabled=true;
				}else{
					miercoles_a.Text="Cerrado";
					miercoles_a.Enabled=false;

					miercoles_c.Text="Cerrado";
					miercoles_c.Enabled=false;
				}
			};

			miercoles_ac.PerformClick ();


			miercoles_a.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó miercoles a");
				currenthour=miercoles_a;
				ShowDialog(TIME_DIALOG_ID);
			};

			miercoles_c.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó miercoles c");
				currenthour=miercoles_c;
				ShowDialog(TIME_DIALOG_ID);
			};
			//TERMINA miercoles

			//INICIA jueves
			jueves_a.InputType = Android.Text.InputTypes.Null;
			jueves_c.InputType = Android.Text.InputTypes.Null;

			Android.Support.V7.Widget.SwitchCompat jueves_ac = FindViewById<Android.Support.V7.Widget.SwitchCompat> (Resource.Id.jueves_ac);

			jueves_ac.Click += (object sender, EventArgs e) => {
				if(jueves_ac.Checked==true){
					jueves_a.Text="";
					jueves_a.Enabled=true;

					jueves_c.Text="";
					jueves_c.Enabled=true;
				}else{
					jueves_a.Text="Cerrado";
					jueves_a.Enabled=false;

					jueves_c.Text="Cerrado";
					jueves_c.Enabled=false;
				}
			};

			jueves_ac.PerformClick ();


			jueves_a.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó jueves a");
				currenthour=jueves_a;
				ShowDialog(TIME_DIALOG_ID);
			};

			jueves_c.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó jueves c");
				currenthour=jueves_c;
				ShowDialog(TIME_DIALOG_ID);
			};
			//TERMINA jueves

			//INICIA viernes
			viernes_a.InputType = Android.Text.InputTypes.Null;
			viernes_c.InputType = Android.Text.InputTypes.Null;

			Android.Support.V7.Widget.SwitchCompat viernes_ac = FindViewById<Android.Support.V7.Widget.SwitchCompat> (Resource.Id.viernes_ac);

			viernes_ac.Click += (object sender, EventArgs e) => {
				if(viernes_ac.Checked==true){
					viernes_a.Text="";
					viernes_a.Enabled=true;

					viernes_c.Text="";
					viernes_c.Enabled=true;
				}else{
					viernes_a.Text="Cerrado";
					viernes_a.Enabled=false;

					viernes_c.Text="Cerrado";
					viernes_c.Enabled=false;
				}
			};

			viernes_ac.PerformClick ();


			viernes_a.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó viernes a");
				currenthour=viernes_a;
				ShowDialog(TIME_DIALOG_ID);
			};

			viernes_c.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó viernes c");
				currenthour=viernes_c;
				ShowDialog(TIME_DIALOG_ID);
			};
			//TERMINA viernes

			//INICIA sabado
			sabado_a.InputType = Android.Text.InputTypes.Null;
			sabado_c.InputType = Android.Text.InputTypes.Null;

			Android.Support.V7.Widget.SwitchCompat sabado_ac = FindViewById<Android.Support.V7.Widget.SwitchCompat> (Resource.Id.sabado_ac);

			sabado_ac.Click += (object sender, EventArgs e) => {
				if(sabado_ac.Checked==true){
					sabado_a.Text="";
					sabado_a.Enabled=true;

					sabado_c.Text="";
					sabado_c.Enabled=true;
				}else{
					sabado_a.Text="Cerrado";
					sabado_a.Enabled=false;

					sabado_c.Text="Cerrado";
					sabado_c.Enabled=false;
				}
			};

			sabado_ac.PerformClick ();


			sabado_a.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó sabado a");
				currenthour=sabado_a;
				ShowDialog(TIME_DIALOG_ID);
			};

			sabado_c.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó sabado c");
				currenthour=sabado_c;
				ShowDialog(TIME_DIALOG_ID);
			};
			//TERMINA sabado

			//INICIA domingo
			domingo_a.InputType = Android.Text.InputTypes.Null;
			domingo_c.InputType = Android.Text.InputTypes.Null;

			Android.Support.V7.Widget.SwitchCompat domingo_ac = FindViewById<Android.Support.V7.Widget.SwitchCompat> (Resource.Id.domingo_ac);

			domingo_ac.Click += (object sender, EventArgs e) => {
				if(domingo_ac.Checked==true){
					domingo_a.Text="";
					domingo_a.Enabled=true;

					domingo_c.Text="";
					domingo_c.Enabled=true;
				}else{
					domingo_a.Text="Cerrado";
					domingo_a.Enabled=false;

					domingo_c.Text="Cerrado";
					domingo_c.Enabled=false;
				}
			};

			domingo_ac.PerformClick ();


			domingo_a.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó domingo a");
				currenthour=domingo_a;
				ShowDialog(TIME_DIALOG_ID);
			};

			domingo_c.Click += (object sender, EventArgs e) => {
				Log.Debug(tag,"Clickeó domingo c");
				currenthour=domingo_c;
				ShowDialog(TIME_DIALOG_ID);
			};
			//TERMINA domingo








			//TERMINA LO DE PARA PONER LAS HORAS


			imhere.Click += async (object sender, EventArgs e) => {
				Log.Debug(tag, "I am here!!");

				if(lat =="" && lng ==""){
					//aun no hay geo
					Log.Debug(tag,"Aun no hay geo");
                    Toast.MakeText (Application.Context, "Aún estamos localizando tu ubicación. Inténtalo en un momento!", ToastLength.Long).Show ();

				}else{
					geo_lat=lat;
					geo_long=lng;

					Geocoder geocoder = new Geocoder(Application.Context, Java.Util.Locale.Default);

					IList<Address> lista;
					lista = await geocoder.GetFromLocationAsync(Convert.ToDouble(lat), Convert.ToDouble(lng), 10);

					Address address = lista.FirstOrDefault();

					if(address!=null){
						StringBuilder deviceAddress = new StringBuilder();
			
						//Obtener calle y número
						string[] lines = Regex.Split(address.GetAddressLine(0), " ");
						string calle="";
						string colonia=address.GetAddressLine(1);

						for(int j=0; j<=lines.Length-2; j++ ){
							calle=calle+lines[j]+" ";
						}

						string numero=lines[lines.Length-1];

						string codigopostal="";

						string[] cpciudad=Regex.Split(address.GetAddressLine(2), " ");

						codigopostal=cpciudad[0];

						string preciudad="";

						for(int k=1; k<=cpciudad.Length-1; k++){
							preciudad=preciudad+cpciudad[k];
						}

						string[] getciudad=Regex.Split(preciudad, ",");

						string ciudad=getciudad[0];

						string paiss="Mexico";

					//	Toast.MakeText (Application.Context, "La calle es:  "+calle, ToastLength.Long).Show ();
						FindViewById<EditText> (Resource.Id.calle).Text=calle;

					//	Toast.MakeText (Application.Context, "El número es:  "+numero, ToastLength.Long).Show ();
						FindViewById<EditText> (Resource.Id.numero).Text=numero;

					//	Toast.MakeText (Application.Context, "La Colonia es:  "+colonia, ToastLength.Long).Show ();
						FindViewById<EditText> (Resource.Id.colonia).Text=colonia;


					//	Toast.MakeText (Application.Context, "El Codigo Postal es:  "+codigopostal, ToastLength.Long).Show ();
						FindViewById<EditText> (Resource.Id.codigopostal).Text=codigopostal;


					//	Toast.MakeText (Application.Context, "La ciudad es:  "+ciudad, ToastLength.Long).Show ();
						FindViewById<EditText> (Resource.Id.codigopostal).Text=codigopostal;


					//	Toast.MakeText (Application.Context, "El País es:  "+paiss, ToastLength.Long).Show ();
						FindViewById<EditText> (Resource.Id.pais).Text=paiss;

					}

				}

			};

			Button replicar = FindViewById<Button> (Resource.Id.replicar);

			LinearLayout horarioscontainer = FindViewById<LinearLayout> (Resource.Id.horarioscontainer);
			TextView horariostext = FindViewById<TextView> (Resource.Id.horariostext);

			horariostext.Click += delegate {

				if(showinghorarios){
					horarioscontainer.Visibility=ViewStates.Gone;
					showinghorarios=false;
				}else{
					horarioscontainer.Visibility=ViewStates.Visible;
					showinghorarios=true;
				}
			};

			replicar.Click += delegate {
				string lunesa = lunes_a.Text; string lunesc = lunes_c.Text;

				martes_a.Text=lunesa;
				martes_c.Text=lunesc;

				miercoles_a.Text=lunesa;
				miercoles_c.Text=lunesc;

				jueves_a.Text=lunesa;
				jueves_c.Text=lunesc;

				viernes_a.Text=lunesa;
				viernes_c.Text=lunesc;

				sabado_a.Text=lunesa;
				sabado_c.Text=lunesc;

				domingo_a.Text=lunesa;
				domingo_c.Text=lunesc;



			};

			Log.Debug (tag,"Aquí iniciamos a poner los botones de las imagenes");

			GridLayout imgnegocioprev = FindViewById<GridLayout> (Resource.Id.imgnegocioprev);
			TextView masimagenesnegocio = FindViewById<TextView> (Resource.Id.masimagenesnegocio);

			//INICIAN BOTONES DE IMAGENES
			imagennegocio=FindViewById<Button> (Resource.Id.imagennegocio);

			imagennegocio.Click += async (object sender, EventArgs e) => {
				source=1;
				ProcesarImagenes(imgnegocioprev, 3, masimagenesnegocio);

			};

			Log.Debug (tag,"pasamos el de imagen negocio");

			imagencamaranegocio = FindViewById<Button> (Resource.Id.imagencamaranegocio);

			imagencamaranegocio.Click += async (object sender, EventArgs e) => {
				source=2;
				ProcesarImagenes(imgnegocioprev, 3, masimagenesnegocio);
			};
			//TERMINAN BOTONES DE IMAGENES
			Log.Debug (tag,"pasamos el de imagen camara negocio");

			deleteimgnegocio.Click += async (sender, e) => {
				//LinearLayout imgcomprev = FindViewById<LinearLayout> (Resource.Id.imgcomprev);
				//LinearLayout imgcontainercomprev = FindViewById<LinearLayout> (Resource.Id.imgcontainercomprev);
				Log.Debug("DELETEBUTTON","Inician las layouts");
				if(imgnegocioprev.ChildCount>0){
					imgnegocioprev.RemoveAllViews();
				}


				Log.Debug("DELETEBUTTON","vistas removidas");

				if(imagencomentario!=null){
					imagencomentario.Recycle();
					imagencomentario=null;
				}

				Log.Debug("DELETEBUTTON","reciclado");
				//deleteimgrev.Visibility=ViewStates.Gone;
				Log.Debug("DELETEBUTTON","se oculta boton borrar (deprecated)");
				//layoutdejaimagenes.Visibility=ViewStates.Gone;
				Log.Debug("DELETEBUTTON","se oculta el contenedor de subir imagenes");

				fossbytes.Clear();
				Log.Debug("DELETEBUTTON","El Arraylist de bytes se resetea");

				imgcount=0;
				Log.Debug("DELETEBUTTON","se resetea el contador de cuantas imagenes se subieron");
				masimagenesnegocio.Text="Carga 3 imágenes más!";
				Log.Debug("DELETEBUTTON","Reseteamos el texto de las imagenes");
			};


			enviarnegocio.Click += async (object sender, EventArgs e) => {
				Log.Debug("boton","hizoclick");

				if(nombre.Text == "" || descripcion.Text == "" || sitioweb.Text == "" || email.Text == "" || telefono.Text == "" || pais.Text == "" || ciudad.Text == "" || colonia.Text == "" || calle.Text == "" || numero.Text == "" || codigopostal.Text == "" || referencias.Text == "" || facebook.Text == "" || twitter.Text == "" || whatsapp.Text=="" || tagsnegocio.Text==""){
					Toast.MakeText (Application.Context, "Te faltan algunos campos de llenar, por favor revísalos!", ToastLength.Long).Show ();
				}else{
				//Información completa.
					//http://plif.mx/AgregarNegociop?droid

					//Aqui nos quedamos. Hay que añadir también
					enviando=true;
					enviarnegocio.Visibility=ViewStates.Gone;
					ProgressBar enviandonegocio= FindViewById<ProgressBar> (Resource.Id.enviandonegocio);
					enviandonegocio.Visibility=ViewStates.Visible;

					Dictionary<string, string> diccionario = new Dictionary<string, string>();
					diccionario.Add("nombre_completo", prefs.GetString("nombre", null)); 
					diccionario.Add("email_user", prefs.GetString("email", null)); 
					diccionario.Add("id_user", prefs.GetString("id",null));

					diccionario.Add("nombre",nombre.Text);
					diccionario.Add("desc",descripcion.Text);

					int selectedregion = region.CheckedRadioButtonId;
					RadioButton nameregion = FindViewById<RadioButton> (selectedregion);
					string nameregiontext=nameregion.Text;


					switch(nameregiontext){
					case "Durango":
						//Toast.MakeText (Application.Context, "Region Durango", ToastLength.Long).Show ();
						diccionario.Add("estado","9");
						break;

					case "Torreón":
						//Toast.MakeText (Application.Context, "Region Torreon", ToastLength.Long).Show ();
						diccionario.Add("estado","3");
						break;

					case "Mazatlán":
						//Toast.MakeText (Application.Context, "Region Mazatlan", ToastLength.Long).Show ();
						diccionario.Add("estado","2");
						break;

					case "Zacatecas":
						//Toast.MakeText (Application.Context, "Region Zacatecas", ToastLength.Long).Show ();
						diccionario.Add("estado","4");
						break;

						default:
						//Toast.MakeText (Application.Context, "None of the above", ToastLength.Long).Show ();
						diccionario.Add("estado","0");
						break;

					}//switch región

					diccionario.Add("categoria",cat_id);
					diccionario.Add("ubicacion",ciudad.Text);
					diccionario.Add("email",email.Text);
					diccionario.Add("sitioweb",sitioweb.Text);
					diccionario.Add("telefono",telefono.Text);
					diccionario.Add("facebook",facebook.Text);
					diccionario.Add("twitter",twitter.Text);
					diccionario.Add("google",""); //No hay campo para google+
					diccionario.Add("pais",pais.Text);
					//diccionario.Add("ciudad","Durango"); //Todos van a ser Durango ahorita
					diccionario.Add("ciudad",ciudad.Text);
					diccionario.Add("geo_long",geo_long);
					diccionario.Add("geo_lat",geo_lat);
					diccionario.Add("cp",codigopostal.Text);
					diccionario.Add("colonia",codigopostal.Text);
					diccionario.Add("calle",calle.Text);
					diccionario.Add("numero",numero.Text);
					diccionario.Add("tags",tagsnegocio.Text);
					diccionario.Add("sub_categoria","0"); //aun no hay campo de subcategoría
					diccionario.Add("referencias",referencias.Text);

					//AÑADIMOS AL DICCIONARIO LOS HORARIOS:
					diccionario.Add("lunes_de",lunes_a.Text);
					diccionario.Add("lunes_a",lunes_c.Text);
					diccionario.Add("martes_de",martes_a.Text);
					diccionario.Add("martes_a",martes_c.Text);
					diccionario.Add("miercoles_de",miercoles_a.Text);
					diccionario.Add("miercoles_a",miercoles_c.Text);
					diccionario.Add("jueves_de",jueves_a.Text);
					diccionario.Add("jueves_a",jueves_c.Text);
					diccionario.Add("viernes_de",viernes_a.Text);
					diccionario.Add("viernes_a",viernes_c.Text);
					diccionario.Add("sabado_de",sabado_a.Text);
					diccionario.Add("sabado_a",sabado_c.Text);
					diccionario.Add("domingo_de",domingo_a.Text);
					diccionario.Add("domingo_a",domingo_c.Text);

					//AÑADIMOS AL DICCIONARIO LA INFORMACION ADICIONAL

					//acepta reservaciones
					diccionario.Add("reservation",FindViewById<RadioButton> (reservaciones.CheckedRadioButtonId).Text);

					//entrega a domicilio
					diccionario.Add("delivery",FindViewById<RadioButton> (entregaadomicilio.CheckedRadioButtonId).Text);

					//para llevar
					diccionario.Add("take-out",FindViewById<RadioButton> (parallevar.CheckedRadioButtonId).Text);

					//acepta tarjeta
					diccionario.Add("credit",FindViewById<RadioButton> (aceptatarjeta.CheckedRadioButtonId).Text);

					//familiar
					diccionario.Add("kids",FindViewById<RadioButton> (ambientefamiliar.CheckedRadioButtonId).Text);

					//estacionamiento
					diccionario.Add("parking",FindViewById<RadioButton> (estacionamiento.CheckedRadioButtonId).Text);

					//ruido
					diccionario.Add("noise",FindViewById<RadioButton> (nivelruido.CheckedRadioButtonId).Text);

					//alcohol
					diccionario.Add("alcohol",FindViewById<RadioButton> (alcohol.CheckedRadioButtonId).Text);

					//tiene tv
					diccionario.Add("tv",FindViewById<RadioButton> (tienetv.CheckedRadioButtonId).Text);

					//tiene meseros
					diccionario.Add("meseros",FindViewById<RadioButton> (tienemeseros.CheckedRadioButtonId).Text);

					//tiene wifi
					diccionario.Add("wi-fi","No"); //aun no hay campo de wifi


					//LIIISTO!!! LOS MANDAMOS POR POST CON EL MULTIPART

					string resp = await plifserver.PostMultiPartForm ("http://plif.mx/AgregarNegociop?droid", fossbytes, "nada", "file[]", "image/jpeg", diccionario, false);
					Log.Debug(tag,"Termina RESP!!!");
					//Log.Debug(tag,"La respuesta es: "+resp);
					Toast.MakeText (Application.Context, "Tu negocio ha sido enviado con éxito y en breve será revisado. ¡Gracias por formar parte de Plif!", ToastLength.Long).Show ();
					Finish();


				}


			};


		}

		protected override Dialog OnCreateDialog (int id)
		{
			if (id == TIME_DIALOG_ID)
				return new TimePickerDialog (this, TimePickerCallback, hour, minute, false);

			return null;
		}

		private void TimePickerCallback (object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			hour = e.HourOfDay;
			minute = e.Minute;
			UpdateDisplay (currenthour);
		}

		// Updates the time we display in the TextView
		private void UpdateDisplay (TextView hourfinal)
		{
			string time = string.Format ("{0}:{1}", hour, minute.ToString ().PadLeft (2, '0'));
			hourfinal.Text = time;
			hourfinal = null;
		}

		//METODO SPINNER CATEGORIAS
		private void c_spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e){
			cat_id = mItems [e.Position].Id;
			Log.Debug ("CategoriasEventHandler","Cambió el ID de la categoría: "+cat_id);
		}

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
			//mToolbar.SetBackgroundColor(Color.Argb((int)(GetOpacity() * 255), 51, 150, 209));
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
			if (enviando) {
				Toast.MakeText (Application.Context, "Por favor espera a que la carga de tu negocio se haya completado", ToastLength.Short).Show ();

			} else {
				base.OnBackPressed ();
			}
		}


		public void OnLocationChanged (Android.Locations.Location location)
		{
			string tag= "OnLocationChanged";
			Log.Debug (tag, "Location changed");
			if (imhere.Enabled == false) {
				imhere.Enabled = true;
				imhere.Text="Estoy Aquí";
			}

			esperadatos.Visibility = ViewStates.Gone;

			if (FragmentMap.Visibility == ViewStates.Gone) {
				FragmentMap.Visibility = ViewStates.Visible;
			}

			lat = location.Latitude.ToString ();
			lng = location.Longitude.ToString ();

			LatLng location2 = new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng));
			CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
			builder.Target(location2);
			builder.Zoom(16);
			//builder.Bearing(155);
			//builder.Tilt(65);
			CameraPosition cameraPosition = builder.Build();
			CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);


			if (map != null)
			{
				map.MapType = GoogleMap.MapTypeNormal;
				MarkerOptions markerOpt1 = new MarkerOptions();
				markerOpt1.SetPosition(new LatLng(Convert.ToDouble(lat), Convert.ToDouble(lng)));
				markerOpt1.SetTitle("Aquí estoy!");
				markerOpt1.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pliflocation3));
				markerOpt1.Draggable(true);
				if (currentm != null) {
					currentm.Remove ();
				}
				currentm = null;
				currentm=map.AddMarker(markerOpt1);
				map.MoveCamera(cameraUpdate);

			}

			Log.Debug (tag, "LATITUD: "+lat+" LONGITUD: "+lng);
		}

		protected override void OnResume ()
		{
			base.OnResume (); 
			string tag="OnResume";
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
				//gpsclass.PutString ("latitud", "nogps");
				//gpsclass.PutString ("longitud", "nogps");
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

		public void OnProviderDisabled (string provider)
		{
					string tag="OnProviderDisabled";
			Log.Debug (tag, provider + " disabled by user");
			OnResume ();
		}
		public void OnProviderEnabled (string provider)
		{
			string tag="OnProviderEnabled";
			Log.Debug (tag, provider + " enabled by user");
			OnResume ();
		}
		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			string tag = "OnStatusChanged";
			Log.Debug (tag, provider + " availability has changed to " + status.ToString());
			OnResume ();
		}

		//INICIAN COSAS DE SELECCIONAR IMAGEN
		//--->INICIAN COSAS DE SELECCIONAR IMAGEN DE LA GALERIA<---//
		public delegate void OnImageResultHandler(bool success, List<string> imagePath);

		protected OnImageResultHandler _imagePickerCallback;

		public void GetImage(OnImageResultHandler callback)
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
				Intent.PutExtra (Intent.ExtraAllowMultiple, true);
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
			using (var cursor = ManagedQuery(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] {doc_id}, null))
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
				using (var cursor = ManagedQuery (Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null)) {
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
		//--->TERMINAN COSAS DE SELECCIONAR IMAGEN DE LA GALERIA<---//


		//--->INICIAN COSAS DE SELECCIONAR IMAGEN DE LA CAMARA<---//

		public Android.Net.Uri setImageUri() {
			// Store image in dcim
			DateTime rightnow = DateTime.Now;
			string fname = "/plifcap_" + rightnow.ToString ("MM-dd-yyyy_Hmmss") + ".jpg";
			Log.Debug ("Set Image Uri","Se crea la fecha y el filename!");

			Java.IO.File folder = new Java.IO.File (Android.OS.Environment.ExternalStorageDirectory + "/PlifCam");
			bool success;
			bool cfile=false;
			Android.Net.Uri imgUri=null;

			Java.IO.File file=null;

			if (!folder.Exists ()) {
				Log.Debug ("Set Image Uri","Crea el folder!");
				//aqui el folder no existe y lo creamos
				success=folder.Mkdir ();
				if (success) {
					Log.Debug ("Set Image Uri","Sip, lo creó correctamente");
					//el folder se creo correctamente!
					file = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/PlifCam/", fname);
					cfile = true;

				}else{
					//el folder no se creó, error.
				}

			} else {
				Log.Debug ("Set Image Uri","Ya existia el folder!");
				//aqui el folder si existe
				file = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/PlifCam/", fname);
				cfile = true;

			}

			if (cfile) {
				Log.Debug ("Set Image Uri", "Si lo creó!!!");

				Log.Debug ("Set Image Uri","Se crea el archivo");
				imgUri = Android.Net.Uri.FromFile (file);
				Log.Debug ("Set Image Uri","se obtiene la URI del archivo");

				if (imgUri == null) {
					Log.Debug ("Set Image Uri", "ESTA NULO DX");
				} else {
					Log.Debug ("Set Image Uri","Nope, no es nulo");
				}

				string rutacam = GetPathToImage (imgUri);
				Log.Debug ("Set Image Uri","Se obtiene el PATH");
				Log.Debug ("setImageUri", "El PATH obtenido es: "+rutacam);

			} else {
				Log.Debug ("Set Image Uri", "Nope, No lo hizo");
			}



			return imgUri;
		}
		//--->TERMINAN COSAS DE SELECCIONAR IMAGEN DE LA CAMARA<---//

		//INICIA PROCESAR IMAGENES
		public void ProcesarImagenes (GridLayout imgcomprev, int limite, TextView imgrestantes){

			if(imgcount<limite){
				GetImage(((b, p) => {
					int countimage = p.Count;
					Log.Debug("GetImage","Imagenes en el list: "+countimage);
					int imgcount2=imgcount+countimage;
					Log.Debug("GetImage","Imagenes totales: "+imgcount2);

					if(imgcount2>limite){
						//int total = limite-imgcount;
						var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						Snackbar
							.Make (fabee, "No puedes publicar mas de "+limite+" imágenes!", Snackbar.LengthLong)
							.SetAction ("Ok", (view) => {  })
							.Show ();	
					}else{
						//ya aqui se hace lo de poner cada imagen
						imgcount=imgcount2;
						Log.Debug("GetImage","Nuevo imgcount: "+imgcount);


						for(int i=0; i<countimage; i++){
							//filenameres= p[i].Substring (p[i].LastIndexOf ("/") + 1);
							//fileextres= filenameres.Substring (filenameres.LastIndexOf(".")+1);

							//Si por algún acaso la memoria no ha sido liberada, la liberamos primero.
							if (imagencomentario != null && !imagencomentario.IsRecycled) {
								imagencomentario.Recycle();
								imagencomentario = null; 
							}

							Java.IO.File f = new Java.IO.File(p[i]);
							stream = this.ContentResolver.OpenInputStream(Android.Net.Uri.FromFile(f));
							imagencomentario=BitmapFactory.DecodeStream(stream);
							//POR AHORA VAMOS A QUITAR ESTO PARA QUE LO HAGA MAS RAPIDO Y DEJAMOS QUE EL SERVIDOR SE ENCARGUE

							/*
							try{
								Android.Media.ExifInterface ei = new Android.Media.ExifInterface(p[i]);
								var orientation= ei.GetAttributeInt(Android.Media.ExifInterface.TagOrientation, -1);
								Log.Debug("GetImageCamera","El orientation es: "+orientation);

								if(orientation==1){
									//No hagas nada, si está bien la imagen. No tiene caso .-.
								}else{


								Log.Debug("Orientation", "Inicia Mutable");
								Bitmap mutable;
								Canvas cnv;
								Matrix matrix;
								Log.Debug("Orientation", "Termina Mutable");

								Log.Debug("Orientation", "Inicia Switch");
								switch(orientation){
								case 6:
									//90 grados
									mutable = imagencomentario.Copy(Bitmap.Config.Argb8888, true);
									cnv = new Canvas(mutable);
									matrix = new Matrix();
									Log.Debug("Orientation", "90 Grados");
									matrix.PostRotate(90);
									imagencomentario=Bitmap.CreateBitmap(mutable, 0, 0,  imagencomentario.Width, imagencomentario.Height, matrix, true);
									break;

								case 3:
									//180 grados
									mutable = imagencomentario.Copy(Bitmap.Config.Argb8888, true);
									cnv = new Canvas(mutable);
									matrix = new Matrix();
									Log.Debug("Orientation", "180 Grados");
									matrix.PostRotate(180);
									imagencomentario=Bitmap.CreateBitmap(mutable, 0, 0,  imagencomentario.Width, imagencomentario.Height, matrix, true);
									break;

								case 8:
									//270 grados
									mutable = imagencomentario.Copy(Bitmap.Config.Argb8888, true);
									cnv = new Canvas(mutable);
									matrix = new Matrix();
									Log.Debug("Orientation", "270 Grados");
									matrix.PostRotate(270);
									imagencomentario=Bitmap.CreateBitmap(mutable, 0, 0,  imagencomentario.Width, imagencomentario.Height, matrix, true);
									break;

								default:
									Log.Debug("Orientation", "0 Grados (0 360, como se quiera ver :P)");
									//0 grados
									//No hagas nada
									break;
								};

								Log.Debug("Orientation", "Termina Switch");
								}//ELSE orientation = 1
							}catch(Exception ex){
								Log.Debug("Orientation", "ERROR! Posiblemente no hay EXIF: "+ex);
							}
							*/

							if(imagencomentario!=null){
								//Toast.MakeText(this, "Si se creó el Bitmap!!!", ToastLength.Long).Show();
								//Lo añadimos a la lista
								//Bitmap tmp = imagencomentario;

								Log.Debug("FOSSBYTES","Inicia conversión a bytes!");
								byte[] tmp2 = PathToByte2(p[i]);
								Log.Debug("FOSSBYTES","Termina conversión a bytes!");
								fossbytes.Add(tmp2);
								//Toast.MakeText(this, "Elementos en lista: "+contenedorimagenes.Count, ToastLength.Long).Show();

								//aqui haremos el img


								//Creamos el imageview con sus parámetros
								ImageView prev = new ImageView(this);
								imgcomprev.AddView(prev);
								GridLayout.LayoutParams lp = new  GridLayout.LayoutParams();    
								lp.SetMargins(15,15,0,15);
								lp.Width=130;
								lp.Height=130;
								prev.LayoutParameters=lp;
								prev.SetScaleType(ImageView.ScaleType.CenterCrop);
								prev.SetImageBitmap(Bitmap.CreateScaledBitmap(imagencomentario, 175, 175, false));
								//prev.StartAnimation(bounce);





								//imgcount++;

								//Liberamos la memoria Inmediatamente!
								if (imagencomentario != null && !imagencomentario.IsRecycled) {
									imagencomentario.Recycle();
									imagencomentario = null; 
								}
								//Mala idea, esto causó que tronara .-. si lo voy a hacer pero cuando no tenga que usarlo

							}else{//por si algun acaso intenta procesar una ruta null
								var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
								Snackbar
									.Make (fabee, "Ocurrió un error. Por favor intenta con una imágen diferente", Snackbar.LengthLong)
									.SetAction ("Ok", (view) => {  })
									.Show ();	
								break;
							}

						}//TERMINA EL FOR PARA CADA IMAGEN 


						int restantes=limite-imgcount;
						if(restantes==0){
							imgrestantes.Text="¡Excelente!";
						}else{
							imgrestantes.Text="Puedes cargar "+restantes+" imágenes más!";
						}



					}//TERMINA EL ELSE DE COMPROBAR SI HAY MAS IMAGENES DE LAS QUE SE PUEDEN CARGAR

				}));//GETIMAGE

			}else{

				var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
				Snackbar
					.Make (fabee, "Solo puedes subir hasta 3 imágenes!", Snackbar.LengthLong)
					.SetAction ("Ok", (view) => {  })
					.Show ();	

			}//ELSE COUNTIMAGES < 3

		}
		//TERMINA PROCESAR IMAGENES

		public static byte[] PathToByte2(string path){
			//byte[] pathData=null;
			Log.Debug ("ImageToByte", "Inicia path to byte");
			Log.Debug ("ImageToByte", "Se crea el nuevo file");
			var imgFile = new Java.IO.File(path);
			Log.Debug ("ImageToByte", "Se crea el nuevo stream");
			var stream = new Java.IO.FileInputStream(imgFile);
			Log.Debug ("ImageToByte", "Se Crea el archivo Byte");
			var bytes = new byte[imgFile.Length()];
			Log.Debug ("ImageToByte", "Se hace el Stream Read");
			stream.Read(bytes);
			stream.Close ();
			stream.Dispose ();
			return bytes;

		}


		//TERMINAN COSAS DE SELECCIONAR IMAGEN
	
	}
}

