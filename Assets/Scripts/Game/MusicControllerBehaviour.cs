using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicControllerBehaviour : MonoBehaviour {

    [Header("Music clips")]

    [SerializeField] private AudioClip[] calmMusicClips;

    [Space(6f)]

    [SerializeField] private AudioClip battleMusicIntroClip;
    [SerializeField] private AudioClip battleMusicLoopClip;
    [SerializeField] private AudioClip battleMusicOutroClip;

    [Header("Calm music parameters")]

    [SerializeField] private float minDurationBetweenCalmMusic = 60f;
    [SerializeField] private float randomDurationBetweenCalmMusic = 60f;

    [Space(6f)]

    [Tooltip("(Recommended) If checked, the music controller will never play the same calm music clip twice in a row, unless there is only one calm music clip assigned.")]
    [SerializeField] private bool alwaysDifferentCalmMusic = true;

    [Header("General music parameters")]

    [SerializeField] private float musicFadeTime = 2f;
    [SerializeField] private float battleCooldownTime = 5f;

    [Space(6f)]

    [SerializeField] private float calmMusicVolume = 0.75f;
    [SerializeField] private float battleMusicVolume = 0.85f;

    private AudioSource audioSource;

    private State musicState;
    private MuteState muteState;
    private CalmState calmState;
    private BattleState battleState;

    // State used when no music is being played
    private class MuteState : State
    {
        MusicControllerBehaviour musicController;

        // The time it takes for a calm music clip to start playing
        float currentDurationBetweenCalmMusic;

        public MuteState(MusicControllerBehaviour musicController)
        {
            this.musicController = musicController;
        }

        public override void Entry()
        {
            // Make sure that no audio is being played
            musicController.audioSource.Stop();

            // Make sure that no audio clip is set
            musicController.audioSource.clip = null;

            // Generate the calm music timer
            currentDurationBetweenCalmMusic = musicController.minDurationBetweenCalmMusic + Random.Range(0f, musicController.randomDurationBetweenCalmMusic);
        }

        public override void Update()
        {
            // If the calm music timer has expired, exit to the calm music state
            if (currentDurationBetweenCalmMusic < 0)
                Exit(musicController.calmState);
            // If not, decrease the calm music timer
            else
                currentDurationBetweenCalmMusic -= Time.unscaledDeltaTime;
        }

        public override void Exit(State exitState)
        {
            musicController.musicState = exitState;
            musicController.musicState.Entry();
        }
    }

    private class CalmState : State
    {
        MusicControllerBehaviour musicController;

        AudioClip previousAudioClip;

        public CalmState(MusicControllerBehaviour musicController)
        {
            this.musicController = musicController;
        }

        public override void Entry()
        {
            // Play calm music
            PlayCalmMusic(musicController.alwaysDifferentCalmMusic);
        }

        public override void Update()
        {
            // If the calm music has stopped playing, exit to the mute state
            if (!musicController.audioSource.isPlaying)
            {
                Exit(musicController.muteState);
            }
        }

        public override void Exit(State exitState)
        {
            musicController.musicState = exitState;
            musicController.musicState.Entry();
        }

        // Plays a new calm music clip
        void PlayCalmMusic(bool alwaysDifferentMusicClips)
        {
            // If the same music clip is allowed to be played twice (or more) in a row, or if no calm music has yet been played
            if (!alwaysDifferentMusicClips || previousAudioClip == null)
            {
                // Set a random calm music clip
                musicController.audioSource.clip = musicController.calmMusicClips[Random.Range(0, musicController.calmMusicClips.Length)];
            }
            else
            {
                List<AudioClip> tempCalmMusicClips = new List<AudioClip>();

                // Add all music clips but the previously played one to the list
                for(int i = 0; i < musicController.calmMusicClips.Length; i++)
                {
                    if (musicController.calmMusicClips[i] != previousAudioClip)
                        tempCalmMusicClips.Add(musicController.calmMusicClips[i]);
                }

                // Set a random calm music clip
                musicController.audioSource.clip = tempCalmMusicClips[Random.Range(0, tempCalmMusicClips.Count)];
            }

            // Play the calm music clip
            musicController.audioSource.Play();

            // Reset the audio source volume to the defined calm music volume
            musicController.audioSource.volume = musicController.calmMusicVolume;

            // Update the previous audio clip
            previousAudioClip = musicController.audioSource.clip;
        }
    }

    private class BattleState : State
    {
        MusicControllerBehaviour musicController;

        float cooldownTime;

        public BattleState(MusicControllerBehaviour musicController)
        {
            this.musicController = musicController;
        }

        public override void Entry()
        {
            // If there is no audio being played
            if(!musicController.audioSource.isPlaying)
            {
                PlayBattleMusic(musicController.alwaysDifferentCalmMusic);
            }
            // If there is already battle music being played
            else if (CheckIfBattleMusic())
            {
                // Reset the battle music (if needed)
                ResetBattleMusic();
            }
        }

        public override void Update()
        {
            // If battle music isn't the current music clip
            if (!CheckIfBattleMusic())
            {
                // Fade out the other music
                FadeOutOtherMusic();
            }
            // If battle music is the current music clip
            else
            {
                // If the audio source is currently stopped
                if (!musicController.audioSource.isPlaying)
                {
                    // If the current music clip is the battle intro clip
                    if (musicController.audioSource.clip == musicController.battleMusicIntroClip)
                    {
                        // Change to the battle loop clip and play it
                        musicController.audioSource.clip = musicController.battleMusicLoopClip;
                        musicController.audioSource.Play();

                        // Set the music to loop
                        musicController.audioSource.loop = true;
                    }
                    // If the current music clip is the battle loop clip
                    else if (musicController.audioSource.clip == musicController.battleMusicLoopClip)
                    {
                        // If the cooldown time has expired
                        if (cooldownTime <= 0)
                        {
                            // Change to the battle outro clip and play it
                            musicController.audioSource.clip = musicController.battleMusicOutroClip;
                            musicController.audioSource.Play();
                        }
                        // If not, decrease the cooldown time
                        else
                            cooldownTime -= Time.unscaledDeltaTime;
                    }
                    // If the current music clip is the battle outro clip
                    else
                    {
                        // Exit to the mute state
                        Exit(musicController.muteState);
                    }
                }
                // If audio source is currently playing
                else
                {
                    // If the current music clip is the battle loop clip
                    if (musicController.audioSource.clip == musicController.battleMusicLoopClip)
                    {
                        // If the cooldown time has expired
                        if (cooldownTime <= 0)
                        {
                            // Stop looping the battle loop clip
                            musicController.audioSource.loop = false;
                        }
                        // If not, decrease the cooldown time
                        else
                            cooldownTime -= Time.unscaledDeltaTime;
                    }
                }
            }
        }

        public override void Exit(State exitState)
        {
            musicController.musicState = exitState;
            musicController.musicState.Entry();
        }

        // Plays a new battle music clip
        void PlayBattleMusic(bool allowSameMusicClipTwice)
        {
            // Play the battle music clip intro clip
            musicController.audioSource.clip = musicController.battleMusicIntroClip;
            musicController.audioSource.Play();

            // Reset the audio source volume to the defined battle music volume
            musicController.audioSource.volume = musicController.battleMusicVolume;
        }

        // Resets the battle music (if needed)
        void ResetBattleMusic()
        {
            // If the current music clip is the battle loop clip
            if(musicController.audioSource.clip == musicController.battleMusicLoopClip)
            {
                // Set the cooldown time to the defined battle cooldown time
                cooldownTime = musicController.battleCooldownTime;

                // Keep looping the audio source
                musicController.audioSource.loop = true;
            }
        }

        // Fades out the other music and then starts playing the battle music
        void FadeOutOtherMusic()
        {
            // If the audio source's volume is above 0
            if (musicController.audioSource.volume > 0)
                // Decrease it based on the defined music fade time
                musicController.audioSource.volume -= Time.unscaledDeltaTime / (musicController.musicFadeTime / 4);
            // If not, start playing the battle music clip
            else
            {
                PlayBattleMusic(musicController.alwaysDifferentCalmMusic);
            }
        }

        // Checks if the audio clip is being played is a battle audio clip
        bool CheckIfBattleMusic()
        {
            // If the current track matches any of the battle music clips, return true
            if (musicController.audioSource.clip == musicController.battleMusicIntroClip)
                return true;
            if (musicController.audioSource.clip == musicController.battleMusicLoopClip)
                return true;
            if (musicController.audioSource.clip == musicController.battleMusicOutroClip)
                return true;

            // If no match was found, return false
            return false;
        }
    }

    void Awake()
    {
        muteState = new MuteState(this);
        calmState = new CalmState(this);
        battleState = new BattleState(this);
    }

    // Use this for initialization
    void Start()
    {
        // If either the calm or battle music clip arrays are empty
        if(calmMusicClips.Length == 0)
        {
            // Throw an error
            Debug.LogError("There are either no calm music clips assigned, music controller disabled");

            // Destroy the music controller and exit this function
            Destroy(this);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        musicState = muteState;
        musicState.Entry();
    }

    void Update()
    {
        musicState.Update();
    }
    
    public void TriggerBattleMusic()
    {
        musicState.Exit(battleState);
    }
}
