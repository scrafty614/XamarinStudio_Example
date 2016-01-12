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
	public class MyReviewsAdapter : BaseAdapter<Review>
	{

		public List<Review> mItems = new List<Review> ();
		private Context mContext;

		public int veces=0;

		public MyReviewsAdapter(Context context, List<Review> items){
			mItems = items;
			mContext = context;
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


		public override Review this[int position]{
			get{ return mItems [position]; }

		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ReviewDetalles holder = null;
			convertView = null;
			View row = convertView;

			if (row==null){
				row = LayoutInflater.From (Application.Context).Inflate (Resource.Layout.listview_comentarios, null, false);
			}


			holder = new ReviewDetalles ();

			//ESTE SETEA LA IMAGEN
			ImageView imagen = row.FindViewById<ImageView> (Resource.Id.user_imagen);



			if (mItems [position].ReviewRuta == null || mItems [position].ReviewRuta == "" || mItems [position].ReviewRuta == "null") {
				//pon la imagen por defecto
				imagen.SetImageResource (Resource.Drawable.noprof);
			} else {
				//TENEMOS QUE VERIFICAR SI LA IMAGEN ES DE GOOGLE O DE NOSOTROS!!!
				string extra="http://plif.mx/";
				string ruta=mItems[position].ReviewRuta;
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
			nombre.Text = mItems [position].ReviewNombre+"AA";

			//ESTE SETEA LAS ESTRELLAS DE LA CALIFICACION
			ImageView cali = row.FindViewById<ImageView> (Resource.Id.user_calificacion);

			string cal = mItems [position].ReviewCalificacion;

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
			comentario.Text = mItems [position].ReviewComentario;

			//Ponemos la fecha y la hora
			TextView fechahora = row.FindViewById<TextView> (Resource.Id.user_fechahora);
			DateTime dt = Convert.ToDateTime(mItems[position].ReviewFecha);   
			fechahora.Text = dt.ToString ("dd/MM/yyyy H:mm");

			//ponemos los likes que tiene
			TextView likes = row.FindViewById<TextView> (Resource.Id.user_numlikes);
			likes.Text = " "+mItems [position].ReviewLikes;
			bool haslike = false;
			//averiguamos si el usuario le ha dado like

			string[] words = mItems[position].ReviewLikesUsers.Split(' ');

			foreach (string word in words)
			{
				if (word == mItems [position].ReviewautorId) {
					haslike = true;
				}
			}

			TextView corazonlike = row.FindViewById<TextView> (Resource.Id.user_corazonlikes);

			if (haslike) {
				corazonlike.Text = mContext.GetString(Resource.String.heart);


				} else {
				corazonlike.Text =mContext.GetString (Resource.String.heartempty);
			}

			Typeface font = Typeface.CreateFromAsset(mContext.Assets, "Fonts/fa.ttf");
			corazonlike.SetTypeface(font, TypefaceStyle.Normal);

			//corazonlike.SetTag (0,holder);
			//corazonlike.SetTag (mItems[position].ReviewId);
			//Log.Debug ("ReviewsAdapter","El id del like es: "+);
			holder = new ReviewDetalles("10000000","si",likes);

			corazonlike.SetTag(10,holder);

			Log.Debug("Adapter","lel");

			return row;
		} //FIN GET VIEW
		
	}
}

