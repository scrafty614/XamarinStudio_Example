using System;
using System.Collections.Generic; //LIST<T>
using Android.Util; //LOG
using Android.Content; //INTENT
using Android.Provider; //MEDIASTORE
using Android.Database;
using Android.App;



namespace PlifToolbarMenu
{
	public class pliffunctions
	{

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






	}
}

