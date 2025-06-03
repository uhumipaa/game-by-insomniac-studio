using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FarmManager : MonoBehaviour
{
    public static FarmManager instance;
    public TileManager tileManager;
    private Dictionary<Vector3Int, FarmTileData> farmTiles = new Dictionary<Vector3Int, FarmTileData>();
    private GameManager gm;
    public GameObject farmTileProgressBarPrefab;
    private Coroutine growCheckRoutine;
    public GameObject waterIconPrefab;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 已經有了就刪除自己
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 保留這份跨場景
        gm = FindFirstObjectByType<GameManager>();
        tileManager = gm.tileManager;

        if (waterIconPrefab == null)
        {
            waterIconPrefab = Resources.Load<GameObject>("waterIcon");
            Debug.Log("✅ 自動載入 waterIcon prefab");
        }
    }

    private void Start()
    {
        StartCoroutine(DelayedLoad());
        //StartCoroutine(CheckGrowthPeriodically());
    }



    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(CheckGrowthPeriodically());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (growCheckRoutine != null)
        {
            StopCoroutine(growCheckRoutine);
            growCheckRoutine = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "farm")
        {
            Debug.Log("🌾 進入農場場景，自動載入農地並檢查作物成長");
            StartCoroutine(DelayedLoadAndGrow());
        }
    }

    private IEnumerator DelayedLoadAndGrow()
    {
        yield return new WaitForSeconds(0.5f);
        LoadFarmTilesFromFile();
        AutoGrowAllTiles(); // 載入完資料後立即檢查一次
    }


    private IEnumerator CheckGrowthPeriodically()
    {
        while (true)
        {
            AutoGrowAllTiles(); // 檢查是否成長
            yield return new WaitForSeconds(5f); // 每 5 秒跑一次
        }
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("🌱 延遲一幀後載入農地資料");
        LoadFarmTilesFromFile();
    }

    public void AddFarmTile(Vector3Int pos, CropData cropData)
    {
        if (!farmTiles.ContainsKey(pos))
        {
            var tileData = new FarmTileData(pos, 0, cropData, false);
            farmTiles.Add(pos, tileData); // 初始狀態是0(剛播種)
            UpdateTileVisual(farmTiles[pos]);

            // 生成進度條
            //如果有進度條，先清除
            if (tileData.progressUI != null)
            {
                Destroy(tileData.progressUI.gameObject);
                tileData.progressUI = null;
            }
            // 取得格子中心點
            Vector3 tileCenter = tileManager.cropTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);

            // 偏移一點 Y 軸讓進度條浮在作物上方
            Vector3 barPosition = tileCenter + new Vector3(0, 0.6f, 0);

            // 生成進度條並對齊旋轉
            GameObject bar = Instantiate(farmTileProgressBarPrefab, barPosition, Quaternion.identity);
            Debug.Log($"[DEBUG] 進度條生成位置: {barPosition}");
            bar.transform.rotation = Quaternion.identity; // 保證面向攝影機


            var progressScript = bar.GetComponentInChildren<GrowthProgressBar>();
            if (progressScript != null)
            {
                progressScript.Setup(tileData);
                tileData.progressUI = progressScript;
            }

            // 生成水滴圖示（初始為未澆水）
            if (waterIconPrefab != null)
            {
                GameObject waterIcon = Instantiate(waterIconPrefab, barPosition + new Vector3(0, 0.3f, 0), Quaternion.identity);
                waterIcon.SetActive(true); // 初始顯示水滴
                tileData.waterIcon = waterIcon;
            }
        }
    }

    public bool TryGrowTile(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
        {
            var tileData = farmTiles[pos];
            var crop = tileData.cropData;
            int floor = TowerManager.Instance.finishfloorthistime;

            //要澆水才會繼續生長
            if (!tileData.isWatered)
            {
                Debug.Log($"❌ 作物尚未澆水，無法生長 ({tileData.cropData.cropName})");
                return false;
            }

            switch (tileData.state)
            {
                case 0: // 播種 -> 發芽
                    if (floor >= crop.sproutFloor)
                    {
                        tileData.state = 1;
                        UpdateTileVisual(tileData);
                        return true;
                    }
                    break;
                case 1: // 發芽 → 成長
                    if (floor >= crop.growFloor)
                    {
                        tileData.state = 2;
                        UpdateTileVisual(tileData);
                        return true;
                    }
                    break;
                case 2: // 成長 → 成熟
                    if (floor >= crop.matureFloor)
                    {
                        tileData.state = 3;
                        UpdateTileVisual(tileData);
                        return true;
                    }
                    break;
                case 3: //成熟 -> 收成
                    if (floor >= crop.harvestFloor)
                    {
                        TryHarvestTile(pos);
                        return true;
                    }
                    break;
                default: //state == 0
                    tileManager.ClearTile(tileData.position); // 播種時清除圖像
                    break;
            }

        }
        return false;
    }

    private void UpdateTileVisual(FarmTileData tileData)
    {
        switch (tileData.state)
        {
            case 1:
                tileManager.SetCropTile(tileData.position, tileData.cropData.sproutTile); // 發芽Tile
                break;
            case 2:
                tileManager.SetCropTile(tileData.position, tileData.cropData.matureTile); // 成長Tile
                break;
            case 3:
                tileManager.SetCropTile(tileData.position, tileData.cropData.harvestTile); // 準備收成Tile
                break;
        }
    }

    public bool HasFarmTile(Vector3Int pos)
    {
        return farmTiles.ContainsKey(pos);
    }

    //田地收成
    public bool TryHarvestTile(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
        {
            var tileData = farmTiles[pos];

            //生成收成物到地圖
            Vector3 worldPos = tileManager.cropTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f); // 讓物品出現在 tile 中央
            GameObject harvestItem = GameObject.Instantiate(tileData.cropData.harvestPrefab, worldPos, Quaternion.identity);

            //綁定collectable資料
            Collectable collectable = harvestItem.GetComponent<Collectable>();
            if (collectable != null)
            {
                collectable.SetItemData(tileData.cropData.harvestItemData, 1);
            }
            else
            {
                Debug.LogWarning("❌ Harvest prefab 上找不到 Collectable 組件！");
            }

            //紀錄生成農作物
            DropManager.instance.SaveDroppedItems();

            //移除進度條
            if (tileData.progressUI != null)
            {
                Destroy(tileData.progressUI.gameObject);
                tileData.progressUI = null;
            }

            //移除水滴圖
            if (tileData.waterIcon != null)
            {
                Destroy(tileData.waterIcon.gameObject);
                tileData.waterIcon = null;
            }

            //清除 tile
            tileManager.ClearTile(pos);

            //移除田地資料
            farmTiles.Remove(pos);
            // Debug.Log($"收成 {tileData.cropData.cropName} 作物完成！");

            SaveFarmTilesToFile();
            return true;

        }

        //Debug.Log("這塊田還不能收成！");
        return false;
    }

    //取得田地資料
    public FarmTileData GetFarmTileData(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
            return farmTiles[pos];
        else
            return null;
    }

    public void AutoGrowAllTiles()
    {
        //Debug.Log("🌿 自動檢查作物成長 at " + Time.time);
        List<Vector3Int> keys = new List<Vector3Int>(farmTiles.Keys);

        foreach (var pos in keys)
        {
            TryGrowTile(pos);
        }

        //更新進度條
        foreach (var tile in farmTiles.Values)
        {
            tile.progressUI?.UpdateProgress();
        }

    }

    //存取田地資料
    public FarmSaveData GetFarmSaveData()
    {
        FarmSaveData saveData = new FarmSaveData();

        foreach (var kvp in farmTiles)
        {
            var tile = kvp.Value;

            saveData.allTiles.Add(new FarmTileSaveData
            {
                x = tile.position.x,
                y = tile.position.y,
                z = tile.position.z,
                state = tile.state,
                cropName = tile.cropData.cropName,
                isWatered = tile.isWatered
            });
        }

        return saveData;
    }

    //寫入json檔
    public void SaveFarmTilesToFile()
    {
        FarmSaveData saveData = GetFarmSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/farm_save.json";

        System.IO.File.WriteAllText(path, json);
        Debug.Log("✅ 農地狀態已儲存到 " + path);
        Debug.Log(Application.persistentDataPath);
    }

    public void LoadFarmTilesFromFile()
    {
        Debug.Log("讀取農地資料中...");
        string path = Application.persistentDataPath + "/farm_save.json";

        if (!System.IO.File.Exists(path))
        {
            Debug.LogWarning("⚠ 沒有農地存檔檔案！");
            return;
        }

        string json = System.IO.File.ReadAllText(path);
        FarmSaveData saveData = JsonUtility.FromJson<FarmSaveData>(json);
        LoadFarmData(saveData);
    }

    public void LoadFarmData(FarmSaveData data)
    {
        if (SceneManager.GetActiveScene().name != "farm") return;
        //先清除場上所有殘留的水滴圖與進度條（防止重複產生）
        foreach (var tile in farmTiles.Values)
        {
            if (tile.progressUI != null)
            {
                Destroy(tile.progressUI.gameObject);
                tile.progressUI = null;
            }

            if (tile.waterIcon != null)
            {
                Destroy(tile.waterIcon.gameObject);
                tile.waterIcon = null;
            }
        }

        farmTiles.Clear();


        foreach (var tile in data.allTiles)
        {
            Vector3Int pos = new Vector3Int(tile.x, tile.y, tile.z);
            CropData crop = PlantManager.instance.allCrops.Find(c => c.cropName == tile.cropName);

            if (crop != null)
            {
                var farmTile = new FarmTileData(pos, tile.state, crop, tile.isWatered);
                farmTiles[pos] = farmTile;
                UpdateTileVisual(farmTile);

                /*重新生成進度條*/
                // 若已有舊的 progress bar，先刪除
                if (farmTile.progressUI != null)
                {
                    Destroy(farmTile.progressUI);
                    farmTile.progressUI = null;
                }

                // 取得格子中心點
                Vector3 tileCenter = tileManager.cropTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0);

                // 偏移一點 Y 軸讓進度條浮在作物上方
                Vector3 barPosition = tileCenter + new Vector3(0, 0.6f, 0);

                // 生成進度條並對齊旋轉
                GameObject bar = Instantiate(farmTileProgressBarPrefab, barPosition, Quaternion.identity);
                Debug.Log($"[DEBUG] 進度條生成位置: {barPosition}");
                bar.transform.rotation = Quaternion.identity; // 保證面向攝影機

                var progressScript = bar.GetComponentInChildren<GrowthProgressBar>();
                if (progressScript != null)
                {
                    progressScript.Setup(farmTile);
                    farmTile.progressUI = progressScript;
                }

                //還沒澆水 重新生成水滴圖示
                if (waterIconPrefab != null)
                {
                    GameObject waterIcon = Instantiate(waterIconPrefab, barPosition + new Vector3(0, 0.3f, 0), Quaternion.identity);
                    waterIcon.SetActive(!farmTile.isWatered); // true 顯示、false 隱藏
                    farmTile.waterIcon = waterIcon;
                }
            }
            else
            {
                Debug.LogWarning($"⚠ 找不到作物：{tile.cropName}");
            }
        }
    }

    public void WaterTile(Vector3Int pos)
    {
        if (farmTiles.ContainsKey(pos))
        {
            var tileData = farmTiles[pos];
            tileData.isWatered = true;
            if (tileData.waterIcon != null)
            {
                tileData.waterIcon.SetActive(false); // 澆水後隱藏圖示
            }
            Debug.Log($"💧 {tileData.cropData.cropName} 已澆水！");
        }
    }
}