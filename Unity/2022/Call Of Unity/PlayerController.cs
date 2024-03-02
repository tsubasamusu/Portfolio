using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CallOfUnity
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : ControllerBase
    {
        protected override void SetUpController()
        {
            Reset();

            Rigidbody rb = GetComponent<Rigidbody>();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (Mathf.Abs((transform.position - Vector3.zero).magnitude) > ConstData.MAX_LENGTH_FROM_CENTER)
                    {
                        float num = 0f;

                        GetComponent<CharacterHealth>().Die(ref num, this);
                    }

                    Move();

                    if (Input.GetKeyDown(ConstData.CHANGE_WEAPON_KEY)) ChangeWeapon();

                    if (Input.GetKeyUp(ConstData.STANCE_KEY))
                    {
                        Camera.main.DOFieldOfView(ConstData.NORMAL_FOV, ConstData.STANCE_TIME);
                    }
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(ConstData.RELOAD_KEY))
                .ThrottleFirst(TimeSpan.FromSeconds(GetReloadTime()))
                .Subscribe(_ => ReloadAsync(this.GetCancellationTokenOnDestroy()).Forget())
                .AddTo(this);

            float timer0 = 100f;

            float timer1 = 100f;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    timer0 += Time.deltaTime;

                    timer1 += Time.deltaTime;

                    if (!Input.GetKey(ConstData.SHOT_KEY) || GetBulletcCount() < 1 || isReloading) return;

                    if (currentWeaponData != GameData.instance.playerWeaponInfo.info0.data)
                    {
                        if (timer0 >= currentWeaponData.rateOfFire)
                        {
                            timer0 = 0f;

                            Shot();
                        }
                    }
                    else
                    {
                        if (timer1 >= currentWeaponData.rateOfFire)
                        {
                            timer1 = 0f;

                            Shot();
                        }
                    }
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(ConstData.STANCE_KEY))
                .ThrottleFirst(TimeSpan.FromSeconds(ConstData.STANCE_TIME))
                .Subscribe(_ => Camera.main.DOFieldOfView(ConstData.STANCE_FOV, ConstData.STANCE_TIME).SetLink(gameObject))
                .AddTo(this);

            void ChangeWeapon()
            {
                if (isReloading || Input.GetKey(ConstData.SHOT_KEY)) return;

                currentWeaponData = currentWeapoonNo == 0 ? GetWeaponInfo(1).weaponData : GetWeaponInfo(0).weaponData;

                currentWeapoonNo = currentWeapoonNo == 0 ? 1 : 0;

                DisplayObjWeapon();
            }

            void Move()
            {
                Vector3 enteredMovement = Vector3.Scale((Camera.main.transform.right * Input.GetAxis("Horizontal") + Camera.main.transform.forward * Input.GetAxis("Vertical")), new Vector3(1f, 0f, 1f));

                rb.isKinematic = enteredMovement.magnitude == 0f;

                rb.MovePosition(rb.position + (enteredMovement * GetMoveVelocity() * Time.deltaTime));

                float GetMoveVelocity()
                {
                    if (CheckObstacles()) return 0f;

                    return Input.GetKey(ConstData.RUN_KEY) ? ConstData.RUN_SPEED : ConstData.WALK_SPEED;
                }

                bool CheckObstacles()
                {
                    var ray = new Ray(transform.position + (Vector3.up * 1f), enteredMovement.normalized);

                    return Physics.Raycast(ray, 1f);
                }
            }
        }

        public override void ReSetUp()
        {
            SetBulletCount(0, GetWeaponInfo(0).weaponData.ammunitionNo);

            SetBulletCount(1, GetWeaponInfo(1).weaponData.ammunitionNo);
        }

        private void Reset()
        {
            isPlayer = true;

            myTeamNo = 0;

            currentWeapoonNo = 0;

            currentWeaponData = GetWeaponInfo(0).weaponData;
        }
    }
}