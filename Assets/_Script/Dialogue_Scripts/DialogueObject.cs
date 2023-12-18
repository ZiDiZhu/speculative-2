using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scriptable object is used to store the dialogue and responses
[CreateAssetMenu(menuName ="Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;
    
    public string[] Dialogue => dialogue; //reference to the dialogue array
    public Response[] Responses => responses; //reference to the responses array
    public bool HasResponses =>Responses !=null &&responses.Length > 0; //returns true if there are responses
}
