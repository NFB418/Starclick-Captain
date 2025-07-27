using UnityEngine;
using System.Collections;

public class BeamScript : MonoBehaviour
{
    [Header("BeamSprite")]
    public GameObject BeamSprite;

    [Header("Audio")]
    public AudioSource audioSource;  // Assign an AudioSource in the Inspector
    public AudioClip loopingSound;   // Assign the sound effect in the Inspector
    public float fadeDuration = 1.0f; // How long the fade-out lasts
    private float startVolume;

    [Header("Laser pieces")]
    public GameObject beamStartSpriteArt;
    public GameObject beamMiddleSpriteArt;
    public GameObject beamEndSpriteArt;

    private GameObject beamStart;
    private GameObject beamMiddle;
    private GameObject beamEnd;

    private Animator beamStartAnimator;
    private Animator beamMiddleAnimator;
    private Animator beamEndAnimator;

    public bool beamToggle;
    public float beamLength;

    private void Start()
    {
        beamToggle = false;
        beamLength = 1.5f;
        startVolume = audioSource.volume;
    }

    public void ActivateBeam(Vector3 endPosition)
    {
        Vector3 startPosition = BeamSprite.transform.position;

        // Calculate the straight-line distance between the start and end positions
        float canvasScaleFactor = gameObject.GetComponentInParent<Canvas>().scaleFactor;
        // - 265 is a manual adjustment to account for the beam starting and ending in front of the objects; TODO: calculate modifier dynamically
        beamLength = Vector3.Distance(startPosition, endPosition) / canvasScaleFactor;
        // Debug.Log($"startPosition: {startPosition} -> endPosition: {endPosition} == beamSize: {beamLength}");
        // Debug.DrawLine(startPosition, endPosition, Color.red, 0.1f);
        beamLength *= 0.1f; // Accounts for scale of BeamSprite being at times 10

        // Set the beam length in the BeamScript
        beamToggle = true;
    }

    // Deactivate the beam effect
    public void DeactivateBeam()
    {
        beamToggle = false;
    }

    void Update()
    {
        if (!beamToggle) {
            PlayDestructionAnimation(); // Trigger destruction animation when beamToggle is false
            return;
        } // If beam is off, destroy existing beam and return

        StartSFXLoop();

        // Create the laser start from the prefab
        if (beamStart == null)
        {
            beamStart = Instantiate(beamStartSpriteArt) as GameObject;
            beamStart.transform.SetParent(this.transform, false);
            beamStart.transform.localPosition = Vector2.zero;

            beamStartAnimator = beamStart.GetComponent<Animator>();
        }

        // Laser middle
        if (beamMiddle == null)
        {
            beamMiddle = Instantiate(beamMiddleSpriteArt) as GameObject;
            beamMiddle.transform.SetParent(this.transform, false);
            beamMiddle.transform.localPosition = Vector2.zero;

            beamMiddleAnimator = beamMiddle.GetComponent<Animator>();
        }

        if (beamEnd == null)
        {
            beamEnd = Instantiate(beamEndSpriteArt) as GameObject;
            beamEnd.transform.SetParent(this.transform, false);
            beamEnd.transform.localPosition = Vector2.zero;

            beamEndAnimator = beamEnd.GetComponent<Animator>();
        }

        // Place things
        // -- Gather some data
        float startSpriteWidth = beamStart.GetComponent<RectTransform>().rect.width;
        float endSpriteWidth = beamEnd.GetComponent<RectTransform>().rect.width;

        // Debug.Log($"currentLaserSize: {beamSize}, startSpriteWidth: {startSpriteWidth}");

        // -- the middle is after start and, as it has a center pivot, have a size of half the laser (minus start and end)
        beamMiddle.transform.localScale = new Vector3(beamLength - (startSpriteWidth + endSpriteWidth) / 2f, beamMiddle.transform.localScale.y, beamMiddle.transform.localScale.z);
        beamMiddle.transform.localPosition = new Vector2((beamLength / 2f), 0f);

        // Debug.Log($"middle.transform.localScale: {beamMiddle.transform.localScale}, middle.transform.localPosition: {beamMiddle.transform.localPosition}");

        // End
        beamEnd.transform.localPosition = new Vector2(beamLength, 0f);

    }

    private void StartSFXLoop()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = startVolume; // Reset volume for next play
            audioSource.clip = loopingSound;
            audioSource.loop = true; // Enables looping
            audioSource.Play(); // Starts playing
        }
    }

    private void StopSFXLoop()
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeOutAndStop(audioSource, fadeDuration));
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float duration)
    {
        // Gradually lower the volume
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        source.volume = 0;
        source.Stop(); // Fully stop after fade-out
    }
    private void PlayDestructionAnimation()
    {
        // Trigger the destruction animation on each part
        if (beamStart != null) beamStartAnimator.SetBool("DestroyBeam", true);
        if (beamMiddle != null) beamMiddleAnimator.SetBool("DestroyBeam", true);
        if (beamEnd != null) beamEndAnimator.SetBool("DestroyBeam", true);
        StopSFXLoop();

        // Clean up after the destruction animation finishes
        StartCoroutine(DestroyAfterAnimation());
    }

    private System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // Wait until all parts finish their destruction animation
        while (!IsAnimationFinished(beamStartAnimator, "BeamStartDestruction") ||
               !IsAnimationFinished(beamMiddleAnimator, "BeamMiddleDestruction") ||
               !IsAnimationFinished(beamEndAnimator, "BeamEndDestruction"))
        {
            yield return null; // Wait for the next frame
        }

        if (beamStart != null) Destroy(beamStart);
        if (beamMiddle != null) Destroy(beamMiddle);
        if (beamEnd != null) Destroy(beamEnd);
    }

    public bool IsLooping()
    {
        if (beamEnd == null) { return false; }

        AnimatorStateInfo stateInfo = beamMiddleAnimator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.IsName("BeamMiddleLoop");
    }

    private bool IsAnimationFinished(Animator animator, string animationName)
    {
        if (animator == null) return true; // If no animator, assume finished

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the current animation is BeamDestroy and it's finished
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= GetAnimationLength(animator, animationName);
    }

    // Get the length of an animation from the Animator's RuntimeAnimatorController
    private float GetAnimationLength(Animator animator, string animationName)
    {
        if (animator == null) return 0f;

        // Get the runtime animator controller
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        if (controller == null) return 0f;

        // Get the animation clip by name
        foreach (var clip in controller.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        return 0f; // Return 0 if the animation name wasn't found
    }
}