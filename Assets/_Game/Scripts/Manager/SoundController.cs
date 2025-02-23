using UnityEngine;

public enum AudioType
{
    Button,
    BallBounce,
    ObstacleDestroy,
    Die,
    Win
}

public class SoundController : Singleton<SoundController>
{
    bool _musicEnable = true;
    bool _fxEnable = true;
    bool _vibrationEnable = true;

    [Space(10)]
    [Range(0, 1)] [SerializeField] float _musicVolume = 1f;
    [Range(0, 1)] [SerializeField] float _fxVolume = 1f;

    [Space(10)]
    [SerializeField] AudioSource _bgMusicSource;
    [SerializeField] AudioClip _bgMusicClip;

    [Header("Sound Effect Clip :")]
    [SerializeField] AudioClip _ballBounceClip;
    [SerializeField] AudioClip _obstacleDestroy;
    [SerializeField] AudioClip _dieClip;
    [SerializeField] AudioClip _buttonClip;
    [SerializeField] AudioClip _winClip;

    private GameObject oneShotGameObject;
    private AudioSource oneShotAudioSource;

    #region UNITY_METHOD
    private void Start() {
        _fxEnable = PlayerPrefs.GetInt("sfxState") == 0;
        _musicEnable = PlayerPrefs.GetInt("musicState") == 0;
        _vibrationEnable = PlayerPrefs.GetInt("vibrationState") == 0;

        if (_fxEnable)
            PlayBackgroundMusic(_bgMusicClip);
    }
    #endregion

    #region PUBLIC_METHOD
    public AudioClip GetClip(AudioType audioType) {
        switch (audioType) {
            case AudioType.Button:
                return _buttonClip;
            case AudioType.ObstacleDestroy:
                return _obstacleDestroy;
            case AudioType.BallBounce:
                return _ballBounceClip;
            case AudioType.Die:
                return _dieClip;
            case AudioType.Win:
                return _winClip;
            default:
                return null;
        }
    }

    public void PlayAudio(AudioType type, Vector3 position, float volume = 1f) // 3D
    {
        // return if audio fx is disable
        if (!_fxEnable) return;

        float tempVol = _fxVolume * volume;
        AudioSource.PlayClipAtPoint(GetClip(type), position, Mathf.Clamp(tempVol, 0, 1));
    }

    public void PlayAudio(AudioType type) // 2D
    {
        // return if audio fx is disable
        if (!_fxEnable) return;

        if (oneShotGameObject == null) {
            oneShotGameObject = new GameObject("Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
        }
        oneShotAudioSource.volume = _fxVolume;

        oneShotAudioSource.PlayOneShot(GetClip(type));
    }

    public void PlayAudio(AudioType type, float volume) // 2D
    {
        // return if audio fx is disable
        if (!_fxEnable) return;

        GameObject gameObject = new GameObject("Sound");
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        AudioClip clip = GetClip(type);

        audioSource.volume = _fxVolume * Mathf.Clamp(volume, 0, 1);
        audioSource.PlayOneShot(clip);

        Destroy(gameObject, clip.length);
    }

    public void ToggleMusic(ref bool state) {
        _musicEnable = !_musicEnable;
        UpdateMusic();
        state = _musicEnable;
        PlayerPrefs.SetInt("musicState", _musicEnable ? 0 : 1);
    }

    public void ToggleFX(ref bool state) {
        _fxEnable = !_fxEnable;
        state = _fxEnable;
        PlayerPrefs.SetInt("sfxState", _fxEnable ? 0 : 1);
    }

    public void ToggleVibration(ref bool state) {
        _vibrationEnable = !_vibrationEnable;
        state = _vibrationEnable;
        PlayerPrefs.SetInt("vibrationState", _vibrationEnable ? 0 : 1);
    }

    public void StartVibration() {
        if (!_vibrationEnable) return;

        Vibration.Vibrate(20);
    }

    public void Destroy() {
        Destroy(gameObject);
    }
    #endregion

    #region PRIVATE_METHOD
    void UpdateMusic() {
        if (!_musicEnable)
            _bgMusicSource.Stop();
        else
            PlayBackgroundMusic(_bgMusicClip);
    }

    void PlayBackgroundMusic(AudioClip clip) {
        // return if music is disable or if musicSource is null or clip is null
        if (!_musicEnable || !_bgMusicSource || !clip) return;

        _bgMusicSource.Stop();
        _bgMusicSource.clip = clip;
        _bgMusicSource.volume = _musicVolume;
        _bgMusicSource.Play();
    }
    #endregion
}