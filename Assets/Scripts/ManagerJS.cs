using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManagerJS : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image _JSBackground;
    private Image _JSStick;
    private Vector2 _posInput;

    void Start()
    {
        _JSBackground = GetComponent<Image>();
        _JSStick = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 jsPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_JSBackground.rectTransform, 
                                                                    eventData.position,
                                                                    null, out _posInput))
        {
            _posInput.x = _posInput.x * 2 /_JSBackground.rectTransform.sizeDelta.x;
            _posInput.y = _posInput.y * 2 /_JSBackground.rectTransform.sizeDelta.y;
            _posInput = (_posInput.magnitude > 1.0f) ? _posInput.normalized : _posInput;
            
            // joystick move
            _JSStick.rectTransform.anchoredPosition = new Vector2(
                _posInput.x * _JSBackground.rectTransform.sizeDelta.x / 2, 
                _posInput.y * _JSBackground.rectTransform.sizeDelta.y / 2);
        
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _posInput = Vector2.zero;
        _JSStick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float inputHorizontal()
    {
        if (_posInput.x != 0) return _posInput.x;
        else return Input.GetAxis("Horizontal");
    }
    public float inputVertical()
    {
        if (_posInput.y != 0) return _posInput.y;
        else return Input.GetAxis("Vertical");
    }

}
