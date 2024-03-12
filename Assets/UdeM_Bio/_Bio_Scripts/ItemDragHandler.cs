using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    private Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if(transform.parent!= originalParent.parent)
        {
            originalParent = transform.parent;
            originalPosition = transform.position;
            transform.SetParent(transform.parent.parent);
            transform.SetAsFirstSibling();
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

    }

    public void SetParent(Transform trasformParent){
        transform.SetParent(trasformParent);
    }

    public void ReturnToOriginalParent(){
        SetParent(originalParent);
    }

    private void Start()
    {
        originalParent = transform.parent;
    }


}
