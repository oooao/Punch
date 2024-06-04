package mono.com.akexorcist.snaptimepicker;


public class SnapTimePickerDialog_ListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.akexorcist.snaptimepicker.SnapTimePickerDialog.Listener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimePicked:(II)V:GetOnTimePicked_IIHandler:Akexorcist.SnapTimePicker.SnapTimePickerDialog/IListenerInvoker, Akexorcist.SnapTimePicker\n" +
			"";
		mono.android.Runtime.register ("Akexorcist.SnapTimePicker.SnapTimePickerDialog+IListenerImplementor, Akexorcist.SnapTimePicker", SnapTimePickerDialog_ListenerImplementor.class, __md_methods);
	}


	public SnapTimePickerDialog_ListenerImplementor ()
	{
		super ();
		if (getClass () == SnapTimePickerDialog_ListenerImplementor.class)
			mono.android.TypeManager.Activate ("Akexorcist.SnapTimePicker.SnapTimePickerDialog+IListenerImplementor, Akexorcist.SnapTimePicker", "", this, new java.lang.Object[] {  });
	}


	public void onTimePicked (int p0, int p1)
	{
		n_onTimePicked (p0, p1);
	}

	private native void n_onTimePicked (int p0, int p1);

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
