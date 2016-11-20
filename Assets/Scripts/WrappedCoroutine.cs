using UnityEngine;
using System.Collections;

class WrappedCoroutine
{
	bool isRunning;
	public bool IsRunning 
	{ 
		get { return isRunning; } 
	}

	MonoBehaviour monoBehaviour;
	IEnumerator iEnumerator;

	public WrappedCoroutine(MonoBehaviour monoBehaviour, IEnumerator iEnumerator)
	{
		this.monoBehaviour = monoBehaviour;
		this.iEnumerator = iEnumerator;
	}

	public void Play()
	{
		if(!isRunning) monoBehaviour.StartCoroutine (_Play(monoBehaviour, iEnumerator));
	}

	private IEnumerator _Play(MonoBehaviour monoBehaviour, IEnumerator ienumerator)
	{
		isRunning = true;
		yield return monoBehaviour.StartCoroutine (ienumerator);
		isRunning = false;
	}
}
