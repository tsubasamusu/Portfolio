using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace yamap
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent agent;

        private Animator animator;

        [SerializeField, Header("ŽË’ö‹——£")]
        private float range;

        [SerializeField]
        private float getItemLength;

        [SerializeField]
        private Transform enemyWeaponTran;

        private bool didPostLandingProcessing;

        private bool gotItem;

        private bool stopFlag;

        private float enemyhp = 100f;

        private float timer;

        private Vector3 firstPos;

        private ItemDataSO.ItemData usedItemData;

        private UIManager uiManager;

        private EnemyGenerator enemyGenerator;

        private Transform shotBulletTran;

        private Transform playerTran;

        private ItemDetail usedItemObj;

        private int nearItemNo;

        private int myNo;

        public void SetUpEnemy(UIManager uiManager, EnemyGenerator enemyGenerator, PlayerController player, int no)
        {
            if (!TryGetComponent(out agent))
            {
                Debug.Log("NavMeshAgent‚ÌŽæ“¾‚ÉŽ¸”s");
            }
            else
            {
                agent.enabled = false;
            }

            if (!TryGetComponent(out animator))
            {
                Debug.Log("Animator‚ÌŽæ“¾‚ÉŽ¸”s");
            }

            this.uiManager = uiManager;

            this.enemyGenerator = enemyGenerator;

            playerTran = player.transform;

            myNo = no;

            StartCoroutine(MeasureTime());

            shotBulletTran = transform.GetChild(3).transform;
        }

        private void FixedUpdate()
        {
            if (!CheckGrounded())
            {
                transform.Translate(0, -GameData.instance.FallSpeed, 0);
            }
        }

        private void Update()
        {
            if (!CheckGrounded())
            {
                return;
            }

            if (!didPostLandingProcessing)
            {
                agent.enabled = true;

                agent.stoppingDistance = 0f;

                StartCoroutine(ControlAnimation());

                didPostLandingProcessing = true;
            }

            if (stopFlag)
            {
                return;
            }

            if (!gotItem)
            {
                SetNearItemNo();

                SetTargetPosition(ItemManager.instance.generatedItemTranList[nearItemNo].position);

                if (GetLengthToNearItem(nearItemNo) <= getItemLength)
                {
                    gotItem = GetItem(nearItemNo);

                    agent.stoppingDistance = 30f;
                }

                return;
            }

            if (!usedItemData.enemyCanUse)
            {
                gotItem = false;

                agent.stoppingDistance = 0f;

                Destroy(usedItemObj);

                return;
            }

            SetTargetPosition(GetNearEnemyPos());

            if (CheckEnemy())
            {
                ShotBullet(usedItemData);
            }
        }

        private Vector3 GetNearEnemyPos()
        {
            Vector3 nearPos = playerTran.position;

            for (int i = 0; i < enemyGenerator.generatedEnemyList.Count; i++)
            {
                if (i == myNo)
                {
                    continue;
                }

                if (enemyGenerator.generatedEnemyList[i] == null)
                {
                    enemyGenerator.generatedEnemyList.RemoveAt(i);

                    continue;
                }

                Vector3 pos = enemyGenerator.generatedEnemyList[i].transform.position;

                if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude)
                {
                    nearPos = pos;
                }
            }

            return nearPos;
        }

        private void SetNearItemNo()
        {
            if (ItemManager.instance.generatedItemTranList.Count <= 0)
            {
                return;
            }

            int itemNo = 0;

            Vector3 nearPos = ItemManager.instance.generatedItemTranList[0].position;

            for (int i = 0; i < ItemManager.instance.generatedItemTranList.Count; i++)
            {
                if (!ItemManager.instance.generatedItemDataList[i].enemyCanUse)
                {
                    continue;
                }

                Vector3 pos = ItemManager.instance.generatedItemTranList[i].position;

                if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude)
                {
                    itemNo = i;

                    nearPos = pos;
                }
            }

            nearItemNo = itemNo;
        }

        private float GetLengthToNearItem(int nearItemNo)
        {
            return Vector3.Scale((ItemManager.instance.generatedItemTranList[nearItemNo].position - transform.position), new Vector3(1, 0, 1)).magnitude;
        }

        private void SetTargetPosition(Vector3 targetPos)
        {
            agent.destination = targetPos;
        }

        public bool CheckGrounded()
        {
            var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            var tolerance = 0.3f;

            return Physics.Raycast(ray, tolerance);
        }

        private bool CheckEnemy()
        {
            Ray ray = new Ray(enemyWeaponTran.position, enemyWeaponTran.forward);

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, range))
            {
                return false;
            }

            if (hitInfo.transform.gameObject.CompareTag("Player") || hitInfo.transform.gameObject.CompareTag("Enemy"))
            {
                return true;
            }

            return false;
        }

        public bool GetItem(int nearItemNo)
        {
            usedItemData = ItemManager.instance.generatedItemDataList[nearItemNo];

            usedItemObj = Instantiate(ItemManager.instance.generatedItemDataList[nearItemNo].itemPrefab, enemyWeaponTran);

            ItemManager.instance.GetItem(nearItemNo, false);

            return true;
        }

        private IEnumerator MeasureTime()
        {
            while (true)
            {
                timer += Time.deltaTime;

                yield return null;
            }
        }

        private void ShotBullet(ItemDataSO.ItemData itemData)
        {
            if (timer < itemData.interval)
            {
                return;
            }

            BulletDetailBase bullet = Instantiate(itemData.weaponPrefab, shotBulletTran.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0)).GetComponent<BulletDetailBase>();

            bullet.SetUpBulletDetail(itemData.attackPower, BulletOwnerType.Enemy, enemyWeaponTran.forward, itemData.seName, itemData.interval, itemData.effectPrefab);

            bullet.transform.parent = transform;

            timer = 0;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.parent == transform)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent(out BulletDetailBase bulletDetail))
            {
                float attackPower = bulletDetail.GetAttackPower();

                if (bulletDetail.BulletOwnerType == BulletOwnerType.Player)
                {
                    uiManager.PrepareGenerateFloatingMessage(Mathf.Abs(attackPower).ToString("F0"), Color.yellow);
                }

                UpdateEnemyHp(attackPower, bulletDetail.BulletOwnerType);

                bulletDetail.AddTriggerBullet(this);
            }
        }

        private void UpdateEnemyHp(float updateValue, BulletOwnerType bulletOwnerType = BulletOwnerType.Player)
        {
            enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

            if (enemyhp == 0.0f)
            {
                if (bulletOwnerType == BulletOwnerType.Player)
                {
                    yamap.GameData.instance.KillCount++;
                }

                Destroy(gameObject);
            }
        }

        public void PrepareTearGasGrenade()
        {
            StartCoroutine(AttackedByTearGasGrenade());
        }

        private IEnumerator AttackedByTearGasGrenade()
        {
            stopFlag = true;

            SetTargetPosition(transform.position);

            yield return new WaitForSeconds(5.0f);

            stopFlag = false;
        }

        private void WasKilled()
        {

        }

        private IEnumerator ControlAnimation()
        {
            while (true)
            {
                firstPos = transform.position;

                yield return new WaitForSeconds(0.1f);

                Vector3 currentPos = transform.position;

                float velocity = (currentPos - firstPos).magnitude / 0.1f;

                animator.SetBool("MovePrevious", velocity > 0.1f);

                animator.SetBool("Idle", velocity <= 0.1f);

                yield return null;
            }
        }
    }
}