package md579b17e78d9c2eb6a26cf457dbf88ba77;


public class LinearDetalles
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("LinearDetalles, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LinearDetalles.class, __md_methods);
	}


	public LinearDetalles () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LinearDetalles.class)
			mono.android.TypeManager.Activate ("LinearDetalles, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LinearDetalles (android.widget.TextView p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LinearDetalles.class)
			mono.android.TypeManager.Activate ("LinearDetalles, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.TextView, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
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
