using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public int score         = 0;
    public int correctBonus  = 10;
    public int wrongPenalty  = 5;

    [Header("Timing")]
    public float orderTimeLimit    = 20f;
    public float nextCustomerDelay = 1.4f;

    [Header("References")]
    public UIManager       uiManager;
    public CustomerManager customerManager;
    public DrinkMaker      drinkMaker;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        score = 0;
        uiManager.UpdateScore(score);
        customerManager.SpawnNextCustomer();
    }

    public void OnDrinkServed(GameData.DrinkType served, GameData.DrinkType ordered)
    {
        bool ok = (served == ordered);
        int  delta = ok ? correctBonus : -wrongPenalty;
        score += delta;
        if (score < 0) score = 0;
        uiManager.UpdateScore(score);
        uiManager.ShowFeedback(ok, delta);
        drinkMaker.ResetMaker();
        StartCoroutine(NextCustomer());
    }

    public void OnTimeOut()
    {
        int delta = -wrongPenalty;
        score += delta;
        if (score < 0) score = 0;
        uiManager.UpdateScore(score);
        uiManager.ShowFeedback(false, delta);
        drinkMaker.ResetMaker();
        StartCoroutine(NextCustomer());
    }

    IEnumerator NextCustomer()
    {
        yield return new WaitForSeconds(nextCustomerDelay);
        customerManager.SpawnNextCustomer();
    }
}
