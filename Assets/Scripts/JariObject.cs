using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JariObject : MultiClickButton
{
    [SerializeField] TMP_Text nameLabel;
    [SerializeField] Image selectImage;
    [SerializeField] Image fixImage;

    private JariBechi _jariBechi;
    private bool _isSelected;
    private bool _isFixed;

    public string studentName => nameLabel.text;

    public bool isMoving { get; set; }

    public bool isSelected
    {
        get => _isSelected;
        set
        {
            selectImage.gameObject.SetActive(value);
            _isSelected = value;
        }
    }

    public bool isFixed
    {
        get => _isFixed;
        private set
        {
            fixImage.gameObject.SetActive(value);
            _isFixed = value;
        }
    }

    private void Awake()
    {
        selectImage.gameObject.SetActive(false);
        fixImage.gameObject.SetActive(false);
        nameLabel.text = string.Empty;
    }

    public void Init(JariBechi jariBechi, string studentName)
    {
        _jariBechi = jariBechi;
        nameLabel.text = studentName;
        selectImage.gameObject.SetActive(false);
    }

    private void OnClickJari()
    {
        isSelected = !isSelected;
        _jariBechi.OnClickJari(this);
    }

    private void OnDoubleClickJari()
    {
        isFixed = !isFixed;
    }

    public override void OnClicked(int count)
    {
        if (count == 1)
        {
            OnClickJari();
        }
        else if (count == 2)
        {
            OnDoubleClickJari();
        }
    }
}