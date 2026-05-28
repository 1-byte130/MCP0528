using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 客人管理器：
///  - 客人身體用彩色圓形圖示
///  - 訂單泡泡只顯示咖啡圖示（無文字）
///  - 倒數計時
/// </summary>
public class CustomerManager : MonoBehaviour
{
    [Header("Customer Visual")]
    public Image customerBodyImage;     // 大圓形客人身體
    public Image customerFaceImage;     // 表情（可選）

    [Header("Order Bubble")]
    public GameObject orderBubble;
    public Image      orderDrinkIcon;   // 訂單咖啡圖示

    [HideInInspector]
    public GameData.DrinkType currentOrder;

    bool              orderActive;
    Coroutine         timerCo;

    // 客人顏色組
    static readonly Color[] BodyColors =
    {
        new Color(0.55f, 0.35f, 0.65f),
        new Color(0.30f, 0.55f, 0.50f),
        new Color(0.65f, 0.40f, 0.28f),
        new Color(0.35f, 0.45f, 0.70f),
        new Color(0.60f, 0.52f, 0.28f),
        new Color(0.35f, 0.60f, 0.40f),
    };

    public void SpawnNextCustomer()
    {
        // 隨機客人顏色
        customerBodyImage.color = BodyColors[Random.Range(0, BodyColors.Length)];

        // 隨機點單
        var vals = System.Enum.GetValues(typeof(GameData.DrinkType));
        currentOrder = (GameData.DrinkType)vals.GetValue(Random.Range(0, vals.Length));

        // 顯示咖啡圖示（無文字）
        var tex = IconFactory.DrinkIcon(currentOrder);
        orderDrinkIcon.sprite = DrinkMaker.TexToSprite(tex);

        orderBubble.SetActive(true);
        orderActive = true;

        if (timerCo != null) StopCoroutine(timerCo);
        timerCo = StartCoroutine(TimerLoop());
    }

    IEnumerator TimerLoop()
    {
        float t = GameManager.Instance.orderTimeLimit;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            GameManager.Instance.uiManager.UpdateTimer(t, GameManager.Instance.orderTimeLimit);
            yield return null;
        }
        if (orderActive)
        {
            orderActive = false;
            orderBubble.SetActive(false);
            GameManager.Instance.OnTimeOut();
        }
    }

    public void OrderFulfilled()
    {
        orderActive = false;
        if (timerCo != null) StopCoroutine(timerCo);
        orderBubble.SetActive(false);
    }
}
