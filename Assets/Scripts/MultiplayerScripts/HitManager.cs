
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour,IOnEventCallback
{
    public GameObject impactPrefab;
    public ImpactAudioData impactAudioData;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /// <summary>
    /// 客户端收到服务器信息后开始解析
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        switch((EventCode)photonEvent.Code)//转换为枚举
        {
            //撞击事件
            case EventCode.HitObject:
                //位置信息解析
                var tmp_HitData = (Dictionary<byte, object>)photonEvent.CustomData;
                var tmp_HitPoint = (Vector3)tmp_HitData[0];
                var tmp_HitNormal = (Vector3)tmp_HitData[1];
                var tmp_HitTag = (string)tmp_HitData[2];

                //子弹碰撞效果
                var tmp_BulletEffect = Instantiate(impactPrefab, tmp_HitPoint, Quaternion.LookRotation(tmp_HitNormal, Vector3.up));
                Destroy(tmp_BulletEffect, 3);

                //子弹碰撞音效
                var tmp_TagWithAudio =
                    impactAudioData.ImpactTagsWithAudios.Find((_audioData) => _audioData.Tag.Equals(tmp_HitTag));

                if (tmp_TagWithAudio == null) return;
                int tmp_Length = tmp_TagWithAudio.ImpactAudioClips.Count;
                AudioClip tmp_AudioClip = tmp_TagWithAudio.ImpactAudioClips[Random.Range(0, tmp_Length)];
                AudioSource.PlayClipAtPoint(tmp_AudioClip, tmp_HitPoint);

                break;
        }
    }
}
