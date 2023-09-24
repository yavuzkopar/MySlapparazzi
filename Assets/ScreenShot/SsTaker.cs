using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SsTaker : NetworkBehaviour
{
    [SerializeField] RenderTexture[] renderTextures;

    [Networked(OnChanged = nameof(OnIndexChanged))]
    public int texIndex { get; set; }
    //  [SerializeField] RenderTexture rt;
    [SerializeField] RawImage image;
    [SerializeField] TextMeshProUGUI timeText;

    [Networked] public TickTimer life { get; set; }
    [Networked] public TickTimer imageSize { get; set; }
    public CameraHolder cameraHolder;
    [SerializeField] ParticleSystem _particleSystem;
    // public override void Spawned()
    public void Init()
    {
        life = TickTimer.CreateFromSeconds(Runner, 10.0f);
        imageSize = new TickTimer();
        texIndex = Random.Range(0, 4);
        
        
    }
    public override void Spawned()
    {
        cameraHolder = FindObjectOfType<CameraHolder>(true);
        _particleSystem.transform.position = cameraHolder.cameras[texIndex].transform.position + Vector3.up;
    }
    float iSize;
    public override void FixedUpdateNetwork()
    {
        //time += Runner.DeltaTime;
        //   if (!Object.HasStateAuthority) return;
        float f = (float) life.RemainingTime(Runner);
        f = Mathf.Round(f * 10) / 10;
        timeText.text =  f.ToString();
        if (life.Expired(Runner))
        {
            _particleSystem.gameObject.SetActive(false);
            imageSize = TickTimer.CreateFromSeconds(Runner, 3f);
            TakePicture();
            //   StartCoroutine(StartShowing());
            life = TickTimer.CreateFromSeconds(Runner, 10.0f);
        }
        if (imageSize.IsRunning)
        {
            iSize = (float)(3f - imageSize.RemainingTime(Runner));
            iSize = Mathf.Clamp01(iSize);
            image.transform.localScale = Vector3.one * iSize;
            if (imageSize.Expired(Runner))
            {
                image.gameObject.SetActive(false);
                // _particleSystem.transform.position = cameraHolder.cameras[texIndex].transform.position + Vector3.up;
                _particleSystem.gameObject.SetActive(true);
            }
        }
    }
    static void OnIndexChanged(Changed<SsTaker> changed)
    {
       // int index = changed.Behaviour.texIndex;
        changed.Behaviour.ChangePartPos();
    }
    void ChangePartPos()
    {
        _particleSystem.transform.position = cameraHolder.cameras[texIndex].transform.position + Vector3.up;
    }
    void TakePicture()
    {
       
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTextures[texIndex];
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();
        image.texture = tex;
        texIndex = Random.Range(0, 4);
        image.gameObject.SetActive(true);
    }
}
