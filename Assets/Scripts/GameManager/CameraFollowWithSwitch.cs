using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowWithSwitch
{
    private CameraSwitch cameraSwitch;
    private CameraFollow cameraFollow;

    public CameraFollowWithSwitch(
        CameraSwitch cameraSwitch,
        CameraFollow cameraFollow
    )
    {
        this.cameraSwitch = cameraSwitch;
        this.cameraFollow = cameraFollow;

        cameraSwitch.beforeSwitch += OnBeforeSwitch;
        cameraSwitch.switched += OnSwitched;
    }

    void OnBeforeSwitch()
    {
        cameraFollow.SetSmoothEnabled(false);
    }

    void OnSwitched(Camera camera)
    {
        cameraFollow.SetCamera(camera);

        cameraFollow.StartCoroutine(RestoreSmooting());
    }

    IEnumerator RestoreSmooting()
    {
        yield return new WaitForEndOfFrame();

        cameraFollow.SetSmoothEnabled(true);
    }
}