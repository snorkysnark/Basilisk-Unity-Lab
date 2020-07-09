using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    [SerializeField] KeyCode quitKey = KeyCode.Escape;
    [SerializeField] float doubleClickTime = 0.2f;

    private float lastClickTime = 0;

    private void Update()
    {
        if(Input.GetKeyDown(quitKey))
        {
            if(Time.time - lastClickTime <= doubleClickTime)
            {
                QuitGame();
            } else
            {
                lastClickTime = Time.time;
            }
        }
    }

    private static void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif  
    }
}
