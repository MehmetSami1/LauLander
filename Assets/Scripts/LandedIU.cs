using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedIU : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI titleTextMesh;
    [SerializeField]private TextMeshProUGUI statsTextMesh;
    [SerializeField]private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField]private Button nextButton;

    private Action nextButttonClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButttonClickAction();
        });
    }


    private void Start()
    {
        Lander.Instance.OnLanded += Landed_OnLanded;

        Hide();
    }

    private void Landed_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success)
        {
            titleTextMesh.text = ("SECCESSFUL LANDİNG");
            nextButtonTextMesh.text = ("CONTINUE");
            nextButttonClickAction = GameManager.Instance.GoToNextLevel;

        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH</color>";
            nextButtonTextMesh.text = ("RETRY");
            nextButttonClickAction = GameManager.Instance.RetryLevel;
            
        }

        statsTextMesh.text =
            Mathf.Round(e.landingSpeed) + "\n" +
            Mathf.Round(e.dotVector )+ "\n" +
            "x"+e.scoreMultiplier + "\n" +
            e.score;

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
