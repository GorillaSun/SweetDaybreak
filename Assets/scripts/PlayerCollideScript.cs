using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollideScript : MonoBehaviour {

    public Text damageText;
    public MainScript mainscript;
    public Sprite[] imageList;

    private int totalDamages = 0;
    private GameObject damagePic;
    private GameObject damageTip;

    // Use this for initialization
    void Start () {
        damageText.text = "0";
        GameObject canvasRoot = GameObject.Find("Canvas");
        damagePic = canvasRoot.transform.Find("carCollider").gameObject;
        damageTip = canvasRoot.transform.Find("DamageTip").gameObject;
        damageTip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (totalDamages > 60)
        {
            damagePic.GetComponent<Image>().overrideSprite = imageList[2];
            mainscript.changeStatus(4);
            totalDamages = 0;
        }
        else if (totalDamages > 40)
        {
            damagePic.GetComponent<Image>().overrideSprite = imageList[1];
            damageTip.SetActive(true);
        }
        else if (totalDamages > 15)
        {
            damagePic.GetComponent<Image>().overrideSprite = imageList[0];
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        totalDamages = totalDamages + 1;
        damageText.text = totalDamages + "";
    }

}
