// using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class JariBechi : MonoBehaviour
{
    public GameObject jariObjectPrefab;
    public DebugPanel debugPanel;

    private JariObject _currentSelected;
    private List<string> _names = new();
    private List<string> _backup;
    private readonly List<JariObject> _jariObjects = new();
    private bool _stopShuffle;
    private readonly List<Tuple<int, string>> _fixedNamesStore = new();

    private void Awake()
    {
        debugPanel.Init();
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        if (directoryInfo.Parent == null)
        {
            return;
        }

        if (!File.Exists(Path.Combine(directoryInfo.Parent.FullName, "Students.txt")))
        {
            File.WriteAllText(Path.Combine(directoryInfo.Parent.FullName, "Students.txt"), string.Empty);
        }

        string[] students = File.ReadAllLines(Path.Combine(directoryInfo.Parent.FullName, "Students.txt"));
        if (students.Length < 1)
        {
            Debug.LogError("Students.txt가 비어있습니다.");
            return;
        }

        foreach (string student in students)
        {
            _names.Add(student);
        }

        /*if (File.Exists(Path.Combine(Application.persistentDataPath, "Backup.txt")))
        {
            _backup = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "Backup.txt")));
        }*/
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                var jari = Instantiate(jariObjectPrefab, transform).GetComponent<JariObject>();
                jari.gameObject.SetActive(true);
                var jariTransform = jari.transform;
                jariTransform.localScale = Vector3.one;
                jariTransform.localPosition = new Vector2(230 * j - 450, 130 * i - 120);
                jari.Init(this, _backup != null ? _backup[19 - (i * 5 + j)] : _names[19 - (i * 5 + j)]);
                _jariObjects.Add(jari);
            }
        }
    }

    // 친절한 설명창을 넣을 예정이었으나 귀찮다 ㅋㅋ
    /* private void Start()
    {
        infoPanel.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("ShowedInfo") != 1)
        {
            infoPanel.gameObject.SetActive(true);
            //PlayerPrefs.SetInt("ShowedInfo", 1);
        }
    }*/

    private void Update()
    {
        if (!_stopShuffle)
        {
            // AudioController.Play("SE_TICK");
            OnClickRandom();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _stopShuffle = true;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _stopShuffle = false;
        }
    }

    public void OnClickJari(JariObject jariObject)
    {
        if (_currentSelected)
        {
            if (_currentSelected.isMoving || jariObject.isMoving)
            {
                return;
            }

            _currentSelected.isMoving = true;
            jariObject.isMoving = true;
            StartCoroutine(SwitchJari(_currentSelected, jariObject));
            _currentSelected = null;
            return;
        }

        _currentSelected = jariObject;
    }

    private IEnumerator SwitchJari(JariObject a, JariObject b)
    {
        Vector3 firstPosA = a.transform.localPosition;
        Vector3 firstPosB = b.transform.localPosition;
        while (true)
        {
            Vector3 posA = a.transform.localPosition;
            Vector3 posB = b.transform.localPosition;

            a.transform.localPosition = Vector3.Lerp(posA, firstPosB, 10 * Time.deltaTime);
            b.transform.localPosition = Vector3.Lerp(posB, firstPosA, 10 * Time.deltaTime);

            if ((a.transform.localPosition - firstPosB).sqrMagnitude <= 0.0001f)
            {
                a.transform.localPosition = Utility.RoundVector3(a.transform.localPosition, 1);
                b.transform.localPosition = Utility.RoundVector3(b.transform.localPosition, 1);
                a.isSelected = false;
                b.isSelected = false;
                a.isMoving = false;
                b.isMoving = false;
                yield break;
            }

            yield return null;
        }
    }

    public void OnClickRandom()
    {
        _fixedNamesStore.Clear();
        var fixedNames = _jariObjects.FindAll(j => j.isFixed).Select(j => j.studentName);
        foreach (var fixedName in fixedNames)
        {
            int i = _names.IndexOf(fixedName);
            _fixedNamesStore.Add(new Tuple<int, string>(i, fixedName));
            _names.Remove(fixedName);
        }

        _names = _names.Shuffle().ToList();
        _fixedNamesStore.Reverse();
        foreach (var fixedNameWithIndex in _fixedNamesStore)
        {
            _names.Insert(fixedNameWithIndex.Item1, fixedNameWithIndex.Item2);
        }

        int index = 0;
        foreach (var jari in _jariObjects)
        {
            jari.Init(this, _names[index]);
            index++;
        }
    }

    // private void OnApplicationQuit()
    // {
    //     string nameSerialized = JsonConvert.SerializeObject(_names);
    //     File.WriteAllText(Path.Combine(Application.persistentDataPath, "Backup.txt"), nameSerialized);
    // }
}