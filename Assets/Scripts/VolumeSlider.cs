using TMPro;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI volumeValue;
    

    // Update is called once per frame
    public void UpdateVolume(float level)
    {
        volumeValue.text = ((int) (100 * level)).ToString() + "%";
    }
}
