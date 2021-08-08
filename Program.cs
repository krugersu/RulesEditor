using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RulesEditor
{
    class Program
    {
        //  private void ConvertRules(string OldRules, string NewRules);
        static string contentsRulesProperty;
        static JObject CurRules;
        static JObject CurRulesTab;
        static object TypeData;
        private static string contentsRulesTabPart;
        static JToken Rls;
        static string FileData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value>Имя объекта метаданных</value>
        static string NameRules { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <value><strong>Файл с правилами шапки</strong></value>
        static string FileRulesProperty { get; set; }

        static string mAction { get; set; }
        static string mSourceName { get; set; }
        static string mRecName { get; set; }
        static string mTypeName { get; set; }
        static string mOrder { get; set; }
        static string mBef { get; set; }


        static void Main(string[] args)
        {

            // NameRules = args[2];
            string PathToFolder = "D:\\home\\Project\\Kruger\\Rules";
            string[] allfiles = Directory.GetFiles(PathToFolder, "S*.json");
            foreach (string filename in allfiles)
            {
                Console.WriteLine(filename);
                NameRules = Path.GetFileNameWithoutExtension(filename).TrimStart('S');
                Console.WriteLine(NameRules.TrimStart('S'));
                string NameMeta = Path.GetFileName(filename);
                ConvertRules(filename, "New\\" + NameMeta);
            }

            allfiles = Directory.GetFiles(PathToFolder, "F*.json");
            NameRules = "ТабличныеЧасти";
            foreach (string filename in allfiles)
            {
                Console.WriteLine(filename);
                Console.WriteLine(NameRules.TrimStart('F'));
                string NameMeta = Path.GetFileName(filename);
                ConvertRulesTab(filename, "New\\" + NameMeta);
            }



            // ConvertRulesTab(args[0], args[1]);
            mRules cRul = new mRules();

            // cRul.MultiValue = false;
            // cRul.NameRules = "Тестовое правило";

            // cRul.listRules.Add(new RulesProperty()
            // {
            //     NameProperty = "Тестовый реквизит",
            //     PropertyNameSource = "Тестовый реквизит",
            //     PropertyNameDestination = "Тестовый22 реквизит",
            //     TypeStringDestination = "Строка",
            //     Action = "Добавить",
            //     Order = 10,
            //     Before = ""
            // });

            // var switchvalue = int.Parse(args[0]);
            // if (switchvalue == 0)
            // {
            //     {
            //         //         string jsonString = JsonSerializer.Serialize(cRul);
            //         string jsonString = JsonConvert.SerializeObject(cRul);
            //         File.WriteAllText("user.json", jsonString);
            //         Console.WriteLine("Data has been saved to file");
            //         Console.WriteLine(jsonString);
            //     }
            // }
            // else if (switchvalue == 1)
            // {
            //     string jsonString = File.ReadAllText("SОрганизацииN.json");
            //     mRules mRules1 = JsonConvert.DeserializeObject<mRules>(jsonString);
            //     List<RulesProperty> tt = mRules1.listRules;
            //     Console.WriteLine(mRules1.NameRules);
            //     Console.WriteLine(tt);

            //     foreach (var p in tt)
            //     {
            //         //  Console.WriteLine(p.NameProperty, p.Action);
            //         Console.WriteLine((p.NameProperty));
            //         Console.WriteLine((p.Action));
            //     }

            // }
            // else if (switchvalue == 2)
            // {
            //     // mRules cRulN = new mRules();
            //     string jsonString = File.ReadAllText("SПоступлениеТоваровN.json");
            //     mRules mRules1 = JsonConvert.DeserializeObject<mRules>(jsonString);
            //     List<RulesProperty> ListRuls = mRules1.listRules;

            // }
        }

        private int SaveFile(mRules obj)
        {
            return 0;
        }

        static void ConvertRules(string OldRules, string NewRules)
        {


            mRules cRul = new mRules();
            cRul.MultiValue = false;
            cRul.NameRules = NameRules;

            Console.WriteLine((OldRules));
            if (File.Exists(OldRules))
            {
                contentsRulesProperty = File.ReadAllText(OldRules);
                // Console.WriteLine((contentsRulesProperty));
                // Разбор файла с правилами шапки
                CurRules = JObject.Parse(contentsRulesProperty);

                Rls = CurRules.SelectToken("$.#value..#value[?(@.name.#value == " + "'" + NameRules + "'" + " )]");

                foreach (var curR in Rls["Value"]["#value"])
                {

                    mAction = curR.SelectToken("$.Value.#value[?(@.name.#value == 'Действие' )].Value.#value").ToString();
                    mSourceName = curR.SelectToken("$.Value.#value[?(@.name.#value == 'ИмяСвойстваИсточник' )].Value.#value").ToString();
                    mRecName = curR.SelectToken("$.Value.#value[?(@.name.#value == 'ИмяСвойстваПриемник' )].Value.#value").ToString();
                    mTypeName = curR.SelectToken("$.Value.#value[?(@.name.#value == 'ТипСтрокойПриемник' )].Value.#value").ToString();
                    mOrder = curR.SelectToken("$.Value.#value[?(@.name.#value == 'Порядок' )].Value.#value").ToString();
                    mBef = curR.SelectToken("$.Value.#value[?(@.name.#value == 'Перед' )].Value.#value").ToString();

                    cRul.listRules.Add(new RulesProperty()
                    {
                        NameProperty = NameRules,
                        PropertyNameSource = mSourceName,
                        PropertyNameDestination = mRecName,
                        TypeStringDestination = mTypeName,
                        Action = mAction,
                        Order = Int32.Parse(mOrder),
                        Before = mBef
                    });

                }

                string jsonString = JsonConvert.SerializeObject(cRul);
                File.WriteAllText(NewRules, jsonString);
                Console.WriteLine("Data has been saved to file - " + NewRules);
                Console.WriteLine(jsonString);



            }


        }
        static void ConvertRulesTab(string OldRules, string NewRules)
        {

            mRulesTabPart cRul = new mRulesTabPart();
            cRul.MultiValue = false;
            cRul.NameRules = NameRules;
            contentsRulesTabPart = File.ReadAllText(OldRules);
            // Разбор файла с правилами табличных частей
            CurRulesTab = JObject.Parse(contentsRulesTabPart);

            Console.WriteLine((OldRules));
            // Разбор правил табличных частей
            //  NameRules = "ТабличныеЧасти";

            // Список табличных частей
            List<RulesProperty> listRuless = new List<RulesProperty>();
            // List<RulesProperty> listRulessTemp = new List<RulesProperty>();
            JToken RlsTab = CurRulesTab.SelectToken("$.#value[?(@.name.#value == " + "'" + NameRules + "'" + " )]");
            if (RlsTab != null) // Если обписание табличных частей отсутствует
            {
                foreach (var curR in RlsTab["Value"]["#value"])
                {

                    string CurTabCh = curR["#value"].ToString();
                    cRul.ListTabPartC.Add(new ListTabPart()
                    {
                        NameProperty = CurTabCh
                    });

                    JToken RlsRk = CurRulesTab.SelectToken("$.#value[?(@.name.#value == " + "'" + curR["#value"].ToString() + "'" + " )]");


                    // int x = 0;
                    foreach (var CurObj in RlsRk["Value"]["#value"])
                    {
                        mAction = CurObj.SelectToken("$.Value.#value[?(@.name.#value == 'Действие' )].Value.#value").ToString();
                        mSourceName = CurObj.SelectToken("$.Value.#value[?(@.name.#value == 'ИмяСвойстваИсточник' )].Value.#value").ToString();
                        mRecName = CurObj.SelectToken("$.Value.#value[?(@.name.#value == 'ИмяСвойстваПриемник' )].Value.#value").ToString();
                        mTypeName = CurObj.SelectToken("$.Value.#value[?(@.name.#value == 'ТипСтрокойПриемник' )].Value.#value").ToString();
                        mOrder = CurObj.SelectToken("$.Value.#value[?(@.name.#value == 'Порядок' )].Value.#value").ToString() == "" ? "1" : CurObj.SelectToken("$.Value.#value[?(@.name.#value == 'Порядок' )].Value.#value").ToString();
                        mBef = CurObj.SelectToken("$.Value.#value[?(@.name.#value == 'Перед' )].Value.#value").ToString();

                        // TypeData = GetTypeValue(TypeName.ToString());
                        // Console.WriteLine(TypeData);
                        // cRul.ListTabPartContent.NameProperty = curR["#value"].ToString();
                        // RulesProperty ttt = new RulesProperty();
                        // cRul.ListTabPartContentC.Insert(0 ,
                        //     NameProperty = curR["#value"].ToString(),
                        //     PropertyNameSource = mSourceName,
                        //     PropertyNameDestination = mRecName,
                        //     TypeStringDestination = mTypeName,
                        //     Action = mAction,
                        //     Order = Int32.Parse(mOrder),
                        //     Before = mBef
                        //     );
                        // RulesProperty listRulesS = new RulesProperty();
                        // listRulesS.NameProperty = curR["#value"].ToString();
                        // listRulesS.PropertyNameSource = mSourceName;
                        // listRulesS.PropertyNameDestination = mRecName;
                        // listRulesS.TypeStringDestination = mTypeName;
                        // listRulesS.Action = mAction;
                        // listRulesS.Order = Int32.Parse(mOrder);
                        // listRulesS.Before = mBef;

                        // cRul.ListTabPartContentC[0].listRules.Insert(x, listRulesS);
                        // cRul.ListTabPartContentC[0].listRules.Add(new RulesProperty()
                        // {
                        //     NameProperty = NameRules,
                        //     PropertyNameSource = mSourceName,
                        //     PropertyNameDestination = mRecName,
                        //     TypeStringDestination = mTypeName,
                        //     Action = mAction,
                        //     Order = Int32.Parse(mOrder),
                        //     Before = mBef
                        // });
                        listRuless.Add(new RulesProperty()

                        {
                            NameProperty = mSourceName,
                            PropertyNameSource = mSourceName,
                            PropertyNameDestination = mRecName,
                            TypeStringDestination = mTypeName,
                            Action = mAction,
                            Order = Int32.Parse(mOrder),
                            Before = mBef
                        });
                        // x++;
                    }
                    // listRulessTemp = listRuless;
                    List<RulesProperty> listRulessTemp = new List<RulesProperty>(listRuless);
                    cRul.ListTabPartContentC.Add(new ListTabPartContent()
                    {
                        NameProperty = curR["#value"].ToString(),
                        listRules = listRulessTemp
                    });
                    // cRul.ListTabPartContentC[0].listRules = (listRuless);
                    listRuless.Clear();

                }

                string jsonString = JsonConvert.SerializeObject(cRul);
                File.WriteAllText(NewRules, jsonString);
                Console.WriteLine("Data has been saved to file - " + NewRules);
                Console.WriteLine(jsonString);
            }
        }
    }

    // static object GetTypeValue(string Act)
    // {
    //     switch (Act)
    //     {
    //         case "Булево":
    //             return false;
    //         case "Строка":
    //             return "";
    //         case "Число":
    //             return "0";
    //         case "Дата":
    //             return "0001-01-01T00:00:00";
    //         case "Неопределено":
    //             return null;
    //         case "":
    //             return "";
    //         default:
    //             return "00000000-0000-0000-0000-000000000000";
    //     }
    // }
}
