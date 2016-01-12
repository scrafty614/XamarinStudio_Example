package md50ef0824d786bfe2d073af1be23438305;


public class GaleriaAdapter_OnImageClickListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnClickListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler:Android.Views.View/IOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("PlifToolbarMenu.GaleriaAdapter/OnImageClickListener, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GaleriaAdapter_OnImageClickListener.class, __md_methods);
	}


	public GaleriaAdapter_OnImageClickListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GaleriaAdapter_OnImageClickListener.class)
			mono.android.TypeManager.Activate ("PlifToolbarMenu.GaleriaAdapter/OnImageClickListener, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public GaleriaAdapter_OnImageClickListener (int p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == GaleriaAdapter_OnImageClickListener.class)
			mono.android.TypeManager.Activate ("PlifToolbarMenu.GaleriaAdapter/OnImageClickListener, PlifToolbarMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
