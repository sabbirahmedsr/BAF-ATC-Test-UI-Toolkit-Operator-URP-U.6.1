> <h1 style="text-align: center;">BAF ATC SIMULATOR</h1>

> <h2 style="text-align: center;">Operator PC</h2>
---
> <h2 style="text-align: center;">ATC_Home_Scene</h2>
> <h3 style="text-align: center;">Script Relation Flow Chart</h3>
```mermaid
flowchart TD
    A["ATC_Home_UIManager"] -- Child Class --> B["ATC_Home_UI_SubClass"]
    B -- Main Menu --> C["ATC_Home_UI_MainMenu"]
    C -- Start <--> E["ATC_Home_UI_SceneSetup"]
    E -- Activates --> G[("OPERATOR SCENE")]
    C -- Setting <--> D1["ATC_Home_UI_Setting"]
    C -- About <--> D2["ATC_Home_UI_About"]
    C -- Exit <--> D3["ATC_Home_UI_ExitWarning"]
    D3 -- Confirm Exit --> F[("Exit Application")]
    n1(["ATC_Home_Controller"]) -- Manual Start --> A
    A <--> n2["UI_Theme_Data"]

    A@{ shape: procs}
    B@{ shape: priority}
    C@{ shape: div-proc}
    D3@{ shape: rect}
    n2@{ shape: cyl}
    style E stroke:#00C853
    style G stroke:#00C853
    style D3 stroke:#D50000
    style F stroke:#D50000
    linkStyle 2 stroke:#00C853,fill:none
    linkStyle 3 stroke:#00C853,fill:none
    linkStyle 6 stroke:#D50000,fill:none
    linkStyle 7 stroke:#D50000,fill:none
```

---
> <h3 style="text-align: center;">Script_Link</h3>
* [ATC_Home_Controller](./ATC_Home_Controller.CS)
    * [ATC_Home_UIManager](./ATC_Home_UIManager.CS)
        * [ATC_Home_UI_SubClass](./ATC_Home_UI_SubClass.CS)
            * [ATC_Home_UI_MainMenu](./ATC_Home_UI_MainMenu.CS)
            * [ATC_Home_UI_SceneSetup](./ATC_Home_UI_SceneSetup.CS)
            * [ATC_Home_UI_Setting](./ATC_Home_UI_Setting.CS)
            * [ATC_Home_UI_About](./ATC_Home_UI_About.CS)
            * [ATC_Home_UI_ExitWarning](./ATC_Home_UI_ExitWarning.CS)
        * [UI_Theme_Data](../../Data/UI_Theme/UI_Theme_Data.cs)
---

![](./Flow_Chart.drawio.svg)