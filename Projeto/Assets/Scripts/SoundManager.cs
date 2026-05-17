using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Image soundImage;

    public Sprite soundOnSprite;

    public Sprite soundOffSprite;

    private bool isMuted = false;

    public void ToggleSound()
    {
        isMuted = !isMuted;

        AudioListener.volume = isMuted ? 0 : 1;

        soundImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}