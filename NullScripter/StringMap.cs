using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NullScripter
{
    class StringMap
    {
        public enum Language { kr, en, jp };
        public enum Status 
        {
            Ready, Compiling, Compiled, Saving, Saved
        }

        public static Dictionary<Status, string> Dic;

        public static void Init(Language mode)
        {
            Dic = new Dictionary<Status, string>();

            switch (mode)
            {
                case Language.kr:
                    Dic.Add(Status.Ready, "준비");
                    Dic.Add(Status.Compiling, "컴파일 시작...");
                    Dic.Add(Status.Compiled, "컴파일되었습니다.");
                    Dic.Add(Status.Saving, "를 저장 중");
                    Dic.Add(Status.Saved, "저장되었습니다.");
                    break;

                case Language.en:
                    Dic.Add(Status.Ready, "Ready");
                    Dic.Add(Status.Compiling, "Compiling...");
                    Dic.Add(Status.Compiled, "Compiled.");
                    Dic.Add(Status.Saving, "Saving");
                    Dic.Add(Status.Saved, "Saved.");
                    break;

                case Language.jp:
                    Dic.Add(Status.Ready, "準備");
                    Dic.Add(Status.Compiling, "コンパイル中...");
                    Dic.Add(Status.Compiled, "コンパイルしました。");
                    Dic.Add(Status.Saving, "をセーブ中");
                    Dic.Add(Status.Saved, "セーブしました。");
                    break;
            }
        }
    }
}
