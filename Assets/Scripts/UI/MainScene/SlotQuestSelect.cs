using TMPro;
using UnityEngine;

public class SlotQuestSelect : MonoBehaviour
{
    [SerializeField] private PopupQuest popupQuest;

    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI statTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI rewardTxt;

    private int myIdx = -1;

    public void SetQSlot(QuestData quest)
    {
        nameTxt.text = quest.QuestName;
        statTxt.text =
            $"STR {quest.needSpecs[0]}, " +
            $"DEF {quest.needSpecs[1]}, " +
            $"INT {quest.needSpecs[2]}, " +
            $"LUK {quest.needSpecs[3]}";

        timeTxt.text = $"{quest.needTime}일";

        rewardTxt.text =
            $"<color=#AC7500>보상 : </color>";
        if (quest.rewardValues[0] != 0)
        {
            rewardTxt.text += $"{quest.rewardValues[0]} 골드";
            if (quest.rewardValues[1] != 0)
            {
                rewardTxt.text += $", {quest.rewardValues[1]} EXP";
            }
        }
        else
        {
            rewardTxt.text += $"{quest.rewardValues[1]} EXP";
        }

        myIdx = quest.id;
    }

    public void OnSlotClicked()
    {
        if (popupQuest != null)
        {
            popupQuest.OnSelectSlot(myIdx);
        }
    }
}