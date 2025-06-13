 <h2 style="text-align: center;">BAF ATC SIMULATOR</h2>
 <h3 style="text-align: center;">BAF ATC Operator [Arrival/Departure]</h3>

---

<h2 style="text-align: center;">ATC_Home_Scene</h2>

---

 ![](./-flow-chart-01.drawio.svg)

---

#### Keypoints

* **`ATC_Home_Controller`** starts **`ATC_Home_UIManager`**.
* **`ATC_Home_UIManager`** manages UI, initiating **`ATC_Home_UI_SubClass`** and leading to the **`ATC_Home_UI_MainMenu`**.
* From **`ATC_Home_UI_MainMenu`**, users can:
    * **"START"** to **`ATC_Home_UI_SceneSetup`**, then to **`GAME SCENE`**.
    * **"EXIT"** to **`ATC_Home_UI_ExitWarning`**, then **"CONFIRM"** to **`EXIT APP`**.
    * Access **`ATC_Home_UI_About`** or **`ATC_Home_UI_Setting`**.
* **`ATC_Home_UI_Setting`** handles data:
    * Loads/stores default data from/to **`Global Data`** (which contains Server and UI Data, and feeds into **`GAME SCENE`**).
    * Loads/saves data from/to **`PlayerPrefs`**.
    * Loads themes from **`UI_Theme_Data`**.

---
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