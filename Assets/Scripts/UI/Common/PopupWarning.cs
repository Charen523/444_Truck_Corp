using TMPro;
using UnityEngine;

public class PopupWarning : MonoBehaviour
{
    public TextMeshProUGUI warnTxt;

    public void SetWarnTxt(string msg)
    {
        warnTxt.text = msg;
    }

    private void OnDisable()
    {
        Destroy(this);
    }
}
