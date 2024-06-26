using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetCanva : MonoBehaviour
{
    public void Reset()
    {
        // Reload the scene containting the Blockly canvas
        SceneManager.LoadScene((int)SceneEnum.Scenes.Blockly);
    }
}
