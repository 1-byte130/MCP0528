using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public Text  scoreText;
    public Slider timerSlider;
    public Image  timerFill;

    [Header("Feedback")]
    public GameObject feedbackPanel;
    public Image      feedbackIcon;   // 打勾 or X 圖示
    public Text       feedbackScore;  // +10 or -5

    Texture2D okTex, ngTex;

    void Start()
    {
        okTex = IconFactory.OkIcon();
        ngTex = IconFactory.NgIcon();
        feedbackPanel.SetActive(false);
    }

    public void UpdateScore(int s) => scoreText.text = s.ToString();

    public void UpdateTimer(float remaining, float total)
    {
        float ratio = remaining / total;
        timerSlider.value = ratio;
        timerFill.color = ratio < 0.25f
            ? Color.Lerp(new Color(1f,0.2f,0.1f), new Color(1f,0.6f,0.1f), ratio * 4f)
            : Color.Lerp(new Color(0.3f,0.75f,1f),  new Color(0.2f,0.9f,0.4f), ratio);
    }

    public void ShowFeedback(bool ok, int delta)
    {
        feedbackPanel.SetActive(true);
        feedbackIcon.sprite = DrinkMaker.TexToSprite(ok ? okTex : ngTex);
        feedbackScore.text  = (delta >= 0 ? "+" : "") + delta;
        feedbackScore.color = ok ? new Color(0.2f,0.9f,0.3f) : new Color(1f,0.25f,0.25f);
        StartCoroutine(HideFeedback());
    }

    IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(1.2f);
        feedbackPanel.SetActive(false);
    }
}
