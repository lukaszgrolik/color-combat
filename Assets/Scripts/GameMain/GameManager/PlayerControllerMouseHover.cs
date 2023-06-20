using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GameCore;

public interface IGroundHitProvider
{
    // bool GroundHitFound { get; }
    Opt<Vector3> GroundHitPoint { get; }
}

public interface IAgentMouseEvents
{
    event System.Action<Agent> agentMouseEntered;
    event System.Action<Agent> agentMouseLeft;
}

public class Opt<T>
{
    public T value;

    public Opt()
    {
        this.value = default(T);
    }

    public Opt<T> Updated(T val)
    {
        this.value = val;

        return this;
    }
}

// static class None : Opt
// {

// }

public class PlayerControllerMouseHover : IGroundHitProvider, IAgentMouseEvents
{
    private IGameLayerMasksProvider gameLayerMasksProvider;
    private IRegistryAgents registryAgents;
    private Camera camera;

    // --- ground

    // private bool groundHitFound;    public bool GroundHitFound => groundHitFound;
    // private Vector3 groundHitPoint; public Vector3 GroundHitPoint => groundHitPoint;
    static  Opt<Vector3> _groundHitPoint = new Opt<Vector3>();
    private Opt<Vector3> groundHitPoint = null; public Opt<Vector3> GroundHitPoint => groundHitPoint;

    // --- agent

    private bool agentHitFound; public bool AgentHitFound => agentHitFound;
    private Agent agentHitAgent;

    // --- dropped item

    private bool droppedItemHitFound; public bool DroppedItemHitFound => droppedItemHitFound;

    // private DroppedItem droppedItemHitItem;
    // public DroppedItem DroppedItemHitItem => droppedItemHitItem;

    // ---

    public event System.Action<Agent> agentMouseEntered;
    public event System.Action<Agent> agentMouseLeft;

    private Ray camToMouseRay;

    public PlayerControllerMouseHover(
        IGameLayerMasksProvider gameLayerMasksProvider,
        IRegistryAgents registryAgents,
        Camera camera
    )
    {
        this.gameLayerMasksProvider = gameLayerMasksProvider;
        this.registryAgents = registryAgents;
        this.camera = camera;
    }

    // @todo @perf OnFixedUpdate?
    public void OnUpdate()
    {
        camToMouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camToMouseRay, out var uiHitInfo, 100, gameLayerMasksProvider.UILayerMask))
        {
            Debug.Log($"uiHitInfo.name: {uiHitInfo.point}");
        }
        else
        {
            DetectInfo();
        }
    }

    void DetectInfo()
    {
        if (Physics.Raycast(camToMouseRay, out var groundHitInfo, 100, gameLayerMasksProvider.GroundLayerMask))
        {
            // groundHitFound = true;
            groundHitPoint = _groundHitPoint.Updated(groundHitInfo.point);
        }
        else
        {
            // groundHitFound = false;
            groundHitPoint = null;
        }

        if (Physics.Raycast(camToMouseRay, out var agentHitInfo, 100, gameLayerMasksProvider.AgentLayerMask))
        {
            if (agentHitFound == false)
            {
                agentHitFound = true;
                // agentHitAgent = gameManager.dict_object_agentCtrl[agentHitInfo.collider.gameObject].Agent;
                agentHitAgent = registryAgents.GetAgentByGameObject(agentHitInfo.collider.gameObject);

                agentMouseEntered?.Invoke(agentHitAgent);
            }
        }
        else
        {
            if (agentHitFound)
            {
                agentMouseLeft?.Invoke(agentHitAgent);
            }

            agentHitFound = false;
        }

        // if (Physics.Raycast(ray, out var droppedItemHitInfo, 100, gameplayManager.DroppedItemLayerMask))
        // {
        //     droppedItemHitFound = true;
        //     droppedItemHitItem = gameplayManager.dict_object_droppedItemMB[droppedItemHitInfo.collider.gameObject].DroppedItem;
        // }
        // else
        // {
        //     droppedItemHitFound = false;
        // }
    }
}