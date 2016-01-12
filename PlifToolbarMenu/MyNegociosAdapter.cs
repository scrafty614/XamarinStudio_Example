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
	public class MyNegociosAdapter : BaseAdapter<Categoria>
	{


		public List<Categoria> mItems = new List<Categoria> ();
		private Context mContext;

		public int veces=0;

		public MyNegociosAdapter(Context context, List<Categoria> items){
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
			
			get { 
				return mItems.Count ();  
				}
		
		}


		public override Categoria this[int position]{
			get{ return mItems [position]; }

		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

			convertView = null;
			View row = convertView;

			if (row==null){
				row = LayoutInflater.From (Application.Context).Inflate (Resource.Layout.spinnerfuck, null, false);
			}



			//ESTE SETEA EL NOMBRE
			TextView nombre = row.FindViewById<TextView> (Resource.Id.company);
			nombre.Text = mItems [position].Nombre;

			return row;
		} //FIN GET VIEW



	}//FIN CLASS
}
