using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NullScripter
{
    class StringMap
    {
        #region Declarement
        public enum Language { kr, en, jp };
        public enum Status 
        {
            Menu_File, Menu_New, Menu_Open, Menu_Project, 
            Menu_Build, Menu_Compile,
            Menu_Setting,
            Menu_Add, Menu_Delete, Menu_Close,

            OpenProject, CreateProject,
            Ready, Compiling, Compiled, Saving, Saved,
            Elapsed, EmptyScript
        }

        public static Dictionary<Status, string> Dic;
        #endregion

        public static void Init(Language mode)
        {
            #region Initializing String Map
            Dic = new Dictionary<Status, string>();

            switch (mode)
            {
                case Language.kr:
                    Dic.Add(Status.Menu_File, "파일");
                    Dic.Add(Status.Menu_New, "새로 만들기");
                    Dic.Add(Status.Menu_Open, "열기");
                    Dic.Add(Status.Menu_Project, "프로젝트");
                    Dic.Add(Status.Menu_Build, "빌드");
                    Dic.Add(Status.Menu_Compile, "컴파일");
                    Dic.Add(Status.Menu_Setting, "설정");
                    Dic.Add(Status.Menu_Add, "추가");
                    Dic.Add(Status.Menu_Delete, "삭제");
                    Dic.Add(Status.Menu_Close, "닫기");

                    Dic.Add(Status.OpenProject, "프로젝트 열기");
                    Dic.Add(Status.CreateProject, "프로젝트 생성");

                    Dic.Add(Status.Ready, "준비");
                    Dic.Add(Status.Compiling, "컴파일 시작...");
                    Dic.Add(Status.Compiled, "컴파일되었습니다.");
                    Dic.Add(Status.Saving, "를 저장 중");
                    Dic.Add(Status.Saved, "저장되었습니다.");

                    Dic.Add(Status.Elapsed, "지남");
                    Dic.Add(Status.EmptyScript, "스크립트가 없습니다");
                    break;

                case Language.en:
                case Language.jp:
                    throw new NotImplementedException();
            }
            #endregion
        }
    }
}
