using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleHeroSlot : MonoBehaviour
{
    [SerializeField] private GameObject nullBtn;
    [SerializeField] private GameObject info;

    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI className;
    [SerializeField] private TextMeshProUGUI heroName;
    [SerializeField] private TextMeshProUGUI strTxt;
    [SerializeField] private TextMeshProUGUI dexTxt;
    [SerializeField] private TextMeshProUGUI intTxt;
    [SerializeField] private TextMeshProUGUI conTxt;
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
        className.text = hero.classData.className;
        heroName.text = hero.name;
        strTxt.text = $"STR : {hero.status.STR}";
        dexTxt.text = $"DEX : {hero.status.DEX}";
        intTxt.text = $"INT : {hero.status.INT}";
        conTxt.text = $"CON : {hero.status.CON}";
        lukTxt.text = $"LUK : {hero.status.LUK}";
    }
}
