using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [Header("Mission Functionalities")]
    public static bool isMission;
    public UnityEvent OnEnableMission;
    public UnityEvent OnDisableMission;

    public Animator objectiveMenu;
    public string isOpen = "isOpen";


    public void EnableMissionHandler()
    {
        objectiveMenu.SetBool(isOpen, true);
        OnEnableMission?.Invoke();

        isMission = true;
    }
    public void DisableMissionHandler()
    {
        OnDisableMission?.Invoke();
        objectiveMenu.SetBool(isOpen, false);
        isMission = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isMission == true)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

}
