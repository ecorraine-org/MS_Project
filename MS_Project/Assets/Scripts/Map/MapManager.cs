using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.SceneStreamer;

public class MapManager : MonoBehaviour
{
    [Header("カメラ")]
    [SerializeField] private Camera _mapcamera;
    [SerializeField] private Transform _playerTransform;

    [Header("描画用設定")]
    [SerializeField] private Color _visitedRoomColor = Color.white;
    [SerializeField] private Color _activeRoomColor = Color.gray;
    [SerializeField] private Vector3 _minimapOffset = new Vector3(0, 10, 0);
    [SerializeField] private float minimapZoom = 10f;

    //マップ情報を保持する辞書
    private Dictionary<string, MinimapRoom> _rooms = new();
    private SceneStreamer _sceneStreamer;
    private string _currentRoomId;



    //LINQを使ってマップの情報を取得
    //動的にマップの情報を取得する


    //初期化時に
    private void Awake()
    {
        _sceneStreamer = FindObjectOfType<SceneStreamer>();
        if (_sceneStreamer != null)
        {
            _sceneStreamer.onLoaded.AddListener(OnSceneLoaded);
        }
    }

    private void InitializeCamera()
    {
        if (_mapcamera == null) return;

        _mapcamera.orthographic = true;
        _mapcamera.orthographicSize = minimapZoom;
        _mapcamera.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    private void Update()
    {
        //プレイヤーの位置を取得
        //プレイヤーの位置を元に現在のマップを取得
        //現在のマップを元に訪問済みのマップを更新
    }

    private void LateUpdate()
    {
        if (_mapcamera == null || _playerTransform == null) return;

        UpdateMinimapPosition();
        UpdateCurrentRoom();
    }

    private void UpdateMinimapPosition()
    {
        Vector3 targetPosition = _playerTransform.position + _minimapOffset;
        _mapcamera.transform.position = targetPosition;
    }

    private void UpdateCurrentRoom()
    {
        if (_sceneStreamer == null) return;

        string newRoomId = _sceneStreamer.GetCurrentScene();
        if (newRoomId != _currentRoomId)
        {
            OnRoomChanged(newRoomId);
        }
    }

    private void OnRoomChanged(string newRoomId)
    {
        _currentRoomId = newRoomId;
        if (_rooms.TryGetValue(newRoomId, out MinimapRoom room))
        {
            room.IsVisited = true;
            room.IsActive = true;
        }
    }

    private void UpdateRoomConnections(MinimapRoom room, string sceneName)
    {
        GameObject sceneRoot = GameObject.Find(sceneName);
        var neighboringScenes = sceneRoot?.GetComponent<NeighboringScenes>();

        if (neighboringScenes != null)
        {
            room.ConnectedRooms.AddRange(neighboringScenes.sceneNames);
        }
    }

    private void OnSceneLoaded(string sceneName)
    {
        //新しいシーンがロードされた時にマップ情報を更新
        var roomData = CreateRoomData(sceneName);
        if (roomData != null)
        {
            //接続されたルームの情報を更新
            UpdateRoomConnections(roomData, sceneName);



            //roomData.UpdateRoomConnections(roomData, sceneName);
            //UpdateRoomConnections(roomData, sceneName);

            _rooms[sceneName] = roomData;
        }
    }

    public MinimapRoom CreateRoomData(string sceneName)
    {
        GameObject sceneRoot = GameObject.Find(sceneName);
        if (sceneRoot == null) return null;

        var bounds = CalculateSceneBounds(sceneRoot);

        return new MinimapRoom()
        {
            RoomId = sceneName,
            RoomBounds = bounds,
            IsVisited = false,
            IsActive = true,
            ConnectedRooms = new List<string>()
        };

        Bounds CalculateSceneBounds(GameObject sceneRoot)
        {
            Bounds bounds = new Bounds(sceneRoot.transform.position, Vector3.zero);

            var renderers = sceneRoot.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            //マップ表示用にY軸を制限
            bounds.size = new Vector3(bounds.size.x, 0.1f, bounds.size.z);

            return bounds;
        }



    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;

        foreach(var room in _rooms.Values)
        {
            //訪れたことのあるルームは白色で描画
            if(room.IsVisited)
               Gizmos.color = _visitedRoomColor;

            //現在のルームは灰色で描画
            else if(room.IsActive)
               Gizmos.color = _activeRoomColor;

            //ルームの境界を描画
            Gizmos.DrawWireCube(room.RoomBounds.center,room.RoomBounds.size);

            //接続されたルームとの関係を線で描画
            if(room.ConnectedRooms != null)
            {
                foreach(string ConnectedRoomId in room.ConnectedRooms)
                {
                    if(_rooms.TryGetValue(ConnectedRoomId,out MinimapRoom ConnectedRoom))
                    {
                        Gizmos.DrawLine(
                            room.RoomBounds.center,
                            ConnectedRoom.RoomBounds.center
                        );
                    }
                }
            }
        }

    }
#endif
}
