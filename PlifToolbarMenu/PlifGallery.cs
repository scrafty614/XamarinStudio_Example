
using System;
using Android.Widget; //BaseAdapter
using Android.Views; //Layoutinflater
using Android.Content; //Context
using Android.App; //Application
using System.Collections.Generic; //List
using Android.Util; //LOG




namespace PlifToolbarMenu
{

	//INICIAAA
	public class PlifGallery : BaseAdapter<ListaImagenes>
	{


		public List<ListaImagenes> mItems = new List<ListaImagenes> ();
		private Context mContext;

		private RelativeLayout.LayoutParams mImageViewLayoutParams;
		private int mItemHeight = 0;
		private int mNumColumns = 0;


		public int veces=0;

		public ListaImagenes este;

		public PlifGallery(Context context, List<ListaImagenes> items){
			mItems = items;
			mContext = context;

			mItemHeight = 0;
			mNumColumns = 0;
			//mImageViewLayoutParams = new RelativeLayout.LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT); //Java Code
			mImageViewLayoutParams = new RelativeLayout.LayoutParams (RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
			}	

		//METODOS 
		// set numcols
		public void setNumColumns(int numColumns) {
			Log.Debug ("SetNumColumns","Inicia SetNumColumns");
			mNumColumns = numColumns;
		}

		public int getNumColumns() {
			Log.Debug ("GetNumColumns","Inicia GetNumColumns");
			return mNumColumns;
		}

		// set photo item height
		public void setItemHeight(int height) {
			Log.Debug ("SetItemHeight","Inicia SetItemHeight");
			Log.Debug ("SetNumColumns","El ItemHeight actual es: "+mItemHeight);
			if (height == mItemHeight) {
				Log.Debug ("SetItemHeight","Es Igual");
				return;
			}
			Log.Debug ("SetItemHeight","No es igual, recalcula parámetros con: "+height);
			mItemHeight = height;
			mImageViewLayoutParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, mItemHeight);
			NotifyDataSetChanged();
		}
		//FIN METODOS

		//NO SE PARA QUE SEA ESTO PERO SI NO SE LO PONGO NO COMPILA:
		public override long GetItemId (int position)
		{
			//throw new NotImplementedException ();
			Log.Debug ("GetItemID","Position");
			este = mItems [position];
			Log.Debug ("Este","Ruta: "+este.Ruta);

			/*
			var galeriaimg = new Intent (mContext, typeof(GaleriaImagenes));
			galeriaimg.AddFlags (ActivityFlags.NewTask);
			galeriaimg.PutExtra("ruta",este.Ruta);
			mContext.StartActivity (galeriaimg);*/

			return position;	
		}


		public override int Count {
			get {Log.Debug ("ListaImagenesCount","Count"); return mItems.Count;  }
		}


		public override ListaImagenes this[int position]{
			
			get{Log.Debug ("ListaImagenesThis","Get"); 
				return mItems [position]; }

		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
	
			convertView = null;
			View row = convertView;

			if (row==null){
				row = LayoutInflater.From (Application.Context).Inflate (Resource.Layout.plif_photoitem, null, false);
			}

			Log.Debug ("PlifGallery","La posicion es: "+position);
			Log.Debug ("PlifGallery","");

			//ESTE SETEA LA IMAGEN
			Log.Debug ("PlifGallery","Se crea el objeto del row");
			ImageView imagen = row.FindViewById<ImageView> (Resource.Id.cover);

			Log.Debug ("PlifGallery","Inicia comparacion");
			if (mItems [position].Ruta == null || mItems [position].Ruta == "" || mItems [position].Ruta == "null") {
				//pon la imagen por defecto
				imagen.SetImageResource (Resource.Drawable.marca);
				Log.Debug ("PlifGallery","No hay imagen");
			} else {
				Log.Debug ("PlifGallery","Inicia verificacion de url");
				//TENEMOS QUE VERIFICAR SI LA IMAGEN ES DE GOOGLE O DE NOSOTROS!!!
				string extra="http://plif.mx/admin/";
				Log.Debug ("PlifGallery","se crea la ruta");
				string ruta=mItems[position].Ruta;
				Log.Debug ("PlifGallery","se crea el primer caracter");
				string first=ruta[0].ToString();

				Log.Debug ("PlifGallery","Inicia IF");
				if(first=="u" || first=="U"){
					//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
					ruta=extra+ruta;
					Log.Debug ("PlifGallery","Si era");
				}else{
					//no hagas nada, la imagen es de google
				}
				Log.Debug ("PlifGallery","No era");

				Log.Debug ("PlifGallery","Asignar Ruta: "+ ruta);

				if (row==null){
					Log.Debug ("PlifGallery","ES NULA Dx");
				}

				Log.Debug ("PARAMETERSHEIGHT","se asignan los layoutparams");

				imagen.LayoutParameters = mImageViewLayoutParams;

				Log.Debug ("PARAMETERSHEIGHT","LP Height: "+imagen.LayoutParameters.Height+" mItemHeight: "+mItemHeight);

				if (imagen.LayoutParameters.Height != mItemHeight) {
					imagen.LayoutParameters = mImageViewLayoutParams;	
				}

					

				Koush.UrlImageViewHelper.SetUrlDrawable (imagen, ruta, Resource.Drawable.bolaplace);

				Log.Debug ("PlifGallery","Asignar TV");
				TextView tmp=row.FindViewById<TextView> (Resource.Id.title);
				tmp.Text = mItems [position].Titulo;

				veces++;

			}//TERMINA SI LA IMAGEN NO ES NULL


			Log.Debug ("PlifGallery","Justo ANTES del return");
			return row;
		} //FIN GET VIEW

	}//FIN CLASS
	//TERMINAAA
}

