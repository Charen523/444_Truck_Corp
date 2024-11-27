using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotScheduleHero : MonoBehaviour
{
    [SerializeField] private GameObject nullBtn;
    [SerializeField] private GameObject info;

    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI heroName;
    [SerializeField] private TextMeshProUGUI strTxt;
    [SerializeField] private TextMeshProUGUI dexTxt;
    [SerializeField] private TextMeshProUGUI intTxt;
    [SerializeField] private TextMeshProUGUI lukTxt;

    public void InitSlot()
    {
        nullBtn.SetActive(true);
        info.SetActive(false);
        img.gameObject.SetActive(false);
    }

    public void SetScheduleSlot(HeroData hero)
    {
        nullBtn.SetActive(false);
        info.SetActive(true);

        img.gameObject.SetActive(true);
        img.sprite = DataManager.Instance.GetSprites(true, hero.spriteIdx);
        heroName.text = hero.name;
        strTxt.text = $"<color=#FF4C4C>STR</color>\n{hero.status.STR}";
        dexTxt.text = $"<color=#4CAF50>DEX</color>\n{hero.status.DEX}";
        intTxt.text = $"<color=#4C79FF>INT</color>\n{hero.status.INT}";
        lukTxt.text = $"<color=#FFD54F>LUK</color>\n{hero.status.LUK}";
    }
}