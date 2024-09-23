using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class AimingCtrl : MonoBehaviour,IPunObservable
{
    //组件
    public Transform arms;
    public Transform aimTarget;

    private PhotonView photonView;

    //变量
    private Vector3 localPosition;
    private Quaternion localRotation;

    public float aimTargetDistance = 5f;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        localPosition = aimTarget.position;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            //获取手臂的角度作为四元数
            localRotation = arms.localRotation;
            localPosition = localRotation * Vector3.forward * aimTargetDistance;//旋转后的向量
        }

        //更改aimTarget的本地角度
        aimTarget.localPosition = Vector3.Lerp(aimTarget.localPosition, localPosition, Time.deltaTime * 20);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //传输数据
        if (stream.IsWriting)
        {
            //发送数据
            stream.SendNext(localPosition);
        }
        else
        {
            //接收数据
            localPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
