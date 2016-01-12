
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

//mias
using Java.Net;
using Android.Graphics;
//using Java.IO;
using Android.Graphics.Drawables;

using System.Net;
using System.IO;
using System.Drawing;

//ASYNC LIBS
using System.Threading.Tasks;
using System.Json;
using System.Collections;
using System.Collections.Specialized;

using Android.Views.InputMethods;

namespace PlifToolbarMenu
{
	public class Fragment2 : Android.Support.V4.App.Fragment

	{

		public JsonValue categorias_array;
		private List<Categoria> mItems;
		private List<Region> rItems;

		private List<Negocio> negItems;
		private ListView mListView;
		public int counter = 0;

		string cat_id=null;
		string reg_id=null;

		string urlnextprev="";
		int pagina;


		string tag = "MainActivity";
		Bundle gps;




	
		public JsonValue negocios;
		JsonValue objeto;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}



		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			base.OnCreateView (inflater, container, savedInstanceState);


			if (savedInstanceState != null) {
				//restore fragment here

			}




			View view = inflater.Inflate (Resource.Layout.Fragment2, container, false);
			//Toast.MakeText (Application.Context, "Estas en el fragmento 2", ToastLength.Long).Show ();

			//ANTES QUE NADA, CREAMOS LOS BOTONES, LOS INPUTS, LAS LAYOUTS Y LOS PROGRESSBAR, Y PORSUPUESTO EL LISTVIEW
			Button busqueda = view.FindViewById<Button> (Resource.Id.busqueda); //pestaña busqueda
			Button resultados = view.FindViewById<Button> (Resource.Id.resultados);//pestaña resultados

			LinearLayout botones = view.FindViewById<LinearLayout> (Resource.Id.linearLayout0); //capa de botones busqueda-resultados
			LinearLayout cuadbusqueda = view.FindViewById<LinearLayout> (Resource.Id.linearLayout1); //capa de cuadro de busqueda
			LinearLayout cuadresultados = view.FindViewById<LinearLayout> (Resource.Id.linearLayout3); //capa de cuadro de busqueda
			LinearLayout prevnext = view.FindViewById<LinearLayout> (Resource.Id.linearLayout4); //capa de botones siguiente y anterior

			EditText cad = view.FindViewById<EditText> (Resource.Id.cadena); //cuadro de texto que contiene las palabras a buscar
			//cad.Text="pulpo";
			Button buscar = view.FindViewById<Button> (Resource.Id.buscar); //botón de buscar en mi ciudad
			Button geo = view.FindViewById<Button> (Resource.Id.geo); //botón de buscar cerca de mi

			Button prev = view.FindViewById<Button> (Resource.Id.prev); //botón de anterior en los resultados
			Button next = view.FindViewById<Button> (Resource.Id.next); //boton de siguiente en los resultados

			ProgressBar wait = view.FindViewById<ProgressBar> (Resource.Id.progressBar1); //progress bar para la búsqueda
			ProgressBar waitpagina = view.FindViewById<ProgressBar> (Resource.Id.progressBar2); //progress bar para las paginas

			ListView reslista = view.FindViewById<ListView> (Resource.Id.listView1); //la lista donde se mmuestran los resultados de la busqueda

			TextView noresults = view.FindViewById<TextView> (Resource.Id.textView2); //texto que dice que no se encontraron resultados

			gps = this.Arguments;
				

			//CREAMOS LAS VARIABLES QUE VA A RECIBIR LA API

			string cadena="";

			string lat_long = "";
			string lat = "";
			string longt = "";

			string region_heredada = null;

			Typeface font = Typeface.CreateFromAsset(Activity.Assets, "Fonts/fa.ttf");
			buscar.SetTypeface(font, TypefaceStyle.Normal);
			geo.SetTypeface(font, TypefaceStyle.Normal);

			//TERMINAMOS DE CREAR LOS ELEMENTOS, AHORA SI A PROGRAMAR TODO LO DEMÁS
			busqueda.Click += (sender, e) => {
				
				busqueda.SetBackgroundResource(Resource.Drawable.busquedaactive); 
				resultados.SetBackgroundResource(Resource.Drawable.busqueda);
				//cambiar colores
				busqueda.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
				resultados.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
				//hacer visible el cuadro de busqueda y hacer invisible el cuadro de resultados
				cuadbusqueda.Visibility=ViewStates.Visible;
				cuadresultados.Visibility = ViewStates.Gone;

			};

			resultados.Click += (sender, e) => {
				busqueda.SetBackgroundResource(Resource.Drawable.busqueda);
				resultados.SetBackgroundResource(Resource.Drawable.busquedaactive);
				//cambiar colores
				busqueda.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
				resultados.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
				//hacer invisible el cuadro de busqueda y visible el cuadro de resultados
				cuadbusqueda.Visibility=ViewStates.Gone;
				cuadresultados.Visibility = ViewStates.Visible;
				};

			//CATEGORIAS
			Spinner categ = view.FindViewById<Spinner> (Resource.Id.categorias);
							
			mItems = new List<Categoria> ();

			mItems.Add(new Categoria(){
				//NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
				Id = "",
				Nombre = "Selecciona una categoría"

			});



			//inicia lo de para traer las categorias
			Button getcats = view.FindViewById<Button> (Resource.Id.getcats);

			getcats.Click += async (sender, e) => {
				
				JsonValue objeto;

				try{
			categorias_array = await FetchWeatherAsync("http://plif.mx/mobile/get_cats_neg");
					objeto = categorias_array["respuesta"];

					foreach(JsonObject data in objeto){
							mItems.Add(new Categoria(){
							Id = data["categorias"]["id"],
							Nombre = data["categorias"]["nombre"]

						});
					}


				}catch(Exception ex){
					Toast.MakeText (Application.Context, "Ocurrió un error al recuperar las categorías", ToastLength.Long).Show ();
				}
			
			};

			getcats.PerformClick ();
			//termina lo de para traer las categorias



			MyNegociosAdapter adapter_c = new MyNegociosAdapter (Application.Context, mItems);
			categ.Adapter = adapter_c;

			//REGIONES
			Spinner regio = view.FindViewById<Spinner> (Resource.Id.regiones);
			ArrayAdapter<String> regadapter;

			//Android.Resource.Layout.SimpleSpinnerDropDownItem
			regadapter = new ArrayAdapter<string>(Application.Context, Resource.Layout.spinnerfuck, Resource.Id.company);

			rItems = new List<Region> ();

			//AGREGAMOS LAS REGIONES
			rItems.Add(new Region(){
				//NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
				Id = "9",
				Nombre = "Durango"

			});

			rItems.Add(new Region(){
				//NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
				Id = "2",
				Nombre = "Mazatlán"

			});

			rItems.Add(new Region(){
				//NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
				Id = "3",
				Nombre = "Torreón"

			});

			rItems.Add(new Region(){
				//NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
				Id = "4",
				Nombre = "Zacatecas"

			});

			MyRegionesAdapter adapter_r = new MyRegionesAdapter (Application.Context, rItems);
			regio.Adapter = adapter_r;


			categ.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (c_spinner_ItemSelected);
			regio.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (r_spinner_ItemSelected);

			buscar.Click += async (sender, e) => {
				counter=0;
				reslista.Adapter=null;
				//ESTE VA A TENER QUE SER ASÍNCRONO!
				cadena=cad.Text;
				noresults.Visibility=ViewStates.Gone;
				//ocultamos el teclado
				InputMethodManager imm = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
				imm.HideSoftInputFromWindow(cad.WindowToken, 0);

				//primero deberíamos validar que el usuario haya introducido algún texto
				if((cadena=="" || cadena==null) && (cat_id=="" || cat_id == null) && (region_heredada == null || region_heredada=="")){
					Toast.MakeText (Application.Context, "¡Espera! Primero dinos que estás buscando.", ToastLength.Long).Show ();
				}else{
			     //antes que nada, comenzamos a armar la url desde la base

					string uri="http://plif.mx/filtrar.json?";

					//comenzaremos desde la pagina 1
					pagina=1;

					//añadimos la cadenas
					uri=uri+"cadena="+cadena;
					//añadimos la categoría
					uri=uri+"&categoria="+cat_id;
					//añadimos el lat_long, que en este caso será nulo
					uri=uri+"&lat_long="+lat_long;
					//añadimos la latitud que pues tambien será nula
					uri=uri+"&lat="+lat;
					//y la longitud igual
					uri=uri+"&long="+longt;
					//añadimos la región
					uri=uri+"&region="+reg_id;
					//guardamos la url hasta ahí para los botones prev next
					urlnextprev=uri;
					//y por último añadimos la página
					uri=uri+"&pagina="+pagina;

				   // Toast.MakeText (Application.Context, uri, ToastLength.Long).Show ();

					//mostrar los botones y los resultados, y ocultar el cuadro de busqueda
					botones.Visibility = ViewStates.Visible;
					cuadresultados.Visibility = ViewStates.Visible;
					cuadbusqueda.Visibility=ViewStates.Gone;
					prev.Visibility=ViewStates.Gone;
					next.Visibility=ViewStates.Gone;



					//mostramos el progressbar
					wait.Visibility=ViewStates.Visible;

					//cambiar el estilo de los botones busqueda y resultados
					busqueda.SetBackgroundResource(Resource.Drawable.busqueda);
					resultados.SetBackgroundResource(Resource.Drawable.busquedaactive);
					//cambiar colores
					busqueda.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
					resultados.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));

					//cambiar el margen de la busqueda
					RelativeLayout.LayoutParams pms = new RelativeLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
					pms.AddRule(LayoutRules.Below, Resource.Id.linearLayout0);
					pms.TopMargin = -90;
					cuadbusqueda.LayoutParameters=pms;

					Log.Debug("BUSQUEDA","LA URI ES: "+uri);
					//Y aqui vamos a pedirle al servidor que busque
					try{
						negocios = await FetchWeatherAsync(uri);
						objeto = negocios["respuesta"];

						negItems = new List<Negocio> ();

						foreach(JsonObject data in objeto){
							Log.Debug("GEO","Waaaaat!!!");
							//AQUI LOS AGREGAMOS
				
								negItems.Add(new Negocio(){
								NegocioId = WebUtility.HtmlDecode(data["Data"]["id"]),
								NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
								NegocioDir = WebUtility.HtmlDecode(data["Data"]["geo_calle"]+" "+data["Data"]["geo_numero"]+" "+data["Data"]["geo_colonia"]),
								NegocioCat = WebUtility.HtmlDecode(data["Data"]["categoria"]),
								NegocioFoto = WebUtility.HtmlDecode(data["Data"]["ruta"]),
								NegocioCal = WebUtility.HtmlDecode(data["Data"]["calificacion"]),
								NegocioPrem = data["Data"]["premium"],
								NegocioNum = counter

							});

							//Toast.MakeText (Application.Context, "YUSS!!", ToastLength.Long).Show ();
							counter++;
						}

						wait.Visibility=ViewStates.Gone;

						if(counter==0){
							noresults.Visibility=ViewStates.Visible;
						}else{

						MyListAdapter res_adapter = new MyListAdapter (Application.Context, negItems);


						

						if(pagina<=1){
							prev.Enabled=false;
						}else{
							prev.Enabled=true;
						}

						if(counter<10){
							next.Enabled=false;
						}else{
							next.Enabled=true;
						}

						prev.Visibility=ViewStates.Visible;
						next.Visibility=ViewStates.Visible;
						reslista.Adapter = res_adapter;

						}//else si no se encontraron resultados

					}catch(Exception ex){
						Log.Debug("BUSQUEDA","Error"+ex);
						busqueda.PerformClick();
						Toast.MakeText (Application.Context, "Ooops! Parece que algo salió mal. ¿Por qué no lo intentas de nuevo?", ToastLength.Long).Show ();
					}


				
				}//ELSE SI LA CADENA NO ESTÁ VACIA
			};

			//AQUI CHECAMOS SI RECIBE UN ID DE REGION PARA LA PANTALLA PRINCIPAL
			region_heredada=gps.GetString("region");
			//Toast.MakeText (Application.Context, "la region es: "+region_heredada, ToastLength.Long).Show ();
			if(region_heredada != "nada"){
				reg_id = region_heredada;
				//Toast.MakeText (Application.Context, "la region es: "+reg_id+" SI HAY", ToastLength.Long).Show ();
				buscar.PerformClick ();

			}

			prev.Click += async (sender, e) => {
				if(pagina==1){
					//No hagas nada
				}else{
				pagina--;
				counter=0;

				prev.Visibility=ViewStates.Gone;
				next.Visibility=ViewStates.Gone;

				waitpagina.Visibility=ViewStates.Visible;

				string newuri = urlnextprev + "&pagina="+pagina;

				try{
					negocios = await FetchWeatherAsync(newuri);
					objeto = negocios["respuesta"];

					negItems = new List<Negocio> ();

						//esto es para la distancia en la geolocalizacion
						float dist=0;
						string dist_final="";
						double dist2=0;

					foreach(JsonObject data in objeto){
						//AQUI LOS AGREGAMOS


							try{
							//HAY QUE VALIDAR ESTO, SI NO VA A TRONAR
							dist=float.Parse(WebUtility.HtmlDecode(data["Data"]["distance"]));
								//Log.Debug (tag, "La Distancia de la BD es: "+dist);

							if(dist<1){
								dist=dist*1000;
								dist2=Math.Round(dist, 1);
								dist_final=dist2.ToString();
								dist_final=dist_final+" mts.";
							}else{
								dist2=Math.Round(dist, 1);
								dist_final=dist2.ToString();
								dist_final=dist_final+" kms.";
							}
							}catch(Exception ex){
								Log.Debug (tag, "Error detectado: NO HAY DISTANCIA");
							}


						negItems.Add(new Negocio(){
							NegocioId = WebUtility.HtmlDecode(data["Data"]["id"]),
							NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
							NegocioDir = WebUtility.HtmlDecode(data["Data"]["geo_calle"]+" "+data["Data"]["geo_numero"]+" "+data["Data"]["geo_colonia"]),
							NegocioCat = WebUtility.HtmlDecode(data["Data"]["categoria"]),
							NegocioFoto = WebUtility.HtmlDecode(data["Data"]["ruta"]),
							NegocioCal = WebUtility.HtmlDecode(data["Data"]["calificacion"]),
						    NegocioPrem = data["Data"]["premium"],
							NegocioNum = counter,
								NegocioDist = dist_final


						});

						//Toast.MakeText (Application.Context, "YUSS!!", ToastLength.Long).Show ();
						counter++;
					}

					MyListAdapter res_adapter = new MyListAdapter (Application.Context, negItems);




					if(pagina<=1){
						prev.Enabled=false;
					}else{
						prev.Enabled=true;
					}

					if(counter<10){
						next.Enabled=false;
					}else{
						next.Enabled=true;
					}



					prev.Visibility=ViewStates.Visible;
					next.Visibility=ViewStates.Visible;

					waitpagina.Visibility=ViewStates.Gone;

					reslista.Adapter = res_adapter;




				}catch(Exception ex){
					if(pagina<=1){
						prev.Enabled=false;
					}else{
						prev.Enabled=true;
					}

					prev.Visibility=ViewStates.Visible;
					next.Visibility=ViewStates.Visible;

					waitpagina.Visibility=ViewStates.Gone;

					Toast.MakeText (Application.Context, "Oops! Parece que algo salió mal. ¿Por qué no oprimes \"Anterior\" para intentarlo de nuevo?", ToastLength.Long).Show ();
				}
			}//else si la pagina no es la primera
					
			};

			next.Click += async (sender, e) => {
				pagina++;
				counter=0;

				prev.Visibility=ViewStates.Gone;
				next.Visibility=ViewStates.Gone;

				waitpagina.Visibility=ViewStates.Visible;

				string newuri = urlnextprev + "&pagina="+pagina;

				try{
					negocios = await FetchWeatherAsync(newuri);
					objeto = negocios["respuesta"];

					negItems = new List<Negocio> ();

					//esto es para la distancia en la geolocalizacion
					float dist=0;
					string dist_final="";
					double dist2=0;

					foreach(JsonObject data in objeto){
						//AQUI LOS AGREGAMOS

						try{
							//HAY QUE VALIDAR ESTO, SI NO VA A TRONAR
							dist=float.Parse(WebUtility.HtmlDecode(data["Data"]["distance"]));

							if(dist<1){
								dist=dist*1000;
								dist2=Math.Round(dist, 1);
								dist_final=dist2.ToString();
								dist_final=dist_final+" mts.";
							}else{
								dist2=Math.Round(dist, 1);
								dist_final=dist2.ToString();
								dist_final=dist_final+" kms.";
							}
						}catch(Exception ex){
							Log.Debug (tag, "Error detectado: NO HAY DISTANCIA");
						}

						negItems.Add(new Negocio(){
							NegocioId = WebUtility.HtmlDecode(data["Data"]["id"]),
							NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
							NegocioDir = WebUtility.HtmlDecode(data["Data"]["geo_calle"]+" "+data["Data"]["geo_numero"]+" "+data["Data"]["geo_colonia"]),
							NegocioCat = WebUtility.HtmlDecode(data["Data"]["categoria"]),
							NegocioFoto = WebUtility.HtmlDecode(data["Data"]["ruta"]),
							NegocioCal = WebUtility.HtmlDecode(data["Data"]["calificacion"]),
							NegocioPrem = data["Data"]["premium"],
							NegocioNum = counter,
							NegocioDist = dist_final

						});

						//Toast.MakeText (Application.Context, "YUSS!!", ToastLength.Long).Show ();
						counter++;
					}

					MyListAdapter res_adapter = new MyListAdapter (Application.Context, negItems);




					if(pagina<=1){
						prev.Enabled=false;
					}else{
						prev.Enabled=true;
					}

					if(counter<10){
						next.Enabled=false;
					}else{
						next.Enabled=true;
					}



					prev.Visibility=ViewStates.Visible;
					next.Visibility=ViewStates.Visible;

					waitpagina.Visibility=ViewStates.Gone;

					reslista.Adapter = res_adapter;




				}catch(Exception ex){
						if(pagina<=1){
						prev.Enabled=false;
					}else{
						prev.Enabled=true;
					}

					prev.Visibility=ViewStates.Visible;
					next.Visibility=ViewStates.Visible;

					waitpagina.Visibility=ViewStates.Gone;

					Toast.MakeText (Application.Context, "Oops! Parece que algo salió mal. ¿Por qué no oprimes \"Siguiente\" para intentarlo de nuevo?", ToastLength.Long).Show ();
				}

			};

			//BOTON DE GEOLOCALIZACION
			geo.Click += async (sender, e) => {
				Log.Debug(tag, "Si clickeó");



				string longitud=gps.GetString("longitud");
				string latitud=gps.GetString("latitud");
				//cad.Text=gps.GetString("longitud")+","+gps.GetString("latitud");

				//HAY QUE CONVERTIR LAS COMAS DE LAS COORDENADAS EN PUNTOS PARA QUE NO MARQUE ERROR EL SERVIDOR
				//input.Replace("_::_", "Areo");
				longitud=longitud.Replace(",",".");
				latitud=latitud.Replace(",",".");

				cadena=cad.Text;
				noresults.Visibility=ViewStates.Gone;
				//ocultamos el teclado
				InputMethodManager imm = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
				imm.HideSoftInputFromWindow(cad.WindowToken, 0);


				if(longitud=="nada" || latitud == "nada"){
					Toast.MakeText (Application.Context, "Parece que aún no hemos podido determinar tu ubicación. Toca para intentarlo de nuevo!", ToastLength.Long).Show ();
				}else{
					if(longitud=="nogps" || latitud == "nogps"){
						Toast.MakeText (Application.Context, "Tienes el GPS y la ubicación desactivados, por favor actívalos!", ToastLength.Long).Show ();
					}else{
						//Toast.MakeText (Application.Context, "Latitud: "+latitud+" Longitud: "+longitud, ToastLength.Long).Show ();

						//AQUI VAMOS A ARMAR TODO!

						//INICIA BUSQUEDA
						//antes que nada, comenzamos a armar la url desde la base

						string uri="http://plif.mx/filtrar.json?";

						//comenzaremos desde la pagina 1
						pagina=1;
						reslista.Adapter=null;

						//añadimos la cadena
						uri=uri+"cadena="+cadena;
						//añadimos la categoría
						uri=uri+"&categoria="+cat_id;
						//añadimos el lat_long
						//uri=uri+"&lat_long="+"(6371*acos(cos(radians("+latitud+")*cos(radians(geo_lat))*cos(radians(geo_long)-radians("+longitud+"))+sin(radians("+latitud+"))*sin(radians(geo_lat))))";
						//uri=uri+"&lat_long="+"(6371*acos(cos(radians("+latitud+"))*cos(radians(geo_lat))*cos(radians(geo_long)-radians("+longitud+"))+sin(radians("+latitud+"))*sin(radians(geo_lat))))";
						uri=uri+"&lat_long="+"&lat_long=%28+6371+*+acos%28+cos%28+radians%28"+latitud+"+%29+%29+*+cos%28+radians%28+geo_lat+%29+%29+*+cos%28+radians%28+geo_long+%29+-+radians%28"+longitud+"%29+%29+%2B+sin%28+radians%28"+latitud+"%29+%29+*+sin%28+radians%28+geo_lat+%29+%29+%29+%29+";
						//añadimos la latitud 
						uri=uri+"&lat="+latitud;
						//y la longitud igual
						uri=uri+"&long="+longitud;
						//añadimos la región
						uri=uri+"&region="+reg_id;
						//guardamos la url hasta ahí para los botones prev next
						urlnextprev=uri;
						//y por último añadimos la página
						uri=uri+"&pagina="+pagina;

						//cad.Text=uri;

						//Toast.MakeText (Application.Context, uri, ToastLength.Long).Show ();

						//mostrar los botones y los resultados, y ocultar el cuadro de busqueda
						botones.Visibility = ViewStates.Visible;
						cuadresultados.Visibility = ViewStates.Visible;
						cuadbusqueda.Visibility=ViewStates.Gone;
						prev.Visibility=ViewStates.Gone;
						next.Visibility=ViewStates.Gone;



						//mostramos el progressbar
						wait.Visibility=ViewStates.Visible;

						//cambiar el estilo de los botones busqueda y resultados
						busqueda.SetBackgroundResource(Resource.Drawable.busqueda);
						resultados.SetBackgroundResource(Resource.Drawable.busquedaactive);
						//cambiar colores
						busqueda.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
						resultados.SetTextColor(Android.Graphics.Color.ParseColor("#FFFFFF"));

						//cambiar el margen de la busqueda
						RelativeLayout.LayoutParams pms = new RelativeLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
						pms.AddRule(LayoutRules.Below, Resource.Id.linearLayout0);
						pms.TopMargin = -90;
						cuadbusqueda.LayoutParameters=pms;

						Log.Debug("GEO","LA URI ES: "+uri);

						//Y aqui vamos a pedirle al servidor que busque
						try{
							Log.Debug("GEO","ENTRAMOS");
							negocios = await FetchWeatherAsync(uri);
							objeto = negocios["respuesta"];
							Log.Debug("GEO","PASAMOS LA ASIGNACION");

							negItems = new List<Negocio> ();

							float dist=0;
							string dist_final;
							double dist2=0;

							foreach(JsonObject data in objeto){
								Log.Debug("GEO","Entramos a FOREACH");
								//AQUI LOS AGREGAMOS

								//INICIA VERIFICACION DE LA IMAGEN
								//TERMINA VERIFICACION DE LA IMAGEN
								dist=float.Parse(WebUtility.HtmlDecode(data["Data"]["distance"]));

								if(dist<1){
									dist=dist*1000;
									dist2=Math.Round(dist, 1);
									dist_final=dist2.ToString();
									dist_final=dist_final+" mts.";
								}else{
									dist2=Math.Round(dist, 1);
									dist_final=dist2.ToString();
									dist_final=dist_final+" kms.";
								}

								negItems.Add(new Negocio(){
									
									NegocioId = WebUtility.HtmlDecode(data["Data"]["id"]),
									NegocioName = WebUtility.HtmlDecode(data["Data"]["titulo"]),
									NegocioDir = WebUtility.HtmlDecode(data["Data"]["geo_calle"]+" "+data["Data"]["geo_numero"]+" "+data["Data"]["geo_colonia"]),
									NegocioCat = WebUtility.HtmlDecode(data["Data"]["categoria"]),
									NegocioFoto = WebUtility.HtmlDecode(data["Data"]["ruta"]),
									NegocioCal = WebUtility.HtmlDecode(data["Data"]["calificacion"]),
									NegocioPrem = data["Data"]["premium"],
									NegocioDist = dist_final,
									NegocioNum = counter

								});

								//Toast.MakeText (Application.Context, "YUSS!!", ToastLength.Long).Show ();
								counter++;
							}

							wait.Visibility=ViewStates.Gone;

							if(counter==0){
								noresults.Visibility=ViewStates.Visible;
							}else{

								MyListAdapter res_adapter = new MyListAdapter (Application.Context, negItems);




								if(pagina<=1){
									prev.Enabled=false;
								}else{
									prev.Enabled=true;
								}

								if(counter<10){
									next.Enabled=false;
								}else{
									next.Enabled=true;
								}

								prev.Visibility=ViewStates.Visible;
								next.Visibility=ViewStates.Visible;
								reslista.Adapter = res_adapter;

							}//else si no se encontraron resultados

						}catch(Exception ex){
							Log.Debug("GEO","El error: "+ex);
							busqueda.PerformClick();
							Toast.MakeText (Application.Context, "Ooops! Parece que algo salió mal. ¿Por qué no lo intentas de nuevo? "+ex, ToastLength.Long).Show ();
						}


						//TERMINA BUSQUEDA



					}//ELSE GPS DESACTIVADO
				}//ELSE NO SE HA ENCONTRADO LA UBICACION


			};

			reslista.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				//Toast.MakeText (Application.Context, "El ID del negocio es: "+negItems [e.Position].NegocioId, ToastLength.Long).Show ();

				if(negItems[e.Position].NegocioPrem!="0"){
					//Toast.MakeText (Application.Context, negItems [e.Position].NegocioName+" Es premium!!"+" NegocioPrem es: "+negItems[e.Position].NegocioPrem, ToastLength.Long).Show ();
					var negocio = new Intent (this.Activity, typeof(PerfilPremium));
					negocio.PutExtra("id",negItems [e.Position].NegocioId);
					negocio.PutExtra("nombre",negItems [e.Position].NegocioName);
					negocio.PutExtra("direccion",negItems [e.Position].NegocioDir);
					negocio.PutExtra("categoria",negItems [e.Position].NegocioCat);
					negocio.PutExtra("calificacion",negItems [e.Position].NegocioCal);

					StartActivity (negocio);
				}else{

				var negocio = new Intent (this.Activity, typeof(PerfilNegocio));
				negocio.PutExtra("id",negItems [e.Position].NegocioId);
				negocio.PutExtra("nombre",negItems [e.Position].NegocioName);
				negocio.PutExtra("direccion",negItems [e.Position].NegocioDir);
				negocio.PutExtra("categoria",negItems [e.Position].NegocioCat);
				negocio.PutExtra("calificacion",negItems [e.Position].NegocioCal);

				StartActivity (negocio);
				}

				//StartActivity(typeof(PerfilNegocio));
				//AQUI INCICIAMOS LA ACTIVIDAD DE PERFIL Y LE PASAMOS DATOS



				/*
				var myActivity = (MainActivity) this.Activity;
				myActivity.VerNegocio(negItems [e.Position].NegocioId);
				*/

			};
		
			return view;
		}

		public void onPause(){
			Log.Debug (tag, "ESTA EN PAUSA");
			base.OnPause ();
				
				

		}



		//ONRESUME
		public override void OnResume ()
		{
			Log.Debug (tag, "SE REINICIA");

			base.OnResume ();
		}

		public override void OnSaveInstanceState (Bundle outState)
		{		
			Log.Debug (tag, "SALVANDO ESTADO DE LA BUSQUEDA!");
			base.OnSaveInstanceState (outState);
		}

		public override void OnStop(){
			Log.Debug (tag, "SALVANDO ESTADO DE LA BUSQUEDA!");
			base.OnStop ();
		}

				//METODO SPINNER CATEGORIAS
		private void c_spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e){
			cat_id = mItems [e.Position].Id;
			}
				//FIN METODO SPINNER CATEGORIAS

		//METODO SPINNER REGIONES
		private void r_spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e){
			reg_id = rItems [e.Position].Id;
			}
		//FIN METODO SPINNER REGIONES


		//ASYNC METHOD
		// Gets weather data from the passed URL.
		private async Task<JsonValue> FetchWeatherAsync (string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";
			request.Headers.Add ("HTTP_USER_AGENT", "PlifAndroidApp");


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

