using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LocalManager : MonoBehaviour
{
    //组件
    public List<MonoBehaviour> localScripts;

    public Camera FP_Camera;
    public Camera ENV_Camera;

    private PhotonView photonView;

    public List<Renderer> TPRenderers;

    public GameObject FPArms;

    //数据

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        //判断是否为本地(是则不接受输入
        if (photonView.IsMine)
        {
            gameObject.AddComponent<AudioListener>();//只添加一个AudioListener
            return;
        }
        
        FPArms.SetActive(false);
        FP_Camera.enabled = false;
        ENV_Camera.enabled = false;

        //关闭其他客户端的脚本（控制脚本
        foreach(MonoBehaviour behaviour in localScripts)
        {
            behaviour.enabled = false;
        }

        //打开其他客户端的全身显示
        foreach(Renderer renderer in TPRenderers)
        {
            renderer.shadowCastingMode = ShadowCastingMode.On;
        }
    }
}
