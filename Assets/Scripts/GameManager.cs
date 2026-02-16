using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score = 0;
    private float time=0;
    private bool isTimerActive;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickup += Landed_OnCoinPickup;
        Lander.Instance.OnLanded += Landed_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal  ;
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void Landed_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
       addScore(e.score);
    }

    private void Landed_OnCoinPickup(object sender, System.EventArgs e)
    {
        addScore(500);
    
    }

    private void addScore(int addScoreAmmount)
    {
        score += addScoreAmmount;
        Debug.Log(score);   
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time; 
    }
}
