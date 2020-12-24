using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class OpeningScript : MonoBehaviour {

    public Text debugText;
    public Text selectText1;
    public Text selectText2;
    public Text skillText;
    public MainImageScript mainImagescript;
    public Sprite[] imageList;

    private int gameRound = 1;
    private GameObject player;
    private float timeFly = 0;

    private GameObject playerImageL;
    private GameObject playerImageM;
    private GameObject playerImageR1;
    private GameObject playerImageR2;

    private int selectedPlayer = 1;
    private int status = 0; //0 init; 1 player run, 2 pic show, 3 buttonshow

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetString("FromScene") == "Main")
        {
            gameRound = PlayerPrefs.GetInt("gameRound");
            PlayerPrefs.DeleteAll();
        }
        GameObject canvasRoot = GameObject.Find("Canvas");
        playerImageM = canvasRoot.transform.Find("playTypeMiddle").gameObject;
        playerImageL = canvasRoot.transform.Find("playTypeLeft").gameObject;
        playerImageR1 = canvasRoot.transform.Find("playTypeRight1").gameObject;
        playerImageR2 = canvasRoot.transform.Find("playTypeRight2").gameObject;
        if (gameRound == 1)
        {
            playerImageL.GetComponent<Image>().overrideSprite = imageList[2];
            playerImageR1.GetComponent<Image>().overrideSprite = imageList[0];
            playerImageR2.GetComponent<Image>().overrideSprite = imageList[1];
        }
        else if (gameRound == 2)
        {
            playerImageL.GetComponent<Image>().overrideSprite = imageList[2];
        }
        playerImageM.SetActive(false);
        playerImageL.SetActive(false);
        playerImageR1.SetActive(false);
        playerImageR2.SetActive(false);

        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update () {
        if (status == 0)
        {
            timeFly = timeFly + Time.deltaTime * 30;
            if (timeFly > 5)
            {
                timeFly = 0;
                status = 1;
            }
        }
        else if (status == 1)
        {
            timeFly = timeFly + Time.deltaTime * 30;
            if (timeFly > 1)
            {
                if (player.transform.position.x > -20)
                {
                    player.transform.Translate(0, 0, 1);
                }
                else
                {
                    status = 2;
                }
                timeFly = 0;
            }
        }
        else if (status == 2)
        {
            mainImagescript.changeImage(2, 0);
            playerImageM.SetActive(true);
            playerImageL.SetActive(true);
            playerImageR1.SetActive(true);
            playerImageR2.SetActive(true);
            selectText1.text = "[←] [→] to switch vehicle";
            selectText2.text = "[SPACE] to select, [ESC] to exit";
            showSkill();
            status = 3;
        }
        else if (status == 3)
        {
            if (Input.GetKeyDown("right"))
            {
                selectPlayer(1);
            }
            else if (Input.GetKeyDown("left"))
            {
                selectPlayer(-1);
            }
            else if (Input.GetKeyDown("space"))
            {
                if ((gameRound == 1 && selectedPlayer < 2)
                    || (gameRound == 2 && selectedPlayer < 4)
                    || (gameRound > 2))
                {
                    PlayerPrefs.SetInt("playerType", selectedPlayer);
                    PlayerPrefs.SetInt("gameRound", gameRound);
                    PlayerPrefs.SetString("FromScene", "Opening");
                    if (gameRound == 1)
                    {
                        SceneManager.LoadScene("Passageway");
                    } else {
                        SceneManager.LoadScene("MainScene");
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    void selectPlayer(int move) {
        if (move == 1) {
            Sprite tempSprite = playerImageL.GetComponent<Image>().overrideSprite;
            playerImageL.GetComponent<Image>().overrideSprite = playerImageM.GetComponent<Image>().overrideSprite;
            playerImageM.GetComponent<Image>().overrideSprite = playerImageR1.GetComponent<Image>().overrideSprite;
            playerImageR1.GetComponent<Image>().overrideSprite = playerImageR2.GetComponent<Image>().overrideSprite;
            playerImageR2.GetComponent<Image>().overrideSprite = tempSprite;
            if (selectedPlayer < 4) {
                selectedPlayer = selectedPlayer + 1;
            } else {
                selectedPlayer = 1;
            }
        }
        else {
            Sprite tempSprite = playerImageR2.GetComponent<Image>().overrideSprite;
            playerImageR2.GetComponent<Image>().overrideSprite = playerImageR1.GetComponent<Image>().overrideSprite;
            playerImageR1.GetComponent<Image>().overrideSprite = playerImageM.GetComponent<Image>().overrideSprite;
            playerImageM.GetComponent<Image>().overrideSprite = playerImageL.GetComponent<Image>().overrideSprite;
            playerImageL.GetComponent<Image>().overrideSprite = tempSprite;
            if (selectedPlayer > 1)
            {
                selectedPlayer = selectedPlayer - 1;
            }
            else
            {
                selectedPlayer = 4;
            }
        }
        showSkill();
    }

    private void showSkill() {
        if (selectedPlayer == 1)
        {
            skillText.text = "Skill: Pretty";
        } else if (selectedPlayer == 2)
        {
            if (gameRound < 2)
            {
                skillText.text = "Skill: ???";
            }
            else {
                skillText.text = "Skill: Un-crushed!";
            }
        }
        else if (selectedPlayer == 3)
        {
            if (gameRound < 2)
            {
                skillText.text = "Skill: ???";
            }
            else
            {
                skillText.text = "Skill: High speed";
            }
        }
        else
        {
            skillText.text = "Skill: ???";
        }
    }
}
