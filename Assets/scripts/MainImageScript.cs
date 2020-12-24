using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainImageScript : MonoBehaviour
{
    public Text debugText;

    public Sprite[] imageList;

    private GameObject showImage1;
    private GameObject showImage2;
    private GameObject showImage3;

    private int haveImage1 = 0;
    private int haveImage2 = 0;
    private int haveImage3 = 0;
    private float timeFly = 0;
    private float timeFly2 = 0;

    // Use this for initialization
    void Start()
    {
        GameObject canvasRoot = GameObject.Find("Canvas");
        showImage1 = canvasRoot.transform.Find("showImage1").gameObject;
        showImage2 = canvasRoot.transform.Find("showImage2").gameObject;
        //showImage3 = canvasRoot.transform.Find("showImage3").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timeFly = timeFly + Time.deltaTime * 30;
        if (timeFly > 1)
        {
            if (haveImage1 == 1)
            {
                showImage1.SetActive(true);
            }
            else if (haveImage1 == 0)
            {
                showImage1.SetActive(false);
            }
            if (haveImage2 == 1)
            {
                if (showImage2.transform.position.y < 440) {
                    showImage2.transform.Translate(0, 40, 0);
                } else {
                    timeFly2 = timeFly2 + timeFly;
                    if (timeFly2 > 10)
                    {
                        timeFly2 = 0;
                        if (imageList.Length > 1 && showImage2.GetComponent<Image>().overrideSprite == imageList[1])
                        {
                            showImage2.GetComponent<Image>().overrideSprite = imageList[2];
                        }
                        else if (imageList.Length > 2 && showImage2.GetComponent<Image>().overrideSprite == imageList[2])
                        {
                            showImage2.GetComponent<Image>().overrideSprite = imageList[1];
                        }
                        else if (imageList.Length > 3 && showImage2.GetComponent<Image>().overrideSprite == imageList[3])
                        {
                            showImage2.GetComponent<Image>().overrideSprite = imageList[4];
                        }
                        else if (imageList.Length > 4 && showImage2.GetComponent<Image>().overrideSprite == imageList[4])
                        {
                            showImage2.GetComponent<Image>().overrideSprite = imageList[3];
                        }
                        else if (imageList.Length > 5 && showImage2.GetComponent<Image>().overrideSprite == imageList[5])
                        {
                            showImage2.GetComponent<Image>().overrideSprite = imageList[6];
                        }
                        else if (imageList.Length > 6 && showImage2.GetComponent<Image>().overrideSprite == imageList[6])
                        {
                            showImage2.GetComponent<Image>().overrideSprite = imageList[5];
                        }
                    }
                }
            }
            else if (haveImage2 == 0 && showImage2.transform.position.y > -250)
            {
                showImage2.transform.Translate(0, -40, 0);
            }
            //if (haveImage3 == 1 && showImage3.transform.position.y < 240)
            //{
            //    showImage3.transform.Translate(0, 40, 0);
            //}
            //else if (haveImage3 == 0 && showImage3.transform.position.y > -250)
            //{
            //    showImage3.transform.Translate(0, -40, 0);
            //}
            timeFly = 0;
        }

    }

    public void changeImage(int pos, int imageNum)
    {
        if (pos == 1)
        {
            this.haveImage1 = 1;
            showImage1.GetComponent<Image>().overrideSprite = imageList[imageNum];
        }
        else if (pos == 2)
        {
            this.haveImage2 = 1;
            showImage2.GetComponent<Image>().overrideSprite = imageList[imageNum];
        }
        //else
        //{
        //    this.haveImage3 = 1;
        //    showImage3.GetComponent<Image>().overrideSprite = imageList[imageNum];
        //}
    }

    public void removeImage()
    {
        this.haveImage1 = 0;
        this.haveImage2 = 0;
        this.haveImage3 = 0;
    }
}