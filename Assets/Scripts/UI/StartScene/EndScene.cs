using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField] Image[] BGs;
    [SerializeField] TextMeshProUGUI[] Txts;

    private void Start()
    {
        BGs[(int)GameManager.Instance.Ending].gameObject.SetActive(true);
        Txts[(int)GameManager.Instance.Ending].gameObject.SetActive(true);
    }
}
