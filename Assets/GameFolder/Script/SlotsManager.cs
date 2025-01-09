using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlotsManager : MonoBehaviour
{
    public LeanTweenType type;
    public GameObject logo;
    public Image winItemImage;
    public Image fillImage;
    public List<Image> itemsImage;
    public List<Sprite> itemsSprite;
    public float animationDuration = 1f;

    public Button homeButton, startButton, exitButton, backButton, spinButton, settingButton,soundButton,increasButton,decreasButton;
    public Text totalAmountText, betAmountText, winAmountText;
    public int totalAmount = 10000;
    public int betAmount = 100;
    public int winAmount = 0;
    public GameObject movePos;
    
   // public List<ParticleSystem> particle;

    public GameObject machine, homePanel, settingPanel,winPanel;
    void Start()
    {
        logo.LeanMoveLocalX(150,4f).setFrom(-150).setEase(type).setLoopPingPong();
        logo.LeanRotateZ(17f, 0.3f).setFrom(2f).setEase(type).setLoopPingPong();
      StartCoroutine(PlayAnimation(itemsSprite[5]));
        UpdateTextUI();
       //StartCoroutine( LineStartAnimation());
        spinButton.onClick.AddListener(()=>StartCoroutine(Spin()));
        startButton.onClick.AddListener(() => OnClickStart(1 > 0,startButton));
        homeButton.onClick.AddListener(() => OnClickStart(0 > 1,homeButton));
        backButton.onClick.AddListener(() => OnClickSetting(0 > 1, backButton));
        settingButton.onClick.AddListener(() => OnClickSetting(1>0, settingButton));
        exitButton.onClick.AddListener(Application.Quit);
        increasButton.onClick.AddListener(() => ChangeBetAmount(1 > 0));
        decreasButton.onClick.AddListener(() => ChangeBetAmount(1 < 0));
     

        foreach (Image i in itemsImage)
        {
            i.sprite = itemsSprite[Random.Range(0, itemsSprite.Count)];
            LeanTween.moveLocalX(i.gameObject, i.gameObject.transform.localPosition.x, 0.5f).setFrom(movePos.transform.localPosition.x).setEaseSpring()
                .setOnComplete(() =>
                {
                    LeanTween.scaleX(i.gameObject, 1f, 0.5f).setFrom(0f).setEaseInQuint();
                });  
        }
    }
    private void AnimateFill(float from, float to)
    {
        fillImage.fillAmount = from; // Set initial value
        LeanTween.value(gameObject, from, to, animationDuration)
            .setOnUpdate((float value) =>
            {
                fillImage.fillAmount = value; // Update fillAmount
            });
            //.setOnComplete(() =>
            //{
            //    if (to == 1f) // If we just finished going to 1, animate back to 0
            //    {
            //        AnimateFill(1f, 0f);
            //    }
            //    else
            //    {
            //        AnimateFill(0f, 1f);
            //    }
            //    // No further action after reaching 0
            //});
    }
    void UpdateTextUI() {
        totalAmountText.text = totalAmount.ToString();
        betAmountText.text = betAmount.ToString();
        winAmountText.text = winAmount.ToString();
    }

    IEnumerator Spin()
    {
        if(totalAmount >= betAmount)
        {
            totalAmount -= betAmount;
            UpdateTextUI();
        }

        AniamtionButton(spinButton.gameObject);
        spinButton.interactable = homeButton.interactable = increasButton.interactable = decreasButton.interactable = settingButton.interactable = false;
        

        machine.LeanScale(Vector3.one * -1f, 0.3f).setFrom(Vector3.one).setEaseInOutCubic().setOnComplete(()=> { machine.LeanScale(Vector3.one, 0.3f).setEaseInOutCubic(); });
        AnimateFill(1, 0);
        yield return new WaitForSeconds(animationDuration);

        foreach (Image i in itemsImage)
        { i.gameObject.SetActive(false); }
            yield return new WaitForSeconds(0.3f);
        foreach(Image i in itemsImage) {
            i.sprite = itemsSprite[Random.Range(0, itemsSprite.Count)];
            i.gameObject.SetActive(true);
            LeanTween.moveLocalY(i.gameObject, i.gameObject.transform.localPosition.y, 0.5f).setFrom(i.gameObject.transform.localPosition.y - 1000).setEaseSpring()
                .setOnComplete(() =>
                {
                    LeanTween.scale(i.gameObject,Vector3.one, 0.5f).setFrom(Vector3.one * 4).setEaseInQuint();
                });
           
            yield return new WaitForSeconds(1f);
        }
        AnimateFill(0, 1);
        yield return new WaitForSeconds(animationDuration);
        CheckWin();
        
    }
    //024
    //135
    public List<GameObject> lineR = new List<GameObject>();
    public List<GameObject> lineC = new List<GameObject>();
    public List<GameObject> lineX = new List<GameObject>();
    IEnumerator LineStartAnimation()
    {
        foreach(GameObject r in lineR)
        {
            LeanTween.scaleX(r, 1, 0.2f).setFrom(0).setEaseInCubic();
            yield return new WaitForSeconds(0.2f);
        }
        foreach (GameObject r in lineC)
        {
            LeanTween.scaleX(r, 1, 0.2f).setFrom(0).setEaseInCubic();
            yield return new WaitForSeconds(0.2f);
        }
        foreach (GameObject r in lineX)
        {
            LeanTween.scaleX(r, 1, 0.2f).setFrom(0).setEaseInCubic();
            yield return new WaitForSeconds(0.2f);
        }
    }
    void CheckWin()
    {
        
           if(
            itemsImage[0].sprite.name == itemsImage[2].sprite.name &&
            itemsImage[2].sprite.name == itemsImage[4].sprite.name)
        {
            totalAmount += betAmount * 4;
            winAmount = betAmount * 3;
            
            print("you win X3");
            StartCoroutine(PlayAnimation(itemsImage[0].sprite));
        }

        if (itemsImage[1].sprite.name == itemsImage[3].sprite.name &&
            itemsImage[3].sprite.name == itemsImage[5].sprite.name)
        {
            totalAmount += betAmount * 4;
            winAmount = betAmount * 3;
            print("you win X3");
            StartCoroutine(PlayAnimation(itemsImage[1].sprite));
        }

        if (itemsImage[0].sprite.name == itemsImage[3].sprite.name &&
           itemsImage[3].sprite.name == itemsImage[4].sprite.name)
        {
            totalAmount += betAmount * 4;
            winAmount = betAmount * 3;
            print("you win X3");
            StartCoroutine(PlayAnimation(itemsImage[1].sprite));
        }

        if (itemsImage[1].sprite.name == itemsImage[2].sprite.name &&
           itemsImage[2].sprite.name == itemsImage[5].sprite.name)
        {
            totalAmount += betAmount * 4;
            winAmount = betAmount * 3;
            print("you win X3");
            StartCoroutine(PlayAnimation(itemsImage[1].sprite));
        }

        //024
        //135
        if (itemsImage[4].sprite.name == itemsImage[5].sprite.name &&
            itemsImage[3].sprite.name == itemsImage[4].sprite.name &&
            itemsImage[2].sprite.name == itemsImage[3].sprite.name
            )
        {
            totalAmount += betAmount * 5;
            winAmount = betAmount * 4;
            print("you win X4");
            StartCoroutine(PlayAnimation(itemsImage[0].sprite));
        }
        if (itemsImage[0].sprite.name == itemsImage[1].sprite.name &&
            itemsImage[1].sprite.name == itemsImage[2].sprite.name &&
            itemsImage[2].sprite.name == itemsImage[3].sprite.name
            )
        {
            totalAmount += betAmount * 5;
            winAmount = betAmount * 4;
            print("you win X4");
            StartCoroutine(PlayAnimation(itemsImage[0].sprite));
        }
        else
        {
            spinButton.interactable = homeButton.interactable = increasButton.interactable = decreasButton.interactable = settingButton.interactable = true;
            print("you lose");
        }
        UpdateTextUI();
    }

    void PlayPartical(Sprite s)
    {
        //particle.ForEach(p =>
        //{
        //    p.Play();
        //    p.textureSheetAnimation.SetSprite(0, s);
        //});
    }
   
    IEnumerator PlayAnimation(Sprite s)
    {
        GameObject panel = winPanel.transform.GetChild(0).gameObject;
        winItemImage.sprite = s;
        machine.LeanMoveLocalY(Screen.height + machine.transform.localPosition.y, 0.5f)
         .setEaseInQuint()
         .setOnComplete(
         () =>
         {
             winPanel.SetActive(true);
             winPanel.LeanMoveLocalY(winPanel.transform.localPosition.y, 0.3f).setFrom(-Screen.height - winPanel.transform.localPosition.y)
             .setEaseInQuint()
             .setOnComplete(() =>
             {
                 winItemImage.gameObject.LeanScaleY(1.54f, 0.3f).setFrom(0).setEase(LeanTweenType.easeSpring)
                .setOnComplete(() =>
                {

                   panel.LeanMoveLocalY(0, 0.3f).setFrom(Screen.height + panel.transform.localPosition.y)
                     .setEaseInQuint();

                });
                 
             });
            

             // PlayPartical(s);
         });
         
       
        
        yield return new WaitForSeconds(Random.Range(4, 5));
        winItemImage.gameObject.LeanScaleY(0, 0.3f).setFrom(1.54f).setEase(LeanTweenType.easeSpring)
           .setOnComplete(() =>
           {
               panel.LeanMoveLocalY(Screen.height + panel.transform.localPosition.y, 0.3f).setFrom(0)
             .setEaseOutQuint()
             .setOnComplete(() =>
             {
                 winPanel.LeanMoveLocalY(-Screen.height - winPanel.transform.localPosition.y, 0.5f)
          .setEaseInQuint()
          .setOnComplete(
          () =>
          {
              winPanel.SetActive(false);
              machine.LeanMoveLocalY(-8.26f, 0.3f)
              .setEaseInQuint().setOnComplete(() =>
              {
                  spinButton.interactable = homeButton.interactable = increasButton.interactable = decreasButton.interactable = settingButton.interactable = true;
              });

          })
          ;
             })
            ;
           });
               
       
        
    }

    void OnClickStart(bool isStart,Button b) {
       
        b.interactable = false;
        if (!isStart)
        {
            homePanel.SetActive(true);
        }
        GameObject logo = homePanel.transform.GetChild(0).gameObject;
        GameObject start = homePanel.transform.GetChild(1).gameObject;
        GameObject exit = homePanel.transform.GetChild(2).gameObject;
        if (isStart)
        {
            AniamtionButton(startButton.gameObject);
            start.LeanScaleX(0, 1f).setEaseOutQuart();
            exit.LeanScaleX(0, 1f).setEaseOutQuart();
            logo.LeanScaleX(0, 1f).setEaseOutQuart()
                .setOnComplete(() => {
                    b.interactable = true;
                    homePanel.SetActive(false);
                })
                ;
        }
        else
        {
            AniamtionButton(homeButton.gameObject);
            exit.LeanScaleX(1, 0.5f).setEaseInQuart();
            start.LeanScaleX(1, 0.5f).setEaseInQuart();
            logo.LeanScaleX(1, 0.5f).setEaseInQuart().setOnComplete(() => { b.interactable = true; })
                ;
        }
       
    }

    void OnClickSetting(bool isStart, Button b)
    {
      
        b.interactable = false;
       
        GameObject panel = settingPanel.transform.GetChild(0).gameObject;
        if (isStart)
        {
            AniamtionButton(settingButton.gameObject);
            settingPanel.SetActive(true);
            panel.LeanScaleX(1f, 0.5f).setEaseOutQuart()
                .setOnComplete(() => { b.interactable = true; })
                ;
        }
        else
        {
            panel.LeanScaleX(0f, 0.5f).setEaseOutQuart().setOnComplete(() => { b.interactable = true; settingPanel.SetActive(false); })
                ;
        }

    }

    void ChangeBetAmount(bool isIncreas)
    {
      
        if (isIncreas)
        {
            AniamtionButton(increasButton.gameObject);
            if (betAmount >= 1000)
            {
                betAmount = 100;
            }
            else
            {
               
                betAmount += 100;
            }
        }
        else
        {
            AniamtionButton(decreasButton.gameObject);
            if (betAmount <= 100)
            {
                betAmount = 1000;
            }
            else
            {
                betAmount -= 100;
            }
        }
        betAmountText.text = betAmount.ToString();
    }

    public AudioSource musicSource, sfxSource;
    public Image soundProgrees;
    public Image musicProgrees;
    int mixS = 10;
    int mixM = 10;
    public void OnClickSound(bool isAdd)
    {
        if (mixS < 10 && isAdd)
        {
            // mixS
            
            //soundProgrees[mixS].SetActive(true);
            mixS += 1;
        }
        else if(!isAdd && mixS > 0)
        {
           
            //soundProgrees[mixS-1].SetActive(false);
            mixS -= 1;
        }
        sfxSource.volume = (float)mixS / 10;
        soundProgrees.fillAmount = (float)mixS / 10;
    }

    public void OnClickMusic(bool isAdd)
    {
        if (mixM < 10 && isAdd)
        {
            // mixS

            //musicProgrees[mixM].SetActive(true);
            mixM += 1;
        }
        else if (!isAdd && mixM > 0)
        {

            //musicProgrees[mixM - 1].SetActive(false);
            mixM -= 1;
        }

        musicSource.volume = (float)mixM / 10;
        musicProgrees.fillAmount = (float)mixM / 10;
    }

    void AniamtionButton(GameObject g)
    {
        g.gameObject.LeanScaleX(0, 0.3f)
           .setEaseOutExpo()
           .setOnComplete(() =>
           {
               g.gameObject.LeanScaleX(1f, 0.3f).setEaseInExpo();
           });
    }

}
