using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class KritDRNonstopDebate : MonoBehaviour
{
    public KMSelectable CheckTruthBulletsSel, StartTrialSel;
    public KMSelectable TruthCilinderSelect, TruthBulletSelect;
    public KMSelectable MenuBullet1, MenuBullet2, MenuBullet3, MenuBullet4, MenuBullet5, MenuBullet6, ReturnToPrepBtn;
    public KMSelectable WarningCheckBulletsBtn, WarningStartTrialBtn;

    public KMBombModule ThisModule;

    public GameObject TruthBullet;
    public GameObject ArgIndicator;
    public GameObject TruthCylinder;
    public GameObject CounterBullet;
    public GameObject NoThatsWrongObject, MemoryObject;

    public TextMesh Argument;
    public TextMesh CountdownText;
    public TextMesh TruthBulletText;
    public TextMesh CounterBulletText, CounterBulletTextBackside;
    public TextMesh Name;

    public TextMesh[] ArgumentOutlines;
    public TextMesh[] CounterOutlines;
    public TextMesh BulletDescription; 

    public Renderer CheckTruthBulletsBar, StartTrialBar;
    public Renderer MenuBullet1Render, MenuBullet2Render, MenuBullet3Render, MenuBullet4Render, MenuBullet5Render, MenuBullet6Render, ReturnToPrepRender;
    public Renderer WarningCheckBulletsRender, WarningStartTrialRender;
    public Renderer AllRiseRenderer;
    public Renderer TruthCilinderAnim;
    public Renderer BulletState;
    public Renderer Background;
    public Renderer NoThatsWrongRender, MemoryRender;
    public Renderer MenuBulletImage;

    public Texture[] CilinderStates, MemoryStates;
    public Texture TruthInitialState, MemoryInitialState;
    public Texture BulletTruthState, BulletMemoryState;
    public Texture BackgroundTruthState, BackgroundMemoryState;
    public Texture[] NoThatsWrongSprites, MemorySprites;
    public Texture[] PossibleBulletImages;
    public Texture[] AllRiseAnimSprites;

    public GameObject GameplayScreen, PreparationScreen, BulletCheckScreen;
    public GameObject AllRiseObj;
    public GameObject WarningBox;
    public List<GameObject> WarningMessageText;

    public KMHighlightable BulletHighlight, CilinderHighlight;

    List<Vector3> ArgPositions = new List<Vector3>()
    {
        new Vector3(0.7635f, 0.1032818f, -0.5215732f),  //Initial (Pos1)
        new Vector3(0.752f, 0.1032818f, -0.401f),       //2nd Arg (Pos2)
        new Vector3(0.735f, 0.1032818f, -0.27f),        //3rd Arg (Pos3)
        new Vector3(0.719f, 0.1032818f, -0.131f),       //4th Arg (Pos4)
        new Vector3(0.7098f, 0.1032818f, -0.0358f)      //Final   (Pos5)
    };


    List<string> OpeningArguments;

#pragma warning disable IDE0044 // Add readonly modifier
    List<string> ArgumentSubjects = new List<string>

    {
        "3D Tunnels",
        "Accumulation",
        "Benedict Cumberbatch",
        "Cheap Checkout",
        "Dr. Doctor",
        "European Travel",
        "Flip The Coin",
        "Grocery Store",
        "HTTP Response",
        "Ice Cream",
        "Listening",
        "Krazy Talk",
        "Module Homework",
        "Neutralization",
        "Only Connect",
        "Password",
        "Question Mark",
        "Radio",
        "Simon",
        "Tennis",
        "USA Maze",
        "Visual Impairment",
        "The Wire",
        "X-Ray",
        "Yahtzee",
        "Zoni",
    };

    public string[] AllArguments;
    public string[] AllNames;

    public List<string> TruthBullets;

    string CurrentBulletText;
    string CounterValue;

    public int CountdownSetup;
    int Countdown;
    int CurrentBullet = 0;
    int AmountOfBullets;
    int Refutations;
    int CurrentPos = 0;
    int CurrentArgument = 0;

    bool CheckedBullets;
    bool Reloading = false;
    bool Firing = false;
    bool SwappingBulletType = false;
    bool UsingMemory = false;

#pragma warning disable IDE0051 // Remove unused private members
    void Start()
    {
        Countdown = CountdownSetup;
        CheckTruthBulletsSel.OnHighlight = delegate
        {
            CheckTruthBulletsBar.material.color = new Color32(202, 58, 60, 0);
        };
        CheckTruthBulletsSel.OnHighlightEnded = delegate
        {
            CheckTruthBulletsBar.material.color = new Color32(33, 29, 33, 0);
        };

        StartTrialSel.OnHighlight = delegate
        {
            StartTrialBar.material.color = new Color32(202, 58, 60, 0);
        };
        StartTrialSel.OnHighlightEnded = delegate
        {
            StartTrialBar.material.color = new Color32(33, 29, 33, 0);
        };
        ThisModule.OnActivate = delegate
        {
            //Drop interactions here

            /*TruthCilinderSelect.OnInteractEnded = StopMemory;
            TruthCilinderSelect.OnInteract = Memory;
            TruthBulletSelect.OnInteract = BulletFire;*/ //Not these yet

            WarningCheckBulletsBtn.OnInteract = CheckBullets;
            WarningStartTrialBtn.OnHighlight = delegate { CheckedBullets = true; };
            WarningStartTrialBtn.OnInteract = StartTrial;
            //Drop interactions here

            CheckTruthBulletsSel.OnInteract = CheckBullets;
            ReturnToPrepBtn.OnInteract = ReturnToMenu;
            StartTrialSel.OnInteract = StartTrial;
            //StartCoroutine("CountdownCoroutine");
            //ArgumentAndBulletCreation();
            //StartCoroutine("SpinAnim");
        };

        int HoveredBullet = 0;

        MenuBullet1.OnHighlightEnded = delegate
        {
            HoveredBullet = 0;
            MenuBullet1Render.material.color = new Color32(33, 29, 33, 0);
        };
        MenuBullet2.OnHighlightEnded = delegate
        {
            HoveredBullet = 0;
            MenuBullet2Render.material.color = new Color32(33, 29, 33, 0);
        };
        MenuBullet3.OnHighlightEnded = delegate
        {
            HoveredBullet = 0;
            MenuBullet3Render.material.color = new Color32(33, 29, 33, 0);
        };
        MenuBullet4.OnHighlightEnded = delegate
        {
            HoveredBullet = 0;
            MenuBullet4Render.material.color = new Color32(33, 29, 33, 0);
        };
        MenuBullet5.OnHighlightEnded = delegate
        {
            HoveredBullet = 0;
            MenuBullet5Render.material.color = new Color32(33, 29, 33, 0);
        };
        MenuBullet6.OnHighlightEnded = delegate
        {
            HoveredBullet = 0;
            MenuBullet6Render.material.color = new Color32(33, 29, 33, 0);
        };
        ReturnToPrepBtn.OnHighlightEnded = delegate
        {
            HoveredBullet = 0;
            ReturnToPrepRender.material.color = new Color32(33, 29, 33, 0);
        };

        MenuBullet1.OnHighlight = delegate
        {
            HoveredBullet = 1;
            MenuBullet1.GetComponent<Renderer>().material.color = new Color32(202, 58, 60, 0);
            MenuBulletImage.material.mainTexture = PossibleBulletImages[HoveredBullet - 1];
            BulletDescription.text = "This is the description of Truth\nBullet #1. This will also contain\nthe important details!";
        };
        MenuBullet2.OnHighlight = delegate
        {
            HoveredBullet = 2;
            MenuBullet2Render.material.color = new Color32(202, 58, 60, 0);
            MenuBulletImage.material.mainTexture = PossibleBulletImages[HoveredBullet - 1];
            BulletDescription.text = "This is the description of Truth\nBullet #2. This will also contain\nthe important details!";
        };
        MenuBullet3.OnHighlight = delegate
        {
            HoveredBullet = 3;
            MenuBullet3Render.material.color = new Color32(202, 58, 60, 0);
            MenuBulletImage.material.mainTexture = PossibleBulletImages[HoveredBullet - 1];
            BulletDescription.text = "This is the description of Truth\nBullet #3. This will also contain\nthe important details!";
        };
        MenuBullet4.OnHighlight = delegate
        {
            HoveredBullet = 4;
            MenuBullet4Render.material.color = new Color32(202, 58, 60, 0);
            MenuBulletImage.material.mainTexture = PossibleBulletImages[HoveredBullet - 1];
            BulletDescription.text = "This is the description of Truth\nBullet #4. This will also contain\nthe important details!";
        };
        MenuBullet5.OnHighlight = delegate
        {
            HoveredBullet = 5;
            MenuBullet5Render.material.color = new Color32(202, 58, 60, 0);
            MenuBulletImage.material.mainTexture = PossibleBulletImages[HoveredBullet - 1];
            BulletDescription.text = "This is the description of Truth\nBullet #5. This will also contain\nthe important details!";
        };
        MenuBullet6.OnHighlight = delegate
        {
            HoveredBullet = 6;
            MenuBullet6Render.material.color = new Color32(202, 58, 60, 0);
            MenuBulletImage.material.mainTexture = PossibleBulletImages[HoveredBullet - 1];
            BulletDescription.text = "This is the description of Truth\nBullet #6. This will also contain\nthe important details!";
        };
        ReturnToPrepBtn.OnHighlight = delegate
        {
            HoveredBullet = 7;
            ReturnToPrepRender.material.color = new Color32(202, 58, 60, 0);
            MenuBulletImage.material.mainTexture = PossibleBulletImages[HoveredBullet - 1];
            BulletDescription.text = "Return to preparation and\nstart the Class Trial.";
        };

        WarningCheckBulletsBtn.OnHighlight = delegate
        {
            Debug.Log("Selected \"Check Truth Bullets\"");
            WarningCheckBulletsRender.material.color = new Color32(202, 58, 60, 0);
        };
        WarningCheckBulletsBtn.OnHighlightEnded = delegate
        {
            WarningCheckBulletsRender.material.color = new Color32(33, 29, 33, 0);
        };
        WarningStartTrialBtn.OnHighlight = delegate
        {
            Debug.Log("Selected \"Start Class Trial\"");
            WarningStartTrialRender.material.color = new Color32(202, 58, 60, 0);
        };
        WarningStartTrialBtn.OnHighlightEnded = delegate
        {
            WarningStartTrialRender.material.color = new Color32(33, 29, 33, 0);
        };

        WarningMessageText[0].SetActive(false);
        WarningMessageText[1].SetActive(false);
        WarningMessageText[2].SetActive(false);

        WarningBox.transform.localScale = new Vector3(1.416986f, 0.03774609f, 0);
        WarningBox.SetActive(false);

        AllRiseObj.SetActive(false);
        PreparationScreen.SetActive(true);
        GameplayScreen.SetActive(false);
        BulletCheckScreen.SetActive(false);
    }
    protected bool CheckBullets()
    {
        WarningBox.SetActive(false);
        PreparationScreen.SetActive(false);
        GameplayScreen.SetActive(false);
        BulletCheckScreen.SetActive(true);
        CheckedBullets = true;

        return false;
    }
    protected bool ReturnToMenu()
    {
        PreparationScreen.SetActive(true);
        GameplayScreen.SetActive(false);
        BulletCheckScreen.SetActive(false);

        return false;
    }
    protected bool StartTrial()
    {
        if (CheckedBullets)
        {
            Debug.Log("Started the trial");
            StartCoroutine(FadeEffect());
        }
        else
        {
            WarningBox.SetActive(true);
            StartCoroutine(WarningBoxShow());
        }
        return false;
    }

    IEnumerator WarningBoxShow()
    {
        float Scale = 0;
        for (int T = 0; T <= 5; T++)
        {
            WarningBox.transform.localScale = new Vector3(1.416986f, 0.03774609f, Scale);
            Scale += 0.15f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        WarningMessageText[0].SetActive(true);
        WarningMessageText[1].SetActive(true);
        WarningMessageText[2].SetActive(true);
    }
    IEnumerator FadeEffect()
    {
        int Frame = 0;
        AllRiseObj.SetActive(true);
        for (int T = 0; T <= AllRiseAnimSprites.Count() - 1; T++)
        {
            AllRiseRenderer.material.mainTexture = AllRiseAnimSprites[Frame];
            Frame++;
            yield return new WaitForSecondsRealtime(0.025f);
        }
        AllRiseObj.SetActive(false);
        PreparationScreen.SetActive(false);
        GameplayScreen.SetActive(true);
        BulletCheckScreen.SetActive(false);
    }

    //All of this is obsolete for now
    /*
    void ArgumentAndBulletCreation()
    {

    }

    void ArgumentSetup()
    {
        Argument.text = AllArguments[0];
        ArgumentOutlines[0].text = AllArguments[0];
        ArgumentOutlines[1].text = AllArguments[0];
        ArgumentOutlines[2].text = AllArguments[0];
        ArgumentOutlines[3].text = AllArguments[0];
    }

    void BulletReload()
    {
        Debug.Log("Checked");
        if (!SwappingBulletType)
        {
            Debug.Log("Swapping was not true");
            if (!Reloading && !Firing)
            {
                Reloading = true;
                CurrentBullet++;
                if (CurrentBullet > AmountOfBullets - 1)
                {
                    CurrentBullet = 0;
                }
                StartCoroutine("BulletReloadAnim");
            }
        }
        else
        {
            Debug.Log("Swapping was true");
            SwappingBulletType = false;
        }
        return;
    }

    protected bool Memory()
    {
        StartCoroutine("CheckForMemory");
        return false;
    }

    protected void StopMemory()
    {
        StopCoroutine("CheckForMemory");
        Debug.Log("Trying to check");
        BulletReload();
        return;
    }

    IEnumerator CheckForMemory()
    {
        for (int T = 1; T >= 0; T--)
        {
            if (T == 0)
            {
                if (UsingMemory) //If already using memory, disable
                {
                    UsingMemory = false;
                    StartCoroutine("BulletReloadAnim");
                    yield return new WaitForSeconds(0.1f);
                    TruthCilinderAnim.material.mainTexture = TruthInitialState;
                    Background.material.mainTexture = BackgroundTruthState;
                    BulletState.material.mainTexture = BulletTruthState;

                    StopCoroutine("CheckForMemory");
                }
                else //If not already using memory, enable
                {
                    UsingMemory = true;
                    StartCoroutine("BulletReloadAnim");
                    yield return new WaitForSeconds(0.1f);
                    TruthCilinderAnim.material.mainTexture = MemoryInitialState;
                    Background.material.mainTexture = BackgroundMemoryState;
                    BulletState.material.mainTexture = BulletMemoryState;
                    StopCoroutine("CheckForMemory");
                }
                SwappingBulletType = true;
            }
            Debug.Log(T);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    protected bool BulletFire()
    {
        if (!Firing && !Reloading)
        {
            CounterValue = TruthBulletText.text;
            CounterBulletText.text = CounterValue;
            CounterBulletTextBackside.text = CounterValue;
            CounterOutlines[0].text = CounterValue;
            CounterOutlines[1].text = CounterValue;
            CounterOutlines[2].text = CounterValue;
            CounterOutlines[3].text = CounterValue;
            CounterOutlines[4].text = CounterValue;
            CounterOutlines[5].text = CounterValue;
            CounterOutlines[6].text = CounterValue;
            CounterOutlines[7].text = CounterValue;
            Firing = true;
            StartCoroutine("BulletFireAnim");
        }
        return false;
    }

    IEnumerator BulletReloadAnim()
    {
        float Size = 1;
        for (int T = 0; T < 6; T++)
        {
            TruthBullet.transform.localScale = new Vector3(Size, 0.8767387f, 0.917183f);
            Size -= 0.2f;
            yield return new WaitForSecondsRealtime(0.016f);
            if (T == 3)
            {
                StartCoroutine("CilinderReloadAnim");
            }
        }
        Size += 0.2f;
        yield return new WaitForSecondsRealtime(0.075f);
        for (int T = 0; T < 6; T++)
        {
            CurrentBulletText = TruthBullets[CurrentBullet];
            TruthBulletText.text = CurrentBulletText;
            TruthBullet.transform.localScale = new Vector3(Size, 0.8767387f, 0.917183f);
            Size += 0.2f;
            yield return new WaitForSecondsRealtime(0.016f);
        }
        Reloading = false;
    }

    IEnumerator BulletFireAnim()
    {
        float CilinderSize = 0.04f;
        float BulletPosX = 0.52f;
        float BulletPosZ = 0.677f;

        BulletHighlight.gameObject.SetActive(false);
        CilinderHighlight.gameObject.SetActive(false);

        for (int T = 0; T < 5; T++)
        {
            CilinderSize += 0.005f;
            BulletPosX -= 0.049f;
            BulletPosZ -= 0.0078f;
            TruthCylinder.transform.localScale = new Vector3(CilinderSize, CilinderSize, CilinderSize);
            TruthBullet.transform.localPosition = new Vector3(BulletPosX, 0.1016573f, BulletPosZ);
            yield return new WaitForSecondsRealtime(0.016f);
        }
        for (int T = 0; T < 5; T++)
        {
            CilinderSize -= 0.005f;
            BulletPosX += 0.049f;
            BulletPosZ += 0.0078f;
            TruthCylinder.transform.localScale = new Vector3(CilinderSize, CilinderSize, CilinderSize);
            TruthBullet.transform.localPosition = new Vector3(BulletPosX, 0.1016573f, BulletPosZ);
            if (T == 4)
            {
                CounterBullet.SetActive(true);
                StartCoroutine("CounterBulletTravel");
            }
            yield return new WaitForSecondsRealtime(0.016f);
        }
        Firing = false;
    }

    IEnumerator CounterBulletTravel()
    {
        float DistanceX = 0.0539f;
        float DistanceY = 0.25f;
        for (int T = 0; T < 7; T++)
        {
            DistanceX -= 0.0077f;
            DistanceY -= 0.025f;
            CounterBullet.transform.localPosition = new Vector3(DistanceX, DistanceY, 0.001840914f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        CounterBullet.SetActive(false);
        StartCoroutine("NoThatsWrongAnim");
    }

    IEnumerator NoThatsWrongAnim()
    {
        int T = 0;
        Argument.gameObject.SetActive(false);
        TruthBulletText.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
        while (true)
        {
            TruthCilinderSelect.OnInteractEnded = EmptyHoldBtn;
            TruthCilinderSelect.OnInteract = EmptyBtn;
            TruthBulletSelect.OnInteract = EmptyBtn;
            if (!UsingMemory)
            {
                NoThatsWrongObject.SetActive(true);
                NoThatsWrongRender.material.mainTexture = NoThatsWrongSprites[T];
                T++;
                if (T > 17)
                {
                    NoThatsWrongObject.SetActive(false);
                    Refutations++;
                    if (Refutations >= 3)
                    {
                        GetComponent<KMBombModule>().HandlePass();
                        TruthCilinderSelect.OnInteractEnded = EmptyHoldBtn;
                        TruthCilinderSelect.OnInteract = EmptyBtn;
                        TruthBulletSelect.OnInteract = EmptyBtn;
                        StopCoroutine("CountdownCoroutine");
                    }
                    else
                    {
                        Argument.gameObject.SetActive(true);
                        TruthBulletText.gameObject.SetActive(true);
                        Name.gameObject.SetActive(true);
                        TruthCilinderSelect.OnInteractEnded = StopMemory;
                        TruthCilinderSelect.OnInteract = Memory;
                        TruthBulletSelect.OnInteract = BulletFire;
                        ArgumentAndBulletCreation();
                    }
                    BulletHighlight.gameObject.SetActive(true);
                    CilinderHighlight.gameObject.SetActive(true);
                    StopCoroutine("NoThatsWrongAnim");
                }
                yield return new WaitForSecondsRealtime(0.05f);
            }
            else
            {
                MemoryObject.SetActive(true);
                MemoryRender.material.mainTexture = MemorySprites[T];
                T++;
                if (T > 40)
                {
                    MemoryObject.SetActive(false);
                    Refutations++;
                    if (Refutations >= 3)
                    {
                        GetComponent<KMBombModule>().HandlePass();
                        StopCoroutine("CountdownCoroutine");
                    }
                    else
                    {
                        Argument.gameObject.SetActive(true);
                        TruthBulletText.gameObject.SetActive(true);
                        Name.gameObject.SetActive(true);
                        TruthCilinderSelect.OnInteractEnded = StopMemory;
                        TruthCilinderSelect.OnInteract = Memory;
                        TruthBulletSelect.OnInteract = BulletFire;
                        ArgumentAndBulletCreation();
                    }
                    BulletHighlight.gameObject.SetActive(true);
                    CilinderHighlight.gameObject.SetActive(true);
                    StopCoroutine("NoThatsWrongAnim");
                }
                yield return new WaitForSecondsRealtime(0.04f);
            }
            
        }
    }

    protected bool EmptyBtn()
    {
        return false;
    }

    protected void EmptyHoldBtn()
    {
        return;
    }

    IEnumerator CilinderReloadAnim()
    {
        for (int T = 0; T < 4; T++)
        {
            if (UsingMemory) //If using memory, use memory sprites
            {
                TruthCilinderAnim.material.mainTexture = MemoryStates[T];
            }
            else //If not using memory, use truth sprites
            {
                TruthCilinderAnim.material.mainTexture = CilinderStates[T];
            }
            yield return new WaitForSecondsRealtime(0.0165f);
        }
    }

    IEnumerator CountdownCoroutine()
    {
        while (true)
        {
            Countdown--;
            if (Countdown < 0)
            {
                CurrentArgument++;
                CurrentPos++;

                if (CurrentArgument > 3) //Change this to 4 after lies added
                {
                    CurrentArgument = 0;
                }
                if (CurrentPos > 3) //Change this to 4 after lies added
                {
                    CurrentPos = 0;
                }

                Argument.text = AllArguments[CurrentArgument];
                ArgumentOutlines[0].text = AllArguments[CurrentArgument];
                ArgumentOutlines[1].text = AllArguments[CurrentArgument];
                ArgumentOutlines[2].text = AllArguments[CurrentArgument];
                ArgumentOutlines[3].text = AllArguments[CurrentArgument];
                Name.text = AllNames[CurrentArgument];
                Debug.LogFormat("Switching... (Current Arg: {0})", CurrentArgument);

                Countdown = CountdownSetup;
                ArgIndicator.transform.localPosition = ArgPositions[CurrentPos];
            }
            CountdownText.text = Countdown.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    IEnumerator SpinAnim()
    {
        float Rotation = 187;
        string Rotating = "Counter";
        while (true)
        {
            Argument.gameObject.transform.localEulerAngles = new Vector3(90, 0, Rotation);
            if (Rotating == "Counter")
            {
                if (Rotation <= 170)
                {
                    Rotation += 0.02f;
                }
                if (Rotation <= 171 && Rotation > 170)
                {
                    Rotation += 0.02f;
                }
                if (Rotation <= 173 && Rotation > 171)
                {
                    Rotation += 0.03f;
                }
                //Center
                if (Rotation > 173 && Rotation < 202)
                {
                    Rotation += 0.05f;
                }
                if (Rotation >= 202 && Rotation < 205)
                {
                    Rotation += 0.03f;
                }
                if (Rotation >= 205 && Rotation < 206)
                {
                    Rotation += 0.02f;
                }
                if (Rotation >= 206)
                {
                    Rotating = "Clock";
                    yield return new WaitForSecondsRealtime(0.5f);
                }
            }
            else
            {
                if (Rotation <= 170)
                {
                    Rotating = "Counter";
                    yield return new WaitForSecondsRealtime(0.5f);
                }
                if (Rotation <= 171 && Rotation > 170)
                {
                    Rotation -= 0.02f;
                }
                if (Rotation <= 173 && Rotation > 171)
                {
                    Rotation -= 0.03f;
                }
                //Center
                if (Rotation > 173 && Rotation < 202)
                {
                    Rotation -= 0.05f;
                }
                if (Rotation >= 202 && Rotation < 205)
                {
                    Rotation -= 0.03f;
                }
                if (Rotation >= 205 && Rotation < 206)
                {
                    Rotation -= 0.02f;
                }
                if (Rotation >= 206)
                {
                    Rotation -= 0.02f;
                }
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
    }
    */
    //All of this is obsolete for now
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore IDE0051 // Remove unused private members
}
