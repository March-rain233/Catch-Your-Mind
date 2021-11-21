using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MakerPanel : MonoBehaviour
{
    private RectTransform _rectTransform;

    /// <summary>
    /// 移动区域
    /// </summary>
    [SerializeField]
    private RectTransform _moveArea;

    /// <summary>
    /// 卡带列表
    /// </summary>
    [SerializeField]
    private List<CardView> _cardViews;

    [SerializeField]
    private float _maxRotation;

    [SerializeField]
    private float _minRotation;

    /// <summary>
    /// info面板卡带的初始位置
    /// </summary>
    private Vector2 _oriPosition;

    /// <summary>
    /// info面板卡带
    /// </summary>
    [SerializeField]
    private RectTransform _infoCard;

    /// <summary>
    /// 选中的卡带
    /// </summary>
    private CardView _select;

    /// <summary>
    /// 信息面板
    /// </summary>
    [SerializeField]
    private CanvasGroup _infoPanel;

    [SerializeField]
    private Button _create;
    [SerializeField]
    private Button _return;
    [SerializeField]
    private Button _insert;

    /// <summary>
    /// 卡带名称
    /// </summary>
    [SerializeField]
    private TMPro.TextMeshProUGUI _cardName;
    /// <summary>
    /// 卡带内容显示面板
    /// </summary>
    [SerializeField]
    private RectTransform _infoContent;

    /// <summary>
    /// 对话队列
    /// </summary>
    [SerializeField]
    private VerticalQueue _dialogQueue;
    /// <summary>
    /// 对话框的预制体
    /// </summary>
    [SerializeField]
    private GameObject _dialogPrefabs;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _oriPosition = _infoCard.localPosition;

        _return.onClick.AddListener(CloseInfoPanel);
        
        _cardViews.ForEach(card =>
        {
            card.RectTransform.localPosition = new Vector2(
                Random.Range(_moveArea.localPosition.x - _moveArea.rect.width / 2, _moveArea.localPosition.x + _moveArea.rect.width / 2),
                Random.Range(_moveArea.localPosition.y - _moveArea.rect.height / 2, _moveArea.localPosition.y + _moveArea.rect.height / 2));
            card.RectTransform.localEulerAngles = new Vector3(0, 0, Random.Range(_minRotation, _maxRotation));
            card.OnDragged = OnCardViewDragged;
            card.OnClicked = OpenInfoPanel;
        });
    }

    /// <summary>
    /// 朝对话序列加入文本框
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public StaticDialog EnqueueDialog(TalkSystem.TextBody body)
    {
        var dialog = Instantiate(_dialogPrefabs, _dialogQueue.transform).GetComponent<StaticDialog>();
        _dialogQueue.Enqueue(dialog.GetComponent<RectTransform>());
        return dialog;
    }

    /// <summary>
    /// 查看当前运行文本框
    /// </summary>
    /// <returns></returns>
    public StaticDialog GetCurrentDialog()
    {
        var last = _dialogQueue.Last();
        if (last)
        {
            return last.GetComponent<StaticDialog>();
        }
        return null;
    }

    private void OnCardViewDragged(CardView cardView, PointerEventData data)
    {
        Vector2 temp = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, data.position, null, out temp);
        if (!RectTransformUtility.RectangleContainsScreenPoint(_moveArea, data.position))
        {
            temp.x = Mathf.Clamp(temp.x, _moveArea.localPosition.x - _moveArea.rect.width/2, _moveArea.localPosition.x + _moveArea.rect.width / 2);
            temp.y = Mathf.Clamp(temp.y, _moveArea.localPosition.y - _moveArea.rect.height/2, _moveArea.localPosition.y + _moveArea.rect.height / 2);
        }
        cardView.RectTransform.DOLocalMove(temp, 0.2f);
    }

    private void OpenInfoPanel(CardView cardView, PointerEventData data)
    {
        if (data.clickCount != 2) { return; }
        float duration = 0.5f;
        _select = cardView;

        _infoPanel.blocksRaycasts = true;
        _infoPanel.interactable = true;
        _infoPanel.DOFade(1, duration);

        cardView.gameObject.SetActive(false);

        _infoCard.position = _select.RectTransform.position;
        _infoCard.rotation = _select.RectTransform.rotation;
        _infoCard.DOLocalMove(_oriPosition, duration);
        _infoCard.DOLocalRotate(new Vector3(0, 0, 0), duration);

    }

    private void InitCardInfo(Card card)
    {
        _cardName.text = card.CardName;
        var info = Instantiate(card.Inspector, _infoContent);
        //var rect = info.GetComponent<RectTransform>();
        
    }

    private void DestoryCardInfo()
    {
        _cardName.text = null;
        while (_infoContent.childCount > 0)
        {
            Destroy(_infoContent.GetChild(0));
        }
    }

    private void CloseInfoPanel()
    {
        float duration = 0.5f;
        _infoPanel.DOFade(0, duration);
        _infoPanel.transform.Find("Blur").gameObject.SetActive(false);

        var position = _select.RectTransform.position;
        var rotation = _select.RectTransform.eulerAngles;
        _select.RectTransform.position = _infoCard.position;
        _select.RectTransform.rotation = _infoCard.rotation;
        _select.gameObject.SetActive(true);
        _infoCard.gameObject.SetActive(false);
        _select.RectTransform.DOMove(position, duration);
        _select.RectTransform.DORotate(rotation, duration).onComplete = () =>
        {
            _infoCard.gameObject.SetActive(true);
            _infoPanel.transform.Find("Blur").gameObject.SetActive(true);
            _infoPanel.blocksRaycasts = false;
            _infoPanel.interactable = false;
            DestoryCardInfo();
            _select = null;
        };
    }
}
