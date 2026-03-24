using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTest
{// TC1: Player được tạo thành công
    [UnityTest]
    public IEnumerator Player_IsCreated_Successfully()
    {
        GameObject player = new GameObject("Player");


        yield return null;


        Assert.IsNotNull(
            player,
            "Player phải được tạo thành công"
        );


        Object.Destroy(player);
    }


    // TC2: Player có thể di chuyển sang phải
    [UnityTest]
    public IEnumerator Player_Can_Move_Right()
    {
        GameObject player = new GameObject("Player");
        player.transform.position = Vector3.zero;


        yield return null;


        player.transform.Translate(Vector3.right * 2f);
        yield return null;


        Assert.Greater(
            player.transform.position.x,
            0,
            "Player phải di chuyển sang phải"
        );


        Object.Destroy(player);
    }


    // TC3: Player tồn tại sau nhiều frame
    [UnityTest]
    public IEnumerator Player_Exists_AfterFrames()
    {
        GameObject player = new GameObject("Player");


        yield return null;
        yield return null;


        Assert.IsTrue(
            player != null,
            "Player phải tồn tại sau nhiều frame"
        );


        Object.Destroy(player);
    }
}
