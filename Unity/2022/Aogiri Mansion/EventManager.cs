using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> eventLightList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> membersList = new List<GameObject>();

    [SerializeField]
    private List<EventDetail> eventDetailList = new();

    [SerializeField]
    private GameObject mainLight;

    [SerializeField]
    private GameObject event8collider;

    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private Animator komaruAnimator;

    [SerializeField]
    private float rotSpeed;

    private void Start()
    {
        SetUpEvents();
    }

    private void SetUpEvents()
    {
        mainLight.SetActive(false);

        event8collider.SetActive(false);

        foreach (GameObject light in eventLightList)
        {
            if (light.TryGetComponent(out Light l))
            {
                l.color = Color.red;
            }

            light.SetActive(false);
        }

        foreach (GameObject menber in membersList)
        {
            menber.SetActive(false);
        }

        for (int i = 0; i < eventDetailList.Count; i++)
        {
            eventDetailList[i].SetUpEvent(GetCharaEvent((CharaName)i));
        }

        UnityAction GetCharaEvent(CharaName charaName)
        {
            return charaName switch
            {
                CharaName.‰ä•”‚è‚¦‚é => PlayEvent1,
                CharaName.ç‘ã‰Y’±”ü => PlayEvent2,
                CharaName.ŒI‹î‚±‚Ü‚é => PreparePlayEvent3,
                CharaName.…ØŒŽ‰ÄŠó => PlayEvent4,
                CharaName.‰¹—ì°Žq => PlayEvent5,
                CharaName.ŽR•‰¹Œº => PlayEvent7,
                CharaName.‘å‘ã^”’ => PreparePlayEvent8,
                _ => null,
            };
        }
    }

    public void PlayEvent1()
    {
        membersList[0].SetActive(true);

        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.NormalHorrorSE));
    }

    public void PlayEvent2()
    {
        eventLightList[0].SetActive(true);

        membersList[1].SetActive(true);

        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.NormalHorrorSE));
    }

    public void PreparePlayEvent3()
    {
        StartCoroutine(PlayEvent3());
    }

    public IEnumerator PlayEvent3()
    {
        membersList[2].SetActive(true);

        membersList[2].transform.DOMoveX(0f, 3f).SetEase(Ease.Linear);

        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.LoomingSE));

        yield return new WaitForSeconds(3f);

        Destroy(membersList[2]);
    }

    public void PlayEvent4()
    {
        membersList[3].SetActive(true);

        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.NormalHorrorSE));
    }

    public void PlayEvent5()
    {
        membersList[4].SetActive(true);

        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.NormalHorrorSE));
    }

    public void PlayEvent7()
    {
        membersList[5].SetActive(true);

        StartCoroutine(TurnCharacter(membersList[5].transform));

        eventLightList[1].SetActive(true);

        AudioSource.PlayClipAtPoint(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.NoriNoriSE).clip, membersList[5].transform.position, 0.5f);

        event8collider.SetActive(true);
    }

    public void PreparePlayEvent8()
    {
        StartCoroutine(PlayEvent8());
    }

    public IEnumerator PlayEvent8()
    {
        membersList[6].SetActive(true);

        membersList[6].transform.DOMoveX(54f, 3f);

        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.ReroReroSE));

        yield return new WaitForSeconds(3f);

        membersList[6].transform.DORotate(new Vector3(0f, 270f, 0f), 3f);
    }

    private IEnumerator TurnCharacter(Transform charaTran)
    {
        while (true)
        {
            charaTran.Rotate(new Vector3(0f, rotSpeed, 0f));

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}