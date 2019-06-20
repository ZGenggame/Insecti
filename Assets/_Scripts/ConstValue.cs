using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ConstValue {

    public Dictionary<int, string> levelStrings = new Dictionary<int, string>();
    public Dictionary<int, string> levelEngLishStrings = new Dictionary<int, string>();
    private static ConstValue _Instance;

    public class CharacterIndex {

        public int BigNumber = -1;
        public int SmallNumber = -1;

        public CharacterIndex(int bigNumber,int smallNumber) {
            this.BigNumber = bigNumber;
            this.SmallNumber = smallNumber;
        }
    }

    public class Character {
        public CharacterIndex WordIndex;
        public string Word;

        public Character(CharacterIndex wordIndex, string word)
        {
            this.WordIndex = wordIndex;
            this.Word = word;
        }
    
    }

    


    public static ConstValue GetInstance() {

        if (_Instance == null)
        {
            _Instance = new ConstValue();
        }
        return _Instance;
    }
    


    private ConstValue()
    {

#region 中文解释
        levelStrings.Add(0, "六十甲子");
        levelStrings.Add(1, "六合之内");
        levelStrings.Add(2, "二十四史");
        levelStrings.Add(3, "小子");
        levelStrings.Add(4, "小心");
        levelStrings.Add(5, "小女子");
        levelStrings.Add(6, "小王子");
        levelStrings.Add(7, "小日子");
        levelStrings.Add(8, "分大小");
        levelStrings.Add(9, "才子");
        levelStrings.Add(10, "刀子");
        levelStrings.Add(11, "日子");
        levelStrings.Add(12, "公子");
        levelStrings.Add(13, "火力");
        levelStrings.Add(14, "一下子");
        levelStrings.Add(15, "一二三四");
        levelStrings.Add(16, "一五一十");
        levelStrings.Add(17, "一目十行");
        levelStrings.Add(18, "二百五");
        levelStrings.Add(19, "三七");
        levelStrings.Add(20, "三公");
        levelStrings.Add(21, "三人行");
        levelStrings.Add(22, "三合土");
        levelStrings.Add(23, "三分天下");
        levelStrings.Add(24, "三三两两");
        levelStrings.Add(25, "四川");
        levelStrings.Add(26, "四方");
        levelStrings.Add(27, "五行");
        levelStrings.Add(28, "六甲");
        levelStrings.Add(29, "七夕");
        levelStrings.Add(30, "田七");
        levelStrings.Add(31, "七上八下");
        levelStrings.Add(32, "七十二行");
        levelStrings.Add(33, "小九九");
        levelStrings.Add(34, "合十");
        levelStrings.Add(35, "百合");
        levelStrings.Add(36, "上山");
        levelStrings.Add(37, "上升");
        levelStrings.Add(38, "上午");
        levelStrings.Add(39, "不上不下");
        levelStrings.Add(40, "下午");
        levelStrings.Add(41, "水下");
        levelStrings.Add(42, "在下");
        levelStrings.Add(43, "下工夫");
        levelStrings.Add(44, "两下子");
        levelStrings.Add(45, "天下太平");
        levelStrings.Add(46, "人才");
        levelStrings.Add(47, "大人");
        levelStrings.Add(48, "小人");
        levelStrings.Add(49, "工人");
        levelStrings.Add(50, "文人");
        levelStrings.Add(51, "内人");
        levelStrings.Add(52, "主人");
        levelStrings.Add(53, "人文");
        levelStrings.Add(54, "心上人");
        levelStrings.Add(55, "主人公");
        levelStrings.Add(56, "人五人六");
        levelStrings.Add(57, "分工");
        levelStrings.Add(58, "工夫");
        levelStrings.Add(59, "女工");
        levelStrings.Add(60, "木工");
        levelStrings.Add(61, "小刀");
        levelStrings.Add(62, "心力");
        levelStrings.Add(63, "主力");
        levelStrings.Add(64, "全力");
        levelStrings.Add(65, "大力士");
        levelStrings.Add(66, "分子力");
        levelStrings.Add(67, "中士");
        levelStrings.Add(68, "中午");
        levelStrings.Add(69, "中心");
        levelStrings.Add(70, "中文");
        levelStrings.Add(71, "人中");
        levelStrings.Add(72, "水火之中");
        levelStrings.Add(73, "大刀");
        levelStrings.Add(74, "大才");
        levelStrings.Add(75, "大山");
        levelStrings.Add(76, "大方");
        levelStrings.Add(77, "大米");
        levelStrings.Add(78, "大丈夫");
        levelStrings.Add(79, "大白天");
        levelStrings.Add(80, "刀山");
        levelStrings.Add(81, "火山");
        levelStrings.Add(82, "山川");
        levelStrings.Add(83, "山水");
        levelStrings.Add(84, "山羊");
        levelStrings.Add(85, "本土");
        levelStrings.Add(86, "天才");
        levelStrings.Add(87, "文才");
        levelStrings.Add(88, "全才");
        levelStrings.Add(89, "正门");
        levelStrings.Add(90, "正文");
        levelStrings.Add(91, "公正");
        levelStrings.Add(92, "方方正正");
        levelStrings.Add(93, "反光");
        levelStrings.Add(94, "反水");
        levelStrings.Add(95, "平反");
        levelStrings.Add(96, "反目");
        levelStrings.Add(97, "之内");
        levelStrings.Add(98, "门户之见");
#endregion 

#region 英文解释
        levelEngLishStrings.Add(0, "A cycle of sixty years");
        levelEngLishStrings.Add(1, "All within the universe");
        levelEngLishStrings.Add(2, "The Twenty-Four Histories");
        levelEngLishStrings.Add(3, "Boy");
        levelEngLishStrings.Add(4, "Take care");
        levelEngLishStrings.Add(5, "Little girl");
        levelEngLishStrings.Add(6, "Princeling");
        levelEngLishStrings.Add(7, "Easy life of a small family");
        levelEngLishStrings.Add(8, "Divided size");
        levelEngLishStrings.Add(9, "Gifted scholar");
        levelEngLishStrings.Add(10, "Knife");
        levelEngLishStrings.Add(11, "Days");
        levelEngLishStrings.Add(12, "Son of a feudal prince or high official");
        levelEngLishStrings.Add(13, "Firepower");
        levelEngLishStrings.Add(14, "Right off");
        levelEngLishStrings.Add(15, "One two three four");
        levelEngLishStrings.Add(16, "Relate in detail");
        levelEngLishStrings.Add(17, "Read ten lines of writing with one single glance");
        levelEngLishStrings.Add(18, "A stupid person");
        levelEngLishStrings.Add(19, "Pseudo-ginseng ");
        levelEngLishStrings.Add(20, "Three counsellors of state");
        levelEngLishStrings.Add(21, "You can learn from everyone");
        levelEngLishStrings.Add(22, "Trinity mixture fill");
        levelEngLishStrings.Add(23, "Three points in the world");
        levelEngLishStrings.Add(24, "In small groups");
        levelEngLishStrings.Add(25, "Sichuan Province");
        levelEngLishStrings.Add(26, "The four directions");
        levelEngLishStrings.Add(27, "The five elements");
        levelEngLishStrings.Add(28, "Pregnancy");
        levelEngLishStrings.Add(29, "Magpie Festival");
        levelEngLishStrings.Add(30, "Radix notoginseng ");
        levelEngLishStrings.Add(31, "In a mental flurry of indecision");
        levelEngLishStrings.Add(32, "All sorts of occupations");
        levelEngLishStrings.Add(33, "Scheme");
        levelEngLishStrings.Add(34, "Put the palms together");
        levelEngLishStrings.Add(35, "Lilium brownii");
        levelEngLishStrings.Add(36, "Go up the mountain");
        levelEngLishStrings.Add(37, "Ascend");
        levelEngLishStrings.Add(38, "Forenoon");
        levelEngLishStrings.Add(39, "Be in a dilemma");
        levelEngLishStrings.Add(40, "Afternoon");
        levelEngLishStrings.Add(41, "Underwater");
        levelEngLishStrings.Add(42, "Infra");
        levelEngLishStrings.Add(43, "Put in time and energy");
        levelEngLishStrings.Add(44, "A few tricks of the trade");
        levelEngLishStrings.Add(45, "The whole world is at peace");
        levelEngLishStrings.Add(46, "A person of ability");
        levelEngLishStrings.Add(47, "Adult");
        levelEngLishStrings.Add(48, "Villain");
        levelEngLishStrings.Add(49, "Worker");
        levelEngLishStrings.Add(50, "Scholar");
        levelEngLishStrings.Add(51, "Wife");
        levelEngLishStrings.Add(52, "Proprietor Amphitryon");
        levelEngLishStrings.Add(53, "Humanity");
        levelEngLishStrings.Add(54, "Sweetheart");
        levelEngLishStrings.Add(55, "Leading character in a novel");
        levelEngLishStrings.Add(56, "Be affected");
        levelEngLishStrings.Add(57, "Divide the work");
        levelEngLishStrings.Add(58, "Time");
        levelEngLishStrings.Add(59, "Woman worker");
        levelEngLishStrings.Add(60, "Carpentry");
        levelEngLishStrings.Add(61, "Knife");
        levelEngLishStrings.Add(62, "Mental and physical efforts");
        levelEngLishStrings.Add(63, "Main force");
        levelEngLishStrings.Add(64, "With all one's strength");
        levelEngLishStrings.Add(65, "Man of unusual strength");
        levelEngLishStrings.Add(66, "Molecular force");
        levelEngLishStrings.Add(67, "Sergeant");
        levelEngLishStrings.Add(68, "Noon");
        levelEngLishStrings.Add(69, "Centrality");
        levelEngLishStrings.Add(70, "The Chinese language");
        levelEngLishStrings.Add(71, "Philtrum");
        levelEngLishStrings.Add(72, "From the mire");
        levelEngLishStrings.Add(73, "Broadsword");
        levelEngLishStrings.Add(74, "Great talent");
        levelEngLishStrings.Add(75, "Huge mountain");
        levelEngLishStrings.Add(76, "Generous");
        levelEngLishStrings.Add(77, "Rice");
        levelEngLishStrings.Add(78, "True man");
        levelEngLishStrings.Add(79, "Daytime");
        levelEngLishStrings.Add(80, "Mountain of swords");
        levelEngLishStrings.Add(81, "Volcano");
        levelEngLishStrings.Add(82, "Landscape");
        levelEngLishStrings.Add(83, "Mountains and rivers");
        levelEngLishStrings.Add(84, "Goat");
        levelEngLishStrings.Add(85, "Local");
        levelEngLishStrings.Add(86, "Genius");
        levelEngLishStrings.Add(87, "Literary talent");
        levelEngLishStrings.Add(88, "A versatile person");
        levelEngLishStrings.Add(89, "Main entrance");
        levelEngLishStrings.Add(90, "Straight matter");
        levelEngLishStrings.Add(91, "Impartial equity ");
        levelEngLishStrings.Add(92, "Square");
        levelEngLishStrings.Add(93, "Reflect light");
        levelEngLishStrings.Add(94, "Turn one's coat");
        levelEngLishStrings.Add(95, "Redress");
        levelEngLishStrings.Add(96, "Quarrel");
        levelEngLishStrings.Add(97, "Within");
        levelEngLishStrings.Add(98, "Parochial prejudice");
#endregion

        StringBuilder checkResult = new StringBuilder();

        //PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.PlayerCount, 0); //测试语句

        //首次进入游戏时所有字符串状态进行初始化所有词条的值，都统一为"0000"
        if (PlayerPrefs.GetInt(ConstValue.XmlDataKeyName.PlayerCount) == 0)
        {
            foreach (var item in levelStrings)
            {
                checkResult.Remove(0, checkResult.Length);
                for (int i = 0; i < item.Value.Length; i++)
                {
                    checkResult.Append("0"); //按照字符长度添加0的个数
                }
                PlayerPrefs.SetString(item.Value, checkResult.ToString());
            }

            //玩家已经开始过游戏
            PlayerPrefs.SetInt(ConstValue.XmlDataKeyName.PlayerCount, 1);
        }

        
    }


    public List<Character> GetCharacter() {
        List<CharacterIndex> words = new List<CharacterIndex>();
        while (words.Count < 4)
        {
            int big = Random.Range(0, levelStrings.Count); //第几个语句
            int small = Random.Range(0, levelStrings[big].Length); //语句中第几个字

            int index = -1;
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].BigNumber == big && words[i].SmallNumber == small) //同一个位置的文字
                {
                    break;
                }
                index = i;
            }
            if (index == words.Count - 1) //没有随机到重复的文字
            {
                words.Add(new CharacterIndex(big,small));
            }
        }

        List<Character> finalWords = new List<Character>();
        foreach (var item in words)
        {
            finalWords.Add(new Character(item, levelStrings[item.BigNumber][item.SmallNumber].ToString()));
        }
        return finalWords;
    }

    /// <summary>
    /// 初始化中心方块字符函数
    /// </summary>
    /// <param name="MaxCount">字数的上限,大于等于1个</param>
    /// <returns></returns>
    public List<Character> GetCharacter(int MaxCount)
    {
        List<CharacterIndex> words = new List<CharacterIndex>();
        while (words.Count < MaxCount)
        {
            int big = Random.Range(0, levelStrings.Count); //第几个语句
            int small = Random.Range(0, levelStrings[big].Length); //语句中第几个字

            int index = -1;
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].BigNumber == big && words[i].SmallNumber == small) //判断列表中是否同一个语句中同一个字 被重复随机到
                {
                    break;
                }
                index = i;
            }
            if (index == words.Count - 1) //没有随机到重复的文字
            {
                words.Add(new CharacterIndex(big, small));
            }
        }

        List<Character> finalWords = new List<Character>();
        foreach (var item in words)
        {
            finalWords.Add(new Character(item, levelStrings[item.BigNumber][item.SmallNumber].ToString()));
        }
        return finalWords;
    }


    /// <summary>
    /// 初始化中心方块字符函数,精确定位到具体那个成语那个字
    /// </summary>
    /// <param name="big">词条序号</param>
    /// <param name="smalls">词条中的汉字序号</param>
    /// <returns></returns>
    public List<Character> GetCharacter(int big,params int[] smalls)
    {
        List<CharacterIndex> words = new List<CharacterIndex>();
        for (int i = 0; i < smalls.Length; i++)
        {
            words.Add(new CharacterIndex(big, smalls[i]));
        }
        List<Character> finalWords = new List<Character>();
        foreach (var item in words)
        {
            finalWords.Add(new Character(item, levelStrings[item.BigNumber][item.SmallNumber].ToString()));
        }
        return finalWords;
    }
    /// <summary>
    /// 获取在给定词条内未被收集到的汉字序号
    /// </summary>
    /// <param name="big"></param>
    /// <returns></returns>
    public List<int> GetZeroWordInCharacter(int big)
    {
        List<int> result = new List<int>();
        char[] words = PlayerPrefs.GetString(ConstValue.GetInstance().levelStrings[big]).ToCharArray();

        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] == '0')
            {
                result.Add(i);
            }
        }
        return result;
    }


	public class TagName{
		public static string MAPPOINT = "MapPoint";
		public static string MOVEPOINT = "MovePoint";
        public static string PLAYER = "Player";
        public static string GAMECONTROLLER = "GameController";
	}

    public class LayerName{
        public static string MAP = "Map";
    }

    public class LevelString {

        public static string Level01 = "三千越甲";
        public static string Level02 = "否极泰来";
        public static string Level03 = "恭喜发财";
    }

    public class AnimatorParameters {

        public static string MoveCubeTriggerName_IsArrive = "IsArrive";
        public static string MoveCubeTriggerName_IsArriveY = "IsArriveY";
        public static string CenterCubeTriggerName_IsRightResult = "IsRightResult";
        public static string CenterCubeTriggerName_IsRestart = "IsRestart";
        public static string CenterCubeTriggerName_IsRotate = "IsRotate";
        public static string CollectedTriggerName_IsCollectedSuccess = "IsCollectedSuccess";
    }

    public class XmlDataKeyName {
        public static string PlayerCount = "PlayerCount";
        public static string PlayerBestScore = "PlayerBestScore";
        public static string PlayerInGameSuccessCount = "PlayerInGameSuccessCount";
        public static string UISoundState = "UISoundState";
        public static string UIMusicState = "UIMusicState";
    }
}
