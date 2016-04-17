using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject CellPrefab;
    public GameObject SpikesPrefab;
    public GameObject KeyPrefab;
    public GameObject SpawnPrefab;
    public GameObject BoxPrefab;
    public GameObject TeslaPrefab;
    public GameObject PlatePrefab;
    public GameObject DoorPrefab;
    public GameObject RabbitPrefab;
    public GameObject HunterPrefab;
    public GameObject TurretPrefab;
    public GameObject ArrowPrefab;
    public GameObject BossPrefab;
    public GameObject LockPrefab;
    public GameObject HintPrefab;
    
    public Cell[,] Cells;
    public List<LevelEntity> Items = new List<LevelEntity>();
    public List<LevelEntity> _itemsToRemove = new List<LevelEntity>();
    public List<LevelEntity> _itemsToAdd = new List<LevelEntity>();
    public List<Plate> Plates = new List<Plate>();

   [HideInInspector] public Vector2 MapSize;
   [HideInInspector] public Vector2 SpawnPoint;

    void Start()
    {
        GameController.GetInstance().GameStep += GameStep;
    }

    private void GameStep(int counter)
    {
        #region GameStart

        if (counter == 1)
        {
            Cells[(int) SpawnPoint.x + 2, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 2, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y + 2].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y - 2].SetState(CellState.Normal);
        }
        if (counter == 2)
        {
            Cells[(int) SpawnPoint.x + 1, (int) SpawnPoint.y + 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 1, (int) SpawnPoint.y - 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 1, (int) SpawnPoint.y + 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x + 1, (int) SpawnPoint.y - 1].SetState(CellState.Normal);
        }
        if (counter == 3)
        {
            Cells[(int) SpawnPoint.x + 1, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 1, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y + 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y - 1].SetState(CellState.Normal);
        }

        #endregion



        for (int i = 0; i < MapSize.y; i++) {
            for (int j = 0; j < MapSize.x; j++) {
                Cells[i, j].OnGameBeat(counter);
            }
        }

        foreach (var item in Items)
        {
            if (item.gameObject != null)
            {
                item.OnGameBeat(counter);
            }
        }

        foreach (var item in _itemsToRemove)
        {
            Items.Remove(item);
            Destroy(item.gameObject);
        }
        _itemsToRemove.Clear();


        foreach (var item in _itemsToAdd) {
            Items.Add(item);
        }
        _itemsToAdd.Clear();
    }

    public bool IsCanStep(Vector2 coords, MoveDirection dir)
    {
        if (GetCell(coords).State != CellState.Disabled && (GetCell(coords).LevelItem == null || GetCell(coords).LevelItem.CanStepOn))
        {
            if (GetCell(coords).CanMoveItem(dir))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void LoadMap()
    {
        var level = Resources.Load<TextAsset>("Level/Level");
        var lines = level.text.Split('\n');

        MapSize.y = lines[0].Length-1;
        MapSize.x = lines.Length;

        print(MapSize);
        // init:
        Cells = new Cell[(int)MapSize.y, (int)MapSize.x];
        for (int i = 0; i < MapSize.y; i++)
        {
            for (int j = 0; j < MapSize.x; j++)
            {
                Cells[i, j] = Instantiate(CellPrefab).GetComponent<Cell>();
                Cells[i, j].transform.parent = transform;
                Cells[i, j].Coords = new Vector2(i, j);
                Cells[i, j].SetPos();
                Cells[i, j].Map = this;
            }
        }

        
        for (int i = 0; i < MapSize.y; i++)
        {
            for (int j = 0; j < MapSize.x; j++)
            {
                var ch = lines[(int)MapSize.x - j - 1][i];
                switch (ch)
                {
                    case '#':
                        Cells[i, j].SetState(CellState.Disabled);
                        break;
                    case '.':
                        Cells[i, j].SetState(CellState.Normal);
                        break;
                    case 'K':
                        Cells[i, j].AddItem(KeyPrefab);
                        break;
                    case 'B':
                        AddItem(BoxPrefab, new Vector2(i, j));
                        break;
                    case 'T':
                        Cells[i, j].AddItem(TurretPrefab);
                        break;
                    case 'S':
                        Cells[i, j].AddItem(SpikesPrefab);
                        break;
                    case '>':
                        SpawnPoint = new Vector2(i, j);
                        Cells[i, j].AddItem(SpawnPrefab);
                        break;
                    case '~':
                        Cells[i, j].AddItem(TeslaPrefab);
                        break;
                    case 'P':
                        Cells[i, j].AddItem(PlatePrefab);
                        break;
                    case 'A':
                        Cells[i, j].AddItem(ArrowPrefab);
                        break;
                    case 'H':
                        AddItem(HunterPrefab, new Vector2(i, j));
                        break;
                    case 'R':
                        AddItem(RabbitPrefab, new Vector2(i, j));
                        break;
                    case 'U':
                        AddItem(BossPrefab, new Vector2(i, j));
                        break;
                    case 'D':
                        Cells[i, j].AddItem(DoorPrefab);
                        break;
                    case 'L':
                        Cells[i, j].AddItem(LockPrefab);
                        break;
                }
            }
        }
        // Adding hint blocks

        Cells[18, 23].AddItem(HintPrefab).GetComponent<Hint>().HintText = "KILL THE BOSS WHILE\nMUSIC IS BEATING";
        Cells[18, 19].AddItem(HintPrefab).GetComponent<Hint>().HintText = "YOU CAN ENDURE ONLY 5 HiTS";
        Cells[17, 20].AddItem(HintPrefab).GetComponent<Hint>().HintText = "PRESS R TO RESTART";
        Cells[19, 20].AddItem(HintPrefab).GetComponent<Hint>().HintText = "YOU CAN MOVE ONLY\nIN MUSIC RYTHM";

        Cells[18, 21].AddItem(HintPrefab).GetComponent<Hint>().HintText = "COLLECT 3 KEYS TO ENTER\nTHE BOSS ROOM";
        Cells[15, 20].AddItem(HintPrefab).GetComponent<Hint>().HintText = "YOU CAN KILL THE CHARGING\nENEMIES BY STEP ON THEM";
        Cells[18, 17].AddItem(HintPrefab).GetComponent<Hint>().HintText = "PURSUERS CAN BE KILLED ONLY\nBY ENVIRONMENT";
        Cells[21, 20].AddItem(HintPrefab).GetComponent<Hint>().HintText = "MOVE ALL BOXES ON PLATES\nTO OPEN THE DOOR";
        Cells[32, 29].AddItem(HintPrefab).GetComponent<Hint>().HintText = "PRESS R TO RESTART";
        Cells[18, 27].AddItem(HintPrefab).GetComponent<Hint>().HintText = "BOSS HAVE 8 HEALTH\nDamage him by step on him!";
        Cells[18, 28].AddItem(HintPrefab).GetComponent<Hint>().HintText = "GOOD LUCK!";

        Cells[16, 28].AddItem(HintPrefab).GetComponent<Hint>().HintText = "THERE IS THE CHEAT\nExplore the map!";
        Cells[5, 10].AddItem(HintPrefab).GetComponent<Hint>().HintText = "ITS NOT SO EASY...";
        Cells[30, 8].AddItem(HintPrefab).GetComponent<Hint>().HintText = "PRESS O TO INSTANTLY\nOPEN BOSS ROOM";
        Cells[1, 20].AddItem(HintPrefab).GetComponent<Hint>().HintText = "You have limited time!";
        Cells[27, 23].AddItem(HintPrefab).GetComponent<Hint>().HintText = "Please leave you feedback\n on LD game page!";


        
    }

    public void MoveFromEvent(Vector2 coords, MoveDirection dir) {
        Cells[(int)coords.x, (int)coords.y].OnMoveFrom();
    }

    public void MoveToEvent(Vector2 coords, MoveDirection dir)
    {
        var needRevert = false;
        Cells[(int)coords.x, (int)coords.y].OnMoveOn(dir);

        foreach (var item in Items)
        {
            if (item.Coords.x == coords.x && item.Coords.y == coords.y)
            {
                item.OnPlayerEnter(dir);
                if (!item.CanStepOn && (item is Rabbit || item is Boss))
                {
                    needRevert = true;
                }
            }
        }

        if (needRevert)
        {
//            GameController.GetInstance().Player.MoveInvert(dir);
        }
    }

    public void StayEvent(Vector2 coords) {
        Cells[(int)coords.x, (int)coords.y].OnStay();
    }

    public Cell GetCell(Vector2 v)
    {
        if (v.x >= MapSize.x || v.y >= MapSize.y || v.x < 0 || v.y < 0)
        {
            return Cells[0, (int)(MapSize.y - 1)];
        }
        return Cells[(int) v.x, (int) v.y];
    }


    public LevelEntity AddItem(GameObject prefab, Vector2 coords)
    {
        LevelEntity item = Instantiate(prefab).GetComponent<LevelEntity>();
        item.transform.parent = transform;
        item.Coords = coords;
        item.SetPosInstantly();
        Items.Add(item);
        return item;
    }

    public LevelEntity AddItemInGame(GameObject prefab, Vector2 coords) {
        LevelEntity item = Instantiate(prefab).GetComponent<LevelEntity>();
        item.transform.parent = transform;
        item.Coords = coords;
        item.SetPosInstantly();
        _itemsToAdd.Add(item);
        return item;
    }

    public void DestroyItem(LevelEntity item)
    {
        _itemsToRemove.Add(item);
    }
}
