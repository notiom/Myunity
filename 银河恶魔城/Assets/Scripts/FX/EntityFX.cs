using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("After image")]
	[SerializeField] private GameObject afterImagePrefab;
	[SerializeField] private float afterImageCooldown;
    [SerializeField] private float colorLooseRate;
    private float afterImageCooldownTimer;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Aliment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFx;
	[SerializeField] private ParticleSystem chillFx;
	[SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
	[SerializeField] private GameObject critcalHitFx;

    [Space]
    [SerializeField] private ParticleSystem dustFx;

    private GameObject myHealthBar;
	void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;
        originalMat = sr.material;

    }

	private void Update()
	{
		afterImageCooldownTimer -= Time.deltaTime;
	}

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(3, 5);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);
        GameObject newText = Instantiate(popUpTextPrefab, transform.position,Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

	public void CreateAfterImage()
    {
        if(afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
			GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
			newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
		}
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        Color currentColor = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(.2f);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white) 
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

	public void ShockFxFor(float _seconds)
	{
        shockFx.Play();
		InvokeRepeating("ShockColorFx", 0, .3f);
		Invoke("CancelColorChange", _seconds);
	}

	public void ChillFxFor(float _seconds)
	{
        chillFx.Play();
		InvokeRepeating("ChillColorFx", 0, .3f);
		Invoke("CancelColorChange", _seconds);
	}

	public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }

    private void ChillColorFx()
    {
		if (sr.color != chillColor[0])
		{
			sr.color = chillColor[0];
		}
		else
		{
			sr.color = chillColor[1];
		}
	}

    private void ShockColorFx()
    {
		if (sr.color != shockColor[0])
		{
			sr.color = shockColor[0];
		}
		else
		{
			sr.color = shockColor[1];
		}
	}

    public void CreateHitFx(Transform _target,bool _critcal)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
		float yPosition = Random.Range(-.5f, .5f);

        GameObject hitPrefab = hitFx;
        if (_critcal)
            hitPrefab = critcalHitFx;

		GameObject newHitFx = Instantiate(hitPrefab,new Vector3(_target.position.x + xPosition,_target.position.y + yPosition),Quaternion.identity);
        newHitFx.transform.parent = _target;
        if (!_critcal)
            newHitFx.transform.Rotate(new Vector3(0, 0, zRotation));
        else
            newHitFx.transform.localScale = new Vector3((int)GetComponent<Entity>().facingDir, 1, 1);

		Destroy(newHitFx,.5f);
    }

    public void PlayDustFX()
    {
        if(dustFx != null)
            dustFx.Play();
    }

	public void MakeTransparent(bool _transparent)
	{
		if (_transparent)
		{
            myHealthBar.SetActive(false);
			sr.color = Color.clear;
		}

		else
		{
            myHealthBar.SetActive(true);
			sr.color = Color.white;
		}
	}
}
