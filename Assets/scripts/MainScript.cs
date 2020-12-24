using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{

    public Text debugText;
    public Text timeText;
    public MainImageScript mainImagescript;

    private int playerType;
    private int gameRound;
    private int missionNum;
    private GameObject player;
    private Ray ray;
    private int ifPressTurn = 0; // 0 none, -1 left, 1 right

    private int totalSeconds = 0;
    private int finishSeconds = 0;
    private float timeFly = 0;

    private int playerSpeed = 0; // 0 stop, 1 speed up, 2 speed down
    private int playerMoveDuration = -1;
    private float playerMoveParam;

    private GameObject timeTip;
    private GameObject missionTip;

    private int status = 0; // 0 start, 1 easterEgg, 2 counttoWin, 4 carCrush, 8 notFishsh, 9 finish

    // Use this for initialization
    void Start()
    {
        playerType = PlayerPrefs.GetInt("playerType");
        gameRound = PlayerPrefs.GetInt("gameRound");
        missionNum = Random.Range(1, 6);
        debugText.text = missionNum + "" + playerType + gameRound;

        //missionNum = 5;//temp code
        //playerType = 3; //temp code

        if (missionNum == 4 || missionNum == 5) {
            GameObject temp = GameObject.Find("signboardRight");
            temp.SetActive(false);
        }
        else {
            GameObject temp = GameObject.Find("signboardLeft");
            temp.SetActive(false);
        }

        if (playerType == 1)
        {
            playerMoveParam = 0.1f;
            player = GameObject.Find("player1");
            Destroy(GameObject.Find("player2"));
            Destroy(GameObject.Find("player3"));
            Destroy(GameObject.Find("player4"));
        }
        else if (playerType == 2)
        {
            playerMoveParam = -0.1f;
            player = GameObject.Find("player2");
            GameObject temp = GameObject.Find("Main Camera");
            temp.transform.parent = player.transform;
            temp.transform.position = player.transform.position;
            temp.transform.Translate(0,3,-10);
            Destroy(GameObject.Find("player1"));
            Destroy(GameObject.Find("player3"));
            Destroy(GameObject.Find("player4"));
        }
        else if (playerType == 3)
        {
            playerMoveParam = 0.3f;
            player = GameObject.Find("player3");
            GameObject temp = GameObject.Find("Main Camera");
            temp.transform.parent = player.transform;
            temp.transform.position = player.transform.position;
            temp.transform.Translate(0, 0, -6);
            Destroy(GameObject.Find("player1"));
            Destroy(GameObject.Find("player2"));
            Destroy(GameObject.Find("player4"));
        }
        else
        {
            player = GameObject.Find("player4");
            GameObject temp = GameObject.Find("Main Camera");
            temp.transform.parent = player.transform;
            temp.transform.position = player.transform.position;
            temp.transform.Translate(0, 1, -11);
            Destroy(GameObject.Find("player1"));
            Destroy(GameObject.Find("player2"));
            Destroy(GameObject.Find("player3"));
        }
        GameObject missionLight = GameObject.Find("MissionLight" + missionNum);
        missionLight.GetComponent<Renderer>().material.color = Color.green;
        GameObject missionCar = GameObject.Find("MissionCar" + missionNum);
        //Destroy(missionCar);
        missionCar.SetActive(false);
        GameObject missionPlane = GameObject.Find("MissionPlane" + missionNum);
        ray = new Ray(missionPlane.transform.position, Vector3.up);

        timeText.color = Color.green;

        GameObject canvasRoot = GameObject.Find("Canvas");
        timeTip = canvasRoot.transform.Find("TimeTip").gameObject;
        timeTip.SetActive(false);
        missionTip = canvasRoot.transform.Find("showMission").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /********************** status compute ************************/
        if (status == 0) {
            if (totalSeconds > 80)
            {
                mainImagescript.changeImage(1, 9);//"Time up, Challenge again?";
                mainImagescript.changeImage(2, 5);
                status = 8;
            } else if (totalSeconds > 60) {
                timeTip.SetActive(true);
            }

        } else if (status == 1) {
            if (finishSeconds >= 3)
            {
                mainImagescript.changeImage(1, 8);//"Good job, EasterEgg";
                mainImagescript.changeImage(2, 1);
                status = 9;
            }
        }
        else if (status == 2)
        {
            if (finishSeconds >= 3)
            {
                mainImagescript.changeImage(1, 7);//"Good job, Challenge again with other vechicel?";
                mainImagescript.changeImage(2, 1);
                status = 9;
            }


        }
        else if(status == 4) {
            mainImagescript.changeImage(1, 10);//"Car crushed, Challenge again?";
            mainImagescript.changeImage(2, 3);
            status = 8;
        }


        /********************** time count ************************/
        timeFly = timeFly + Time.deltaTime * 15;
        if (timeFly > 20)
        {
            timeFly = 0;
            if (status == 0)
            {
                timeText.color = Color.green;
                totalSeconds = totalSeconds + 1;
                timeText.text = "Time left: " + (80 - totalSeconds);
                if (missionTip.activeSelf == true) {
                    missionTip.SetActive(false);
                } else {
                    missionTip.SetActive(true);
                }
            }
            else if (status == 2 || status == 1)
            {
                timeText.color = Color.red;
                finishSeconds = finishSeconds + 1;
                timeText.text = "Finish at: " + finishSeconds;
            }
        }

        /********************** hit compute ************************/
        if (checkEasterEgghit(player.transform.position.x, player.transform.position.z) == 1) {
            if (status == 0) status = 1;
        } else {
            if (status == 1)
            {
                status = 0;
                finishSeconds = 0;
            }
            if (playerType == 4)
            {
                if (checkPlayer4hit(player.transform.position.x, player.transform.position.z) == 1)
                {
                    if (status == 0) status = 2;
                }
                else
                {
                    if (status == 2)
                    {
                        status = 0;
                        finishSeconds = 0;
                    }
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    string hitName = hit.collider.gameObject.name;

                    if (hitName.Length > 6 && hitName.Substring(0, 6) == "player")
                    {
                        if (status == 0) status = 2;
                    }
                    else
                    {
                        if (status == 2 || status == 1) status = 0;
                        finishSeconds = 0;
                    }
                }
            }
        }

        /********************** input key ************************/
        if (status == 0 || status == 1 || status == 2)
        {
            //debugText.text = "aaaa" + player.transform.position.x + " " + player.transform.position.y + " " + player.transform.position.z;
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    mainImagescript.changeImage(1, 7);//"Good job, Challenge again with other vechicel?";
            //    mainImagescript.changeImage(2, 5);
            //    status = 9;
            //}
            if (playerType == 4)
            {
                // playerType4 move specific process
                if (Input.GetKey("up") || Input.GetKey(KeyCode.W))
                {
                    player.transform.Translate(0, 0.3f, 0);
                    if (ifPressTurn != 0) player.transform.Rotate(0, 0, 2 * ifPressTurn);
                }
                else if (Input.GetKey("down") || Input.GetKey(KeyCode.S))
                {
                    player.transform.Translate(0, -0.3f, 0);
                    if (ifPressTurn != 0) player.transform.Rotate(0, 0, -2 * ifPressTurn);
                }
            }
            else
            {
                // playerType 1 / 2 / 3
                if (Input.GetKeyDown("up") || Input.GetKeyDown(KeyCode.W))
                {
                    playerSpeed = 1;
                    playerMoveDuration = totalSeconds;
                }
                if (Input.GetKeyUp("up") || Input.GetKeyUp(KeyCode.W))
                {
                    playerSpeed = 0;
                    playerMoveDuration = totalSeconds;
                }

                if (playerSpeed == 1)
                {
                    if (totalSeconds - playerMoveDuration < 1)
                    {
                        player.transform.Translate(0, 0, playerMoveParam);
                    }
                    else if (totalSeconds - playerMoveDuration < 2)
                    {
                        player.transform.Translate(0, 0, playerMoveParam * 2);
                    }
                    else
                    {
                        player.transform.Translate(0, 0, playerMoveParam * 3);
                    }
                    if (ifPressTurn != 0) player.transform.Rotate(0, 1 * ifPressTurn, 0);
                }
                //else if (playerSpeed == 2)
                //{
                //    if (totalSeconds - playerMoveDuration < 1)
                //    {
                //        player.transform.Translate(0, 0, playerMoveParam / 2);
                //    }
                //    else
                //    {
                //        playerSpeed = 0;
                //    }
                //    if (ifPressTurn != 0) player.transform.Rotate(0, 3 * ifPressTurn, 0);
                //}
                if (Input.GetKey("down") || Input.GetKey(KeyCode.S))
                {
                    playerSpeed = 0;
                    player.transform.Translate(0, 0, playerMoveParam * -1);
                    if (ifPressTurn != 0) player.transform.Rotate(0, -1 * ifPressTurn, 0);
                }
                if (playerSpeed == 0)
                {
                    //player3 no need speed for turn
                    if (ifPressTurn != 0) player.transform.Rotate(0, 1 * ifPressTurn, 0);
                }
            }

            if (Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.A))
            {
                ifPressTurn = -1;
            }
            else if (Input.GetKeyUp("left") || Input.GetKeyUp(KeyCode.A))
            {
                ifPressTurn = 0;
            }
            else if (Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D))
            {
                ifPressTurn = 1;
            }
            else if (Input.GetKeyUp("right") || Input.GetKeyUp(KeyCode.D))
            {
                ifPressTurn = 0;
            }
            if (Input.anyKey == false)
            {
                player.transform.position = player.transform.position;
            }
        }
        else if (status == 8 || status == 9)
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                mainImagescript.removeImage();
                if (status == 9) gameRound = gameRound + 1;
                PlayerPrefs.SetInt("gameRound", gameRound);
                PlayerPrefs.SetString("FromScene", "Main");
                SceneManager.LoadScene("Opening");
            }
        }

    }

    public void changeStatus(int status)
    {
        this.status = status;
    }

    private int checkEasterEgghit(float x, float z)
    {
        if (x > 103 && x < 106 && z > 74 && z < 77) {
            return 1;
        } else {
            return 0;
        }
    }

    private int checkPlayer4hit(float x, float z) {
        if (missionNum == 1) {
            if (x > 176 && x < 179 && z > 96 && z < 103) {
                return 1;
            } else {
                return 0;
            }
        }
        else if (missionNum == 2)
        {
            if (x > 117 && x < 123 && z > 106 && z < 108)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else if (missionNum == 3)
        {
            if (x > 104 && x < 111 && z > 94 && z < 96)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else if (missionNum == 4)
        {
            if (x > 78 && x < 84 && z > 92 && z < 94)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (x > -124 && x < -119 && z > 124 && z < 126)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
    }
}
