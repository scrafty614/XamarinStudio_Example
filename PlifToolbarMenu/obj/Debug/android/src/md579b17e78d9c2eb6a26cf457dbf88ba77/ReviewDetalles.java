package md579b17e78d9c2eb6a26cf457dbf88ba77;


public class ReviewDetalles
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ReviewDetalles, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ReviewDetalles.class, __md_methods);
	}


	public ReviewDetalles () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ReviewDetalles.class)
			mono.android.TypeManager.Activate ("ReviewDetalles, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ReviewDetalles (java.lang.String p0, java.lang.String p1, android.widget.TextView p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ReviewDetalles.class)
			mono.android.TypeManager.Activate ("ReviewDetalles, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2 });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
