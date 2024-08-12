using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CustomCoroutine : MonoBehaviour
{
    public UnityEvent routineToStart;
    public float timeToStart = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartRoutine());
    }

    // Update is called once per frame
    IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(timeToStart);
        routineToStart?.Invoke();
    }
}
