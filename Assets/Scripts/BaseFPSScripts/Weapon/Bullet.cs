using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
//using UnityTemplateProjects.MultiplayerScripts;
using Random = UnityEngine.Random;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;

        private Transform bulletTransform;
        private Vector3 prevPosition;

        private void Start()
        {
            bulletTransform = transform;
            prevPosition = bulletTransform.position;
        }

        private void Update()
        {
            prevPosition = bulletTransform.position;

            bulletTransform.Translate(0, 0, BulletSpeed * Time.deltaTime);

            if (!Physics.Raycast(prevPosition,
                (bulletTransform.position - prevPosition).normalized,
                out RaycastHit tmp_Hit,
                (bulletTransform.position - prevPosition).magnitude)) return;


            //if (tmp_Hit.collider.TryGetComponent(out IDamager tmp_Damager))
            //{
            //    tmp_Damager.TakeDamage(10);
            //}

            //传输的信息字典
            Dictionary<byte, object> tmp_HitData = new Dictionary<byte, object>();
            tmp_HitData.Add(0, tmp_Hit.point);
            tmp_HitData.Add(1, tmp_Hit.normal);
            tmp_HitData.Add(2, tmp_Hit.collider.tag);


            RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};//接收全部
            SendOptions tmp_SendOptions = SendOptions.SendReliable;//可信的

            //EventCode枚举Tag转发给客户端，客户端接收后按Tag进行解析，执行相应的效果
            PhotonNetwork.RaiseEvent((byte) EventCode.HitObject, tmp_HitData, tmp_RaiseEventOptions, tmp_SendOptions);

            //Destroy(this.gameObject);
        }
    }
}