using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultiClickButton : MonoBehaviour, IPointerClickHandler
{
    private float _lastClickTime = 0;

    public virtual void OnClicked(int clickCount)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float time = Time.unscaledTime;
        OnClicked(1);
        if (_lastClickTime + 0.35f > time)
        {
            OnClicked(2);
        }
        _lastClickTime = time;
    }
}