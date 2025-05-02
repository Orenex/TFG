using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovimientoCarta : MonoBehaviour
{
    #region Fields and Properties
    private bool _isSelected;
    private Canvas _cardCanvas;
    private RectTransform _rectTransform;
    private Carta _card;

    private readonly string CANVAS_TAG = "CardCanvas";

    #endregion


    # region Methods

    private void Start()
    {
        _cardCanvas = GameObject.FindGameObjectWithTag(CANVAS_TAG).GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
    }


    public void OnClick()
    {

    }

    #endregion
}
