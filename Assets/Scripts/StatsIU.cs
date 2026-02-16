using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsIU : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;

    [SerializeField]private GameObject speedUpArrow;
    [SerializeField]private GameObject speedDownArrow;
    [SerializeField]private GameObject speedLeftArrow;
    [SerializeField]private GameObject speedRightArrow;
    [SerializeField] private Image fuelImage;

    private void Update()
    {
        UpdateStatsTextMesh();
    }
    private void UpdateStatsTextMesh()
    {
        speedUpArrow.gameObject.SetActive(Lander.Instance.GetSpeedY()>= 0);
        speedDownArrow.gameObject.SetActive(Lander.Instance.GetSpeedY() < 0);
        speedLeftArrow.gameObject.SetActive(Lander.Instance.GetSpeedX() >= 0 );
        speedRightArrow.gameObject.SetActive(Lander.Instance.GetSpeedX() < 0 );

        fuelImage.fillAmount=Lander.Instance.GetFuelAmountNormalized();
        statsTextMesh.text = 
            GameManager.Instance.GetScore() + "\n" +
            Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX()*10f) )+ "\n" +
            Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY()*10f) );
    }
}
