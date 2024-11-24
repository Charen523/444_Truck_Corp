using System.Collections.Generic;
using UnityEngine;

public class PotionToggleGroup : MonoBehaviour
{
    [Header("Image Toggles")]
    public List<ImageToggle> toggles = new(); 
    public List<int> selectedIndices = new(); 

    public void OnToggleClicked(int index)
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

    public int[] GetSelectedIndices()
    {
        return selectedIndices.ToArray(); 
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
