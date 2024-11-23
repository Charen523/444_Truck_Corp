using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectSlot : MonoBehaviour
{
    public PopupHeroSelect popupClassSelect;
    public int listIdx;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI strTxt;
    [SerializeField] private TextMeshProUGUI dexTxt;
    [SerializeField] private TextMeshProUGUI intTxt;
    [SerializeField] private TextMeshProUGUI conTxt;
    [SerializeField] private TextMeshProUGUI lukTxt;

    public void InitHeroSlot(int idx, PopupHeroSelect p, HeroData hero)
    {
        popupClassSelect = p;
        listIdx = idx;

        if (toggle != null)
            toggle.group = popupClassSelect.toggleGroup;
    }

    public void SetHeroSlot(HeroData hero, ref bool toggleOn)
    {
        if (toggle != null)
        {
            /*toggle settings*/
            toggle.interactable = HeroManager.Instance.heroStates[hero.id] == eHeroState.FREE;
            if (toggle.interactable && !toggleOn)
            {
                toggleOn = true;
                toggle.isOn = toggleOn;
            }
        }
        
        /*ui info*/
        thumbnail.sprite = DataManager.Instance.GetSprites(false, hero.spriteIdx);
        nameTxt.text = hero.name;
        strTxt.text = $"STR : {hero.status.STR}";
        dexTxt.text = $"DEX : {hero.status.DEX}";
        intTxt.text = $"INT : {hero.status.INT}";
        conTxt.text = $"CON : {hero.status.CON}";
        lukTxt.text = $"LUK : {hero.status.LUK}";
    }

    public void OnSlotToggle(bool input)
    {
        if (popupClassSelect != null)
        {
            if (input)
            {
                popupClassSelect.selectedHeroIdx = listIdx;
            }
        }
    }
}
