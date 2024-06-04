package crc643c61ea2ced9767ab;


public class AppSettingsInterface
	extends crc643c61ea2ced9767ab.MainActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("people_errandd.Droid.AppSettingsInterface, people_errandd.Android", AppSettingsInterface.class, __md_methods);
	}


	public AppSettingsInterface ()
	{
		super ();
		if (getClass () == AppSettingsInterface.class)
			mono.android.TypeManager.Activate ("people_errandd.Droid.AppSettingsInterface, people_errandd.Android", "", this, new java.lang.Object[] {  });
	}


	public AppSettingsInterface (int p0)
	{
		super (p0);
		if (getClass () == AppSettingsInterface.class)
			mono.android.TypeManager.Activate ("people_errandd.Droid.AppSettingsInterface, people_errandd.Android", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
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
