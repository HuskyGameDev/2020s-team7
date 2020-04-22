using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A script I found that makes it so that selection due to mouseover, and selection due to keyboard navigation are unified
/// </summary>
[RequireComponent(typeof(Selectable))]
public class HighlightFix : MonoBehaviour, IPointerEnterHandler, IDeselectHandler {
	public void OnPointerEnter(PointerEventData eventData) {
		if (!EventSystem.current.alreadySelecting)
			EventSystem.current.SetSelectedGameObject(this.gameObject);
	}

	public void OnDeselect(BaseEventData eventData) {
		this.GetComponent<Selectable>().OnPointerExit(null);
	}
}
