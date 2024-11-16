using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonJump : MonoBehaviour , IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) => GameManager.instance.player.JumpingButton();

}
