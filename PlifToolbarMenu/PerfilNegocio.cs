
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
	[Activity (Label = "", Theme = "@style/MyTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]						
	public class PerfilNegocio : AppCompatActivity, ViewTreeObserver.IOnScrollChangedListener
	{

		private const int FULLY_VISIBLE_AT = 2;
		private SupportToolBar mToolbar;
		private ScrollView mScrollView;
		private int mScreenHeight;
		public string propietario;


		string tag="PerfilNegocio";
		JsonValue negocio;
		JsonValue objeto;
		JsonValue respremium;
		string extra = "http://plif.mx/admin/";
		ImageView portada;

		bool horariovisible=false;

		public MapFragment mapFrag;
		public GoogleMap map;
		public bool haslike=false;

		Animation bounce;
		Animation flip;
		Animation floatbounce;

		public int numlikes;


		private List<Review> revItems;
		JsonValue ReviewsObj;

		string userid;

		TextView fullfotos;


		public readonly String DOUBLE_BYTE_SPACE = "\u3000";

		//Objetos comentarios
		TextView corazonlike;
		bool processlike=false;

		//calificacions
		int calificacion=0;

		//Datos POST
		Dictionary<string, string> diccionario; 
		ContentValues valores;

		//Seleccionar imagen para la reseña;
		public const int PickImageId = 1000;
		public const int PickCameraId = 1500;
		public Bitmap imagencomentario;
		string filenameres;
		string fileextres;

		//Boton para eliminar la imagen de la reseña
		Button deleteimgrev;

		//boton de agregar comentario
		Button addcomentbtn;

		//VARIABLES QUE CONTIENEN EL ID Y EL TITULO DEL NEGOCIO

		string titulores;
		string idres;
		int imgcount=0;
    	List <byte[]> fossbytes; 
		TextView masimagenes;

		EditText commentrev;



		//VARIABLE PARA SABER SI SE SELECCIONA DE LA CAMARA O DE LA GALERIA
		int source;

		//VARIABLE QUE CONTENDRÁ LA URI DE LA IMAGEN DESDE LA CAMARA
		Android.Net.Uri imgUri;
		string apath;

		//STREAM PARA DECODIFICAR EL BITMAP
		System.IO.Stream stream;

		//ESTO ES PARA CARGAR IMAGENES AL NEGOCIO!!!!
		LinearLayout layoutdejaimagenes; 
		GridLayout imgnegocioprev;
		Button deleteimgnegocio, imagennegocio, imagencamaranegocio, enviarimgneg;
		TextView masimagenesnegocio;

		string negocioid;




		protected override async void OnCreate (Bundle bundle)
		{
			//RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (bundle);

			//CREAMOS EL OBJETO QUE VA A LEER/ESCRIBIR LAS PREFERENCIAS DEL USUARIO
			var prefs = this.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);

			//ASIGNAMOS EL DICCIONARIO

			valores = new ContentValues ();
			source = 1;

			negocioid = Intent.GetStringExtra ("id");

			//CREAMOS EL OBJETO DEL OBJETO (lel) que va a editar las preferencias
			var editor = prefs.Edit ();
			userid = prefs.GetString("id", null);

			//SETEAMOS LA VISTA A CONTROLAR
			SetContentView(Resource.Layout.perfil_negocio);

			imgnegocioprev = FindViewById<GridLayout> (Resource.Id.imgnegocioprev);
			masimagenesnegocio = FindViewById<TextView> (Resource.Id.masimagenesnegocio);
			imagennegocio = FindViewById <Button> (Resource.Id.imagennegocio);
			imagencamaranegocio = FindViewById<Button> (Resource.Id.imagencamaranegocio);
			enviarimgneg = FindViewById<Button> (Resource.Id.enviarimgneg);
			LinearLayout containernegocio = FindViewById<LinearLayout> (Resource.Id.containernegocio);
			LinearLayout waitlayout = FindViewById <LinearLayout> (Resource.Id.waitlayout);


				

			mToolbar = FindViewById<SupportToolBar> (Resource.Id.toolbar);
			mScrollView = FindViewById<ScrollView> (Resource.Id.scrollView);

			SetSupportActionBar (mToolbar);
			//ESTA ES LA FLECHA PARA ATRÁS
			SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_arrow_back);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			//SupportActionBar.SetHomeButtonEnabled (true);

			layoutdejaimagenes = FindViewById<LinearLayout> (Resource.Id.layoutdejaimagenes);

			Point size = new Point ();
			Display display = WindowManager.DefaultDisplay;
			display.GetSize (size);
			mScreenHeight = size.Y;

			mScrollView.ViewTreeObserver.AddOnScrollChangedListener (this);

			//despues del show del action var, vamos a obtener el negocio

			SupportActionBar.Title = Intent.GetStringExtra("nombre");

			portada = FindViewById<ImageView> (Resource.Id.coverneg);

			TextView abiertocerrado = FindViewById<TextView>(Resource.Id.abiertocerrado);

			bounce = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.bounce);
			flip = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.flip);
			floatbounce = AnimationUtils.LoadAnimation (Application.Context, Resource.Animation.floatbounce);

			idres=Intent.GetStringExtra ("id");
			titulores = Intent.GetStringExtra ("nombre");

			//BOTONES DE RESEÑA
			Button enviarrev = FindViewById<Button> (Resource.Id.enviarrev);
			Button imagenrev = FindViewById<Button> (Resource.Id.imagenrev);
			Button imagenrev2 = FindViewById<Button> (Resource.Id.imagenrev2);
			Button imagencamara = FindViewById<Button> (Resource.Id.imagencamara);


			masimagenes = FindViewById<TextView> (Resource.Id.masimagenes);

			imgUri = null;
			apath = "";


			revItems = new List<Review>();

			//fuente
			Typeface font = Typeface.CreateFromAsset(Assets, "Fonts/fa.ttf");

			//COORDINATOR LAYOUT:
			var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);

			Button addimg = FindViewById<Button> (Resource.Id.addimg);
			Button addcomment = FindViewById<Button> (Resource.Id.addcomment);
			Button addtip = FindViewById<Button> (Resource.Id.addtip);
			Button enviarmensaje = FindViewById<Button> (Resource.Id.enviarmensaje);
			Button addlike = FindViewById<Button> (Resource.Id.addlike);
			fossbytes = new List<byte[]>();

			deleteimgnegocio = FindViewById<Button> (Resource.Id.deleteimgnegocio);

			addimg.SetTypeface(font, TypefaceStyle.Normal);
			addcomment.SetTypeface(font, TypefaceStyle.Normal);
			addtip.SetTypeface(font, TypefaceStyle.Normal);
			enviarmensaje.SetTypeface(font, TypefaceStyle.Normal);
			addlike.SetTypeface(font, TypefaceStyle.Normal);

			TextView corazonlikes = FindViewById<TextView> (Resource.Id.corazonlikes);

			TextView alikes = FindViewById<TextView> (Resource.Id.alikes);
			TextView likes = FindViewById<TextView> (Resource.Id.likes);
			TextView gustalikes = FindViewById<TextView> (Resource.Id.gustalikes);


			corazonlikes.SetTypeface(font, TypefaceStyle.Normal);
			alikes.SetTypeface(font, TypefaceStyle.Normal);
			likes.SetTypeface(font, TypefaceStyle.Normal);
			gustalikes.SetTypeface(font, TypefaceStyle.Normal);

			LinearLayout layoutdiascontainer = FindViewById<LinearLayout> (Resource.Id.layoutdiascontainer);
			//layoutdiascontainer.Animate().TranslationY(0);

			ScrollView scrollview = FindViewById<ScrollView> (Resource.Id.scrollView);

			ProgressBar waitpb = FindViewById<ProgressBar> (Resource.Id.waitpb);

			deleteimgrev = FindViewById<Button> (Resource.Id.deleteimgrev);


			try{
				negocio = await plifserver.FetchWeatherAsync ("http://plif.mx/mobile/get_negocio?id="+Intent.GetStringExtra("id")+"&uid="+userid);
				objeto = negocio["respuesta"][0]["Data"];


			    
				LinearLayout layoutpublicidad = FindViewById<LinearLayout> (Resource.Id.layoupublicidad);
				TextView negsimilares = FindViewById<TextView> (Resource.Id.negsimilares);

				string espremium;
				try{
				espremium=Intent.GetStringExtra("espremium");
				}catch{
					espremium=null;
				}

				if(espremium==null){
				try{
				//PRIMERO VAMOS A INFLAR LA PUBLICIDAD
				respremium = await plifserver.FetchWeatherAsync("http://plif.mx/negocio/"+Intent.GetStringExtra("id")+".json");
				LayoutInflater inflaterpub = LayoutInflater.From(this);
				View view = inflaterpub.Inflate(Resource.Layout.respremium_row, layoutpublicidad, false);

				Koush.UrlImageViewHelper.SetUrlDrawable (view.FindViewById<ImageView> (Resource.Id.NegocioFoto), respremium["i"]["ruta"], Resource.Drawable.dcover);
				TextView tituloprem=view.FindViewById<TextView> (Resource.Id.negocioname); 
				tituloprem.Text=respremium["n"]["titulo"];

				string cal=respremium["r"]["calificacion"];
				ImageView estrellasprem=view.FindViewById<ImageView> (Resource.Id.calificacion);

				switch(cal){
				case "0":
				case "":
				case null:
				case "null":
					estrellasprem.SetImageResource(Resource.Drawable.e0);
					break;

				case "1":
					estrellasprem.SetImageResource(Resource.Drawable.e1);
					break;

				case "2":
					estrellasprem.SetImageResource(Resource.Drawable.e2);
					break;

				case "3":
					estrellasprem.SetImageResource(Resource.Drawable.e3);
					break;

				case "4":
					estrellasprem.SetImageResource(Resource.Drawable.e4);
					break;

				case "5":
					estrellasprem.SetImageResource(Resource.Drawable.e5);
					break;
				}

				view.FindViewById<TextView> (Resource.Id.numcomentarios).Visibility=ViewStates.Gone;
				view.FindViewById<TextView> (Resource.Id.autor).Text=respremium["u"]["nombre"]+" escribió:";

				layoutpublicidad.AddView(view);

				tituloprem.Click += async (object sender, EventArgs e) => {
					Log.Debug(tag,"CLICKEAMOS EN EL PREMIUM!! y su ID es: "+respremium["r"]["negocio_id"]);
					JsonValue negociopremium = await plifserver.FetchWeatherAsync ("http://plif.mx/mobile/get_negocio?id="+respremium["r"]["negocio_id"]+"&uid="+userid);


					string nidprem=respremium["r"]["negocio_id"];
					string titprem=respremium["n"]["titulo"];
					string negdirprem=negociopremium["respuesta"][0]["Data"]["geo_calle"];
					string catprem= negociopremium["respuesta"][0]["Data"]["categoria"];
					string calprem=respremium["r"]["calificacion"];

					var negociopremint = new Intent (this, typeof(PerfilNegocio));
					Log.Debug(tag,"LEL1");
					negociopremint.PutExtra("id",nidprem);
					Log.Debug(tag,"LEL2");
					negociopremint.PutExtra("nombre",titprem);
					Log.Debug(tag,"LEL3");
					negociopremint.PutExtra("direccion",negdirprem);
					Log.Debug(tag,"LEL4");
					negociopremint.PutExtra("categoria",catprem);
					Log.Debug(tag,"LEL5");
					negociopremint.PutExtra("calificacion",calprem);
						Log.Debug(tag,"LEL5.5");
						negociopremint.PutExtra("espremium","si");
					Log.Debug(tag,"LEL6");
				StartActivity (negociopremint);

				};
				}catch(Exception ex){
					Log.Debug(tag,"Algo salió mal o no hay publicidad para mostrar");
					negsimilares.Visibility=ViewStates.Gone;
					layoutpublicidad.Visibility=ViewStates.Gone;

				}
				}else{//else espremium diferente de null
					Log.Debug(tag,"El negocio es premium, no se hace nada de publicidad");
					negsimilares.Visibility=ViewStates.Gone;
					layoutpublicidad.Visibility=ViewStates.Gone;
				}
				//TERMINAMOS DE INFLAR LA PUBLICIDAD


		    //ya que la recuperamos, ponemos la imagen de la portada



				Log.Debug (tag, "La ruta es: "+objeto["ruta"]);



				if(negocio["respuesta"][0]["0"]["has_like"]=="1"){
					haslike=true;
				}else{
					haslike=false;
				}

				if (haslike) {
					addlike.Text = GetString (Resource.String.heart);
				} else {
					addlike.Text = GetString (Resource.String.heartempty);
				}

				numlikes=objeto["likes"];


				if (objeto["ruta"] == null || objeto["ruta"] == "" || objeto["ruta"] == "null") {
					//pon la imagen por defecto
					portada.SetImageResource (Resource.Drawable.dcover);
					Log.Debug (tag, "No hay ruta!");
				} else {
					//TENEMOS QUE VERIFICAR SI LA IMAGEN ES DE GOOGLE O DE NOSOTROS!!!
					string ruta=objeto["ruta"];
					string first=ruta[0].ToString();

					if(first=="u" || first=="U"){
						//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
						ruta=extra+ruta;
					}else{
						//no hagas nada, la imagen es de google
					}

					Koush.UrlImageViewHelper.SetUrlDrawable (portada, ruta, Resource.Drawable.dcover);

				}//TERMINA SI LA IMAGEN NO ES NULL

				//Ponemos el titulo
				TextView titulo = FindViewById<TextView> (Resource.Id.titulo);
				titulo.Text=Intent.GetStringExtra("nombre");

				//Ponemos las estrellas
				ImageView estrellas = FindViewById<ImageView> (Resource.Id.estrellas);

				switch (Intent.GetStringExtra("calificacion")) {

				case "0":
					estrellas.SetImageResource (Resource.Drawable.e0);
					break;

				case "":
					estrellas.SetImageResource (Resource.Drawable.e0);
					break;

				case "null":
					estrellas.SetImageResource (Resource.Drawable.e0);
					break;

				case null:
					estrellas.SetImageResource (Resource.Drawable.e0);
					break;

				case "1":
					estrellas.SetImageResource (Resource.Drawable.e1);
					break;

				case "2":
					estrellas.SetImageResource (Resource.Drawable.e2);
					break;

				case "3":
					estrellas.SetImageResource (Resource.Drawable.e3);
					break;

				case "4":
					estrellas.SetImageResource (Resource.Drawable.e4);
					break;

				case "5":
					estrellas.SetImageResource (Resource.Drawable.e5);
					break;



				default:
					estrellas.SetImageResource (Resource.Drawable.e0);
					break;
				}

				//categoria y subcategoria
				TextView categoria = FindViewById<TextView> (Resource.Id.categoria);
				TextView subcategoria = FindViewById<TextView> (Resource.Id.subcategoria);
				TextView guioncat = FindViewById<TextView> (Resource.Id.guioncat);

				categoria.Text=objeto["categoria"];

				if(objeto["propietario"]==null || objeto["propietario"]=="null"){
					propietario="0";
				}else{
					propietario=objeto["propietario"];
				}

				if(objeto["subcategoria"]=="" || objeto["subcategoria"]==null || objeto["subcategoria"]=="null"){
					subcategoria.Text="";
					guioncat.Text="";
				}else{
					subcategoria.Text=objeto["subcategoria"];
				}

				//likes
				likes.Text=objeto["likes"]+" Personas";

				//mapa?
				try{
				mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
				map = mapFrag.Map;

					LatLng location = new LatLng(objeto["geo_lat"], objeto["geo_long"]);
					CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
					builder.Target(location);
					builder.Zoom(16);
					//builder.Bearing(155);
					//builder.Tilt(65);
					CameraPosition cameraPosition = builder.Build();
					CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);



				if (map != null)
				{
				    map.MapType = GoogleMap.MapTypeNormal;
					
						MarkerOptions markerOpt1 = new MarkerOptions();
						markerOpt1.SetPosition(new LatLng(objeto["geo_lat"], objeto["geo_long"]));
						markerOpt1.SetTitle(objeto["titulo"]);
						markerOpt1.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pliflocation3));
						markerOpt1.Draggable(true);
						map.AddMarker(markerOpt1);
						map.MoveCamera(cameraUpdate);


					
					
				}
				}catch(Exception ex){
					Log.Debug (tag, "GMAPS ERROR:"+ex);
				}
			

				//direccion
				TextView callenum = FindViewById<TextView> (Resource.Id.callenum);
				callenum.Text=objeto["geo_calle"]+" "+objeto["geo_numero"];
				TextView colonia = FindViewById<TextView> (Resource.Id.colonia);
				colonia.Text=objeto["geo_colonia"];
				TextView regpais = FindViewById<TextView> (Resource.Id.regpais);
				regpais.Text=objeto["geo_estado"]+", "+objeto["geo_pais"];

				//acerca de este negocio
				TextView mapmarker1 = FindViewById<TextView> (Resource.Id.sobreeste);
				mapmarker1.SetTypeface(font, TypefaceStyle.Normal);

				TextView descripcion = FindViewById<TextView> (Resource.Id.descripcion);
				string desc=objeto["descripcion"];

				descripcion.Text=desc+DOUBLE_BYTE_SPACE;

				//telefono
				TextView webico =FindViewById<TextView> (Resource.Id.webico);
				TextView telico =FindViewById<TextView> (Resource.Id.telico);

				TextView telefono= FindViewById<TextView> (Resource.Id.telefono);
				telico.SetTypeface(font, TypefaceStyle.Normal);
				telefono.Text= objeto["telefono"]+" ";


				//webpage
				TextView web = FindViewById<TextView> (Resource.Id.pagweb);
				webico.SetTypeface(font, TypefaceStyle.Normal);
				web.Text=objeto["website"]+" ";

				//Horarios
				TextView horarios=FindViewById<TextView> (Resource.Id.horarios);
				horarios.SetTypeface(font, TypefaceStyle.Normal);

				abiertocerrado = FindViewById<TextView>(Resource.Id.abiertocerrado);

				LinearLayout lunescontainer = FindViewById<LinearLayout> (Resource.Id.lunescontainer);
				TextView lunes = FindViewById<TextView>(Resource.Id.lunes);
				TextView luneshora = FindViewById<TextView>(Resource.Id.luneshora);

				LinearLayout martescontainer = FindViewById<LinearLayout> (Resource.Id.martescontainer);
				TextView martes = FindViewById<TextView>(Resource.Id.martes);
				TextView marteshora = FindViewById<TextView>(Resource.Id.marteshora);

				LinearLayout miercolescontainer = FindViewById<LinearLayout> (Resource.Id.miercolescontainer);
				TextView miercoles = FindViewById<TextView>(Resource.Id.miercoles);
				TextView miercoleshora = FindViewById<TextView>(Resource.Id.miercoleshora);

				LinearLayout juevescontainer = FindViewById<LinearLayout> (Resource.Id.juevescontainer);
				TextView jueves = FindViewById<TextView>(Resource.Id.jueves);
				TextView jueveshora = FindViewById<TextView>(Resource.Id.jueveshora);

				LinearLayout viernescontainer = FindViewById<LinearLayout> (Resource.Id.viernescontainer);
				TextView viernes = FindViewById<TextView>(Resource.Id.viernes);
				TextView vierneshora = FindViewById<TextView>(Resource.Id.vierneshora);

				LinearLayout sabadocontainer = FindViewById<LinearLayout> (Resource.Id.sabadocontainer);
				TextView sabado = FindViewById<TextView>(Resource.Id.sabado);
				TextView sabadohora = FindViewById<TextView>(Resource.Id.sabadohora);

				LinearLayout domingocontainer = FindViewById<LinearLayout> (Resource.Id.domingocontainer);
				TextView domingo = FindViewById<TextView>(Resource.Id.domingo);
				TextView domingohora = FindViewById<TextView>(Resource.Id.domingohora);

				//poner los horarios donde corresponde
				if(objeto["lunes_a"]=="" || objeto["lunes_c"]==""){
					lunescontainer.Visibility=ViewStates.Gone;
				}else{
					if(objeto["lunes_a"]=="Cerrado" || objeto["lunes_c"]=="Cerrado"){
						luneshora.Text="Cerrado";
					}else{
						luneshora.Text=objeto["lunes_a"]+" - "+objeto["lunes_c"];
					}
						
				}


				if(objeto["martes_a"]=="" || objeto["martes_c"]==""){
					martescontainer.Visibility=ViewStates.Gone;
				}else{
					if(objeto["martes_a"]=="Cerrado" || objeto["martes_c"]=="Cerrado"){
						marteshora.Text="Cerrado";
					}else{
						marteshora.Text=objeto["martes_a"]+" - "+objeto["martes_c"];
					}

				}

				if(objeto["miercoles_a"]=="" || objeto["miercoles_c"]==""){
					miercolescontainer.Visibility=ViewStates.Gone;
				}else{
					if(objeto["miercoles_a"]=="Cerrado" || objeto["miercoles_c"]=="Cerrado"){
						miercoleshora.Text="Cerrado";
					}else{
						miercoleshora.Text=objeto["miercoles_a"]+" - "+objeto["miercoles_c"];
					}

				}

				if(objeto["jueves_a"]=="" || objeto["jueves_c"]==""){
					juevescontainer.Visibility=ViewStates.Gone;
				}else{
					if(objeto["jueves_a"]=="Cerrado" || objeto["jueves_c"]=="Cerrado"){
						jueveshora.Text="Cerrado";
					}else{
						jueveshora.Text=objeto["jueves_a"]+" - "+objeto["jueves_c"];
					}

				}

				if(objeto["viernes_a"]=="" || objeto["viernes_c"]==""){
					viernescontainer.Visibility=ViewStates.Gone;
				}else{
					if(objeto["viernes_a"]=="Cerrado" || objeto["viernes_c"]=="Cerrado"){
						vierneshora.Text="Cerrado";
					}else{
						vierneshora.Text=objeto["viernes_a"]+" - "+objeto["viernes_c"];
					}

				}

					if(objeto["sabado_a"]=="" || objeto["sabado_c"]==""){
						sabadocontainer.Visibility=ViewStates.Gone;
					}else{
						if(objeto["sabado_a"]=="Cerrado" || objeto["sabado_c"]=="Cerrado"){
							sabadohora.Text="Cerrado";
						}else{
							sabadohora.Text=objeto["sabado_a"]+" - "+objeto["sabado_c"];
						}

					}

					if(objeto["domingo_a"]=="" || objeto["domingo_c"]==""){
						domingocontainer.Visibility=ViewStates.Gone;
					}else{
						if(objeto["domingo_a"]=="Cerrado" || objeto["domingo_c"]=="Cerrado"){
							domingohora.Text="Cerrado";
						}else{
							domingohora.Text=objeto["domingo_a"]+" - "+objeto["domingo_c"];
						}

					}







				//vemos que dia es hoy y lo ponemos en verde
				DateTime localDate = DateTime.Now;
				int dia=(int)localDate.DayOfWeek; 
				//1 es lunes, 7 es domingo y así

				string hoy_abre="";
				string hoy_cierra="";

				switch(dia){
				case 1:
					lunes.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					luneshora.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					hoy_abre = objeto ["lunes_a"];
					hoy_cierra = objeto ["lunes_c"];
					break;

				case 2:
					martes.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					marteshora.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					hoy_abre = objeto ["martes_a"];
					hoy_cierra = objeto ["martes_c"];
					break;

				case 3:
					miercoles.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					miercoleshora.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					hoy_abre = objeto ["miercoles_a"];
					hoy_cierra = objeto ["miercoles_c"];
					break;

				case 4:
					jueves.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					jueveshora.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					hoy_abre = objeto ["jueves_a"];
					hoy_cierra = objeto ["jueves_c"];
					break;

				case 5:
					viernes.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					vierneshora.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					hoy_abre = objeto ["viernes_a"];
					hoy_cierra = objeto ["viernes_c"];
					break;

				case 6:
					sabado.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					sabadohora.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					hoy_abre = objeto ["sabado_a"];
					hoy_cierra = objeto ["sabado_c"];
					break;

				case 7:
					domingo.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					domingohora.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
					hoy_abre = objeto ["domingo_a"];
					hoy_cierra = objeto ["domingo_c"];
					break;

				default:
					hoy_abre = "cerrado";
					hoy_cierra = "cerrado";
					break;

				}

				if(hoy_abre == "Cerrado" || hoy_cierra=="Cerrado"){
					abiertocerrado.Text="Cerrado por el dia de hoy.";
					abiertocerrado.SetTextColor(Android.Graphics.Color.ParseColor("#C41200"));
					}else{

						if(hoy_abre == "" || hoy_cierra==""){
							abiertocerrado.Text="No disponemos de los horarios para el dia de hoy.";
							abiertocerrado.SetTextColor(Android.Graphics.Color.ParseColor("#C41200"));
						}else{
							//TODO AQUI
						//AQUI HAY QUE VER SI ESTÁ ABIERTO O CERRADO
						DateTime ahora = DateTime.Now;
						DateTime abre = Convert.ToDateTime(hoy_abre);
						DateTime cierra = Convert.ToDateTime(hoy_cierra);
						DateTime cierra2 = DateTime.Now;



						int diasig=DateTime.Compare(abre,cierra);
						//Toast.MakeText (Application.Context, "Cierra primero: "+cierra.ToString(), ToastLength.Long).Show ();

						if(diasig>0){
							//sumale un dia a la hora de cierre
							//Toast.MakeText (Application.Context, "si es mayor!: "+diasig, ToastLength.Long).Show ();
							cierra2=cierra.AddDays(1);
						}else{
							cierra2=cierra;
							//no hagas nada
						}

						int f1=DateTime.Compare(abre, ahora);
						int f2=DateTime.Compare(ahora,cierra2);

						if(f1<0 && f2<0){
							//abierto
							abiertocerrado.Text="Abierto justo ahora!";
							abiertocerrado.SetTextColor(Android.Graphics.Color.ParseColor("#2F8E2F"));
						}else{
							//cerrado
							abiertocerrado.Text="Cerrado ahora";
							abiertocerrado.SetTextColor(Android.Graphics.Color.ParseColor("#C41200"));
						}

						}

					}

				TextView revs_tit= FindViewById<TextView> (Resource.Id.revs_tit);
				revs_tit.SetTypeface(font, TypefaceStyle.Normal);
				containernegocio.Visibility=ViewStates.Visible;
				waitlayout.Visibility=ViewStates.Gone;






			}
			catch(Exception ex){
				Log.Debug (tag, "ERROR FETCHING DATA: "+ex);
				Toast.MakeText (Application.Context, "Ocurrió un error al recuperar la información del negocio", ToastLength.Long).Show ();
				Finish ();
			}

			//aqui en un nuevo try catch, cargamos los comentarios

			ProgressBar waitrv = FindViewById<ProgressBar> (Resource.Id.waitrv);
			try{

				ReviewsObj=await plifserver.FetchWeatherAsync("http://plif.mx/mobile/get_neg_reviews?nid="+Intent.GetStringExtra("id")+"&uid="+userid);
				//Aqui vamos a cargar los reviews con ayuda del adapter

				LinearLayout reviewscont = FindViewById<LinearLayout> (Resource.Id.reviewscont);
				LayoutInflater inflater = LayoutInflater.From(this);

				/*
				for(int i=0; i<=5; i++){
					View view = inflater.Inflate(Resource.Layout.listview_comentarios, reviewscont, false);
					TextView revlikes = view.FindViewById<TextView> (Resource.Id.user_corazonlikes);
					revlikes.SetTypeface(font, TypefaceStyle.Normal);
					reviewscont.AddView(view);
				
				
				}*/

				foreach(JsonObject data in ReviewsObj){
					View row = inflater.Inflate(Resource.Layout.listview_comentarios, reviewscont, false);
					ReviewDetalles holder=null;
					LinearDetalles preholder=null;
					/*TextView revlikes = view.FindViewById<TextView> (Resource.Id.user_corazonlikes);
					revlikes.SetTypeface(font, TypefaceStyle.Normal);*/
							

					//EMPIEZA
					//ESTE SETEA LA IMAGEN
					ImageView imagen = row.FindViewById<ImageView> (Resource.Id.user_imagen);

					if (data["i"]["ruta"] == null || data["i"]["ruta"] == "" || data["i"]["ruta"] == "null") {
						//pon la imagen por defecto
						imagen.SetImageResource (Resource.Drawable.noprof);
					} else {
						//TENEMOS QUE VERIFICAR SI LA IMAGEN ES DE GOOGLE O DE NOSOTROS!!!
						string extra="http://plif.mx/";
						string ruta=data["i"]["ruta"];
						string first=ruta[0].ToString();

						if(first=="u" || first=="U"){
							ruta=extra+ruta;
						}else{
							//no hagas nada, la imagen es de google
						}

						Koush.UrlImageViewHelper.SetUrlDrawable (imagen, ruta, Resource.Drawable.bolaplace);

					}//TERMINA SI LA IMAGEN NO ES NULA

					//Ponemos el nombre
					TextView nombre = row.FindViewById<TextView> (Resource.Id.user_nombre);
					nombre.Text = data["u"]["nombre"]+" "+data["u"]["apellidos"];

					//ESTE SETEA LAS ESTRELLAS DE LA CALIFICACION
					ImageView cali = row.FindViewById<ImageView> (Resource.Id.user_calificacion);

					string cal = data["cm"]["calificacion"];

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

					//Ponemos el comentario
					TextView comentario = row.FindViewById<TextView> (Resource.Id.user_comentario);
					comentario.Text = data["cm"]["comentario"];

					//Ponemos la fecha y la hora
					TextView fechahora = row.FindViewById<TextView> (Resource.Id.user_fechahora);
					string fc=data["cm"]["fecha"];
					DateTime dt = System.Convert.ToDateTime(fc);   
					fechahora.Text = dt.ToString ("dd/MM/yyyy H:mm");

					//ponemos los likes que tiene
					TextView likescom = row.FindViewById<TextView> (Resource.Id.user_numlikes);
					likescom.Text = " "+data["cm"]["num_likes"];
					bool haslike = false;
					//averiguamos si el usuario le ha dado like

					try{
					string ulk=" ";
						if(data["user_likes"]["user_likes"]!= null & data["user_likes"]["user_likes"]!= "null"){
					ulk=data["user_likes"]["user_likes"];
					string[] words;
					words = ulk.Split(' ');

					foreach (string word in words)
					{
						if (word == userid) {
							haslike = true;
						}
					}
						}else{
							//no hagas nada!
							Log.Debug("Split","Los likes son nulos");
						}
					}catch(Exception ex){
						Log.Debug("Split","Fue null: "+ex);
					}

					corazonlike = row.FindViewById<TextView> (Resource.Id.user_corazonlikes);
					string hl="";

					if (haslike) {
						corazonlike.Text = GetString(Resource.String.heart);
						hl="si";

					} else {
						corazonlike.Text = GetString (Resource.String.heartempty);
						hl="no";
					}

					//Typeface font = Typeface.CreateFromAsset(mContext.Assets, "Fonts/fa.ttf");
					corazonlike.SetTypeface(font, TypefaceStyle.Normal);





					holder = new ReviewDetalles(data["cm"]["id"],hl,likescom);

					corazonlike.SetTag(Resource.String.lel,holder);



					LinearLayout layoutlike = row.FindViewById<LinearLayout> (Resource.Id.layoutlike);
					preholder=new LinearDetalles(corazonlike);
					layoutlike.SetTag(Resource.String.lel,preholder);



					//TERMINA
					reviewscont.AddView(row);
				};



				/*
				 * 
				foreach(JsonObject data in ReviewsObj){
					revItems.Add(
						new Review(){
							ReviewId=data["cm"]["id"],
							ReviewnegId=data["cm"]["negocio_id"],
							ReviewNombre=data["u"]["nombre"]+" "+data["u"]["apellidos"],
							ReviewEmail=data["u"]["autor_email"],
							ReviewComentario=data["cm"]["comentario"],
							ReviewCalificacion=data["cm"]["calificacion"],
							ReviewFecha=data["cm"]["fecha"],
							ReviewLikes=data["cm"]["num_likes"],
							ReviewautorId=data["u"]["id"],
							ReviewRuta=data["i"]["ruta"],		
					        ReviewLikesUsers=data["user_likes"]["user_likes"]	
						}
					);

					Log.Debug("AñadirReviews","Añadida!");


				}
*/
				//MyReviewsAdapter rev_adapter = new MyReviewsAdapter(Application.Context, revItems);
				//ListView revslista = FindViewById<ListView> (Resource.Id.revslista);
				//revslista.Adapter=rev_adapter;
				waitrv.Visibility=ViewStates.Gone;
				//revslista.Visibility=ViewStates.Visible;



			}catch(Exception ex){
				Log.Debug("ErrorTodo", ex.ToString());
				waitrv.Visibility=ViewStates.Gone;
				//var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
				Snackbar
					.Make (fab, "Ooops! Ocurrió un error al recuperar las reseñas. Inténtalo nuevamente!", Snackbar.LengthLong)
					.SetAction ("Ok", (view) => {  })
					.Show ();				
			}

			try{

				//Traemos la galeria de imagenes
				TextView fotos_tit = FindViewById<TextView> (Resource.Id.fotos_tit);
				fotos_tit.SetTypeface(font, TypefaceStyle.Normal);

				Utils utils = new Utils(this);
				JsonValue contenedorimg = await utils.getFilePaths("http://plif.mx/mobile/get_img_neg?id="+Intent.GetStringExtra("id")+"&prev");

				LinearLayout fotosprevcontain = FindViewById<LinearLayout> (Resource.Id.fotosprevcontain);
				LayoutInflater inflater2 = LayoutInflater.From(this);

				string extra="http://plif.mx/admin/";
				string rutaa="";
				string first="";
				View rowimg = inflater2.Inflate(Resource.Layout.img_layout, fotosprevcontain, false);

				int numfotos=0;

				try{
					rutaa=contenedorimg[0]["imagenes"]["ruta"];
					first=rutaa[0].ToString();

					if(first=="u" || first=="U"){
						//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
						rutaa=extra+rutaa;
					}else{
						//no hagas nada, la imagen es de google
					}

					Log.Debug ("GaleriaAdapter", "Procesando: "+rutaa);

					ImageView cont = rowimg.FindViewById<ImageView> (Resource.Id.img_galeria1);
				    Koush.UrlImageViewHelper.SetUrlDrawable (cont, rutaa, Resource.Drawable.bola);
					numfotos++;
				}
				catch(Exception ex){
					ImageView cont = rowimg.FindViewById<ImageView> (Resource.Id.img_galeria1);
					cont.Visibility=ViewStates.Gone;
					Log.Debug("imgscont","no hay imagen uno");


				}

					////LA QUE SIGUE!!!
				try{
					rutaa=contenedorimg[1]["imagenes"]["ruta"];
					first=rutaa[0].ToString();
					if(first=="u" || first=="U"){
						//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
						rutaa=extra+rutaa;
					}else{
						//no hagas nada, la imagen es de google
					}

					Log.Debug ("GaleriaAdapter", "Procesando: "+rutaa);

					ImageView cont2 = rowimg.FindViewById<ImageView> (Resource.Id.img_galeria2);
					Koush.UrlImageViewHelper.SetUrlDrawable (cont2, rutaa, Resource.Drawable.bola);
					numfotos++;
				}
				catch(Exception ex){
					ImageView cont = rowimg.FindViewById<ImageView> (Resource.Id.img_galeria2);
					cont.Visibility=ViewStates.Gone;
					Log.Debug("imgscont","no hay imagen dos");

				}

					////LA ULTIMA!!!
				try{
					rutaa=contenedorimg[2]["imagenes"]["ruta"];
					first=rutaa[0].ToString();
					if(first=="u" || first=="U"){
						//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
						rutaa=extra+rutaa;
					}else{
						//no hagas nada, la imagen es de google
					}

					Log.Debug ("GaleriaAdapter", "Procesando: "+rutaa);

					ImageView cont3 = rowimg.FindViewById<ImageView> (Resource.Id.img_galeria3);
					Koush.UrlImageViewHelper.SetUrlDrawable (cont3, rutaa, Resource.Drawable.bola);
					numfotos++;
				}
			catch(Exception ex){
					ImageView cont = rowimg.FindViewById<ImageView> (Resource.Id.img_galeria3);
					cont.Visibility=ViewStates.Gone;
				Log.Debug("imgscont","no hay imagen tres");

			}



					fotosprevcontain.AddView(rowimg);

				fullfotos= FindViewById<TextView> (Resource.Id.fullfotos);

				if(numfotos>0){
					fullfotos.Visibility=ViewStates.Visible;
				}



				

			}catch(Exception ex){
				Log.Debug("ErrorLista", ex.ToString());
				var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
				Snackbar
					.Make (fabee, "Ooops! Ocurrió un error al recuperar las imágenes. Inténtalo nuevamente!", Snackbar.LengthLong)
					.SetAction ("Ok", (view) => {  })
					.Show ();	
			
			}

			EditText nombrerev = FindViewById<EditText> (Resource.Id.nombrerev);
			EditText emailrev = FindViewById<EditText> (Resource.Id.emailrev);
			commentrev = FindViewById<EditText> (Resource.Id.commentrev);

			nombrerev.Text=prefs.GetString("nombre", null);
			emailrev.Text=prefs.GetString("email", null);

			//nombrerev.EditableText = false;
			//emailrev.EditableText = false;
			nombrerev.Enabled = false;
			emailrev.Enabled = false;

			ImageButton e1 = FindViewById<ImageButton> (Resource.Id.estrella1);
			ImageButton e2 = FindViewById<ImageButton> (Resource.Id.estrella2);
			ImageButton e3 = FindViewById<ImageButton> (Resource.Id.estrella3);
			ImageButton e4 = FindViewById<ImageButton> (Resource.Id.estrella4);
			ImageButton e5 = FindViewById<ImageButton> (Resource.Id.estrella5);

			TextView califspan = FindViewById<TextView> (Resource.Id.califspan);

			addcomentbtn = FindViewById<Button> (Resource.Id.addcomment);

			enviarmensaje.Click += (object sender, EventArgs e) => {
				var enviarmsj = new Intent (this, typeof(EnviarMensaje));
				enviarmsj.PutExtra("negocioid",idres);
				enviarmsj.PutExtra("titulo", titulores);
				enviarmsj.PutExtra("propietario",propietario);
				StartActivity (enviarmsj);
			};


			addcomentbtn.Click += (sender, e) => {
				enviarrev.Focusable=true;
				enviarrev.FocusableInTouchMode=true;
				if(enviarrev.RequestFocus()){
					Log.Debug("BotonArribaReseña", "It dit it!");
					enviarrev.ClearFocus();
					enviarrev.FocusableInTouchMode=false;
				}

			};

			e1.Click += delegate(object sender, EventArgs e) {
				
				califspan.StartAnimation(flip);
				califspan.Text="1.0";
				calificacion=1;
				e1.SetImageResource(Resource.Drawable.estrellafull);
				e2.SetImageResource(Resource.Drawable.estrellaempty);
				e3.SetImageResource(Resource.Drawable.estrellaempty);
				e4.SetImageResource(Resource.Drawable.estrellaempty);
				e5.SetImageResource(Resource.Drawable.estrellaempty);
			};

			e2.Click += delegate(object sender, EventArgs e) {
				
				califspan.StartAnimation(flip);
				califspan.Text="2.0";
				calificacion=2;
				e1.SetImageResource(Resource.Drawable.estrellafull);
				e2.SetImageResource(Resource.Drawable.estrellafull);
				e3.SetImageResource(Resource.Drawable.estrellaempty);
				e4.SetImageResource(Resource.Drawable.estrellaempty);
				e5.SetImageResource(Resource.Drawable.estrellaempty);
			};

			e3.Click += delegate(object sender, EventArgs e) {
				
				califspan.StartAnimation(flip);
				califspan.Text="3.0";
				calificacion=3;
				e1.SetImageResource(Resource.Drawable.estrellafull);
				e2.SetImageResource(Resource.Drawable.estrellafull);
				e3.SetImageResource(Resource.Drawable.estrellafull);
				e4.SetImageResource(Resource.Drawable.estrellaempty);
				e5.SetImageResource(Resource.Drawable.estrellaempty);
			};

			e4.Click += delegate(object sender, EventArgs e) {
				
				califspan.StartAnimation(flip);
				califspan.Text="4.0";
				calificacion=4;
				e1.SetImageResource(Resource.Drawable.estrellafull);
				e2.SetImageResource(Resource.Drawable.estrellafull);
				e3.SetImageResource(Resource.Drawable.estrellafull);
				e4.SetImageResource(Resource.Drawable.estrellafull);
				e5.SetImageResource(Resource.Drawable.estrellaempty);
			};

			e5.Click += delegate(object sender, EventArgs e) {
				
				califspan.StartAnimation(flip);
				califspan.Text="5.0";
				calificacion=5;
				e1.SetImageResource(Resource.Drawable.estrellafull);
				e2.SetImageResource(Resource.Drawable.estrellafull);
				e3.SetImageResource(Resource.Drawable.estrellafull);
				e4.SetImageResource(Resource.Drawable.estrellafull);
				e5.SetImageResource(Resource.Drawable.estrellafull);
			};

	
			//nenenenenenene
			addimg.Click += async (object sender, EventArgs e) => {
				layoutdejaimagenes.Visibility=ViewStates.Visible;
			};

			deleteimgrev.Click += async (sender, e) => {
				GridLayout imgcomprev = FindViewById<GridLayout> (Resource.Id.imgcomprev);
				LinearLayout imgcontainercomprev = FindViewById<LinearLayout> (Resource.Id.imgcontainercomprev);
				Log.Debug("DELETEBUTTON","Inician las layouts");
					if(imgcomprev.ChildCount>0){
					imgcomprev.RemoveAllViews();
				}


				Log.Debug("DELETEBUTTON","vistas removidas");

				if(imagencomentario!=null){
				imagencomentario.Recycle();
				imagencomentario=null;
				}

				Log.Debug("DELETEBUTTON","reciclado");

				//deleteimgrev.Visibility=ViewStates.Gone;
				Log.Debug("DELETEBUTTON","se oculta boton borrar (deprecated)");
				imgcontainercomprev.Visibility=ViewStates.Gone;
				Log.Debug("DELETEBUTTON","se oculta el contenedor de subir imagenes");
				imagenrev = FindViewById<Button> (Resource.Id.imagenrev);
				imagenrev.Visibility=ViewStates.Visible;
				Log.Debug("DELETEBUTTON","se muestra el boton añadir");

				fossbytes.Clear();
				Log.Debug("DELETEBUTTON","El Arraylist de bytes se resetea");

				imgcount=0;
				Log.Debug("DELETEBUTTON","se resetea el contador de cuantas imagenes se subieron");
				masimagenes.Text="Carga 3 imágenes más!";
				Log.Debug("DELETEBUTTON","Reseteamos el texto de las imagenes");


			};

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
				layoutdejaimagenes.Visibility=ViewStates.Gone;
				Log.Debug("DELETEBUTTON","se oculta el contenedor de subir imagenes");
	
				fossbytes.Clear();
				Log.Debug("DELETEBUTTON","El Arraylist de bytes se resetea");

				imgcount=0;
				Log.Debug("DELETEBUTTON","se resetea el contador de cuantas imagenes se subieron");
				masimagenesnegocio.Text="Carga 10 imágenes más!";
				Log.Debug("DELETEBUTTON","Reseteamos el texto de las imagenes");
			};

			imagenrev.Click += async (object sender, EventArgs e) => {

				LinearLayout imgcontainercomprev = FindViewById<LinearLayout> (Resource.Id.imgcontainercomprev);
				imgcontainercomprev.Visibility=ViewStates.Visible;
				imagenrev.Visibility=ViewStates.Gone;
				
			};

			imagennegocio.Click += async (object sender, EventArgs e) => {
				source=1;
				ProcesarImagenes(imgnegocioprev, 10, masimagenesnegocio);
			};

			imagencamaranegocio.Click += async (object sender, EventArgs e) => {
				source=2;
				ProcesarImagenes(imgnegocioprev, 10, masimagenesnegocio);
			};

			imagenrev2.Click += async (object sender, EventArgs e) => {
				source=1;
				GridLayout imgcomprev = FindViewById<GridLayout> (Resource.Id.imgcomprev);
				ProcesarImagenes(imgcomprev,3,masimagenes);

			};//imagenrev click

			imagencamara.Click += async (sender, e) => {
				source=2;
				GridLayout imgcomprev = FindViewById<GridLayout> (Resource.Id.imgcomprev);
				ProcesarImagenes(imgcomprev,3,masimagenes);
			};

			enviarimgneg.Click += async (object sender, EventArgs e) => {
				if(imgcount>0){

					diccionario = new Dictionary<string, string>();
					diccionario.Add("negocio", idres); 
					diccionario.Add("description", "Foto en "+titulores); 
					diccionario.Add("autor", prefs.GetString("id", null));


					//LIMPIAMOS LA PANTALLA Y DESHABILITAMOS LOS BOTONES
					ProgressBar waitnegociors=FindViewById<ProgressBar>(Resource.Id.waitnegociors);
					waitnegociors.Visibility=ViewStates.Visible;

					TextView subiendofotos = FindViewById<TextView> (Resource.Id.subiendofotos);
					subiendofotos.Visibility=ViewStates.Visible;
					//deleteimgrev.PerformClick();

					//se deshabilitan ambos botones
					//enviarrev.Enabled=false;
					//imagenrev.Enabled=false;



					//LinearLayout imgcontainercomprev = FindViewById<LinearLayout> (Resource.Id.imgcontainercomprev); 
					layoutdejaimagenes.Visibility=ViewStates.Gone;
					Log.Debug("EnviarImagen","se oculta el contenedor de subir imagenes");
					imagenrev = FindViewById<Button> (Resource.Id.imagenrev);
					imagenrev.Visibility=ViewStates.Visible;
					Log.Debug("EnviarImagenes","se muestra el boton añadir");
					masimagenesnegocio.Text="Carga 10 imágenes más!";
					Log.Debug("EnviarImagenes","Reseteamos el texto de las imagenes");


					int countbytes=fossbytes.Count;
					if(fossbytes!=null && countbytes>0){
						Log.Debug ("EnviarImagenes", "Si hay imágenes byte en el array!");
						Log.Debug("CONVERTIR","Elementos convertidos a Byte: "+countbytes);

					}


					string resp = await plifserver.PostMultiPartForm ("http://plif.mx/pages/UploadImg/0", fossbytes, "nada", "file[]", "image/jpeg", diccionario, false);
					Log.Debug("SUBIRIMAGENES",resp);
					waitnegociors.Visibility=ViewStates.Gone;
					subiendofotos.Visibility=ViewStates.Gone;
					deleteimgnegocio.PerformClick();

					var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
					Snackbar
						.Make (fabee, "Hemos recibido tus fotos. Gracias por ser parte de Plif!", Snackbar.LengthLong)
						.SetAction ("Ok", (view) => {  })
						.Show ();	

				}else{
					var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
					Snackbar
						.Make (fabee, "Selecciona al menos una imágen!", Snackbar.LengthLong)
						.SetAction ("Ok", (view) => {  })
						.Show ();		
				}

			};

			enviarrev.Click += async (object sender, EventArgs e) => {
				if(calificacion==0){
					var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
					Snackbar
						.Make (fabee, "Por favor califica al negocio!", Snackbar.LengthLong)
						.SetAction ("Ok", (view) => {  })
						.Show ();	
				}else{
					if(commentrev.Text==""){
						var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						Snackbar
							.Make (fabee, "Por favor escribe un comentario!", Snackbar.LengthLong)
							.SetAction ("Ok", (view) => {  })
							.Show ();	
					}else{
						//Aqui mero!!!!



						diccionario = new Dictionary<string, string>();
						diccionario.Add("comentario", commentrev.Text); 
						diccionario.Add("autor", prefs.GetString("id", null));
						diccionario.Add("calificacion", calificacion.ToString());
						diccionario.Add("negocio", idres);
						diccionario.Add("titulo", titulores);
						Log.Debug("EnviarComentario","Clickeo");

						//LIMPIAMOS LA PANTALLA Y DESHABILITAMOS LOS BOTONES
						ProgressBar waitrs=FindViewById<ProgressBar>(Resource.Id.waitrs);
						waitrs.Visibility=ViewStates.Visible;
						//deleteimgrev.PerformClick();

						//se deshabilitan ambos botones
						enviarrev.Enabled=false;
						imagenrev.Enabled=false;

						LinearLayout imgcontainercomprev = FindViewById<LinearLayout> (Resource.Id.imgcontainercomprev); 
						imgcontainercomprev.Visibility=ViewStates.Gone;
						Log.Debug("EnviarComentario","se oculta el contenedor de subir imagenes");
						imagenrev = FindViewById<Button> (Resource.Id.imagenrev);
						imagenrev.Visibility=ViewStates.Visible;
						Log.Debug("EnviarComentario","se muestra el boton añadir");
						masimagenes.Text="Carga 3 imágenes más!";
						Log.Debug("EnviarComentario","Reseteamos el texto de las imagenes");

						string filename="";
						int countbytes=fossbytes.Count;
						if(fossbytes!=null && countbytes>0){
						Log.Debug ("EnviarComentario", "Si hay imágenes byte en el array!");
						Log.Debug("CONVERTIR","Elementos convertidos a Byte: "+countbytes);

						}

						//AQUI LO ENVIAMOS, EL PRIMER PARAMETRO ES LA PÁGINA, EL SEGUNDO ES LA IMAGEN CODIFICADA EN BITS, EL TERCERO ES EL NOMBRE DEL PARÁMETRO QUE RECIBE PHP, EL CUARTO ES EL MIMETYPE DE LA IMAGEN, Y EL ULTIMO ES EL DICCTIONARY CON TODOS LOS DEMÁS VALORES STRING
						string resp = await plifserver.PostMultiPartForm ("http://plif.mx/comentario_negocio", fossbytes, filename, "file[]", "image/jpeg", diccionario, false);

						//deleteimgrev.PerformClick();
						enviarrev.Enabled=true;
						imagenrev.Enabled=true;
						waitrs.Visibility=ViewStates.Gone;
						calificacion=0;
						commentrev.Text="";

						califspan.StartAnimation(flip);
						califspan.Text="0.0";
						calificacion=0;
						e1.SetImageResource(Resource.Drawable.estrellaempty);
						e2.SetImageResource(Resource.Drawable.estrellaempty);
						e3.SetImageResource(Resource.Drawable.estrellaempty);
						e4.SetImageResource(Resource.Drawable.estrellaempty);
						e5.SetImageResource(Resource.Drawable.estrellaempty);

						Log.Debug("MULTIPARTRESPONSE",resp);
						deleteimgrev.PerformClick();

						var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						Snackbar
							.Make (fabee, "Tu reseña ha sido enviada. ¡Gracias por ser parte de Plif!", Snackbar.LengthLong)
							.SetAction ("Ok", (view) => {  })
							.Show ();	

					}
				}

			};


			fullfotos.Click += delegate(object sender, EventArgs e) {
				var galeriaimg = new Intent (this, typeof(GaleriaImagenes));
				galeriaimg.PutExtra("negocioid",idres);

				/*negocio.PutExtra("id",negItems [e.Position].NegocioId);
				negocio.PutExtra("nombre",negItems [e.Position].NegocioName);
				negocio.PutExtra("direccion",negItems [e.Position].NegocioDir);
				negocio.PutExtra("categoria",negItems [e.Position].NegocioCat);
				negocio.PutExtra("calificacion",negItems [e.Position].NegocioCal);*/
				StartActivity (galeriaimg);
				//StartActivity(typeof(GaleriaImagenes));
				/*
				var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
				Snackbar
					.Make (fabee, "El lunes se lo pongo!", Snackbar.LengthLong)
					.SetAction ("Ok", (view) => {  })
					.Show ();	
					*/
			};



			abiertocerrado.Click += delegate {
				if(!horariovisible){
					//layoutdiascontainer.Animate().TranslationY(300);
					layoutdiascontainer.Visibility=ViewStates.Visible;
					//int val=GetX(layoutdiascontainer);
					//scrollview.ScrollTo(0,val);
					horariovisible=true;
				}else{
					//layoutdiascontainer.Animate().TranslationY(0);
					layoutdiascontainer.Visibility=ViewStates.Gone;
					horariovisible=false;
				}


			};

			addlike.Click += async (sender, e) => {
				
				try{
				if(!haslike){
						waitpb.Visibility=ViewStates.Visible;	
					//var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						string likestring="http://plif.mx/pages/LikeNegocio/"+negocioid+"/1?uid="+userid;
						Log.Debug("Like","La URL Es: "+likestring);
						JsonValue likeans=await plifserver.FetchWeatherAsync("http://plif.mx/pages/LikeNegocio/"+negocioid+"/1?uid="+userid);					haslike=true;
					addlike.StartAnimation(bounce);
					addlike.Text = GetString (Resource.String.heart);
						numlikes++;
						likes.Text=numlikes+" Personas";
					waitpb.Visibility=ViewStates.Gone;

						try{
					Snackbar
						.Make (fab, "Te gusta este negocio!", Snackbar.LengthLong)
						.SetAction ("Deshacer", (view) => {
									addlike.PerformClick();
								})
						.Show ();
						}
						catch(Exception ex){
							Log.Debug("FAB","FUUUCK2");
						}
						
				}else{
				    waitpb.Visibility=ViewStates.Visible;	
					fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						string likestring="http://plif.mx/pages/LikeNegocio/"+negocioid+"/1?uid="+userid;
						Log.Debug("Like","La URL Es: "+likestring);
						JsonValue likeans=await plifserver.FetchWeatherAsync("http://plif.mx/pages/LikeNegocio/"+negocioid+"/2?uid="+userid);					haslike=false;
					addlike.StartAnimation(bounce);
					addlike.Text = GetString (Resource.String.heartempty);
						numlikes--;
						likes.Text=numlikes+" Personas";
						waitpb.Visibility=ViewStates.Gone;
					Snackbar
						.Make (fab, "Ya no te gusta este negocio.", Snackbar.LengthLong)
						.SetAction ("Deshacer", (view) => {
								addlike.PerformClick();
							})
						.Show ();
						
					
				}
				}catch(Exception ex){
					waitpb.Visibility=ViewStates.Gone;
					//var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
					Snackbar
						.Make (fab, "Ooops! Ocurrió un error al intentar procesar tu solicitud. Inténtalo nuevamente!", Snackbar.LengthLong)
						.SetAction ("Ok", (view) => {  })
						.Show ();
				}

			};

			//AQUIPONEMOS LAS ESTRELLAS
	
		}

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
									prev.StartAnimation(bounce);
									




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

		[Java.Interop.Export("layoutlikeclick")]
		public void layoutlikeclick(View v){
			Log.Debug ("Layoutclick","Hizo click el layout");

			LinearLayout ll = (LinearLayout)v;
			LinearDetalles cn = new LinearDetalles ();
			cn = ll.GetTag (Resource.String.lel) as LinearDetalles;

			cn.corazonlikes.PerformClick ();

		
		}

		[Java.Interop.Export("clickLike")]
		public async void clickLike(View v){
			
			if(!processlike){
			processlike = true;
			TextView contain = (TextView)v;
			ReviewDetalles rv = new ReviewDetalles();
			rv = contain.GetTag(Resource.String.lel) as ReviewDetalles;

			if (rv == null) {
				Log.Debug ("Metodo", "es nulo :c");
				var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
				Snackbar
					.Make (fab, "Ooops! Ocurrió un error al intentar procesar tu solicitud. Inténtalo nuevamente!", Snackbar.LengthLong)
					.SetAction ("Ok", (view) => {  })
					.Show ();
					processlike = false;

			} else {
				Log.Debug ("Metodo", "No es nulo D:"+rv.id+" "+rv.haslike);	
				string ruta="http://plif.mx/mobile/add_rmv_likecm?cid="+rv.id+"&uid="+userid+"&case=";
				TextView likestotal = rv.numlikes;

				if (rv.haslike == "no") {
					ruta = ruta + "1";
					contain.Text = GetString(Resource.String.heart); //Cambiamos inmediatamente el icono
					int newlikes=Int32.Parse(likestotal.Text);
					newlikes++;                                      //Y aumentamos inmediatamente el contador
					likestotal.Text = " "+newlikes;

					try{
						JsonValue res= await plifserver.FetchWeatherAsync(ruta);
						rv.haslike="si";
							processlike=false;
					}catch{
						contain.Text = GetString(Resource.String.heartempty); //lo regresamos si hay algun error
						newlikes--;                                      //Y aumentamos inmediatamente el contador
						likestotal.Text = " "+newlikes;
						rv.haslike="no";
						var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						Snackbar
							.Make (fab, "Ooops! Ocurrió un error al intentar procesar tu solicitud. Inténtalo nuevamente!", Snackbar.LengthLong)
							.SetAction ("Ok", (view) => {  })
							.Show ();
							processlike = false;
					}
				}else{
					ruta=ruta+"2";
					contain.Text = GetString(Resource.String.heartempty); //Cambiamos inmediatamente el icono
					int newlikes=Int32.Parse(likestotal.Text);
					newlikes--;                                            //Reducimos el contador de likes
					likestotal.Text = " "+newlikes;
				    try{
						JsonValue res= await plifserver.FetchWeatherAsync(ruta);
						rv.haslike="no";
							processlike=false;
					}catch{
						contain.Text = GetString(Resource.String.heart); //Cambiamos inmediatamente el icono
						newlikes++;                                            //Reducimos el contador de likes
						likestotal.Text = " "+newlikes;
						rv.haslike="si";
						var fab = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
						Snackbar
							.Make (fab, "Ooops! Ocurrió un error al intentar procesar tu solicitud. Inténtalo nuevamente!", Snackbar.LengthLong)
							.SetAction ("Ok", (view) => {  })
							.Show ();
							processlike = false;
					}
				
					
					}//else haslike == no

			
			
			}//else rv==null

			}//!processlike
			else{
				Log.Debug ("Metodo", "Like en proceso");	
			}//else !processlike

		}//fin metodo like comentario

						
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
			//ESTE ES EL QUE ME CIERRA TODO, CAMBIAR
			case Android.Resource.Id.Home:
				//StartActivity(typeof(MainActivity));
				Finish();
				return true;
			}
			return base.OnOptionsItemSelected (item);
		}
		public override void OnBackPressed ()
		{
			Rect scrollBounds = new Rect();
			ScrollView sv = FindViewById<ScrollView> (Resource.Id.scrollView);
			sv.GetHitRect(scrollBounds);
			if (commentrev.GetLocalVisibleRect(scrollBounds)) {
				Log.Debug (tag, "No salimos porque está visible");

				Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder (this);

				alert.SetTitle("Salir?");
				alert.SetIcon (Resource.Drawable.bola);
				alert.SetMessage ("Parece que te encuentras en la sección de Reseñas, pero aún no has enviado ninguna. ¿Estás seguro que quieres irte ahora?");

				alert.SetPositiveButton ("Si", (senderAlert, args) => {
					base.OnBackPressed ();
				});

				alert.SetNegativeButton ("No", (senderAlert, args) => {
				
				}); 

				alert.Show ();

			} else {
				if (commentrev.Text != "") {
					// No salimos porque hay texto en la caja de reseñas
					Log.Debug (tag, "No salimos porque está visible");

					Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder (this);

					alert.SetTitle("Salir?");
					alert.SetIcon (Resource.Drawable.bola);
					alert.SetMessage ("Parece que comenzaste a escribir una reseña pero aún no la has enviado. ¿Seguro que deseas irte ahora?");

					alert.SetPositiveButton ("Si", (senderAlert, args) => {
						base.OnBackPressed ();
					});

					alert.SetNegativeButton ("No", (senderAlert, args) => {

					}); 

					alert.Show ();
				} else {
					base.OnBackPressed ();
				}
			}


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

		//TERMINAN COSAS DE SELECCIONAR IMAGEN



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

		public static byte[] ImageToByte2(Bitmap bitmap)
		{
			/*
			Log.Debug ("ImageToByte", "Inicia Funcion Alternativa");
			byte[] bitmapData;
			//Java.IO.File tf = new Java.IO.File (bitmap);
	return bitmapData;
			Log.Debug ("ImageToByte", "Termina Funcion Alternativa");
			*/

			 byte[] bitmapData;
			using (var stream = new MemoryStream())
			{			
				Log.Debug ("ImageToByte", "Inicia compressformat");
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
				Log.Debug ("ImageToByte", "Inicia stream to array");
				bitmapData = stream.ToArray();
				Log.Debug ("ImageToByte", "Inicia stream dispose");
				stream.Dispose ();
				Log.Debug ("ImageToByte", "Termina Método");
			}

            return bitmapData;
		

		
		}

	

	}


}

