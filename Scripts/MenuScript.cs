using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void exitpressed() {
        //we quit!
        Debug.Log("we quit!");
        Application.Quit();
    }

     public void playpressed() {
        SceneManager.LoadScene("Main");
    }
}
