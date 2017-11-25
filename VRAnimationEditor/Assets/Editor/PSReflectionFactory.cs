using UnityEngine;
using System.Collections;

public class PSReflectionFactory
{
	private static PSReflectionFactory instance;

	public static PSReflectionFactory GetInstance ()
	{
		if (instance == null)
			instance = new PSReflectionFactory ();
		return instance;
	}

	public PSBaseReflection GetReflection ()
	{
		return new PSReflectionForUnity510 ();
	}
}
