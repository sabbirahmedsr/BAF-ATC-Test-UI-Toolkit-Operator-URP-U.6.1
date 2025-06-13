> <h1 style="text-align: center;">BAF ATC SIMULATOR</h1>

> <h2 style="text-align: center;">Operator PC</h2>
---
> <h2 style="text-align: center;">ATC_Home_Scene</h2>
> <h3 style="text-align: center;">Script Relation Flow Chart</h3>
```mermaid
flowchart TD
    A["ATC_Home_UIManager"] -- Child Class Structure --> B["ATC_Home_UI_SubClass"]
    C["ATC_Home_UI_MainMenu"] <-- Exit --> D3["ATC_Home_UI_ExitWarning"]
    B -- Main Menu --> C
    C <-- About --> D2["ATC_Home_UI_About"]
    E["ATC_Home_UI_SceneSetup"] -- Activates --> G["OPERATOR SCENE"]
    C <-- Setting --> D1["ATC_Home_UI_Setting"]
    D3 -- Confirm Exit --> F["Exit Application"]
    n1(["ATC_Home_Controller"]) -- Manual Start --> A
    C <-- Start --> E
    n2["UI_Theme_Data"] --> D1
    D1 <-- Load Data<br>On First Run<br><br>Store Data<br>On Change<br><br>Store Data<br>Before Exit --> n3["Global Data"]
    D1 <-- Load Data<br>On Initialize<br><br>Save Data<br>On Change --> n4["PlayerPrefs"]
    n3 --> G
    A@{ shape: procs}
    B@{ shape: priority}
    C@{ shape: div-proc}
    D3@{ shape: rect}
    G@{ shape: hex}
    F@{ shape: hex}
    n2@{ shape: cyl}
    n3@{ shape: cyl}
    n4@{ shape: cyl}
     G:::Aqua
     F:::Rose
     n2:::Sky
     n3:::Sky
     n4:::Sky
    classDef Rose stroke-width:1px, stroke-dasharray:none, stroke:#FF5978, fill:#FFDFE5, color:#8E2236
    classDef Aqua stroke-width:1px, stroke-dasharray:none, stroke:#46EDC8, fill:#DEFFF8, color:#378E7A
    classDef Sky stroke-width:1px, stroke-dasharray:none, stroke:#374D7C, fill:#E2EBFF, color:#374D7C
    style D3 stroke:#D50000
    style E stroke:#00C853
    linkStyle 2 stroke:#00C853,fill:none
    linkStyle 3 stroke:#00C853,fill:none
    linkStyle 6 stroke:#D50000,fill:none
    linkStyle 7 stroke:#D50000,fill:none
```

---
> <h3 style="text-align: center;">Script_Link</h3>
* [ATC_Home_Controller](./ATC_Home_Controller.cs)
    * [ATC_Home_UIManager](./ATC_Home_UIManager.cs)
        * [ATC_Home_UI_SubClass](./ATC_Home_UI_SubClass.cs)
            * [ATC_Home_UI_MainMenu](./ATC_Home_UI_MainMenu.cs)
            * [ATC_Home_UI_SceneSetup](./ATC_Home_UI_SceneSetup.cs)
            * [ATC_Home_UI_Setting](./ATC_Home_UI_Setting.cs)
            * [ATC_Home_UI_About](./ATC_Home_UI_About.cs)
            * [ATC_Home_UI_ExitWarning](./ATC_Home_UI_ExitWarning.cs)
        * [UI_Theme_Data](../../Data/UI_Theme/UI_Theme_Data.cs)
---

![](./-flow-chart-01.svg)