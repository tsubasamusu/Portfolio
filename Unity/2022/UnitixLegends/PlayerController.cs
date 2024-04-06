using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

namespace yamap
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float previousSpeed = 10;

        [SerializeField]
        private float backSpeed = 2;

        [SerializeField]
        private float speedX = 2;

        [SerializeField, Range(1.0f, 30.0f)]
        private float normalZoomFOV = 20;

        [SerializeField, Range(1.0f, 30.0f)]
        private float ScopeZoomFOV = 5;

        [SerializeField]
        private float getItemLength = 1;

        [SerializeField]
        private KeyCode stoopKey = KeyCode.E;

        [SerializeField]
        private KeyCode getItemKey = KeyCode.Q;

        [SerializeField]
        private KeyCode discardKey = KeyCode.X;

        private Rigidbody playerRb;

        private BoxCollider boxCollider;

        private Animator anim;

        private PlayerHealth playerHealth;

        public PlayerHealth PlayerHealth
        {
            get => playerHealth;
        }

        private UIManager uiManager;

        [SerializeField]
        private CinemachineFollowZoom followZoom;

        private Transform mainCameraTran;

        [SerializeField]
        private Transform playerCharacterTran;

        private Vector3 moveDirection = Vector3.zero;

        private Vector3 desiredMove = Vector3.zero;

        private Vector3 firstColliderCenter;

        private Vector3 firstColliderSize;

        private bool landed;

        private int selectedItemNo = 1;

        public int SelectedItemNo
        {
            get
            {
                return selectedItemNo;
            }
        }

        private enum PlayerCondition
        {
            Idle,
            MoveBack,
            MovePrevious,
            MoveRight,
            MoveLeft,
            Stooping
        }

        private void Reset()
        {
            if (!TryGetComponent(out playerHealth))
            {
                Debug.Log("PlayerHealth 取得出来ません");
            }

            if (!TryGetComponent(out playerRb))
            {
                Debug.Log("Rigidbody 取得出来ません");
            }
            else
            {
                playerRb.useGravity = false;

                playerRb.isKinematic = true;
            }

            if (!TryGetComponent(out boxCollider))
            {
                Debug.Log("boxCollider 取得出来ません");
            }
            else
            {
                firstColliderCenter = boxCollider.center;

                firstColliderSize = boxCollider.size;
            }

            if (!TryGetComponent(out anim))
            {
                Debug.Log("Animator 取得出来ません");
            }

            mainCameraTran = Camera.main.transform;

            previousSpeed = 10;

            backSpeed = 2;

            speedX = 2;

            normalZoomFOV = 20;

            ScopeZoomFOV = 5;

            stoopKey = KeyCode.E;

            getItemKey = KeyCode.Q;

            discardKey = KeyCode.X;
        }

        public void SetUpPlayer(UIManager uiManager)
        {
            Reset();

            this.uiManager = uiManager;

            playerHealth?.SetUpHealth(uiManager);
        }

        void Update()
        {
            if (transform.position.y <= -1f)
            {
                transform.position = Vector3.zero;

                return;
            }

            if (CheckToppled())
            {
                uiManager.SetMessageText("I'm\nTrying To\nRecover", Color.red);

                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            }

            if (!CheckGrounded())
            {
                return;
            }

            PlayAnimation(MovePlayer());

            ControlItem();
        }

        private void FixedUpdate()
        {
            playerRb.MovePosition(transform.position + (desiredMove * Time.fixedDeltaTime));

            if (landed)
            {
                return;
            }

            if (!CheckGrounded())
            {
                transform.Translate(0, -GameData.instance.FallSpeed, 0);
            }
            else
            {
                SoundManager.instance.PlaySE(SeName.LandingSE);

                landed = true;

                playerRb.isKinematic = false;

                playerRb.useGravity = true;
            }
        }

        private bool CheckToppled()
        {
            if (transform.eulerAngles.x < 40f && transform.eulerAngles.x >= 0f)
            {
                return false;
            }
            else if (transform.eulerAngles.x <= 360 && transform.eulerAngles.x > 320f)
            {
                return false;
            }

            if (transform.eulerAngles.z < 40f && transform.eulerAngles.z >= 0f)
            {
                return false;
            }
            else if (transform.eulerAngles.z <= 360 && transform.eulerAngles.z > 320f)
            {
                return false;
            }

            return true;
        }

        private void PlayAnimation(PlayerCondition playerCondition)
        {
            anim.Play(playerCondition.ToString());
        }

        private PlayerCondition MovePlayer()
        {
            playerCharacterTran.eulerAngles = new Vector3(0f, mainCameraTran.eulerAngles.y, 0f);

            desiredMove = (mainCameraTran.forward * moveDirection.z) + (mainCameraTran.right * moveDirection.x);

            if (desiredMove.magnitude < 1f)
            {
                desiredMove = Vector3.zero;
            }

            if (Input.GetAxis("Vertical") > 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * previousSpeed;

                return PlayerCondition.MovePrevious;
            }
            else if (Input.GetAxis("Vertical") < 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * backSpeed;

                return PlayerCondition.MoveBack;
            }

            if (Input.GetAxis("Horizontal") > 0.0f)
            {
                moveDirection.x = Input.GetAxis("Horizontal") * speedX;

                return PlayerCondition.MoveRight;
            }
            else if (Input.GetAxis("Horizontal") < 0.0f)
            {
                moveDirection.x = Input.GetAxis("Horizontal") * speedX;

                return PlayerCondition.MoveLeft;
            }

            if (Input.GetKey(stoopKey))
            {
                boxCollider.center = new Vector3(0f, 0.25f, 0f);

                boxCollider.size = new Vector3(0.5f, 0.5f, 0.5f);

                return PlayerCondition.Stooping;
            }
            else
            {
                boxCollider.center = firstColliderCenter;

                boxCollider.size = firstColliderSize;
            }

            return PlayerCondition.Idle;
        }

        public bool CheckGrounded()
        {
            var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            var tolerance = 0.3f;

            return Physics.Raycast(ray, tolerance);
        }

        private void ControlItem()
        {
            if (Input.anyKeyDown)
            {
                ChangeItem(CheckKey());
            }

            if (Input.GetKeyDown(discardKey))
            {
                SoundManager.instance.PlaySE(SeName.DiscardItemSE);

                ItemManager.instance.DiscardItem(SelectedItemNo - 1);
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                ItemManager.instance.UseItem(ItemManager.instance.GetSelectedItemData(), playerHealth);
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (ItemManager.instance.GetSelectedItemData().itemName != ItemDataSO.ItemName.Sniper)
                {
                    followZoom.m_MaxFOV = normalZoomFOV;

                    followZoom.m_MinFOV = 1.0f;
                }
                else
                {
                    followZoom.m_MaxFOV = ScopeZoomFOV;

                    followZoom.m_MinFOV = 1.0f;

                    uiManager.PeekIntoTheScope();
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                followZoom.m_MaxFOV = 30.0f;

                followZoom.m_MinFOV = 30.0f;

                uiManager.NotPeekIntoTheScope();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (!ItemManager.instance.GetSelectedItemData().enemyCanUse)
                {
                    return;
                }

                SoundManager.instance.PlaySE(SeName.BePreparedSE);
            }

            if (ItemManager.instance.LengthToNearItem > getItemLength || ItemManager.instance.generatedItemDataList.Count == 0)
            {
                uiManager.SetMessageText("", Color.black);

                return;
            }

            ItemManager.instance.CheckIsFull();

            if (ItemManager.instance.IsFull && ItemManager.instance.generatedItemDataList[ItemManager.instance.NearItemNo].itemType != ItemDataSO.ItemType.Bullet)
            {
                uiManager.SetMessageText("Tap 'X' To\nDiscard\nThe Item", Color.red);
            }
            else
            {
                uiManager.SetMessageText("Tap 'Q' To\nGet The\nItem", Color.green);
            }

            if (Input.GetKeyDown(getItemKey))
            {
                ItemManager.instance.GetItem(ItemManager.instance.NearItemNo, true, playerHealth);
            }
        }

        private KeyCode CheckKey()
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    return code;
                }
            }

            return KeyCode.None;
        }

        private void ChangeItem(KeyCode code)
        {
            switch (code)
            {
                case KeyCode.Alpha1:
                    ItemManager.instance.SelectedItemNo = 0;
                    break;

                case KeyCode.Alpha2:
                    ItemManager.instance.SelectedItemNo = 1;
                    break;

                case KeyCode.Alpha3:
                    ItemManager.instance.SelectedItemNo = 2;
                    break;

                case KeyCode.Alpha4:
                    ItemManager.instance.SelectedItemNo = 3;
                    break;

                case KeyCode.Alpha5:
                    ItemManager.instance.SelectedItemNo = 4;
                    break;

                default:
                    return;
            }

            uiManager.SetItemSlotBackgroundColor(ItemManager.instance.SelectedItemNo, Color.red);

            if (ItemManager.instance.GetSelectedItemData().itemName == ItemDataSO.ItemName.None)
            {
                SoundManager.instance.PlaySE(SeName.NoneItemSE);
            }
            else
            {
                SoundManager.instance.PlaySE(SeName.SelectItemSE);
            }
        }
    }
}