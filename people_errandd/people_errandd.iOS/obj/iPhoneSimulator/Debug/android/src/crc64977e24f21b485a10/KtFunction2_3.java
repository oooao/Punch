package crc64977e24f21b485a10;


public class KtFunction2_3
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		kotlin.jvm.functions.Function2,
		kotlin.Function
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_invoke:(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;:GetInvoke_Ljava_lang_Object_Ljava_lang_Object_Handler:Kotlin.Jvm.Functions.IFunction2Invoker, Xamarin.Kotlin.StdLib\n" +
			"";
		mono.android.Runtime.register ("Akexorcist.SnapTimePicker.KtFunction2`3, Akexorcist.SnapTimePicker", KtFunction2_3.class, __md_methods);
	}


	public KtFunction2_3 ()
	{
		super ();
		if (getClass () == KtFunction2_3.class)
			mono.android.TypeManager.Activate ("Akexorcist.SnapTimePicker.KtFunction2`3, Akexorcist.SnapTimePicker", "", this, new java.lang.Object[] {  });
	}


	public java.lang.Object invoke (java.lang.Object p0, java.lang.Object p1)
	{
		return n_invoke (p0, p1);
	}

	private native java.lang.Object n_invoke (java.lang.Object p0, java.lang.Object p1);

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
