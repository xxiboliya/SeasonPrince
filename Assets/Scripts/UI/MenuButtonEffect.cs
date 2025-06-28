using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // 如果你用的是TextMeshPro

public class MenuButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // 拖拽你的按钮文字到这里
    public Image arrowImage;           // 拖拽你的粉色箭头Image到这里

    void Start()
    {
        buttonText.gameObject.SetActive(true);
        arrowImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.gameObject.SetActive(false);
        arrowImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.gameObject.SetActive(true);
        arrowImage.gameObject.SetActive(false);
    }

    public void ResetToDefault()
    {
        if (buttonText != null) buttonText.gameObject.SetActive(true);
        if (arrowImage != null) arrowImage.gameObject.SetActive(false);
    }
}