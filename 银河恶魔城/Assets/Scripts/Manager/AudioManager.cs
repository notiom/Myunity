using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 由于缺少音频,只做示范,音频播放应该是在state下或者是animationtrigger中 
    // 勾选Loop可以循环播放音频
    public static AudioManager instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private float sfxMininumDistance;

    public bool playBgm;
    private int bgmIndex;

    private bool canPlaySFX;
	private void Awake()
	{
		if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        Invoke("AllowSFX", 1f);
	}

	private void Update()
	{
		if(!playBgm) 
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
	}

	public void PlaySFX(int _sfxIndex,Transform _source)
    {
        if(!canPlaySFX) { return; }
        // 如果当前正在播放,就取消播放
        if (sfx[_sfxIndex].isPlaying) 
        {
            return;
        }

        // 距离太远的僵尸或者其他就不会发出声音
        if(_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position,_source.position) > sfxMininumDistance)
        {
            return;
        }

        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);

			sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while(_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.25f);

            if(_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0,bgm.Length);
        PlayBGM(bgmIndex);
    }
	public void PlayBGM(int _bgmIndex)
	{
        bgmIndex = _bgmIndex;
        StopAllBGM();

		bgm[bgmIndex].Play();
	}

    public void StopAllBGM()
    {
        for(int i = 0;i < bgm.Length;i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
