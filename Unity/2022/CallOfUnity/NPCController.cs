using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CallOfUnity
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCController : ControllerBase
    {
        private NavMeshAgent agent;

        protected override void SetUpController()
        {
            Transform bodyTran = transform.GetChild(0);

            Reset();

            ShotReloadAsync(this.GetCancellationTokenOnDestroy()).Forget();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    Vector3 nearEnemyPos = GetNearEnemyPos();

                    SetTargetPos(nearEnemyPos);

                    Vector3 bodyDir = ((nearEnemyPos + new Vector3(0f, 1.5f, 0f)) - (transform.position + new Vector3(0f, 1.5f, 0f))).normalized;

                    bodyTran.forward = Vector3.Lerp(bodyTran.forward, bodyDir, Time.deltaTime * ConstData.WEAPON_ROT_SMOOTH);
                })
                .AddTo(this);
        }

        private void Reset()
        {
            agent = GetComponent<NavMeshAgent>();

            currentWeaponData = GameData.instance.WeaponDataSO.weaponDataList[UnityEngine.Random.Range(0, GameData.instance.WeaponDataSO.weaponDataList.Count)];

            transform.GetChild(1).GetComponent<MeshRenderer>().material = myTeamNo == 0 ? GameData.instance.Team0Material : GameData.instance.Team1Material;

            GetComponent<CharacterHealth>().SetUp();
        }

        public override void ReSetUp()
        {
            bulletCountForNpc = currentWeaponData.ammunitionNo;
        }

        private async UniTask ShotReloadAsync(CancellationToken token)
        {
            while (true)
            {
                if (GetBulletcCount() <= 0)
                {
                    ReloadAsync(this.GetCancellationTokenOnDestroy()).Forget();

                    await UniTask.Delay(TimeSpan.FromSeconds(GetReloadTime()), cancellationToken: token);
                }

                if (CheckEnemy() && GetBulletcCount() >= 1 && !isReloading)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(currentWeaponData.rateOfFire), cancellationToken: token);

                    Shot();
                }

                await UniTask.Yield(token);
            }
        }

        private void SetTargetPos(Vector3 targetPos)
        {
            agent.destination = targetPos;
        }

        private bool CheckEnemy()
        {
            var ray = new Ray(weaponTran.position, weaponTran.forward);

            if (!Physics.Raycast(ray, out RaycastHit hit, currentWeaponData.firingRange)) return false;

            return (hit.transform.TryGetComponent(out ControllerBase controller) && myTeamNo != controller.myTeamNo);
        }

        private Vector3 GetNearEnemyPos()
        {
            ControllerBase nearEnemy = myTeamNo == 0 ? GameData.instance.npcControllerBaseList[ConstData.TEAMMATE_NUMBER - 1] : GameData.instance.PlayerControllerBase;

            float nearLength = ((myTeamNo == 0 ? GameData.instance.npcControllerBaseList[ConstData.TEAMMATE_NUMBER - 1] : GameData.instance.PlayerControllerBase).transform.position - transform.position).magnitude;

            for (int i = 0; i < GameData.instance.npcControllerBaseList.Count; i++)
            {
                if (GameData.instance.npcControllerBaseList[i].myTeamNo == myTeamNo) continue;

                float length = (GameData.instance.npcControllerBaseList[i].transform.position - transform.position).magnitude;

                if (length < nearLength)
                {
                    nearEnemy = GameData.instance.npcControllerBaseList[i];

                    nearLength = length;
                }
            }

            return nearEnemy.transform.position;
        }
    }
}