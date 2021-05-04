using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{

    private void Start()
    {
        
    }

    //public TMP_Text score;
    void Update()
    {
        //GameObject p = GameObject.Find("result");
        transform.GetComponent<TMPro.TextMeshProUGUI>().text = GameObject.Find("info").GetComponent<Text>().text;
    }

    public void exitpressed() {
        //we quit!
        Debug.Log("we quit!");
        Application.Quit();
    }

     public void playpressed() {
        SceneManager.LoadScene("MainMenu");
    }
}
