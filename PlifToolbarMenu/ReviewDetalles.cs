using System;
using Android.Widget;

public class ReviewDetalles : Java.Lang.Object
{

	public String  id;
	public String  haslike;
	public TextView numlikes;


public ReviewDetalles()
{
		id = null;
		haslike = null;
}

	public ReviewDetalles(String setid, String sethaslike, TextView nlikes)
{
		id = setid;
		haslike = sethaslike;
		numlikes = nlikes;
}



}

