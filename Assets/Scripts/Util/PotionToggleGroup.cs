using System.Collections.Generic;
using UnityEngine;

public class PotionToggleGroup : MonoBehaviour
{
    [Header("Image Toggles")]
    public List<ImageToggle> toggles = new(); 
    public List<int> selectedIndices = new(); 

    private void Start()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i; 
            toggles[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnToggleClicked(index));
        }
    }

    private void OnToggleClicked(int index)
    {
        var toggle = toggles[index];

        if (toggle.isOn)
        {
            if (!selectedIndices.Contains(index))
            {
                selectedIndices.Add(index);
            }
        }
        else
        {
            if (selectedIndices.Contains(index))
            {
                selectedIndices.Remove(index);
            }
        }
    }

    public List<int> GetSelectedIndices()
    {
        return new List<int>(selectedIndices); 
    }

    public List<ImageToggle> GetSelectedItems()
    {
        List<ImageToggle> selectedItems = new List<ImageToggle>();

        foreach (int index in selectedIndices)
        {
            selectedItems.Add(toggles[index]);
        }

        return selectedItems;
    }
}
