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
		
	public class EnviarMensaje : AppCompatActivity
	{
		private SupportToolBar mToolbar;
		EditText mensaje;
		Button enviar;
		Button cancelar;
		Dictionary<string, string> diccionario; 
		bool sending = false;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.enviar_mensaje);
		    // Create your application here

			mToolbar = FindViewById<SupportToolBar> (Resource.Id.toolbar);
			SetSupportActionBar (mToolbar);
			//ESTA ES LA FLECHA PARA ATRÁS
			SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.ic_arrow_back);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			//SupportActionBar.SetHomeButtonEnabled (true);
			SupportActionBar.Title = "Mensaje para "+Intent.GetStringExtra("titulo");

			//INICIA PROGRAMACION
			mensaje = FindViewById<EditText> (Resource.Id.mensaje);
			enviar = FindViewById<Button> (Resource.Id.enviar);
			cancelar = FindViewById<Button> (Resource.Id.cancelar);

			var prefs = this.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);

			enviar.Click += async (object sender, EventArgs e) => {
				if(mensaje.Text=="" || mensaje.Text==null){
					var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
					Snackbar
						.Make (fabee, "Por favor escribe un mensaje!", Snackbar.LengthLong)
						.SetAction ("Ok", (view) => {  })
						.Show ();	
				}else{
					sending=true;

					//DATOS DE POST:           
					//data:{"nid":nid,"ownerid":ownerid, "mensaje":mensaje, "cus":cus, "ownmail":ownmail, 
					//"negocio":titulo, "anunciante": anunciante, "user":remitente, "reclamado":reclamado},
					ProgressBar waitrs = FindViewById<ProgressBar> (Resource.Id.waitrs);
					waitrs.Visibility=ViewStates.Visible;

					string rec="";

					if(Intent.GetStringExtra("propietario")=="0"){
						rec="false";
					}else{
						rec="true";
					}

					diccionario = new Dictionary<string, string>();
					diccionario.Add("nid", Intent.GetStringExtra("negocioid")); 
					diccionario.Add("ownerid", Intent.GetStringExtra("propietario")); 
					diccionario.Add("mensaje", mensaje.Text);
					diccionario.Add("ownmail", "fuckthisshit");
					diccionario.Add("negocio", Intent.GetStringExtra("titulo"));
					diccionario.Add("anunciante", "dontneeedit");
					diccionario.Add("user", prefs.GetString("nombre", null));
					diccionario.Add("reclamado",rec);
					diccionario.Add("cus", prefs.GetString("id", null));

					string resp = await plifserver.PostMultiPartForm ("http://plif.mx/pages/savemensaje", null, "nada", "file[]", "image/jpeg", diccionario, true);
					Log.Debug("Respuesta",resp);
					mensaje.Text="";
					sending=false;
					Toast.MakeText(this, "Tu mensaje fué enviado exitosamente!", ToastLength.Long).Show();
					waitrs.Visibility=ViewStates.Gone;
					Finish();


					
				}
			};

			cancelar.Click += (object sender, EventArgs e) => {
				Finish();
			};

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
			
			if (sending) {
				var fabee = FindViewById<CoordinatorLayout> (Resource.Id.snackbarPosition);
				Snackbar
					.Make (fabee, "Por favor espera a que tu mensaje se haya enviado.", Snackbar.LengthLong)
					.SetAction ("Ok", (view) => {  })
					.Show ();	
				base.OnBackPressed ();
			}
		}

	}
}

