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

    /// <summary>
    /// 预设的正确序列
    /// </summary>
    [SerializeField]
    private List<Card> _correctCardList;
    /// <summary>
    /// 当前选中的卡的序列
    /// </summary>
    [SerializeField]
    private CardView[] _selectedCardList;

    /// <summary>
    /// 侧边卡带按钮组
    /// </summary>
    [SerializeField]
    private List<Button> _edgeCard;

    /// <summary>
    /// 侧边栏
    /// </summary>
    [SerializeField]
    private MoveAround _edge;
    [SerializeField]
    private FillSlider _timeView;

    /// <summary>
    /// 选择槽
    /// </summary>
    [SerializeField]
    private List<Toggle> _slotGroup;

    /// <summary>
    /// 选择插入的槽
    /// </summary>
    private int _selectedSlot;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _oriPosition = _infoCard.localPosition;

        _return.onClick.AddListener(CloseInfoPanel);
        _create.onClick.AddListener(Create);
        _insert.onClick.AddListener(Insert);

        GameManager.Instance.TimeChanged += TimeChangedHandler;

        _infoPanel.blocksRaycasts = false;
        _infoPanel.alpha = 0;
        _infoPanel.interactable = false;

        for(int i = 0; i < _slotGroup.Count; ++i)
        {
            int temp = i;
            _slotGroup[i].onValueChanged.AddListener((v) =>
            {
                if (v)
                {
                    Debug.Log(temp);
                    _selectedSlot = temp;
                }
            });
        }

        _selectedCardList = new CardView[4];

        for(int i = 0; i < _edgeCard.Count; ++i)
        {
            int temp = i;
            _edgeCard[i].onClick.AddListener(() => PopCard(temp));
        }

        _cardViews.ForEach(card =>
        {
            PutCard(card);
        });
    }

    private void PutCard(CardView card)
    {
        card.gameObject.SetActive(true);
        card.RectTransform.localPosition = new Vector2(
            Random.Range(_moveArea.localPosition.x - _moveArea.rect.width / 2, _moveArea.localPosition.x + _moveArea.rect.width / 2),
            Random.Range(_moveArea.localPosition.y - _moveArea.rect.height / 2, _moveArea.localPosition.y + _moveArea.rect.height / 2));
        card.RectTransform.localEulerAngles = new Vector3(0, 0, Random.Range(_minRotation, _maxRotation));
        card.OnDragged = OnCardViewDragged;
        card.OnClicked = OpenInfoPanel;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.TimeChanged -= TimeChangedHandler;
        }
    }

    private void TimeChangedHandler(float value)
    {
        _timeView.Value = value / GameManager.Instance.MaxTime;
    }

    /// <summary>
    /// 朝对话序列加入文本框
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public StaticDialog EnqueueDialog(TalkSystem.TextBody body)
    {
        var dialog = Instantiate(_dialogPrefabs, _dialogQueue.transform).GetComponent<StaticDialog>();
        _dialogQueue.Enqueue(dialog.transform);
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

        //信息面板展示
        _infoPanel.blocksRaycasts = true;
        _infoPanel.interactable = true;
        _infoPanel.DOFade(1, duration);

        cardView.gameObject.SetActive(false);

        var content = Instantiate(cardView.Card.Inspector, _infoContent).GetComponent<RectTransform>();
        //content.anchorMin = Vector2.zero;
        //content.anchorMax = Vector2.one;
        //content.offsetMin = Vector2.zero;
        //content.offsetMax = Vector2.zero;

        _infoCard.GetComponent<Image>().sprite = cardView.Card.CardSprite;
        _infoCard.position = _select.RectTransform.position;
        _infoCard.rotation = _select.RectTransform.rotation;
        _infoCard.DOLocalMove(_oriPosition, duration);
        _infoCard.DOLocalRotate(new Vector3(0, 0, 0), duration);

        _cardName.text = _select.Card.CardName;

        _edge.Move();

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
        for(int i = _infoContent.childCount - 1; i >= 0; --i)
        {
            Destroy(_infoContent.GetChild(i).gameObject);
        }
    }

    private void CloseInfoPanel()
    {
        float duration = 0.5f;
        _infoPanel.DOFade(0, duration).onComplete = ()=>
        {
            _infoCard.gameObject.SetActive(true);
            _infoPanel.transform.Find("Blur").gameObject.SetActive(true);
            _infoPanel.blocksRaycasts = false;
            _infoPanel.interactable = false;
            DestoryCardInfo();
            _select = null;
        };
        _infoPanel.transform.Find("Blur").gameObject.SetActive(false);

        _edge.Back();

        if (_select)
        {
            var position = _select.RectTransform.position;
            var rotation = _select.RectTransform.eulerAngles;
            _select.RectTransform.position = _infoCard.position;
            _select.RectTransform.rotation = _infoCard.rotation;
            _select.gameObject.SetActive(true);
            _infoCard.gameObject.SetActive(false);
            _select.RectTransform.DOMove(position, duration);
            _select.RectTransform.DORotate(rotation, duration);
        }
    }

    private void Insert()
    {
        _edgeCard[_selectedSlot].image.sprite = _select.Card.PressSprite;
        _edgeCard[_selectedSlot].spriteState = new SpriteState() { highlightedSprite = _select.Card.PressSpriteFloat };
        var color = _edgeCard[_selectedSlot].image.color;
        color.a = 1;
        _edgeCard[_selectedSlot].image.color = color;
        _edgeCard[_selectedSlot].image.raycastTarget = true;

        _slotGroup[_selectedSlot].interactable = false;

        _selectedCardList[_selectedSlot] = _select;
        _selectedSlot = -1;
        _select = null;

        CloseInfoPanel();
    }

    /// <summary>
    /// 取出卡带
    /// </summary>
    /// <param name="i"></param>
    private void PopCard(int i)
    {
        var color = _edgeCard[i].image.color;
        color.a = 0;
        _edgeCard[i].image.color = color;
        _edgeCard[i].image.raycastTarget = false;

        _slotGroup[i].interactable = true;

        var card = _selectedCardList[i];
        _selectedCardList[i] = null;

        PutCard(card);
    }

    /// <summary>
    /// 合成按钮的Handler
    /// </summary>
    /// <remarks>
    /// 执行卡带正确性检查
    /// </remarks>
    private void Create()
    {
        if(_correctCardList.Count != _selectedCardList.Length)
        {
            CreateFail();
            return;
        }
        for(int i = _correctCardList.Count - 1; i >= 0; --i)
        {
            if(_correctCardList[i] != _selectedCardList[i].Card)
            {
                CreateFail();
                return;
            }
        }
        CreateSuccess();
    }

    private void CreateSuccess()
    {
        //todo:胜利效果
    }

    private void CreateFail()
    {
        //todo:失败效果
    }
}
