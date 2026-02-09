using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultpilierTextMesh;

    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        scoreMultpilierTextMesh.text = "x" + landingPad.GetScoreMultiplier();
    }
}
