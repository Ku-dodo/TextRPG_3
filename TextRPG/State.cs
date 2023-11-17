﻿namespace TextRPG
{
    class Scene
    {
        protected Dictionary<string, Scene> _next = new Dictionary<string, Scene>();

        protected Scene _prev;
        public Scene Prev { get { return _prev; } set { _prev = value; } }

        protected string _name = "";
        public string Name { get { return _name; } }

        protected string _comment = "";

        protected string[] _choices = { };
        public string[] Option { get { return _choices; } }

        protected string[] _display = { };
        public string[] Display { get { return _display; } }

        static MessageBox _board = new MessageBox(33, 28, 40, 5);
        static TextBlock _goldText = new TextBlock(63, 20, 15, 3);
        static TextBlock _potionText = new TextBlock(48, 20, 15, 3);
        static TextBlock _pagination = new TextBlock(3, 20, 25, 3);

        protected void ThrowMessage(string msg)
        {
            _board.SetText(msg);
            _board.Draw();
            Thread.Sleep(1000);
            GameManager.Instance.RefreshScene();
        }

        protected void ShowGold()
        {
            string playerHasPotion = GameManager.Instance.Player.hasPotion.ToString();
            _potionText.SetText($"포션 {playerHasPotion,5}EA");
            _potionText.Draw();

            string playerGold = GameManager.Instance.Player.Gold.ToString();
            _goldText.SetText($"{playerGold,10} G");
            _goldText.Draw();
        }

        protected void ShowPagination()
        {
            _pagination.SetText("◀ 이전 Q / E 다음 ▶");
            _pagination.Draw();
        }

        virtual public void HandleInput(GameManager game, ConsoleKey key) { }

        virtual public void Update(GameManager game) { }

        virtual public void DrawScene()
        {
            Screen.ShowMapName(_name, _comment);
            Screen.DrawBotScreen(Option, 3, true);
        }
    }


    class TitleScene : Scene
    {
        TextBlock tb;
        public TitleScene()
        {
            _name = "타이틀";
            _display = File.ReadAllLines(@"..\..\..\art\Title.txt");

            _next.Add("ClassSelect", new SelectClassScene(this));
        }

        override public void HandleInput(GameManager game, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Enter:
                    game.ChangeScene(_next["ClassSelect"]);
                    break;
            }
        }

        public override void DrawScene()
        {
            Screen.SetSize(80, 40);
            Screen.DrawScreen(Display, 5, 0);
        }
    }

    //직업 선택 씬
    class SelectClassScene : Scene
    {

        public SelectClassScene(Scene parent)
        {
            _name = "직업 선택";
            _comment = "원하는 직업을 선택하세요";
            _prev = parent;

            //선택 화면에 출력
            _choices = new string[] { "전사", "마법사", "이어하기"};

            //다음으로 넘어갈 TownScene을 딕셔너리에 추가
            _next.Add("Town", new TownScene(this));

            //위 화면에 출력할 아스키아트 로드
            SetDisplay();
        }

        override public void HandleInput(GameManager game, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;
                case ConsoleKey.D1://전사 클래스 선택 시, 플레이어 직업 반영
                    //game.Player.SetWarrior();
                    game.ChangeScene(_next["Town"]);
                    break;
                case ConsoleKey.D2://마법사 클래스 선택 시, 플레이어 직업 반영
                    //game.Player.SetWizard();
                    game.ChangeScene(_next["Town"]);
                    break;
                case ConsoleKey.D3://이어하기
                    game.ChangeScene(_next["Town"]);
                    break;

            }
        }

        //아스키 아트 로드
        void SetDisplay()
        {
            _display = File.ReadAllLines(@"..\..\..\art\Class_art.txt");
        }


        public override void DrawScene()
        {
            //씬 이름과 설명 출력
            base.DrawScene();

            //선택창을 위해 화면 분할
            Screen.Split();
            //화면 맨 위부터 화면 그리기
            Screen.DrawTopScreen(Display, 2);
        }

    }

    class TownScene : Scene
    {
        public TownScene(Scene parent)
        {
            _name = "마을";
            _comment = "거래, 휴식 등을 할 수 있습니다.";
            _prev = parent;

            _choices = new string[] { "상태보기", "인벤토리", "상점", "던전 입구", "신전" };

            _next.Add("Status", new StatusScene(this));
            _next.Add("Inventory", new InventoryScene(this));
            _next.Add("Shop", new ShopScene(this));
            _next.Add("DungunEntrance", new DungunEntranceScene(this));
            _next.Add("Temple", new TempleScene(this));

            SetDisplay();
        }

        override public void HandleInput(GameManager game, ConsoleKey key)
        {
            // 맵이 늘어날 때 마다 스위치 추가하는거 불편한데 . . . 
            // Dictionary 말고 List 로 할까 
            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                case ConsoleKey.D1:
                    game.ChangeScene(_next["Status"]);
                    break;

                case ConsoleKey.D2:
                    game.ChangeScene(_next["Inventory"]);
                    break;

                case ConsoleKey.D3:
                    game.ChangeScene(_next["Shop"]);
                    break;

                case ConsoleKey.D4:
                    game.ChangeScene(_next["DungunEntrance"]);
                    break;

                case ConsoleKey.D5:
                    game.ChangeScene(_next["Temple"]);
                    break;
            }
        }

        void SetDisplay()
        {
            _display = File.ReadAllLines(@"..\..\..\art\Town.txt");
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.Split();
            Screen.DrawTopScreen(Display, 2);
        }
    }

    class StatusScene : Scene
    {
        StatusWidget _statusWidget;
        public StatusScene(Scene parent)
        {
            _name = "능력치";
            _comment = "플레이어의 능력치를 확인합니다.";
            _prev = parent;
            _statusWidget = new StatusWidget(6, 2);
        }

        override public void HandleInput(GameManager game, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;
            }
        }

        public override void Update(GameManager game)
        {
            base.Update(game);
            Player player = game.Player;
            _statusWidget.SetPlayer(player);
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display);
            _statusWidget.Draw();
        }
    }

    class InventoryScene : Scene
    {
        GridBox _inventoryWidget;
        public GridBox InventoryWidget { get { return _inventoryWidget; } }
        public InventoryScene(Scene parent)
        {
            _name = "인벤토리";
            _comment = "플레이어의 인벤토리를 확인합니다.";
            _prev = parent;
            _choices = new string[] { "장착관리", "아이템 정렬" };
            _inventoryWidget = new GridBox();
            _next.Add("Equip", new EquipScene(this));
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            base.HandleInput(game, key);

            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                case ConsoleKey.D1:
                    game.ChangeScene(_next["Equip"]);
                    break;

                case ConsoleKey.D2:
                    game.Player.SortInventory();
                    game.RefreshScene();
                    break;
            }
        }

        public override void Update(GameManager game)
        {
            base.Update(game);
            Player player = game.Player;
            _inventoryWidget.Clear();
            for (int i = 0; i < player.Inventory.Count; ++i)
            {
                ItemSlot slot = new ItemSlot();
                slot.SetItem(i, player.Inventory[i]);
                _inventoryWidget.AddItem(slot);
            }
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display);

            _inventoryWidget.Draw();
            ShowGold();
        }
    }

    class EquipScene : Scene
    {
        GridBox _inventoryWidget;
        public EquipScene(Scene parent)
        {
            _name = "장착관리";
            _comment = "플레이어의 착용 장비를 관리합니다.";
            _prev = parent;
            _inventoryWidget = ((InventoryScene)parent).InventoryWidget;
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            if (key < ConsoleKey.D0 || key >= ConsoleKey.D1 + _choices.Length) return;

            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                default:
                    game.Player.EquipItem((int)key - 49);
                    game.RefreshScene();
                    break;
            }
        }

        public override void Update(GameManager game)
        {
            base.Update(game);
            Player player = game.Player;

            SetOption(player);

            SetDisplay(player);
        }

        void SetDisplay(Player player)
        {
            _inventoryWidget.Clear();
            for (int i = 0; i < player.Inventory.Count; ++i)
            {
                ItemSlot slot = new ItemSlot();
                slot.SetItem(i, player.Inventory[i]);
                _inventoryWidget.AddItem(slot);
            }
        }

        void SetOption(Player player)
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < player.Inventory.Count; ++i)
            {
                Item item = player.Inventory[i];
                string line = $"{item.Name}";
                if (item.bEquip) line = line.Insert(0, "[E]");
                lines.Add(line);
            }
            _choices = lines.ToArray();
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display);
            _inventoryWidget.Draw();
        }
    }

    class ShopScene : Scene
    {
        public Shop shop;
        ShopInformationDeskWidget _widget;

        public ShopScene(Scene parent)
        {
            _name = "상점";
            _comment = "아이템을 구입 또는 판매합니다.";
            _prev = parent;
            _widget = new ShopInformationDeskWidget(35, 3);

            _choices = new string[] { "구입", "판매" };
            shop = new Shop();


            _display = File.ReadAllLines(@"..\..\..\art\npc.txt");


            _next.Add("Buy", new BuyScene(this));
            _next.Add("Sell", new SellScene(this));
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            base.HandleInput(game, key);

            // Set Display
            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                case ConsoleKey.D1:
                    game.ChangeScene(_next["Buy"]);
                    break;

                case ConsoleKey.D2:
                    game.ChangeScene(_next["Sell"]);
                    break;
            }
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display);
            _widget.Draw();
            ShowGold();
        }
    }

    class BuyScene : Scene
    {
        Shop _shop;

        GridBox _ShopItems;

        int _pagination = 0;

        public BuyScene(Scene parent)
        {
            _name = "구입";
            _comment = "아이템을 구입합니다.";
            _prev = parent;
            _shop = ((ShopScene)Prev).shop;
            _ShopItems = new GridBox();
            SetDisplay(_shop.storePageBundle, _pagination);
            SetOption(_shop.storePageBundle, _pagination);
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            base.HandleInput(game, key);
            if ((key < ConsoleKey.D0 || key >= ConsoleKey.D1 + _choices.Length)&&(key != ConsoleKey.Q && key != ConsoleKey.E)) return;
  
            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                case ConsoleKey.Q:
                    ForewardItemBundle();
                    SetDisplay(_shop.storePageBundle, _pagination);
                    SetOption(_shop.storePageBundle, _pagination);
                    DrawScene();
                    break;

                case ConsoleKey.E:
                    BackwardItemBundle();
                    SetDisplay(_shop.storePageBundle, _pagination);
                    SetOption(_shop.storePageBundle, _pagination);
                    DrawScene();
                    break;

                default:
                    Item item = _shop.storePageBundle[_pagination][(int)key - 49];
                    try
                    {
                        game.Player.Buy(item);
                    }
                    catch (GoldShortageException e)
                    {
                        ThrowMessage("골드가 부족합니다.");
                        return;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        ThrowMessage("인벤토리가 가득찼습니다.");
                        return;
                    }
                    ThrowMessage($"{item.Name} 을 구입했습니다.");
                    break;
            }
        }

        void SetDisplay(List<List<Item>> itembundle, int _page)
        {
            _ShopItems.Clear();
            for (int i = 0; i < itembundle[_page].Count; ++i)
            {
                ItemSlot slot = new ItemSlot();
                slot.SetItem(i, _shop.storePageBundle[_pagination][i]);
                _ShopItems.AddItem(slot);
            }
        }

        void SetOption(List<List<Item>> itembundle, int _page)
        {
            List<string> lines = new List<string>();
            foreach (Item item in itembundle[_page])
            {
                string line = $"{item.Name}";
                lines.Add(line);
            }
            _choices = lines.ToArray();
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display);
            _ShopItems.Draw();
            ShowPagination();
            ShowGold();
        }
        
        // 아이템 번들 전환 시 화면을 다시 그림
        public void ReDrawItem()
        {
            base.DrawScene();
            ShowGold();
        }

        //_pagination 전환 코드
        public void ForewardItemBundle()
        {
            if (_pagination  == 0)
            {
                _pagination = _shop.storePageBundle.Count - 1;
            }
            else
            {
                _pagination -= 1;
            }
        }
        public void BackwardItemBundle()
        {
            if (_pagination ==  _shop.storePageBundle.Count - 1)
            {
                _pagination = 0;
            }
            else
            {
                _pagination +=  1;
            }
        }

    }

    class SellScene : Scene
    {
        GridBox _playerInventory;

        public SellScene(Scene parent)
        {
            _name = "판매";
            _comment = "아이템을 판매합니다.";
            _prev = parent;
            _playerInventory = new GridBox();
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            base.HandleInput(game, key);
            if (key < ConsoleKey.D0 || key >= ConsoleKey.D1 + _choices.Length) return;

            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                default:
                    string ItemName = game.Player.Inventory[(int)key - 49].Name;
                    try
                    {
                        game.Player.Sell((int)key - 49);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        return;
                    }
                    catch (EquippedItemException e)
                    {
                        ThrowMessage("장비를 해제 후 판매해주세요.");
                        return;
                    }
                    ThrowMessage($"{ItemName} 을 판매했습니다.");

                    break;
            }
        }

        public override void Update(GameManager game)
        {
            base.Update(game);
            Player player = game.Player;

            SetDisplay(player);
            SetOption(player);
        }

        void SetDisplay(Player player)
        {
            _playerInventory.Clear();
            for (int i = 0; i < player.Inventory.Count; ++i)
            {
                ItemSlot slot = new ItemSlot();
                slot.SetItem(i, player.Inventory[i]);
                _playerInventory.AddItem(slot);
            }
        }

        void SetOption(Player player)
        {
            List<string> lines = new List<string>();
            foreach (Item item in player.Inventory)
            {
                string line = $"{item.Name}";
                lines.Add(line);
            }
            _choices = lines.ToArray();
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display);
            _playerInventory.Draw();
            ShowGold();
        }
    }

    class TempleScene : Scene
    {
        public TempleScene(Scene parent)
        {
            _name = "신전";
            _comment = "체력을 회복할 수 있습니다.";
            _prev = parent;
            SetDisplay();
            _choices = new string[] { "회복하기 ( 300 G )" };
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                case ConsoleKey.D1:
                    if (game.Player.Rest() == false)
                    {
                        ThrowMessage("골드가 부족합니다.");
                    }
                    else
                    {
                        ThrowMessage("체력을 회복했습니다.");
                    }
                    break;
            }
        }

        void SetDisplay()
        {
            _display = File.ReadAllLines(@"..\..\..\art\Church.txt");
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display, 5);

            ShowGold();
        }
    }

    class DungunEntranceScene : Scene
    {
        string[] _recommendDef;

        GridBox _panel;
        public DungunEntranceScene(Scene parent)
        {
            _name = "던전 입구";
            _comment = "입장할 던전을 선택합니다.";
            _prev = parent;
            _panel = new GridBox();
            _panel.SetColomn(1);
            _panel.SetMargine(1, 1);
            _choices = new string[] { "쉬운 던전", "일반 던전", "어려운 던전" };
            _recommendDef = new string[] { "1 ~ 3", "5 ~ 10", "10 ~ 20" };


            for (int i = 0; i < 3; ++i)
            {
                TextBlock textBlock = new TextBlock(2 + 50 * i, 1 + 3 * i, 50, 3);
                string dungeon = Utility.MatchCharacterLength(_choices[i], 20);
                textBlock.SetText($"{i + 1}. {dungeon} | 권장 방어력 {_recommendDef[i]}");
                _panel.AddItem(textBlock);
            }
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            base.HandleInput(game, key);
            if (game.Player.Hp <= 0 && key != ConsoleKey.D0)
            {
                ThrowMessage("체력이 0 입니다.");
                return;
            }

            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;

                case ConsoleKey.D1:
                    game.ChangeScene(new EasyDungeonScene(this));
                    break;

                case ConsoleKey.D2:
                    game.ChangeScene(new NormalDungeonScene(this));
                    break;

                case ConsoleKey.D3:
                    game.ChangeScene(new HardDungeonScene(this));
                    break;
            }
        }

        public override void DrawScene()
        {
            base.DrawScene();
            Screen.DrawTopScreen(Display);
            _panel.Draw();
        }
    }

    class BaseDungeonScene : Scene
    {
        protected Dungeon _dungeon;

        int _yLine = 2;

        string[] msg;
        TextBlock _textBlock;
        ResultWidget _resultWidget;
        public BaseDungeonScene()
        {
            msg = new string[] { "공략 중 . . .", "공략 성공!! ㄴㅇㄱ", "공략 실패 ㅠㅁㅠ" };
            _textBlock = new TextBlock();
            _textBlock.SetSize(70, 3);
            _resultWidget = new ResultWidget(3, 1, 39, 20);
        }

        public override void HandleInput(GameManager game, ConsoleKey key)
        {
            if (_dungeon.state == Dungeon.EDungunState.Continue) return;

            switch (key)
            {
                case ConsoleKey.D0:
                    game.ChangeScene(_prev);
                    break;
            }
        }

        public override void Update(GameManager game)
        {
            _dungeon.Enter(game.Player);
        }

        public override void DrawScene()
        {
            Screen.Clear();
            Screen.Split();

            Screen.ShowMapName(_name);

            do
            {
                int result = (int)_dungeon.Progress();
                _textBlock.SetText(msg[result]);

                _textBlock.SetPosition(2, _yLine);
                _textBlock.Draw();
                _yLine += _textBlock.Height;
                Thread.Sleep(1000);

            } while (_dungeon.state == Dungeon.EDungunState.Continue);

            Screen.DrawTopScreen(Display);
            _resultWidget.SetResult(_dungeon.beforeRecord, _dungeon.afterRecord);
            _resultWidget.Draw();

            Screen.DrawBotScreen(Option);
        }
    }

    class EasyDungeonScene : BaseDungeonScene
    {
        public EasyDungeonScene(Scene parent)
        {
            _dungeon = new Dungeon("마을 근처", 0, 2, 2);
            _name = _dungeon.Name;
            _prev = parent;
        }
    }

    class NormalDungeonScene : BaseDungeonScene
    {
        public NormalDungeonScene(Scene parent)
        {
            _dungeon = new Dungeon("성벽 외곽", 1, 7, 7);
            _name = _dungeon.Name;
            _prev = parent;
        }
    }

    class HardDungeonScene : BaseDungeonScene
    {
        public HardDungeonScene(Scene parent)
        {
            _dungeon = new Dungeon("지하 미궁", 2, 20, 14);
            _name = _dungeon.Name;
            _prev = parent;
        }
    }
}
