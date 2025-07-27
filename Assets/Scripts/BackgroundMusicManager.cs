using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip[] tracks; // Assign 8 tracks in Inspector
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    public Button muteButton;
    public Sprite muteIcon; // Assign mute icon in Inspector
    public Sprite unmuteIcon; // Assign unmute icon in Inspector
    private bool isMuted = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (tracks.Length > 0)
        {
            PlayTrack(currentTrackIndex);
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !isMuted)
        {
            NextTrack();
        }
    }

    private void PlayTrack(int index)
    {
        if (index < 0 || index >= tracks.Length) return;

        audioSource.clip = tracks[index];
        if (!isMuted) audioSource.Play();
    }

    private void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % tracks.Length; // Loop from 1 to 8, then restart at 1
        PlayTrack(currentTrackIndex);
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted; // Mute/unmute audio source

        // Change the image on the button based on mute state
        Image buttonImage = muteButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? muteIcon : unmuteIcon; // Swap icons
        }
    }

}
