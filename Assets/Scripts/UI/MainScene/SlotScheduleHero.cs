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
    }

    public void SetScheduleSlot(HeroData hero)
    {
        nullBtn.SetActive(false);
        info.SetActive(true);

        img.sprite = DataManager.Instance.GetSprites(true, hero.spriteIdx);
        heroName.text = hero.name;
        strTxt.text = $"<size=14>STR</size> {hero.status.STR}";
        dexTxt.text = $"<size=14>DEX</size> {hero.status.DEX}";
        intTxt.text = $"<size=14>INT</size> {hero.status.INT}";
        lukTxt.text = $"<size=14>LUK</size> {hero.status.LUK}";
    }
}
