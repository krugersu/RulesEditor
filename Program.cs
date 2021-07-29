﻿using System;
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
            // ConvertRules(args[0], args[1]);

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

            var switchvalue = int.Parse(args[0]);
            if (switchvalue == 0)
            {
                {
                    //         string jsonString = JsonSerializer.Serialize(cRul);
                    string jsonString = JsonConvert.SerializeObject(cRul);
                    File.WriteAllText("user.json", jsonString);
                    Console.WriteLine("Data has been saved to file");
                    Console.WriteLine(jsonString);
                }
            }
            else
            {
                string jsonString = File.ReadAllText("SПоступлениеТоваровN.json");
                mRules mRules1 = JsonConvert.DeserializeObject<mRules>(jsonString);
                List<RulesProperty> tt = mRules1.listRules;
                Console.WriteLine(mRules1.NameRules);
                Console.WriteLine(tt);

                foreach (var p in tt)
                {
                    //  Console.WriteLine(p.NameProperty, p.Action);
                    Console.WriteLine((p.NameProperty));
                    Console.WriteLine((p.Action));
                }
            }
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

    }
}
