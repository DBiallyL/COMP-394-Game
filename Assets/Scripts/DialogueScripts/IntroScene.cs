using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class IntroScene : MonoBehaviour
{
    [YarnCommand("nm_appear")]
    public static void ChangeAnimation(GameObject obj, string animState) {
        obj.GetComponent<Animator>().Play(animState);
    }


}
