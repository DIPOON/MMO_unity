using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction;
    public Action<Define.MouseEvent> MouseAction;
    
    bool _pressed = false;

    public void OnUpdate()
    {
        // UI 버튼 클릭 시 무시
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        // 마우스에 영향을 주지 않고자 Input.anyKey 이쪽으로
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                    MouseAction.Invoke(Define.MouseEvent.Click);
                _pressed = false;
            }
        }
    }
}
