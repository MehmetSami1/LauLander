using TMPro;
using UnityEngine;

public class LandedIU : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField]private TextMeshProUGUI statsTextMesh;


 
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
        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH</color>";
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
