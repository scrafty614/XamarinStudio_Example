using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Text;	
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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




namespace PlifToolbarMenu
{
	public class MyListAdapter : BaseAdapter<Negocio >
	{
		

		public List<Negocio> mItems = new List<Negocio> ();
		private Context mContext;

		public int veces=0;

		public MyListAdapter(Context context, List<Negocio> items){
			mItems = items;
			context = mContext;
			}	

		//NO SE PARA QUE SEA ESTO PERO SI NO SE LO PONGO NO COMPILA:
		public override long GetItemId (int position)
		{
			//throw new NotImplementedException ();
			return position;	
		}


		public override int Count {
		get { return mItems.Count ();  }
		}


		public override Negocio this[int position]{
			get{ return mItems [position]; }

		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

			convertView = null;
			View row = convertView;

			if (row==null){
				row = LayoutInflater.From (Application.Context).Inflate (Resource.Layout.listview_row, null, false);
			}

			//PRIMERO QUE NADA CHECAMOS SI ES PREMIUM, Y SI SI CAMBIAMOS EL COLOR DEL FONDO

			if (mItems [position].NegocioPrem == "1") {
				LinearLayout fondonegocio = row.FindViewById<LinearLayout> (Resource.Id.fondonegocio);
				//fondonegocio.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#FFFFDB"));
				TextView recomendado = row.FindViewById<TextView> (Resource.Id.recomendado);
				//recomendado.Visibility = ViewStates.Visible;
			} else {
			//Nada, no es premium. No lo merece!
			}



			//ESTE SETEA EL NOMBRE
			TextView nombre = row.FindViewById<TextView> (Resource.Id.NegocioName);
			nombre.Text = mItems [position].NegocioName;

			//ESTE SETEA LA DIRECCION
			TextView direccion = row.FindViewById<TextView> (Resource.Id.NegocioDir);
			direccion.Text = mItems [position].NegocioDir;

			//ESTE SETEA LA CATEGORIA
			TextView categoria = row.FindViewById<TextView> (Resource.Id.NegocioCat);
			categoria.Text = mItems [position].NegocioCat;

			//ESTE SETEA LA DISTANCIA (en caso de geolocalizacion)
			TextView distancia = row.FindViewById<TextView> (Resource.Id.distancia);
			distancia.Text = mItems [position].NegocioDist;

			//ESTE SETEA LAS ESTRELLAS DE LA CALIFICACION
			ImageView cali = row.FindViewById<ImageView> (Resource.Id.calificacion);

			string cal = mItems [position].NegocioCal;

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

			//ESTE SETEA LA IMAGEN
			ImageView imagen = row.FindViewById<ImageView> (Resource.Id.NegocioFoto);

				if (mItems [position].NegocioFoto == null || mItems [position].NegocioFoto == "" || mItems [position].NegocioFoto == "null") {
					//pon la imagen por defecto
					imagen.SetImageResource (Resource.Drawable.marca);
				} else {
				//TENEMOS QUE VERIFICAR SI LA IMAGEN ES DE GOOGLE O DE NOSOTROS!!!
				string extra="http://plif.mx/admin/";
				string ruta=mItems[position].NegocioFoto;
				string first=ruta[0].ToString();

				if(first=="u" || first=="U"){
					//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
					ruta=extra+ruta;
				}else{
					//no hagas nada, la imagen es de google
				}

				Koush.UrlImageViewHelper.SetUrlDrawable (imagen, ruta, Resource.Drawable.bolaplace);
	
				}//TERMINA SI LA IMAGEN NO ES NULL
				
			return row;
		} //FIN GET VIEW

	}//FIN CLASS
}

