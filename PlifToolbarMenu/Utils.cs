using System;
using Android.Content;
using Java.Util;

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

namespace PlifToolbarMenu

{
	public class Utils
	{
		private Context _context;

		public Utils (Context context)
		{
			this._context = context;
		}

		public async Task<JsonValue> getFilePaths(string url) {
		
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";

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

		public int getScreenWidth() {
			int columnWidth;
			IWindowManager wm = (IWindowManager) _context.GetSystemService(Context.WindowService);
			Display display = wm.DefaultDisplay;

			Point point = new Point();
			try {
				display.GetSize(point);
			} catch (Exception ex) { // Older device
				point.X = display.Width;
				point.Y = display.Height;
			}
			columnWidth = point.X;
			return columnWidth;
		}

	}
}

