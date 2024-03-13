using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableNutrient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    private Vector3 originalPosition;
    Nutrient nutrient;

    private void Awake()
    {
        nutrient = GetComponent<Nutrient>();
    }

    private void Start()
    {
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if(transform.parent!= originalParent.parent)
        {
            originalParent = transform.parent;
            originalPosition = transform.position;
            transform.SetParent(transform.parent.parent);
            transform.SetAsLastSibling(); //to make sure the dragged item is over all other items
        }
        GetComponent<Image>().raycastTarget = false; //to make sure the raycast doesn't hit the image itself
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        GetComponent<Image>().raycastTarget = true;

    }

    public void SetParent(Transform trasformParent){
        transform.SetParent(trasformParent);
    }

    public void ReturnToOriginalParent(){
        SetParent(originalParent);
    }

    


}
