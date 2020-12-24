using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class PassagewayScript : MonoBehaviour {

    public Text debugText;
    public MainImageScript mainImagescript;

    private int playerType;
    private int gameRound;
    private GameObject mainCamera;
    private float timeFly = 0;

    private int status = 0; //0 go front, 1 turn, 2 show pic, 4 navi

    // Use this for initialization
    void Start () {
        playerType = PlayerPrefs.GetInt("playerType");
        gameRound = PlayerPrefs.GetInt("gameRound");
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update () {
        if (status == 0)
        {
            debugText.text = "aaa" + mainCamera.transform.position.x + " " + mainCamera.transform.position.y + " " + mainCamera.transform.position.z;
            timeFly = timeFly + Time.deltaTime * 30;
            if (timeFly > 1)
            {
                if (mainCamera.transform.position.z < 0)
                {
                    mainCamera.transform.Translate(-0.5f, -0.1f, 0);
                }
                else
                {
                    status = 1;
                }
                timeFly = 0;
            }
        }
        else if (status == 1)
        {
            debugText.text = "bbb" + mainCamera.transform.rotation.x + " " + mainCamera.transform.rotation.y + " " + mainCamera.transform.rotation.z;
            timeFly = timeFly + Time.deltaTime * 30;
            if (timeFly > 1)
            {
                if (mainCamera.transform.rotation.y > -0.2)
                {
                    mainCamera.transform.Rotate(0, -0.5f, 0);
                }
                else
                {
                    timeFly = 0;
                    status = 2;
                }
            }
        }
        else if (status == 2)
        {
            debugText.text = "ccc" + timeFly;
            timeFly = timeFly + Time.deltaTime * 30;
            if (timeFly > 1 && timeFly < 2)
            {
                mainImagescript.changeImage(1, 1);
                mainImagescript.changeImage(2, 0);
            } else if (timeFly > 60) {
                status = 3;
                timeFly = 0;
            }
        }
        else
        {
            PlayerPrefs.SetInt("playerType", playerType);
            PlayerPrefs.SetInt("gameRound", gameRound);
            PlayerPrefs.SetString("FromScene", "Passageway");
            SceneManager.LoadScene("MainScene");
        }
    }
}
