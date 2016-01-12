using System;
using Android.Widget;
using Android.App;

//async task
using Java.Net;
using System.Threading.Tasks;
using System.Json;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;

using Android.Graphics;
using Android.Service.Dreams;
using Android.Views;
using Java.Util;
using System.Collections.Generic;

using Org.Apache.Http.Client.Methods;

using Android.Util;


namespace PlifToolbarMenu
{
	public class GaleriaAdapter : BaseAdapter
	{

		private Activity _activity;
		private List<String> _filePaths = new List<String>();
		private int imageWidth;
		private int _listlength = 0;
		public string TAG="GaleriaAdapter";

		public GaleriaAdapter (Activity activity, List<String> filePaths, int imageWidth)
		{
			this._activity = activity;
			this._filePaths = filePaths;
			this.imageWidth = imageWidth;	
		}

		public override int Count {
			get { return _filePaths.Count; }
		}

		public override Java.Lang.Object GetItem(int position) {
			//return this._filePaths[position];
			return null;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {

			ImageView imageView;
			if (convertView == null) {
				imageView = new ImageView(_activity);
			} else {
				imageView = (ImageView) convertView;
			}

			string extra="http://plif.mx/admin/";
			string ruta=_filePaths[position];
			string first=ruta[0].ToString();

			if(first=="u" || first=="U"){
				//Toast.MakeText (Application.Context, "EMPIEZA CON U!!!", ToastLength.Long).Show ();
				ruta=extra+ruta;
			}else{
				//no hagas nada, la imagen es de google
			}

			Log.Debug ("GaleriaAdapter", "Procesando: "+ruta);

			imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
			//imageView.setLayoutParams(  new GridView.LayoutParams(imageWidth,	imageWidth));
			imageView.LayoutParameters = new GridView.LayoutParams(imageWidth,	imageWidth);
			//l.LayoutParameters = new LinearLayout.LayoutParams(w / 

			Koush.UrlImageViewHelper.SetUrlDrawable (imageView, ruta, Resource.Drawable.bolaplace);

			imageView.Click += (object sender, EventArgs e) => {
				new OnImageClickListener(position);
			};

			return imageView;
		}

		class OnImageClickListener : Java.Lang.Object,View.IOnClickListener {

			int _postion;

			// constructor
			public OnImageClickListener(int position) {
				this._postion = position;
				Log.Debug("imageclick","lel "+position);
			}

			public void OnClick(View v) {
				Log.Debug("clickview","yay "+_postion);
				// on selecting grid view image
				// launch full screen activity
				/*AQUI VAMOS A PONER LO DE PARA QUE SE ABRA TODO EL ACTIVITY CON LA IMAGEN
			Intent i = new Intent(_activity, FullScreenViewActivity.class);
			i.putExtra("position", _postion);
			_activity.startActivity(i);
			*/
			}

		}

	}






}

