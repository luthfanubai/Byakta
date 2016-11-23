using UnityEngine;
using System.Collections;


public class WrappedCoroutine
{
    public delegate IEnumerator Routine();
    private Routine routine;

    private MonoBehaviour monobehaviour;

    public bool IsRunning
    {
        get;
        private set;
    }

    public WrappedCoroutine(MonoBehaviour mono, Routine routine)
    {
        this.monobehaviour = mono;
        this.routine = routine;
    }

    public void Play()
    {
        if (!IsRunning)
        {
            monobehaviour.StartCoroutine(_Play());
        }
    }

    private IEnumerator _Play()
    {
        IsRunning = true;
        yield return monobehaviour.StartCoroutine(routine());
        IsRunning = false;

    }




}


/////OLD WrappedCoroutine
//class WrappedCoroutine
//{
//	bool isRunning;
//	public bool IsRunning 
//	{ 
//		get { return isRunning; } 
//	}
//
//	MonoBehaviour monoBehaviour;
//	IEnumerator iEnumerator;
//
//	public WrappedCoroutine(MonoBehaviour monoBehaviour, IEnumerator iEnumerator)
//	{
//		this.monoBehaviour = monoBehaviour;
//		this.iEnumerator = iEnumerator;
//	}
//
//	public void Play()
//	{
//		if(!isRunning) monoBehaviour.StartCoroutine (_Play(monoBehaviour, iEnumerator));
//	}
//
//	private IEnumerator _Play(MonoBehaviour monoBehaviour, IEnumerator ienumerator)
//	{
//		isRunning = true;
//		yield return monoBehaviour.StartCoroutine (ienumerator);
//		isRunning = false;
//	}
//}
