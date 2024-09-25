using UnityEngine;
using UnityEngine.EventSystems;

public class HandCardDragHover : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Card Properties")]
    public HandCard card;
    public CanvasGroup canvasGroup;

    [Header("Card Hover")]
    public bool canHover = false;

    [Header("Card Drag")]
    public bool canDrag = false;
    Transform parentReturnTo = null;
    public GameObject EmptyCard;
    private GameObject temp;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canHover) return;

        card.transform.localScale = new Vector2(0.8f, 0.8f);
        card.transform.localPosition = new Vector2(card.transform.localPosition.x, 190);
        int index = card.transform.GetSiblingIndex();

        Player.gameManager.isHovering = true;
        Player.gameManager.CmdOnCardHover(-25, index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!canHover) return;

        card.transform.localScale = new Vector2(0.5f, 0.5f);
        card.transform.localPosition = new Vector2(card.transform.localPosition.x, 0);
        int index = card.handIndex;

        Player.gameManager.CmdOnCardHover(0, index);
        Player.gameManager.isHovering = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        temp = Instantiate(EmptyCard);
        temp.transform.SetParent(this.transform.parent, false);

        temp.transform.SetSiblingIndex(transform.GetSiblingIndex());

        parentReturnTo = this.transform.parent;
        transform.SetParent(this.transform.parent.parent, false);

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        Vector3 screenPoint = eventData.position;
        screenPoint.z = 10.0f;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        transform.SetParent(parentReturnTo, false);
        transform.SetSiblingIndex(temp.transform.GetSiblingIndex());
        canvasGroup.blocksRaycasts = true;
        Destroy(temp);
    }
}
